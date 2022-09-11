using Microsoft.AspNetCore.Mvc;
using MobileBff.Models.Corporate.GetAccount;
using MobileBff.Models.Corporate.GetAccountFutureEvents;
using MobileBff.Models.Corporate.GetAccounts;
using MobileBff.Models.Corporate.GetAccountTransactions;
using MobileBff.Services;
using MobileBff.Services.ResponseValidation;

namespace MobileBff.Controllers
{
    [ApiController]
    [Route("/corp/v{version:apiVersion}/accounts")]
    [ApiVersion("1.0")]
    public class CorporateAccountsController : BffControllerBase
    {
        private readonly ICorporateAccountsService corporateAccountsService;

        public CorporateAccountsController(ICorporateAccountsService corporateAccountsService, IResponseValidator responseValidator)
            : base(responseValidator)
        {
            this.corporateAccountsService = corporateAccountsService;
        }

        [ProducesResponseType(typeof(CorporateGetAccountsResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CorporateGetAccountsResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet]
        public async Task<IActionResult> GetAccounts(
            [FromHeader(Name = Constants.Headers.Principal)] string principal,
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion)
        {
            var result = await corporateAccountsService.GetAccounts(principal, jwtAssertion);

            return OkOrPartialContent(result);
        }

        [ProducesResponseType(typeof(CorporateGetAccountResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CorporateGetAccountResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccount(
            [FromHeader(Name = Constants.Headers.Principal)] string principal,
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion,
            string accountId)
        {
            var result = await corporateAccountsService.GetAccount(principal, jwtAssertion, accountId);

            return OkOrPartialContent(result);
        }

        [ProducesResponseType(typeof(CorporateGetAccountTransactionsResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CorporateGetAccountTransactionsResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet("{accountId}/transactions")]
        public async Task<IActionResult> GetAccountTransactions(
            [FromHeader(Name = Constants.Headers.Principal)] string principal,
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion,
            [FromQuery(Name = Constants.Headers.PaginatingKey)] string? paginatingKey,
            [FromQuery(Name = Constants.Headers.PaginatingSize)] string? paginatingSize,
            string accountId)
        {
            var result = await corporateAccountsService.GetAccountTransactions(principal, jwtAssertion, accountId, paginatingKey, paginatingSize);

            return OkOrPartialContent(result);
        }

        [ProducesResponseType(typeof(CorporateGetAccountFutureEventsResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CorporateGetAccountFutureEventsResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet("{accountId}/future-events")]
        public async Task<IActionResult> GetAccountFutureEvents(
            [FromHeader(Name = Constants.Headers.Principal)] string principal,
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion,
            string accountId)
        {
            var result = await corporateAccountsService.GetAccountFutureEvents(principal, jwtAssertion, accountId);

            return OkOrPartialContent(result);
        }

        [ProducesResponseType(typeof(CorporateGetAccountTransactionsResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CorporateGetAccountTransactionsResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet("{accountId}/reserved-amounts")]
        public async Task<IActionResult> GetAccountReservedAmounts(
            [FromHeader(Name = Constants.Headers.Principal)] string principal,
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion,
            string accountId)
        {
            var result = await corporateAccountsService.GetAccountReservedAmounts(principal, jwtAssertion, accountId);

            return OkOrPartialContent(result);
        }
    }
}
