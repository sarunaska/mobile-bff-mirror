using Microsoft.AspNetCore.Mvc;
using MobileBff.Models.Private.GetAccount;
using MobileBff.Models.Private.GetAccountFutureEvents;
using MobileBff.Models.Private.GetAccountReservedAmounts;
using MobileBff.Models.Private.GetAccounts;
using MobileBff.Models.Private.GetAccountTransactions;
using MobileBff.Services;
using MobileBff.Services.ResponseValidation;
using MobileBff.Utilities;

namespace MobileBff.Controllers
{
    [ApiController]
    [Route("/private/v{version:apiVersion}/accounts")]
    public class PrivateAccountsController : BffControllerBase
    {
        private readonly IPrivateAccountsService privateAccountsService;
        private readonly IJwtParser jwtParser;

        public PrivateAccountsController(IPrivateAccountsService privateAccountsService, IResponseValidator responseValidator, IJwtParser jwtParser)
            : base(responseValidator)
        {
            this.privateAccountsService = privateAccountsService;
            this.jwtParser = jwtParser;
        }

        [ProducesResponseType(typeof(PrivateGetAccountsResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PrivateGetAccountsResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet]
        public async Task<IActionResult> GetAccounts(
            [FromHeader(Name = Constants.Headers.Principal)] string? principal,
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion)
        {
            var userId = principal ?? jwtParser.GetUserId(jwtAssertion);
            var result = await privateAccountsService.GetAccounts(userId, jwtAssertion);

            return OkOrPartialContent(result);
        }

        [ProducesResponseType(typeof(PrivateGetAccountResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PrivateGetAccountResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccount(
            [FromHeader(Name = Constants.Headers.Principal)] string? principal,
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion,
            string accountId)
        {
            var userId = principal ?? jwtParser.GetUserId(jwtAssertion);
            var result = await privateAccountsService.GetAccount(userId, jwtAssertion, accountId);

            return OkOrPartialContent(result);
        }

        [ProducesResponseType(typeof(PrivateGetAccountTransactionsResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PrivateGetAccountTransactionsResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet("{accountId}/transactions")]
        public async Task<IActionResult> GetAccountTransactions(
            [FromHeader(Name = Constants.Headers.Principal)] string? principal,
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion,
            [FromQuery(Name = Constants.Headers.PaginatingKey)] string? paginatingKey,
            [FromQuery(Name = Constants.Headers.PaginatingSize)] string? paginatingSize,
            string accountId)
        {
            var userId = principal ?? jwtParser.GetUserId(jwtAssertion);
            var result = await privateAccountsService.GetAccountTransactions(userId, jwtAssertion, accountId, paginatingKey, paginatingSize);

            return OkOrPartialContent(result);
        }

        [ProducesResponseType(typeof(PrivateGetAccountFutureEventsResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PrivateGetAccountFutureEventsResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet("{accountId}/future-events")]
        public async Task<IActionResult> GetAccountFutureEvents(
            [FromHeader(Name = Constants.Headers.Principal)] string? principal,
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion,
            string accountId)
        {
            var userId = principal ?? jwtParser.GetUserId(jwtAssertion);
            var result = await privateAccountsService.GetAccountFutureEvents(userId, jwtAssertion, accountId);

            return OkOrPartialContent(result);
        }

        [ProducesResponseType(typeof(PrivateGetAccountReservedAmountsResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PrivateGetAccountReservedAmountsResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet("{accountId}/reserved-amounts")]
        public async Task<IActionResult> GetAccountReservedAmounts(
            [FromHeader(Name = Constants.Headers.Principal)] string? principal,
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion,
            string accountId)
        {
            var userId = principal ?? jwtParser.GetUserId(jwtAssertion);
            var result = await privateAccountsService.GetAccountReservedAmounts(userId, jwtAssertion, accountId);

            return OkOrPartialContent(result);
        }
    }
}
