using AdapiClient;
using AdapiClient.Endpoints.GetAccounts;
using MobileBff;
using MobileBff.ExtensionMethods;
using MobileBff.Formatters;
using MobileBff.Resources;
using MobileBff.Services;
using MobileBff.Utilities;
using Moq;
using SebCsClient;
using SebCsClient.Models;
using Tests.ResponseProviders;

namespace Tests.Services
{
    public class YouthAccountsServiceTests
    {
        [Fact]
        public async Task GetAccounts_WhenOneAccountIsReceivedFromAdapi_ShouldReturnAccount()
        {
            var adapiResponse = AdapiResponseProvider.CreateGetAccountsResponse(currency: Constants.Currencies.SEK);

            var adapiClientMock = new Mock<IAdapiClient>();
            adapiClientMock
                .Setup(x => x.GetAccounts(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();
            var jwtParserMock = new Mock<IJwtParser>();

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var accounts = await service.GetAccounts("Test user ID", "test-jwtAssertion");

            Assert.Equal(adapiResponse.Result!.RetrievedDateTime, accounts.RetrievedDateTime);

            Assert.Single(accounts.AccountGroups);
            Assert.Single(accounts.AccountGroups?[0].Accounts);

            Assert.Equal(adapiResponse.Result.Accounts![0].Name, accounts.AccountGroups?[0].Accounts[0].Name);
            Assert.Equal(adapiResponse.Result.Accounts[0].Product!.Id, accounts.AccountGroups?[0].Accounts[0].ProductId);
            Assert.Equal(adapiResponse.Result.Accounts[0].Identifications!.ResourceId, accounts.AccountGroups?[0].Accounts[0].ResourceId);

            var expectedBalance = adapiResponse.Result.Accounts[0].Balances!
                    .Where(x => x.Type == Constants.BalanceTypes.Booked)
                    .Select(x => x.Amount!)
                    .Single()
                    .ToBankingDecimal();

            Assert.Equal(expectedBalance, accounts.AccountGroups?[0].Accounts[0].Balance);

            var expectedAvailable = adapiResponse.Result.Accounts[0].Balances!
                    .Where(x => x.Type == Constants.BalanceTypes.Available)
                    .Select(x => x.Amount!)
                    .Single()
                    .ToBankingDecimal();

            Assert.Equal(expectedAvailable, accounts.AccountGroups?[0].Accounts[0].Available);

            Assert.True(accounts.AccountGroups?[0].Accounts[0].CanWithdraw);
            Assert.True(accounts.AccountGroups?[0].Accounts[0].CanDeposit);
        }

        [Fact]
        public async Task GetAccounts_WhenAdapiReturnsAccountsWithForeignCurrency_ShouldReturnAccountsOnlyWithCurrencySEK()
        {
            var sekAccountId = "SEK account id";

            var adapiResponse = new GetAccountsResponse
            {
                Result = new GetAccountsResult
                {
                    Accounts = new[]
                    {
                        AdapiResponseProvider.CreateAccountModel(accountResourceId: sekAccountId, currency: Constants.Currencies.SEK),
                        AdapiResponseProvider.CreateAccountModel()
                    }
                }
            };

            var adapiClientMock = new Mock<IAdapiClient>();
            adapiClientMock
                .Setup(x => x.GetAccounts(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();
            var jwtParserMock = new Mock<IJwtParser>();

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var accounts = await service.GetAccounts("Test user ID", "test-jwtAssertion");

            Assert.Single(accounts.AccountGroups);
            Assert.Single(accounts.AccountGroups![0].Accounts);
            Assert.Equal(sekAccountId, accounts.AccountGroups![0].Accounts[0].ResourceId);
        }

        [Fact]
        public async Task GetAccounts_WhenAdapiReturnsAccountsWithIpsProductCode_ShouldReturnAccountsWithoutIpsProductCode()
        {
            var nonIpsAccountId = "non-IPS account id";

            var adapiResponse = new GetAccountsResponse
            {
                Result = new GetAccountsResult
                {
                    Accounts = new[]
                    {
                        AdapiResponseProvider.CreateAccountModel(productCode: Constants.ProductCodes.Ips, currency: Constants.Currencies.SEK),
                        AdapiResponseProvider.CreateAccountModel(accountResourceId: nonIpsAccountId, currency: Constants.Currencies.SEK)
                    }
                }
            };

            var adapiClientMock = new Mock<IAdapiClient>();
            adapiClientMock
                .Setup(x => x.GetAccounts(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();
            var jwtParserMock = new Mock<IJwtParser>();

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var accounts = await service.GetAccounts("Test user ID", "test-jwtAssertion");

            Assert.Single(accounts.AccountGroups);
            Assert.Single(accounts.AccountGroups![0].Accounts);
            Assert.Equal(nonIpsAccountId, accounts.AccountGroups![0].Accounts[0].ResourceId);
        }

        [Fact]
        public async Task GetAccounts_WhenAdapiReturnsAccountsWithIskProductCode_ShouldReturnAccountsWithoutIskProductCode()
        {
            var nonIskAccountId = "non-ISK account id";

            var adapiResponse = new GetAccountsResponse
            {
                Result = new GetAccountsResult
                {
                    Accounts = new[]
                    {
                        AdapiResponseProvider.CreateAccountModel(productCode: Constants.ProductCodes.IskDepositAccounts, currency: Constants.Currencies.SEK),
                        AdapiResponseProvider.CreateAccountModel(accountResourceId: nonIskAccountId, currency: Constants.Currencies.SEK)
                    }
                }
            };

            var adapiClientMock = new Mock<IAdapiClient>();
            adapiClientMock
                .Setup(x => x.GetAccounts(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();
            var jwtParserMock = new Mock<IJwtParser>();

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var accounts = await service.GetAccounts("Test user ID", "test-jwtAssertion");

            Assert.Single(accounts.AccountGroups);
            Assert.Single(accounts.AccountGroups![0].Accounts);
            Assert.Equal(nonIskAccountId, accounts.AccountGroups![0].Accounts[0].ResourceId);
        }

        [Fact]
        public async Task GetAccounts_ShouldGetUserIdFromJwtToken()
        {
            var adapiClientMock = new Mock<IAdapiClient>();
            var sebCsClientMock = new Mock<ISebCsClient>();

            var userId = "Test user ID";

            var jwtParserMock = new Mock<IJwtParser>();
            jwtParserMock
                .Setup(x => x.GetUserId(It.IsAny<string>()))
                .Returns(userId);

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var accounts = await service.GetAccounts("Test user ID", "test-jwtAssertion");

            adapiClientMock.Verify(x => x.GetAccounts(userId, It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task GetAccount_WhenAccountIsReceivedFromAdapi_ShouldReturnAccount()
        {
            var accountId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountResponse(accountId);

            var adapiClientMock = new Mock<IAdapiClient>();
            adapiClientMock
                .Setup(x => x.GetAccount(userId, It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var ownerName = "Test Owner Name";

            var sebCsClientMock = new Mock<ISebCsClient>();
            sebCsClientMock
                .Setup(x => x.GetAccountOwner(userId, It.IsAny<string>()))
                .ReturnsAsync(new AccountOwner { Id = userId, Name = ownerName });

            var jwtParserMock = new Mock<IJwtParser>();
            jwtParserMock
                .Setup(x => x.GetUserId(It.IsAny<string>()))
                .Returns(userId);

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var account = await service.GetAccount(userId, "test-jwtAssertion", accountId);

            Assert.Equal(adapiResponse.Result!.RetrievedDateTime, account.RetrievedDateTime);

            Assert.Equal(adapiResponse.Result.Account!.Identifications!.ResourceId, account.ResourceId);

            Assert.Single(account.Details);

            Assert.Equal(Titles.Account_Details, account.Details?[0].Title);

            var accountName = account.Details?[0].Items
                    .Where(x => x.Title == Titles.AccountItem_AccountName && x.Action == Constants.Actions.ChangeAccountName)
                    .Select(x => x.Value).Single();
            Assert.Equal(adapiResponse.Result.Account.Name, accountName);

            Assert.Equal(adapiResponse.Result.Account.Product!.Name,
                account.Details?[0].Items.Where(x => x.Title == Titles.AccountItem_AccountType).Select(x => x.Value).Single());

            var accountNumber = account.Details?[0].Items
                    .Where(x => x.Title == Titles.AccountItem_AccountNumber && x.Action == Constants.Actions.Copy)
                    .Select(x => x.Value).Single();
            Assert.Equal(adapiResponse.Result.Account.Identifications.DomesticAccountNumber, accountNumber);

            var actualOwnerName = account.Details?[0].Items
                    .Where(x => x.Title == Titles.AccountItem_AccountOwner)
                    .Select(x => x.Value)
                    .Single();
            Assert.Equal(ownerName, actualOwnerName);

            var expectedIban = IbanFormatter.Format(adapiResponse.Result.Account.Identifications.Iban);
            var iban = account.Details?[0].Items
                    .Where(x => x.Title == Constants.Titles.Iban && x.Action == Constants.Actions.Copy)
                    .Select(x => x.Value).Single();
            Assert.Equal(expectedIban, iban);

            var bic = account.Details?[0].Items
                    .Where(x => x.Title == Constants.Titles.Bic && x.Action == Constants.Actions.Copy)
                    .Select(x => x.Value).Single();
            Assert.Equal(adapiResponse.Result.Account.Identifications.Bic, bic);
        }

        [Fact]
        public async Task GetAccount_ShouldGetUserIdFromJwtToken()
        {
            var adapiClientMock = new Mock<IAdapiClient>();
            var sebCsClientMock = new Mock<ISebCsClient>();

            var userId = "Test user ID";

            var jwtParserMock = new Mock<IJwtParser>();
            jwtParserMock
                .Setup(x => x.GetUserId(It.IsAny<string>()))
                .Returns(userId);

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var accounts = await service.GetAccount("Test user ID", "test-jwtAssertion", "Test account ID");

            adapiClientMock.Verify(x => x.GetAccount(userId, It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task GetAccountTransactions_WhenAccountTransactionsAreReceivedFromAdapi_ShouldReturnAccountTransactions()
        {
            var accountId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountTransactionsResponse();

            var adapiClientMock = new Mock<IAdapiClient>();
            adapiClientMock
                .Setup(x => x.GetAccountTransactions(It.IsAny<string>(), It.IsAny<string>(), accountId, null, null))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();
            var jwtParserMock = new Mock<IJwtParser>();

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("Test user ID", "test-jwtAssertion", accountId, null, null);

            Assert.Equal(adapiResponse.Result!.RetrievedDateTime, accountTransactions.RetrievedDateTime);

            Assert.Equal(adapiResponse.Result.Account!.Identifications!.ResourceId, accountTransactions.Account.ResourceId);
            Assert.Equal(adapiResponse.Result.Account.Identifications.DomesticAccountNumber, accountTransactions.Account.AccountNumber);

            Assert.Equal(adapiResponse.Result.PaginatingInformation!.Paginating, accountTransactions.PaginatingInformation.Paginating);
            Assert.Equal(adapiResponse.Result.PaginatingInformation.DateFrom, accountTransactions.PaginatingInformation.DateFrom);
            Assert.Equal(adapiResponse.Result.PaginatingInformation.PaginatingKey, accountTransactions.PaginatingInformation.PaginatingKey);

            Assert.Equal(adapiResponse.Result.DepositEntryEvent!.BookingEntries![0].TransactionId, accountTransactions.Entries?[0].TransactionId);
            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].ValueDate, accountTransactions.Entries?[0].ValueDate);
            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].BookingDate, accountTransactions.Entries?[0].BookingDate);

            var expectedAmount = adapiResponse.Result.DepositEntryEvent.BookingEntries[0].TransactionAmount!.Amount!.ToBankingDecimal();
            Assert.Equal(expectedAmount, accountTransactions.Entries?[0].Amount);

            var expectedBookedBalance = adapiResponse.Result.DepositEntryEvent.BookingEntries[0].BookedBalance!.Amount!.ToBankingDecimal();
            Assert.Equal(expectedBookedBalance, accountTransactions.Entries?[0].BookedBalance);

            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].TransactionAmount!.Currency, accountTransactions.Entries?[0].Currency);
            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].Message1, accountTransactions.Entries?[0].Message);

            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].BankTransactionCode!.EntryType, accountTransactions.Entries?[0].Type.Code);
            Assert.Equal(Titles.TransactionType_Transfer, accountTransactions.Entries?[0].Type.Text);

            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].Links!.TransactionDetails!.Href,
                accountTransactions.Entries?[0].Links!.TransactionDetails.Href);
        }

        [Fact]
        public async Task GetAccountTransactions_WhenAccountWithoutTransactionsIsReceivedFromAdapi_ShouldReturnAccountWithoutTransactions()
        {
            var accountId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountTransactionsResponse(hasTransaction: false);

            var adapiClientMock = new Mock<IAdapiClient>();
            adapiClientMock
                .Setup(x => x.GetAccountTransactions(It.IsAny<string>(), It.IsAny<string>(), accountId, null, null))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();
            var jwtParserMock = new Mock<IJwtParser>();

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("Test user ID", "test-jwtAssertion", accountId, null, null);

            Assert.Empty(accountTransactions.Entries);
        }

        [Fact]
        public async Task GetAccountTransactions_WhenAccountTransactionHasNoLinks_ShouldReturnAccountTransactionWithoutLinks()
        {
            var accountId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountTransactionsResponse(hasLinks: false);

            var adapiClientMock = new Mock<IAdapiClient>();
            adapiClientMock
                .Setup(x => x.GetAccountTransactions(It.IsAny<string>(), It.IsAny<string>(), accountId, null, null))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();
            var jwtParserMock = new Mock<IJwtParser>();

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("Test user ID", "test-jwtAssertion", accountId, null, null);

            Assert.Null(accountTransactions.Entries?[0].Links);
        }

        [Fact]
        public async Task GetAccountTransactions_WhenAccountCardTransactionIsReceivedFromAdapi_ShouldReturnMerchantNameAsTransactionMessage()
        {
            var accountId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountTransactionsResponse(isCardTransaction: true);

            var adapiClientMock = new Mock<IAdapiClient>();
            adapiClientMock
                .Setup(x => x.GetAccountTransactions(It.IsAny<string>(), It.IsAny<string>(), accountId, null, null))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();
            var jwtParserMock = new Mock<IJwtParser>();

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("Test user ID", "test-jwtAssertion", accountId, null, null);

            Assert.Equal(adapiResponse.Result!.DepositEntryEvent!.BookingEntries![0].CardBookingEntryDetails!.MerchantName, accountTransactions.Entries?[0].Message);
        }

        [Fact]
        public async Task GetAccountTransactions_ShouldGetUserIdFromJwtToken()
        {
            var adapiClientMock = new Mock<IAdapiClient>();
            var sebCsClientMock = new Mock<ISebCsClient>();

            var userId = "Test user ID";

            var jwtParserMock = new Mock<IJwtParser>();
            jwtParserMock
                .Setup(x => x.GetUserId(It.IsAny<string>()))
                .Returns(userId);

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var accounts = await service.GetAccountTransactions("Test user ID", "test-jwtAssertion", "Test account ID", null, null);

            adapiClientMock.Verify(x => x.GetAccountTransactions(userId, It.IsAny<string>(), It.IsAny<string>(), null, null), Times.Once());
        }

        [Fact]
        public async Task GetAccountReservedAmounts_WhenAccountReservedAmountsAreReceivedFromAdapi_ShouldReturnAccountReservedAmounts()
        {
            var accountId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountReservedAmountsResponse();

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountReservedAmounts(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();

            var jwtParserMock = new Mock<IJwtParser>();

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var accountTransactions = await service.GetAccountReservedAmounts("Test user ID", "test-jwtAssertion", accountId);

            Assert.Equal(adapiResponse.Result!.RetrievedDateTime, accountTransactions.RetrievedDateTime);

            Assert.Equal(adapiResponse.Result.Account!.Identifications!.ResourceId, accountTransactions.Account.ResourceId);
            Assert.Equal(adapiResponse.Result.Account.Identifications.DomesticAccountNumber, accountTransactions.Account.AccountNumber);

            Assert.Single(accountTransactions.Reservations);
            Assert.Equal(adapiResponse.Result.AccountReservations![0].Origin, accountTransactions.Reservations?[0].Origin);
            Assert.Equal(adapiResponse.Result.AccountReservations![0].ReservationDate, accountTransactions.Reservations?[0].ReservationDate);
            Assert.Equal(adapiResponse.Result.AccountReservations![0].Amount.ToBankingDecimal(), accountTransactions.Reservations?[0].Amount);
            Assert.Equal(adapiResponse.Result.AccountReservations![0].Currency, accountTransactions.Reservations?[0].Currency);
            Assert.Equal(adapiResponse.Result.AccountReservations![0].DescriptiveText, accountTransactions.Reservations?[0].Message);
        }

        [Fact]
        public async Task GetAccountReservedAmounts_WhenAccountWithoutReservedAmountsIsReceivedFromAdapi_ShouldReturnAccountWithoutReservedAmounts()
        {
            var accountId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountReservedAmountsResponse(hasReservation: false);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountReservedAmounts(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();

            var jwtParserMock = new Mock<IJwtParser>();

            var service = new YouthAccountsService(adapiClientMock.Object, sebCsClientMock.Object);

            var accountTransactions = await service.GetAccountReservedAmounts("Test user ID", "test-jwtAssertion", accountId);

            Assert.Empty(accountTransactions.Reservations);
        }
    }
}
