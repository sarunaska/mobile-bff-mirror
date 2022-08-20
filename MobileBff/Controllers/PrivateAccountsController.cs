using Microsoft.AspNetCore.Mvc;
using MobileBff.Models.Private.GetAccount;
using MobileBff.Models.Private.GetAccounts;
using MobileBff.Models.Private.GetAccountTransactions;
using MobileBff.Services;

namespace MobileBff.Controllers
{
    [ApiController]
    [Route("private/accounts")]
    public class PrivateAccountsController : ControllerBase
    {
        private readonly IPrivateAccountsService privateAccountsService;

        public PrivateAccountsController(IPrivateAccountsService privateAccountsService)
        {
            this.privateAccountsService = privateAccountsService;
        }

        [ProducesResponseType(typeof(PrivateGetAccountsResponseModel), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAccounts(
            [FromHeader(Name = "organization-id")] string organizationId,
            [FromHeader(Name = "jwt-Assertion")] string jwtAssertion)
        {
            var result = await privateAccountsService.GetAccounts(organizationId, jwtAssertion);

            return Ok(result);
        }

        [ProducesResponseType(typeof(PrivateGetAccountResponseModel), StatusCodes.Status200OK)]
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccount(
            [FromHeader(Name = "organization-id")] string organizationId,
            [FromHeader(Name = "jwt-Assertion")] string jwtAssertion,
            string accountId)
        {
            var result = await privateAccountsService.GetAccount(organizationId, jwtAssertion, accountId);

            return Ok(result);
        }

        [ProducesResponseType(typeof(PrivateGetAccountTransactionsResponseModel), StatusCodes.Status200OK)]
        [HttpGet("{accountId}/transactions")]
        public async Task<IActionResult> GetAccountTransactions(
            [FromHeader(Name = "organization-id")] string organizationId,
            [FromHeader(Name = "jwt-Assertion")] string jwtAssertion,
            [FromQuery(Name = "paginating_key")] string? paginatingKey,
            [FromQuery(Name = "paginating_size")] string? paginatingSize,
            string accountId)
        {
            var result = await privateAccountsService.GetAccountTransactions(organizationId, jwtAssertion, accountId, paginatingKey, paginatingSize);

            return Ok(result);
        }
    }
}
