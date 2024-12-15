using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;
using ReactApp1.Server.Services;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/")]
    public class TaxController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        #region Endpoints

        [HttpGet("tax")]
        public async Task<IActionResult> GetAllTaxes([FromQuery] int pageNumber, int pageSize)
        {
            var result = await _taxService.GetAllTaxes(pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("tax/{taxId}")]
        public async Task<IActionResult> GetTaxById(int taxId)
        {
            var tax = await _taxService.GetTaxById(taxId);

            return Ok(tax);
        }

        [HttpPost("tax")]
        public async Task<IActionResult> AddTax([FromBody] TaxModel tax)
        {
            await _taxService.AddTax(tax);

            return Ok();
        }

        [HttpPut("tax")]
        public async Task<IActionResult> UpdateTax([FromBody] TaxModel tax)
        {
            await _taxService.UpdateTax(tax);

            return NoContent();
        }

        [HttpPost("tax/item")]
        public async Task<IActionResult> AddItemTax([FromBody] ItemTax itemTax)
        {
            await _taxService.AddItemTax(itemTax);

            return Ok();
        }

        [HttpPost("tax/service")]
        public async Task<IActionResult> AddServiceTax([FromBody] ServiceTax serviceTax)
        {
            await _taxService.AddServiceTax(serviceTax);

            return Ok();
        }

        [HttpDelete("tax/item")]
        public async Task<IActionResult> RemoveItemTax([FromBody] ItemTax itemTax)
        {
            await _taxService.RemoveItemTax(itemTax);

            return Ok();
        }

        [HttpDelete("tax/service")]
        public async Task<IActionResult> RemoveServiceTax([FromBody] ServiceTax serviceTax)
        {
            await _taxService.RemoveServiceTax(serviceTax);

            return Ok();
        }

        [HttpGet("tax/item/{itemId}")]
        public async Task<IActionResult> GetItemTaxes(int itemId)
        {
            var taxes = await _taxService.GetItemTaxes(itemId);

            return Ok(taxes);
        }

        [HttpGet("tax/service/{serviceId}")]
        public async Task<IActionResult> GetServiceTaxes(int serviceId)
        {
            var taxes = await _taxService.GetServiceTaxes(serviceId);

            return Ok(taxes);
        }

        #endregion Endpoints
    }
}
