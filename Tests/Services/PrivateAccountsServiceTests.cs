using A3SClient;
using AdapiClient;
using AdapiClient.Endpoints.GetAccounts;
using MobileBff;
using MobileBff.ExtensionMethods;
using MobileBff.Formatters;
using MobileBff.Resources;
using MobileBff.Services;
using Moq;
using SebCsClient;
using SebCsClient.Models;
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

            var sebCsClientMock = new Mock<ISebCsClient>();

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

            var accounts = await service.GetAccounts("Test User Id", "test-jwtAssertion");

            Assert.Equal(adapiResponse.Result!.RetrievedDateTime, accounts.RetrievedDateTime);

            Assert.Single(accounts.AccountGroups);
            Assert.Null(accounts.AccountGroups![0].Owner);

            Assert.Single(accounts.AccountGroups[0].Accounts);

            Assert.Equal(adapiResponse.Result.Accounts![0].Name, accounts.AccountGroups[0].Accounts?[0].Name);
            Assert.Equal(adapiResponse.Result.Accounts[0].Product!.Id, accounts.AccountGroups[0].Accounts?[0].ProductId);
            Assert.Equal(adapiResponse.Result.Accounts[0].Identifications!.ResourceId, accounts.AccountGroups[0].Accounts?[0].ResourceId);

            var expectedBalance = adapiResponse.Result.Accounts[0].Balances!
                .Where(x => x.Type == Constants.BalanceTypes.Booked)
                .Select(x => x.Amount!)
                .Single()
                .ToBankingDecimal();

            Assert.Equal(expectedBalance, accounts.AccountGroups[0].Accounts?[0].Balance);

            var expectedAvailable = adapiResponse.Result.Accounts[0].Balances!
                .Where(x => x.Type == Constants.BalanceTypes.Available)
                .Select(x => x.Amount!)
                .Single().ToBankingDecimal();

            Assert.Equal(expectedAvailable, accounts.AccountGroups[0].Accounts?[0].Available);

            Assert.True(accounts.AccountGroups[0].Accounts?[0].CanWithdraw);
            Assert.True(accounts.AccountGroups[0].Accounts?[0].CanDeposit);
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

            var sebCsClientMock = new Mock<ISebCsClient>();
            sebCsClientMock
                .Setup(x => x.GetAccountOwner(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string userId, string jwtToken) => Task.FromResult<AccountOwner?>(new AccountOwner { Id = userId, Name = $"{userId}_name" }));

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

            var userId = "Test User Id";
            var accounts = await service.GetAccounts(userId, "test-jwtAssertion");

            Assert.Equal(adapiResponse.Result!.RetrievedDateTime, accounts.RetrievedDateTime);

            Assert.Equal(2, accounts.AccountGroups!.Count());
            Assert.Single(accounts.AccountGroups!.Where(x => x.Owner == null));
            Assert.Single(accounts.AccountGroups!.Where(x => x.Owner?.Id != null && x.Owner?.Id != userId));
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

            var sebCsClientMock = new Mock<ISebCsClient>();

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

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

            var sebCsClientMock = new Mock<ISebCsClient>();

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

            var accounts = await service.GetAccounts("Test User Id", "test-jwtAssertion");

            Assert.Single(accounts.AccountGroups);
            Assert.Single(accounts.AccountGroups![0].Accounts);
            Assert.Equal(sekAccountId, accounts.AccountGroups![0].Accounts?[0].ResourceId);
        }

        [Fact]
        public async Task GetAccounts_WhenThereIsMoreThanOneAccountsGroup_ShouldReturnAccountGroupsSortedByOwner()
        {
            var currentUserId = "Current user ID";

            var powerOfAttorneyAccountOwners = new List<(string UserId, string Name)>
            {
                ("UserId1", "C User"),
                ("UserId2", "A User"),
                ("UserId3", "B User")
            };

            var userIds = new[]
            {
                currentUserId,
                powerOfAttorneyAccountOwners[0].UserId,
                powerOfAttorneyAccountOwners[1].UserId,
                powerOfAttorneyAccountOwners[2].UserId
            };

            var a3sResponse = A3SResponseProvider.CreateGetUserCustomersResponse(userIds);

            var a3sClientMock = new Mock<IA3SClient>();
            a3sClientMock
                .Setup(x => x.GetUserCustomers(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(a3sResponse);

            var adapiResponse = AdapiResponseProvider.CreateGetAccountsResponse(currency: Constants.Currencies.SEK);

            var adapiClientMock = new Mock<IAdapiClient>();
            adapiClientMock
                .Setup(x => x.GetAccounts(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();

            powerOfAttorneyAccountOwners.ForEach(owner =>
                sebCsClientMock
                    .Setup(x => x.GetAccountOwner(owner.UserId, It.IsAny<string>()))
                    .ReturnsAsync(new AccountOwner { Id = owner.UserId, Name = owner.Name }));

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

            var accounts = await service.GetAccounts(currentUserId, "test-jwtAssertion");

            Assert.Equal(4, accounts.AccountGroups?.Count);
            Assert.Null(accounts.AccountGroups![0].Owner);
            Assert.Equal(powerOfAttorneyAccountOwners[1].Name, accounts.AccountGroups![1].Owner?.Name);
            Assert.Equal(powerOfAttorneyAccountOwners[2].Name, accounts.AccountGroups![2].Owner?.Name);
            Assert.Equal(powerOfAttorneyAccountOwners[0].Name, accounts.AccountGroups![3].Owner?.Name);
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

            var sebCsClientMock = new Mock<ISebCsClient>();

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

            var accounts = await service.GetAccounts("Test User Id", "test-jwtAssertion");

            Assert.Single(accounts.AccountGroups);
            Assert.Single(accounts.AccountGroups![0].Accounts);
            Assert.Equal(nonIpsAccountId, accounts.AccountGroups![0].Accounts?[0].ResourceId);
        }

        [Fact]
        public async Task GetAccount_WhenAccountIsReceivedFromAdapi_ShouldReturnAccount()
        {
            var accountId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            var a3sClientMock = new Mock<IA3SClient>();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountResponse(accountId);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var ownerName = "Test Owner Name";

            var sebCsClientMock = new Mock<ISebCsClient>();

            sebCsClientMock
                .Setup(x => x.GetAccountOwner(userId, It.IsAny<string>()))
                .ReturnsAsync(new AccountOwner { Id = userId, Name = ownerName });

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

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
        public async Task GetAccountTransactions_WhenAccountTransactionsAreReceivedFromAdapi_ShouldReturnAccountTransactions()
        {
            var accountId = Guid.NewGuid().ToString();

            var a3sClientMock = new Mock<IA3SClient>();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountTransactionsResponse();

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountTransactions(It.IsAny<string>(), It.IsAny<string>(), accountId, null, null))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("Test user ID", "test-jwtAssertion", accountId, null, null);

            Assert.Equal(adapiResponse.Result!.RetrievedDateTime, accountTransactions.RetrievedDateTime);

            Assert.Equal(adapiResponse.Result.Account!.Identifications!.ResourceId, accountTransactions.Account.ResourceId);
            Assert.Equal(adapiResponse.Result.Account.Identifications.DomesticAccountNumber, accountTransactions.Account.AccountNumber);

            Assert.Equal(adapiResponse.Result.PaginatingInformation!.Paginating, accountTransactions.PaginatingInformation.Paginating);
            Assert.Equal(adapiResponse.Result.PaginatingInformation.DateFrom, accountTransactions.PaginatingInformation.DateFrom);
            Assert.Equal(adapiResponse.Result.PaginatingInformation.PaginatingKey, accountTransactions.PaginatingInformation.PaginatingKey);

            Assert.Equal(adapiResponse.Result.DepositEntryEvent!.BookingEntries![0].TransactionId, accountTransactions.Entries?[0].TransactionId);
            Assert.Equal(adapiResponse.Result.DepositEntryEvent.BookingEntries[0].ExternalId, accountTransactions.Entries?[0].ExternalId);
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

            var a3sClientMock = new Mock<IA3SClient>();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountTransactionsResponse(hasTransaction: false);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountTransactions(It.IsAny<string>(), It.IsAny<string>(), accountId, null, null))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("Test user ID", "test-jwtAssertion", accountId, null, null);

            Assert.Empty(accountTransactions.Entries);
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

            var sebCsClientMock = new Mock<ISebCsClient>();

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

            var accountTransactions = await service.GetAccountTransactions("Test user ID", "test-jwtAssertion", accountId, null, null);

            Assert.Null(accountTransactions.Entries?[0].Links);
        }

        [Fact]
        public async Task GetAccountFutureEvents_WhenAccountFutureEventsAreReceivedFromAdapi_ShouldReturnAccountFutureEvents()
        {
            var accountId = Guid.NewGuid().ToString();

            var a3sClientMock = new Mock<IA3SClient>();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountFutureEventsResponse();

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountFutureEvents(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

            var accountFutureEvents = await service.GetAccountFutureEvents("Test user ID", "test-jwtAssertion", accountId);

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

            var a3sClientMock = new Mock<IA3SClient>();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountFutureEventsResponse(hasFutureEvent: false);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountFutureEvents(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

            var accountFutureEvents = await service.GetAccountFutureEvents("Test user ID", "test-jwtAssertion", accountId);

            Assert.Empty(accountFutureEvents.FutureEvents);
        }

        [Fact]
        public async Task GetAccountFutureEvents_WhenCreditDebitIndicatorIsDbitAndReceivedAmountFromAdapiIsPositive_ShouldReturnNegativeAmount()
        {
            var accountId = Guid.NewGuid().ToString();

            var a3sClientMock = new Mock<IA3SClient>();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountFutureEventsResponse(
                creditDebitIndicator: Constants.CreditDebitIndicators.DBIT,
                amount: "10");

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountFutureEvents(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

            var accountFutureEvents = await service.GetAccountFutureEvents("Test user ID", "test-jwtAssertion", accountId);

            Assert.Equal(-10, accountFutureEvents.FutureEvents?[0].Amount);
        }

        [Fact]
        public async Task GetAccountFutureEvents_WhenCreditDebitIndicatorIsDbitAndReceivedAmountFromAdapiIsNegative_ShouldReturnNegativeAmount()
        {
            var accountId = Guid.NewGuid().ToString();

            var a3sClientMock = new Mock<IA3SClient>();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountFutureEventsResponse(
                creditDebitIndicator: Constants.CreditDebitIndicators.DBIT,
                amount: "-10");

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountFutureEvents(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

            var accountFutureEvents = await service.GetAccountFutureEvents("Test user ID", "test-jwtAssertion", accountId);

            Assert.Equal(-10, accountFutureEvents.FutureEvents?[0].Amount);
        }

        [Fact]
        public async Task GetAccountReservedAmounts_WhenAccountReservedAmountsAreReceivedFromAdapi_ShouldReturnAccountReservedAmounts()
        {
            var accountId = Guid.NewGuid().ToString();

            var a3sClientMock = new Mock<IA3SClient>();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountReservedAmountsResponse();

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountReservedAmounts(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

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
            Assert.Equal(adapiResponse.Result.AccountReservations![0].ControlId, accountTransactions.Reservations?[0].ExternalId);
        }

        [Fact]
        public async Task GetAccountReservedAmounts_WhenAccountWithoutReservedAmountsIsReceivedFromAdapi_ShouldReturnAccountWithoutReservedAmounts()
        {
            var accountId = Guid.NewGuid().ToString();

            var a3sClientMock = new Mock<IA3SClient>();

            var adapiResponse = AdapiResponseProvider.CreateGetAccountReservedAmountsResponse(hasReservation: false);

            var adapiClientMock = new Mock<IAdapiClient>();

            adapiClientMock
                .Setup(x => x.GetAccountReservedAmounts(It.IsAny<string>(), It.IsAny<string>(), accountId))
                .ReturnsAsync(adapiResponse);

            var sebCsClientMock = new Mock<ISebCsClient>();

            var service = new PrivateAccountsService(a3sClientMock.Object, adapiClientMock.Object, sebCsClientMock.Object);

            var accountTransactions = await service.GetAccountReservedAmounts("Test user ID", "test-jwtAssertion", accountId);

            Assert.Empty(accountTransactions.Reservations);
        }
    }
}
