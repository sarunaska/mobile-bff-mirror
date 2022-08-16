using Microsoft.AspNetCore.Mvc;
using MobileBff.Models.Corporate.GetAccount;
using MobileBff.Models.Corporate.GetAccounts;
using MobileBff.Models.Corporate.GetAccountTransactions;
using MobileBff.Services;

namespace MobileBff.Controllers
{
    [ApiController]
    [Route("corp/accounts")]
    public class CorporateAccountsController : ControllerBase
    {
        private readonly ICorporateAccountsService corporateAccountsService;

        public CorporateAccountsController(ICorporateAccountsService corporateAccountsService)
        {
            this.corporateAccountsService = corporateAccountsService;
        }

        [ProducesResponseType(typeof(CorporateGetAccountsResponseModel), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAccounts(
            [FromHeader(Name = "organization-id")] string organizationId,
            [FromHeader(Name = "jwt-Assertion")] string jwtAssertion)
        {
            var result = await corporateAccountsService.GetAccounts(organizationId, jwtAssertion);

            return Ok(result);
        }

        [ProducesResponseType(typeof(CorporateGetAccountResponseModel), StatusCodes.Status200OK)]
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccount(
            [FromHeader(Name = "organization-id")] string organizationId,
            [FromHeader(Name = "jwt-Assertion")] string jwtAssertion,
            string accountId)
        {
            var result = await corporateAccountsService.GetAccount(organizationId, jwtAssertion, accountId);

            return Ok(result);
        }

        [ProducesResponseType(typeof(CorporateGetAccountTransactionsResponseModel), StatusCodes.Status200OK)]
        [HttpGet("{accountId}/transactions")]
        public async Task<IActionResult> GetAccountTransactions(
            [FromHeader(Name = "organization-id")] string organizationId,
            [FromHeader(Name = "jwt-Assertion")] string jwtAssertion,
            [FromQuery(Name = "paginating_key")] string? paginatingKey,
            [FromQuery(Name = "paginating_size")] string? paginatingSize,
            string accountId)
        {
            var result = await corporateAccountsService.GetAccountTransactions(organizationId, jwtAssertion, accountId, paginatingKey, paginatingSize);

            return Ok(result);
        }
    }
}
