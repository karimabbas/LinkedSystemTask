using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkSystem.Data;
using LinkSystem.Services;
using Microsoft.AspNetCore.Mvc;
using LinkSystem.Dto;
using LinkSystem.Models;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace LinkSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Admins")]
    [Authorize(Policy = "Mangers")]
    public class ProdcutController(IProdcutService prodcutService, IWebHostEnvironment webHostEnvironment) : ControllerBase
    {
        private readonly IProdcutService _prodcutService = prodcutService;
        private readonly IWebHostEnvironment _webHostEnvironmen = webHostEnvironment;


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProdcutDto prodcutDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var uploads = Path.Combine(_webHostEnvironmen.WebRootPath, "ProdcutImages");
            var filePath = Path.Combine(uploads, prodcutDto.FormFile.FileName);
            using var filestream = new FileStream(filePath, FileMode.Create);
            await prodcutDto.FormFile.CopyToAsync(filestream);

            var product = new Prodcut
            {
                Name = prodcutDto.Name,
                Descripition = prodcutDto.Descripition,
                Price = prodcutDto.Price,
                Image = prodcutDto.FormFile.FileName.ToString()
            };
            try
            {
                await _prodcutService.CreateProduct(product);
                return StatusCode(201, "Prodcut Added Successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var porducts = await _prodcutService.GetProdcuts();
                return Ok(porducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
        {
            try
            {
                var porduct = await _prodcutService.GetProdcutById(id);
                if (porduct is not null)
                {
                    return Ok(porduct);
                }
                return NotFound("No product Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            try
            {
                var porduct = await _prodcutService.DeleteProdcut(id);
                if (porduct)
                {
                    return StatusCode(200, "product deleted successfully");
                }
                return NotFound("Faild to delete product");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct([FromRoute] Guid id, [FromForm] ProdcutDto prodcutDto)
        {
            var existingPro = await _prodcutService.GetProdcutById(id);

            try
            {
                if (!string.IsNullOrEmpty(prodcutDto.FormFile?.FileName))
                {
                    var uploads = Path.Combine(_webHostEnvironmen.WebRootPath, "ProdcutImages");
                    var filePath = Path.Combine(uploads, prodcutDto.FormFile.FileName);
                    using var filestream = new FileStream(filePath, FileMode.Create);
                    await prodcutDto.FormFile.CopyToAsync(filestream);
                    existingPro.Image = prodcutDto.FormFile?.FileName.ToString();
                }

                existingPro.Name = prodcutDto.Name;
                existingPro.Price = prodcutDto.Price;
                existingPro.Descripition = prodcutDto.Descripition;
                // existingPro.Image = existingPro.Image;

                await _prodcutService.UpdateProdcut(existingPro);
                return StatusCode(200, "Prodcut Updated Successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
    }

}