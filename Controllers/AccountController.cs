using System.Reflection.Metadata.Ecma335;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LinkSystem.Dto;
using LinkSystem.Services;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using LinkSystem.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LinkSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(UserManager<User> userManager, IUserService userService, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IUserService _userService = userService;


        [HttpPost("SginUp")]
        public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
        {
            // var userDB = await _userService.GetUser(userRegister.Email);
            var userDB = await _userManager.FindByEmailAsync(userRegister.Email);
            if (userDB is not null)
                return Ok(new { message = "User Is already Registerd Before" });
            try
            {
                PasswordHelper.CreateHashing(userRegister.Password, out byte[] hash, out byte[] salt);

                var RegisterdUser = new User
                {
                    FirstName = userRegister.FirstName,
                    LastName = userRegister.LastName,
                    Email = userRegister.Email,
                    UserName = userRegister.FirstName + userRegister.LastName,
                    HashPassword = Convert.ToBase64String(hash),
                    SaltPassword = Convert.ToBase64String(salt)
                };

                var result = await _userManager.CreateAsync(RegisterdUser, userRegister.Password);
                await _signInManager.SignInAsync(RegisterdUser, isPersistent: false);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(RegisterdUser, UserRole.User);

                    return StatusCode(200, "User Registered Successfully");
                }
                foreach (var item in result.Errors)
                {
                    return Ok(item.Description);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("SginIn")]
        public async Task<IActionResult> LogIn([FromBody] UserLogin userLogin)
        {
            var user = await _userService.GetUser(userLogin.Email);
            if (user is null)
                return Ok(new { message = "there's no Account Registered with this email" });

            var Ispassword = await _userManager.CheckPasswordAsync(user, userLogin.Password);
            if (Ispassword)
            {
                try
                {
                    var result = await _signInManager.PasswordSignInAsync(user, userLogin.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Ok(user);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(new { message = "Error, Wrong Password" });
        }

        [HttpGet("SignOut")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok("User Loged out Successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "Admins")]
        [HttpGet("SetUpPermissons")]
        public async Task<IActionResult> SetUpPermissons()
        {
            var admin = await _roleManager.FindByNameAsync(UserRole.Admin);
            if (admin is not null)
            {
                await _roleManager.AddClaimAsync(admin, new Claim(CustomCalimTypes.Permission, Permissions.ProductCRUD));
                await _roleManager.AddClaimAsync(admin, new Claim(CustomCalimTypes.Permission, Permissions.SetUpRoles));
            }
            var manger = await _roleManager.FindByNameAsync(UserRole.Manger);
            if (manger is not null)
            {
                await _roleManager.AddClaimAsync(manger, new Claim(CustomCalimTypes.Permission, Permissions.ProductCRUD));
            }

            return Ok("Permission Signed Successfully");
        }

        [Authorize(Policy = "Admins")]
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            try
            {
                var users = await _userService.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [Authorize(Policy = "Admins")]
        [HttpPost("MakeAdmin")]
        public async Task<IActionResult> MakeUserAdmin([FromForm] string email)
        {
            try
            {
                var admin = await _userService.MakeAmdin(email);
                if (admin)
                    return Ok(new { message = "User becomes Admin successfully", email });
                return BadRequest(new { message = "faild to be admin" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [Authorize(Policy = "Admins")]
        [HttpPost("MakeManger")]
        public async Task<IActionResult> MakeUserManger([FromForm] string email)
        {
            try
            {
                var admin = await _userService.MakeManger(email);
                if (admin)
                    return Ok(new { message = "User becomes Manger successfully", email });
                return BadRequest(new { message = "ther's no Role assaigend to this user" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }


    }
}