using Microsoft.AspNetCore.Mvc;
using MobileBff.Models.Youth.GetAccount;
using MobileBff.Models.Youth.GetAccountReservedAmounts;
using MobileBff.Models.Youth.GetAccounts;
using MobileBff.Models.Youth.GetAccountTransactions;
using MobileBff.Services;
using MobileBff.Services.ResponseValidation;
using MobileBff.Utilities;

namespace MobileBff.Controllers
{
    [ApiController]
    [Route("/youth/v{version:apiVersion}/accounts")]
    [ApiVersion("1.0")]
    public class YouthAccountsController : BffControllerBase
    {
        private readonly IYouthAccountsService youthAccountsService;
        private readonly IJwtParser jwtParser;

        public YouthAccountsController(IYouthAccountsService youthAccountsService, IResponseValidator responseValidator, IJwtParser jwtParser)
            : base(responseValidator)
        {
            this.youthAccountsService = youthAccountsService;
            this.jwtParser = jwtParser;
        }

        [ProducesResponseType(typeof(YouthGetAccountsResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(YouthGetAccountsResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet]
        public async Task<IActionResult> GetAccounts(
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion)
        {
            var userId = jwtParser.GetUserId(jwtAssertion);
            var result = await youthAccountsService.GetAccounts(userId, jwtAssertion);

            return OkOrPartialContent(result);
        }

        [ProducesResponseType(typeof(YouthGetAccountResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(YouthGetAccountResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccount(
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion,
            string accountId)
        {
            var userId = jwtParser.GetUserId(jwtAssertion);
            var result = await youthAccountsService.GetAccount(userId, jwtAssertion, accountId);

            return OkOrPartialContent(result);
        }

        [ProducesResponseType(typeof(YouthGetAccountTransactionsResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(YouthGetAccountTransactionsResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet("{accountId}/transactions")]
        public async Task<IActionResult> GetAccountTransactions(
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion,
            [FromQuery(Name = Constants.Headers.PaginatingKey)] string? paginatingKey,
            [FromQuery(Name = Constants.Headers.PaginatingSize)] string? paginatingSize,
            string accountId)
        {
            var userId = jwtParser.GetUserId(jwtAssertion);
            var result = await youthAccountsService.GetAccountTransactions(userId, jwtAssertion, accountId, paginatingKey, paginatingSize);

            return OkOrPartialContent(result);
        }

        [ProducesResponseType(typeof(YouthGetAccountReservedAmountsResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(YouthGetAccountReservedAmountsResponseModel), StatusCodes.Status206PartialContent)]
        [HttpGet("{accountId}/reserved-amounts")]
        public async Task<IActionResult> GetAccountReservedAmounts(
            [FromHeader(Name = Constants.Headers.JwtAssertion)] string jwtAssertion,
            string accountId)
        {
            var userId = jwtParser.GetUserId(jwtAssertion);
            var result = await youthAccountsService.GetAccountReservedAmounts(userId, jwtAssertion, accountId);

            return OkOrPartialContent(result);
        }
    }
}
