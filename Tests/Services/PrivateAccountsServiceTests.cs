using A3SClient;
using AdapiClient;
using AdapiClient.Endpoints.GetAccounts;
using MobileBff.Formatters;
using MobileBff.Models;
using MobileBff.Resources;
using MobileBff.Services;
using Moq;
using Tests.ResponseProviders;

namespace Tests.Services
{
    public class PrivateAccountsServiceTests
    {
        [Fact]
        public async Task GetAccounts_WhenA3SReturnsNoPowerOfAttorneyAccount_ShouldReturnOneAccountsGroup()
        {
            var a3sResponse = A3SResponseProvider.CreateGetUserCustomersResponse();
            var a3sClientMock = new Mock<IA3SClient>();

            a3sClientMock
                .Setup(x => x.GetUserCustomers(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(a3sResponse);

            var adapiResponse = AdapiResponseProvider.CreateGetAccountsResponse(currency: Constants.Currencies.SEK);
            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccounts(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(adapiResponse);

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object);

            var accounts = await service.GetAccounts("Test User Id", "test-jwtAssertion");

            Assert.Equal(adapiResponse.Result.RetrievedDateTime, accounts.RetrievedDateTime);

            Assert.Single(accounts.AccountGroups);
            Assert.Null(accounts.AccountGroups![0].Owner);

            Assert.Single(accounts.AccountGroups[0].Accounts);

            Assert.Equal(adapiResponse.Result.Accounts[0].Name,
                accounts.AccountGroups[0].Accounts[0].Name);

            Assert.Equal(adapiResponse.Result.Accounts[0].Product.Id,
                accounts.AccountGroups[0].Accounts[0].ProductId);

            Assert.Equal(adapiResponse.Result.Accounts[0].Identifications.ResourceId,
                accounts.AccountGroups[0].Accounts[0].ResourceId);

            Assert.Equal(adapiResponse.Result.Accounts[0].Balances.Where(x => x.Type == Constants.BalanceTypes.Booked).Select(x => x.Amount).Single(),
                accounts.AccountGroups[0].Accounts[0].Balance);

            Assert.Equal(adapiResponse.Result.Accounts[0].Balances.Where(x => x.Type == Constants.BalanceTypes.Available).Select(x => x.Amount).Single(),
                accounts.AccountGroups[0].Accounts[0].Available);

            Assert.True(accounts.AccountGroups[0].Accounts[0].CanWithdraw);

            Assert.True(accounts.AccountGroups[0].Accounts[0].CanDeposit);
        }

        [Fact]
        public async Task GetAccounts_WhenA3SReturnsOnePowerOfAttorneyAccount_ShouldReturnTwoAccountsGroups()
        {
            var a3sResponse = A3SResponseProvider.CreateGetUserCustomersResponse(addPowerOfAttorneyAccount: true);
            var a3sClientMock = new Mock<IA3SClient>();

            a3sClientMock
                .Setup(x => x.GetUserCustomers(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(a3sResponse);

            var adapiResponse = AdapiResponseProvider.CreateGetAccountsResponse();
            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccounts(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(adapiResponse);

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object);

            var accounts = await service.GetAccounts("Test User Id", "test-jwtAssertion");

            Assert.Equal(adapiResponse.Result.RetrievedDateTime, accounts.RetrievedDateTime);

            Assert.Equal(2, accounts.AccountGroups!.Count());
            Assert.Single(accounts.AccountGroups!.Where(x => x.Owner == null));
            Assert.Single(accounts.AccountGroups!.Where(x => x.Owner?.Id != null));
        }

        [Fact]
        public async Task GetAccounts_WhenA3SReturnsNoUsers_ShouldReturnNoAccountsGroups()
        {
            var a3sResponse = A3SResponseProvider.CreateGetUserCustomersResponse(returnNoUserCustomers: true);
            var a3sClientMock = new Mock<IA3SClient>();

            a3sClientMock
                .Setup(x => x.GetUserCustomers(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(a3sResponse);

            var adapiClientMock = new Mock<IAdapiClient>();
            adapiClientMock.Verify(x => x.GetAccounts(It.IsAny<string>(), It.IsAny<string>()), Times.Never());

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object);

            var accounts = await service.GetAccounts("Test User Id", "test-jwtAssertion");

            Assert.Null(accounts.AccountGroups);
        }

        [Fact]
        public async Task GetAccounts_WhenAdapiReturnsAccountsWithForeignCurrency_ShouldReturnAccountsOnlyWithCurrencySEK()
        {
            var a3sResponse = A3SResponseProvider.CreateGetUserCustomersResponse();
            var a3sClientMock = new Mock<IA3SClient>();

            a3sClientMock
                .Setup(x => x.GetUserCustomers(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(a3sResponse);

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

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object);

            var accounts = await service.GetAccounts("Test User Id", "test-jwtAssertion");

            Assert.Single(accounts.AccountGroups);
            Assert.Single(accounts.AccountGroups![0].Accounts);
            Assert.Equal(sekAccountId, accounts.AccountGroups![0].Accounts[0].ResourceId);
        }

        [Fact]
        public async Task GetAccounts_WhenAdapiReturnsAccountsWithIpsProductCode_ShouldReturnAccountsWithoutIpsProductCode()
        {
            var a3sResponse = A3SResponseProvider.CreateGetUserCustomersResponse();
            var a3sClientMock = new Mock<IA3SClient>();

            a3sClientMock
                .Setup(x => x.GetUserCustomers(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(a3sResponse);

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

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object);

            var accounts = await service.GetAccounts("Test User Id", "test-jwtAssertion");

            Assert.Single(accounts.AccountGroups);
            Assert.Single(accounts.AccountGroups![0].Accounts);
            Assert.Equal(nonIpsAccountId, accounts.AccountGroups![0].Accounts[0].ResourceId);
        }

        [Fact]
        public async Task GetAccount_WhenAccountIsReceivedFromAdapi_ShouldReturnAccount()
        {
            var accountId = Guid.NewGuid().ToString();

            var a3sClientMock = new Mock<IA3SClient>();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountResponse(accountId);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object);

            var account = await service.GetAccount("test-organizationId", "test-jwtAssertion", accountId);

            Assert.Equal(adapiResponse.Result.RetrievedDateTime, account.RetrievedDateTime);

            Assert.Equal(adapiResponse.Result.Account.Identifications.ResourceId, account.ResourceId);

            Assert.Equal(adapiResponse.Result.Account.Name,
                account.Details[0].Items
                    .Where(x => x.Title == Titles.AccountItem_AccountName && x.Action == Constants.Actions.ChangeAccountName)
                    .Select(x => x.Value).Single());

            Assert.Equal(adapiResponse.Result.Account.Product.Name,
                account.Details[0].Items.Where(x => x.Title == Titles.AccountItem_AccountType).Select(x => x.Value).Single());

            Assert.Equal(adapiResponse.Result.Account.Identifications.DomesticAccountNumber,
                account.Details[0].Items
                    .Where(x => x.Title == Titles.AccountItem_AccountNumber && x.Action == Constants.Actions.Copy)
                    .Select(x => x.Value).Single());

            Assert.Null(account.Details[0].Items.Where(x => x.Title == Titles.AccountItem_AccountOwner).Select(x => x.Value).Single());

            var expectedIban = IbanFormatter.Format(adapiResponse.Result.Account.Identifications.Iban);
            Assert.Equal(expectedIban,
                account.Details[0].Items
                    .Where(x => x.Title == Constants.Titles.Iban && x.Action == Constants.Actions.Copy)
                    .Select(x => x.Value).Single());

            Assert.Equal(adapiResponse.Result.Account.Identifications.Bic,
                account.Details[0].Items
                    .Where(x => x.Title == Constants.Titles.Bic && x.Action == Constants.Actions.Copy)
                    .Select(x => x.Value).Single());
        }

        [Fact]
        public async Task GetAccountTransactions_WhenAccountTransactionsAreReceivedFromAdapi_ShouldReturnAccountTransactions()
        {
            var accountId = Guid.NewGuid().ToString();

            var a3sClientMock = new Mock<IA3SClient>();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountTransactionsResponse();

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountTransactions(It.IsAny<string>(), It.IsAny<string>(), accountId, null, null))
                .ReturnsAsync(adapiResponse);

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("test-organizationId", "test-jwtAssertion", accountId, null, null);

            Assert.Equal(adapiResponse.Result.RetrievedDateTime, accountTransactions.RetrievedDateTime);

            Assert.Equal(adapiResponse.Result.Account.Identifications.ResourceId, accountTransactions.Account.ResourceId);
            Assert.Equal(adapiResponse.Result.Account.Identifications.DomesticAccountNumber, accountTransactions.Account.AccountNumber);

            Assert.Equal(adapiResponse.Result.PaginatingInformation.Paginating, accountTransactions.PaginatingInformation.Paginating);
            Assert.Equal(adapiResponse.Result.PaginatingInformation.DateFrom, accountTransactions.PaginatingInformation.DateFrom);
            Assert.Equal(adapiResponse.Result.PaginatingInformation.PaginatingKey, accountTransactions.PaginatingInformation.PaginatingKey);

            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].TransactionId, accountTransactions.Entries[0].TransactionId);
            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].ExternalId, accountTransactions.Entries[0].ExternalId);
            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].ValueDate, accountTransactions.Entries[0].ValueDate);
            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].BookingDate, accountTransactions.Entries[0].BookingDate);
            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].TransactionAmount.Amount, accountTransactions.Entries[0].Amount);
            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].BookedBalance.Amount, accountTransactions.Entries[0].BookedBalance);
            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].TransactionAmount.Currency, accountTransactions.Entries[0].Currency);
            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].Message1, accountTransactions.Entries[0].Message);

            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].BankTransactionCode.EntryType, accountTransactions.Entries[0].Type.Code);
            Assert.Equal(Titles.TransactionType_Transfer, accountTransactions.Entries[0].Type.Text);

            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].Links!.TransactionDetails.Href,
                accountTransactions.Entries[0].Links!.TransactionDetails.Href);
        }

        [Fact]
        public async Task GetAccountTransactions_WhenAccountWithoutTransactionsIsReceivedFromAdapi_ShouldReturnAccountWithoutTransactions()
        {
            var accountId = Guid.NewGuid().ToString();

            var a3sClientMock = new Mock<IA3SClient>();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountTransactionsResponse(hasTransaction: false);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountTransactions(It.IsAny<string>(), It.IsAny<string>(), accountId, null, null))
                .ReturnsAsync(adapiResponse);

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("test-organizationId", "test-jwtAssertion", accountId, null, null);

            Assert.False(adapiResponse.Result.DepositEntryEvent.BookingEntries.Any());
        }

        [Fact]
        public async Task GetAccountTransactions_WhenAccountTransactionHasNoLinks_ShouldReturnAccountTransactionWithoutLinks()
        {
            var accountId = Guid.NewGuid().ToString();

            var a3sClientMock = new Mock<IA3SClient>();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountTransactionsResponse(hasLinks: false);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountTransactions(It.IsAny<string>(), It.IsAny<string>(), accountId, null, null))
                .ReturnsAsync(adapiResponse);

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("test-organizationId", "test-jwtAssertion", accountId, null, null);

            Assert.Null(accountTransactions.Entries[0].Links);
        }
    }
}
