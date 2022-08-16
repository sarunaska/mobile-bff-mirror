using AdapiClient;
using AdapiClient.Endpoints.GetAccount;
using AdapiClient.Endpoints.GetAccounts;
using AdapiClient.Endpoints.GetAccountTransactions;
using AdapiClient.Models;
using MobileBff.Formatters;
using MobileBff.Models;
using MobileBff.Resources;
using MobileBff.Services;
using Moq;
using Tests.ResponseProviders;

namespace Tests.Services
{
    public class CorporateAccountsServiceTests
    {
        [Fact]
        public async Task GetAccounts_WhenOneAccountIsReceivedFromAdapi_ShouldReturnAccount()
        {
            var adapiResponse = AdapiResponseProvider.CreateGetAccountsResponse();

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccounts(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(adapiResponse);

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accounts = await service.GetAccounts("test-organizationId", "test-jwtAssertion");

            Assert.Equal(adapiResponse.Result.RetrievedDateTime, accounts.RetrievedDateTime);

            Assert.Single(accounts.AccountGroups);
            Assert.Equal(adapiResponse.Result.Accounts[0].Currency, accounts.AccountGroups[0].Currency);

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

            Assert.Equal(adapiResponse.Result.Accounts[0].Aliases!.Where(x => x.Type == Constants.AliasTypes.Swish).Select(x => x.Id),
                accounts.AccountGroups[0].Accounts[0].Aliases?.Where(x => x.Type == Constants.AliasTypes.Swish).Select(x => x.Id));

            Assert.Equal(adapiResponse.Result.Accounts[0].Aliases!.Where(x => x.Type == Constants.AliasTypes.BankGiro).Select(x => x.Id),
                accounts.AccountGroups[0].Accounts[0].Aliases?.Where(x => x.Type == Constants.AliasTypes.BankGiro).Select(x => x.Id));
        }

        [Fact]
        public async Task GetAccounts_WhenMultipleAccountsAreReceivedFromAdapi_ShouldReturnAccountsGroupedByCurrency()
        {
            var adapiResponse = new GetAccountsResponse
            {
                Result = new GetAccountsResult
                {
                    RetrievedDateTime = DateTime.Now,
                    Accounts = new[]
                    {
                        AdapiResponseProvider.CreateAccountModel(currency: "EUR"),
                        AdapiResponseProvider.CreateAccountModel(currency: Constants.Currencies.SEK),
                        AdapiResponseProvider.CreateAccountModel(currency: "EUR")
                    }
                }
            };

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccounts(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(adapiResponse);

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accounts = await service.GetAccounts("test-organizationId", "test-jwtAssertion");

            Assert.Equal(adapiResponse.Result.RetrievedDateTime, accounts.RetrievedDateTime);
            Assert.Equal(2, accounts.AccountGroups.Length);

            Assert.Contains(accounts.AccountGroups, x => x.Currency == "EUR");
            Assert.Contains(accounts.AccountGroups, x => x.Currency == null);

            Assert.Equal(2, accounts.AccountGroups.Where(x => x.Currency == "EUR").Single().Accounts.Length);
            Assert.Single(accounts.AccountGroups.Where(x => x.Currency == null).Single().Accounts);
        }

        [Fact]
        public async Task GetAccounts_WhenNoAccountsAreReceivedFromAdapi_ShouldReturnNoAccounts()
        {
            var adapiResponse = AdapiResponseProvider.CreateGetAccountsResponse(hasAccounts: false);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccounts(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(adapiResponse);

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accounts = await service.GetAccounts("test-organizationId", "test-jwtAssertion");

            Assert.Equal(adapiResponse.Result.RetrievedDateTime, accounts.RetrievedDateTime);
            Assert.Empty(accounts.AccountGroups);
        }

        [Fact]
        public async Task GetAccount_WhenAccountIsReceivedFromAdapi_ShouldReturnAccount()
        {
            var accountId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountResponse(accountId);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var account = await service.GetAccount("test-organizationId", "test-jwtAssertion", accountId);

            Assert.Equal(adapiResponse.Result.RetrievedDateTime, account.RetrievedDateTime);

            Assert.Equal(adapiResponse.Result.Account.Identifications.ResourceId, account.ResourceId);

            Assert.Equal(adapiResponse.Result.Account.Product.Name,
                account.Details[0].Items.Where(x => x.Title == Titles.AccountItem_AccountType).Select(x => x.Value).Single());

            Assert.Equal(adapiResponse.Result.Account.Identifications.DomesticAccountNumber,
                account.Details[0].Items
                    .Where(x => x.Title == Titles.AccountItem_AccountNumber && x.Action == Constants.Actions.Copy)
                    .Select(x => x.Value).Single());

            var adapiSwishCorporateAlias = adapiResponse.Result.Account.Aliases!.Where(x => x.ProductId == Constants.ProductIds.SwishCorporate).Select(x => x).Single();
            var expectedSwishCorporateItemValue = SwissValueFormatter.Format(adapiSwishCorporateAlias.Id, adapiSwishCorporateAlias.Name);
            Assert.Equal(expectedSwishCorporateItemValue,
                account.Details[0].Items.Where(x => x.Title == Titles.AccountItem_SwishCorporate).Select(x => x.ValueList![0]).Single());

            var adapiForMerchantsAlias = adapiResponse.Result.Account.Aliases!.Where(x => x.ProductId == Constants.ProductIds.SwishForMerchants).Select(x => x).Single();
            var expectedForMerchantsItemValue = SwissValueFormatter.Format(adapiForMerchantsAlias.Id, adapiForMerchantsAlias.Name);
            Assert.Equal(expectedForMerchantsItemValue,
                account.Details[0].Items.Where(x => x.Title == Titles.AccountItem_SwishForMerchants).Select(x => x.ValueList![0]).Single());

            Assert.Equal(adapiResponse.Result.Account.Aliases!.Where(x => x.Type == Constants.AliasTypes.BankGiro).Select(x => x.Id).Single(),
                account.Details[0].Items.Where(x => x.Title == Titles.AccountItem_BankGiro).Select(x => x.ValueList![0]).Single());

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
        public async Task GetAccount_WhenAccountWithoutAliasesIsReceivedFromAdapi_ShouldReturnAccountWithoutAliases()
        {
            var accountId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountResponse(hasAliases: false);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var account = await service.GetAccount("test-organizationId", "test-jwtAssertion", accountId);

            Assert.False(account.Details[0].Items.Where(x => x.Title == Titles.AccountItem_SwishCorporate).Any());
            Assert.False(account.Details[0].Items.Where(x => x.Title == Titles.AccountItem_SwishForMerchants).Any());
            Assert.False(account.Details[0].Items.Where(x => x.Title == Titles.AccountItem_BankGiro).Any());
        }

        [Fact]
        public async Task GetAccount_WhenAccountWithoutSwishAliasNameIsReceivedFromAdapi_ShouldReturnAccountWithoutSwishAliasName()
        {
            var accountId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountResponse(accountId, hasSwissAliasName: false);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var account = await service.GetAccount("test-organizationId", "test-jwtAssertion", accountId);

            var adapiSwishCorporateAlias = adapiResponse.Result.Account.Aliases!.Where(x => x.ProductId == Constants.ProductIds.SwishCorporate).Select(x => x).Single();
            var expectedSwishCorporateItemValue = SwissValueFormatter.Format(adapiSwishCorporateAlias.Id, null);
            Assert.Equal(expectedSwishCorporateItemValue,
                account.Details[0].Items.Where(x => x.Title == Titles.AccountItem_SwishCorporate).Select(x => x.ValueList![0]).Single());

            var adapiForMerchantsAlias = adapiResponse.Result.Account.Aliases!.Where(x => x.ProductId == Constants.ProductIds.SwishForMerchants).Select(x => x).Single();
            var expectedForMerchantsItemValue = SwissValueFormatter.Format(adapiForMerchantsAlias.Id, null);
            Assert.Equal(expectedForMerchantsItemValue,
                account.Details[0].Items.Where(x => x.Title == Titles.AccountItem_SwishForMerchants).Select(x => x.ValueList![0]).Single());
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

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("test-organizationId", "test-jwtAssertion", accountId, null, null);

            Assert.Equal(adapiResponse.Result.RetrievedDateTime, accountTransactions.RetrievedDateTime);

            Assert.Equal(adapiResponse.Result.Account.Identifications.ResourceId, accountTransactions.Account.ResourceId);
            Assert.Equal(adapiResponse.Result.Account.Identifications.DomesticAccountNumber, accountTransactions.Account.AccountNumber);

            Assert.Equal(adapiResponse.Result.PaginatingInformation.Paginating, accountTransactions.PaginatingInformation.Paginating);
            Assert.Equal(adapiResponse.Result.PaginatingInformation.DateFrom, accountTransactions.PaginatingInformation.DateFrom);
            Assert.Equal(adapiResponse.Result.PaginatingInformation.PaginatingKey, accountTransactions.PaginatingInformation.PaginatingKey);

            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].TransactionId, accountTransactions.Entries[0].TransactionId);
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

            var adapiResponse = AdapiResponseProvider.CreateGetAccountTransactionsResponse(hasTransaction: false);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountTransactions(It.IsAny<string>(), It.IsAny<string>(), accountId, null, null))
                .ReturnsAsync(adapiResponse);

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("test-organizationId", "test-jwtAssertion", accountId, null, null);

            Assert.False(adapiResponse.Result.DepositEntryEvent.BookingEntries.Any());
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

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("test-organizationId", "test-jwtAssertion", accountId, null, null);

            Assert.Null(accountTransactions.Entries[0].Links);
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

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("test-organizationId", "test-jwtAssertion", accountId, null, null);

            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].CardBookingEntryDetails!.MerchantName, accountTransactions.Entries[0].Message);
        }
    }
}
