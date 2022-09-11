using AdapiClient;
using AdapiClient.Endpoints.GetAccounts;
using MobileBff;
using MobileBff.ExtensionMethods;
using MobileBff.Formatters;
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

            Assert.Equal(adapiResponse.Result!.RetrievedDateTime, accounts.RetrievedDateTime);

            Assert.Single(accounts.AccountGroups);
            Assert.Equal(adapiResponse.Result!.Accounts![0].Currency, accounts.AccountGroups?[0].Currency);

            Assert.Single(accounts.AccountGroups?[0].Accounts);

            Assert.Equal(adapiResponse.Result.Accounts[0].Name, accounts.AccountGroups?[0].Accounts?[0].Name);
            Assert.Equal(adapiResponse.Result.Accounts[0].Product!.Id, accounts.AccountGroups?[0].Accounts?[0].ProductId);
            Assert.Equal(adapiResponse.Result.Accounts[0].Identifications!.ResourceId, accounts.AccountGroups?[0].Accounts?[0].ResourceId);

            var expectedBalance = adapiResponse.Result.Accounts[0].Balances!
                .Where(x => x.Type == Constants.BalanceTypes.Booked)
                .Select(x => x.Amount!)
                .Single()
                .ToBankingDecimal();

            Assert.Equal(expectedBalance, accounts.AccountGroups?[0].Accounts?[0].Balance);

            var expectedAvailable = adapiResponse.Result.Accounts[0].Balances!
                    .Where(x => x.Type == Constants.BalanceTypes.Available)
                    .Select(x => x.Amount!)
                .Single()
                .ToBankingDecimal();

            Assert.Equal(expectedAvailable, accounts.AccountGroups?[0].Accounts?[0].Available);

            Assert.Equal(adapiResponse.Result.Accounts[0].Aliases!.Where(x => x.Type == Constants.AliasTypes.Swish).Select(x => x.Id),
                accounts.AccountGroups?[0].Accounts?[0].Aliases?.Where(x => x.Type == Constants.AliasTypes.Swish).Select(x => x.Id));

            Assert.Equal(adapiResponse.Result.Accounts[0].Aliases!.Where(x => x.Type == Constants.AliasTypes.BankGiro).Select(x => x.Id),
                accounts.AccountGroups?[0].Accounts?[0].Aliases?.Where(x => x.Type == Constants.AliasTypes.BankGiro).Select(x => x.Id));

            Assert.True(accounts.AccountGroups?[0].Accounts?[0].CanWithdraw);
            Assert.True(accounts.AccountGroups?[0].Accounts?[0].CanDeposit);
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
            Assert.Equal(2, accounts.AccountGroups?.Count);

            Assert.Contains(accounts.AccountGroups, x => x.Currency == "EUR");
            Assert.Contains(accounts.AccountGroups, x => x.Currency == null);

            Assert.Equal(2, accounts.AccountGroups?.Where(x => x.Currency == "EUR").Single().Accounts?.Count);
            Assert.Single(accounts.AccountGroups?.Where(x => x.Currency == null).Single().Accounts);
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

            Assert.Equal(adapiResponse.Result!.RetrievedDateTime, accounts.RetrievedDateTime);
            Assert.Empty(accounts.AccountGroups);
        }

        [Fact]
        public async Task GetAccounts_WhenThereIsMoreThanOneAccountsGroup_ShouldReturnAccountGroupsSortedByCurrency()
        {
            var currencyEur = "EUR";
            var currencyGbp = "GBP";

            var adapiResponse = new GetAccountsResponse
            {
                Result = new GetAccountsResult
                {
                    RetrievedDateTime = DateTime.Now,
                    Accounts = new[]
                    {
                        AdapiResponseProvider.CreateAccountModel(currency: currencyGbp),
                        AdapiResponseProvider.CreateAccountModel(currency: Constants.Currencies.SEK),
                        AdapiResponseProvider.CreateAccountModel(currency: currencyEur)
                    }
                }
            };

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccounts(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(adapiResponse);

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accounts = await service.GetAccounts("test-organizationId", "test-jwtAssertion");

            Assert.Equal(3, accounts.AccountGroups?.Count);
            Assert.Null(accounts.AccountGroups?[0].Currency);
            Assert.Equal(currencyEur, accounts.AccountGroups?[1].Currency);
            Assert.Equal(currencyGbp, accounts.AccountGroups?[2].Currency);
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

            Assert.Equal(adapiResponse.Result!.RetrievedDateTime, account.RetrievedDateTime);

            Assert.Equal(adapiResponse.Result.Account!.Identifications!.ResourceId, account.ResourceId);

            Assert.Single(account.Details);

            Assert.Equal(Titles.Account_Details, account.Details?[0].Title);

            Assert.Equal(adapiResponse.Result.Account.Product!.Name,
                account.Details?[0].Items.Where(x => x.Title == Titles.AccountItem_AccountType).Select(x => x.Value).Single());

            var accountNumber = account.Details?[0].Items
                    .Where(x => x.Title == Titles.AccountItem_AccountNumber && x.Action == Constants.Actions.Copy)
                    .Select(x => x.Value).Single();
            Assert.Equal(adapiResponse.Result.Account.Identifications.DomesticAccountNumber, accountNumber);

            var adapiSwishCorporateAlias = adapiResponse.Result.Account.Aliases!.Where(x => x.ProductId == Constants.ProductIds.SwishCorporate).Select(x => x).Single();
            var expectedSwishCorporateItemValue = SwissValueFormatter.Format(adapiSwishCorporateAlias.Id!, adapiSwishCorporateAlias.Name);
            Assert.Equal(expectedSwishCorporateItemValue,
                account.Details?[0].Items.Where(x => x.Title == Titles.AccountItem_SwishCorporate).Select(x => x.ValueList![0]).Single());

            var adapiForMerchantsAlias = adapiResponse.Result.Account.Aliases!.Where(x => x.ProductId == Constants.ProductIds.SwishForMerchants).Select(x => x).Single();
            var expectedForMerchantsItemValue = SwissValueFormatter.Format(adapiForMerchantsAlias.Id!, adapiForMerchantsAlias.Name);
            Assert.Equal(expectedForMerchantsItemValue,
                account.Details?[0].Items.Where(x => x.Title == Titles.AccountItem_SwishForMerchants).Select(x => x.ValueList![0]).Single());

            Assert.Equal(adapiResponse.Result.Account.Aliases!.Where(x => x.Type == Constants.AliasTypes.BankGiro).Select(x => x.Id).Single(),
                account.Details?[0].Items.Where(x => x.Title == Titles.AccountItem_BankGiro).Select(x => x.ValueList![0]).Single());

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

            Assert.False(account.Details?[0].Items.Where(x => x.Title == Titles.AccountItem_SwishCorporate).Any());
            Assert.False(account.Details?[0].Items.Where(x => x.Title == Titles.AccountItem_SwishForMerchants).Any());
            Assert.False(account.Details?[0].Items.Where(x => x.Title == Titles.AccountItem_BankGiro).Any());
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

            var adapiSwishCorporateAlias = adapiResponse.Result!.Account!.Aliases!.Where(x => x.ProductId == Constants.ProductIds.SwishCorporate).Select(x => x).Single();
            var expectedSwishCorporateItemValue = SwissValueFormatter.Format(adapiSwishCorporateAlias.Id!, null);
            Assert.Equal(expectedSwishCorporateItemValue,
                account.Details?[0].Items.Where(x => x.Title == Titles.AccountItem_SwishCorporate).Select(x => x.ValueList![0]).Single());

            var adapiForMerchantsAlias = adapiResponse.Result.Account.Aliases!.Where(x => x.ProductId == Constants.ProductIds.SwishForMerchants).Select(x => x).Single();
            var expectedForMerchantsItemValue = SwissValueFormatter.Format(adapiForMerchantsAlias.Id!, null);
            Assert.Equal(expectedForMerchantsItemValue,
                account.Details?[0].Items.Where(x => x.Title == Titles.AccountItem_SwishForMerchants).Select(x => x.ValueList![0]).Single());
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

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("test-organizationId", "test-jwtAssertion", accountId, null, null);

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

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("test-organizationId", "test-jwtAssertion", accountId, null, null);

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

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("test-organizationId", "test-jwtAssertion", accountId, null, null);

            Assert.Equal(adapiResponse.Result!.DepositEntryEvent!.BookingEntries![0].CardBookingEntryDetails!.MerchantName, accountTransactions.Entries?[0].Message);
        }

        [Fact]
        public async Task GetAccountFutureEvents_WhenAccountFutureEventsAreReceivedFromAdapi_ShouldReturnAccountFutureEvents()
        {
            var accountId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountFutureEventsResponse();

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountFutureEvents(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accountFutureEvents = await service.GetAccountFutureEvents("test-organizationId", "test-jwtAssertion", accountId);

            Assert.Equal(adapiResponse.Result!.RetrievedDateTime, accountFutureEvents.RetrievedDateTime);

            Assert.Equal(adapiResponse.Result.Account!.Identifications!.ResourceId, accountFutureEvents.Account.ResourceId);
            Assert.Equal(adapiResponse.Result.Account.Identifications.DomesticAccountNumber, accountFutureEvents.Account.AccountNumber);

            Assert.Single(accountFutureEvents.FutureEvents);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].PaymentType, accountFutureEvents.FutureEvents?[0].PaymentType);

            Assert.Equal(adapiResponse.Result.FutureEvents![0].PaymentDetailType, accountFutureEvents.FutureEvents?[0].DetailType?.Code);
            Assert.NotNull(accountFutureEvents.FutureEvents?[0].DetailType?.Text);

            Assert.Equal(adapiResponse.Result.FutureEvents![0].PaymentRelatedId, accountFutureEvents.FutureEvents?[0].PaymentRelatedId);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].CreditAccount, accountFutureEvents.FutureEvents?[0].CreditAccount);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].CreditorName, accountFutureEvents.FutureEvents?[0].CreditorName);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].DebitAccount, accountFutureEvents.FutureEvents?[0].DebitAccount);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].ValueDate, accountFutureEvents.FutureEvents?[0].ValueDate);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].Amount.ToBankingDecimal(), accountFutureEvents.FutureEvents?[0].Amount);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].Currency, accountFutureEvents.FutureEvents?[0].Currency);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].DescriptiveText, accountFutureEvents.FutureEvents?[0].Message);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].Ocr, accountFutureEvents.FutureEvents?[0].Ocr);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].BankPrefix, accountFutureEvents.FutureEvents?[0].BankPrefix);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].Suti, accountFutureEvents.FutureEvents?[0].Suti);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].IsEquivalentAmount, accountFutureEvents.FutureEvents?[0].IsEquivalentAmount);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].DeleteAllowed, accountFutureEvents.FutureEvents?[0].DeleteAllowed);
            Assert.Equal(adapiResponse.Result.FutureEvents![0].ModifyAllowed, accountFutureEvents.FutureEvents?[0].ModifyAllowed);
        }

        [Fact]
        public async Task GetAccountFutureEvents_WhenAccountWithoutFutureEventsIsReceivedFromAdapi_ShouldReturnAccountWithoutFutureEvents()
        {
            var accountId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountFutureEventsResponse(hasFutureEvent: false);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountFutureEvents(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accountFutureEvents = await service.GetAccountFutureEvents("test-organizationId", "test-jwtAssertion", accountId);

            Assert.Empty(accountFutureEvents.FutureEvents);
        }

        [Fact]
        public async Task GetAccountFutureEvents_WhenCreditDebitIndicatorIsDbitAndReceivedAmountFromAdapiIsPositive_ShouldReturnNegativeAmount()
        {
            var accountId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountFutureEventsResponse(
                creditDebitIndicator: Constants.CreditDebitIndicators.DBIT,
                amount: "10");

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountFutureEvents(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accountFutureEvents = await service.GetAccountFutureEvents("test-organizationId", "test-jwtAssertion", accountId);

            Assert.Equal(-10, accountFutureEvents.FutureEvents?[0].Amount);
        }

        [Fact]
        public async Task GetAccountFutureEvents_WhenCreditDebitIndicatorIsDbitAndReceivedAmountFromAdapiIsNegative_ShouldReturnNegativeAmount()
        {
            var accountId = Guid.NewGuid().ToString();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountFutureEventsResponse(
                creditDebitIndicator: Constants.CreditDebitIndicators.DBIT,
                amount: "-10");

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountFutureEvents(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accountFutureEvents = await service.GetAccountFutureEvents("test-organizationId", "test-jwtAssertion", accountId);

            Assert.Equal(-10, accountFutureEvents.FutureEvents?[0].Amount);
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

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accountTransactions = await service.GetAccountReservedAmounts("test-organizationId", "test-jwtAssertion", accountId);

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

            var service = new CorporateAccountsService(adapiClientMock.Object);

            var accountTransactions = await service.GetAccountReservedAmounts("test-organizationId", "test-jwtAssertion", accountId);

            Assert.Empty(accountTransactions.Reservations);
        }
    }
}
