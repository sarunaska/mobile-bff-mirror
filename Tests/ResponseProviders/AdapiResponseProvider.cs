using AdapiClient.Endpoints.GetAccount;
using AdapiClient.Endpoints.GetAccountFutureEvents;
using AdapiClient.Endpoints.GetAccountReservedAmounts;
using AdapiClient.Endpoints.GetAccounts;
using AdapiClient.Endpoints.GetAccountTransactions;
using AdapiClient.Models;
using MobileBff;

namespace Tests.ResponseProviders
{
    public class AdapiResponseProvider
    {
        public static GetAccountTransactionsResponse CreateGetAccountTransactionsResponse(
            bool hasPaging = true,
            bool hasTransaction = true,
            bool hasLinks = true,
            string? transactionTypeCode = null,
            bool isCardTransaction = false)
        {
            return new GetAccountTransactionsResponse
            {
                Result = new GetAccountTransactionsResult
                {
                    RetrievedDateTime = DateTime.Now,
                    Account = new Account
                    {
                        Identifications = new Identifications
                        {
                            ResourceId = "Test Resource ID",
                            DomesticAccountNumber = "Test DomesticAccountNumber"
                        }
                    },
                    PaginatingInformation = hasPaging
                        ? new PaginatingInformation
                        {
                            Paginating = true,
                            DateFrom = DateTime.Now,
                            PaginatingKey = "Test PaginatingKey"
                        }
                        : new PaginatingInformation
                        {
                            Paginating = false
                        },
                    DepositEntryEvent = new DepositEntryEvent
                    {
                        BookingEntries = hasTransaction
                        ? new[]
                          {
                              new BookingEntry
                              {
                                  TransactionId = "Test Transaction ID",
                                  ValueDate = "2022-03-01",
                                  BookingDate = "2022-03-02",
                                  TransactionAmount = new TransactionAmount
                                  {
                                      Amount = "100.125",
                                      Currency = "EUR"
                                  },
                                  BookedBalance = new BookedBalance
                                  {
                                      Amount = "200.135",
                                      Currency = "SEK"
                                  },
                                  Message1 = "Test Message",
                                  CardBookingEntryDetails = new CardBookingEntryDetails
                                  {
                                      MerchantName = "Test MerchantName"
                                  },
                                  BankTransactionCode = new BankTransactionCode
                                  {
                                      EntryType = transactionTypeCode ?? (isCardTransaction ? "021" : "181")
                                  },
                                  Links = hasLinks
                                  ? new Links
                                    {
                                        TransactionDetails = new TransactionDetails
                                        {
                                            Href = "Test href"
                                        }
                                    }
                                  : null
                              }
                          }
                        : Array.Empty<BookingEntry>()
                    }
                }
            };
        }

        public static GetAccountResponse CreateGetAccountResponse(
            string accountResourceId = "Test Resource ID",
            bool hasAliases = true,
            bool hasSwissAliasName = true)
        {
            return new GetAccountResponse
            {
                Result = new GetAccountResult
                {
                    RetrievedDateTime = DateTime.Now,
                    Account = CreateAccountModel(
                        accountResourceId: accountResourceId,
                        hasAliases: hasAliases,
                        hasSwissAliasName: hasSwissAliasName)
                }
            };
        }

        public static GetAccountsResponse CreateGetAccountsResponse(
            bool hasAccounts = true,
            string? currency = null)
        {
            return new GetAccountsResponse
            {
                Result = new GetAccountsResult
                {
                    RetrievedDateTime = DateTime.Now,
                    Accounts = hasAccounts
                        ? new[]
                          {
                              CreateAccountModel(currency: currency)
                          }
                        : Array.Empty<Account>()
                }
            };
        }

        public static Account CreateAccountModel(
            string? accountResourceId = null,
            bool hasAliases = true,
            bool hasSwissAliasName = true,
            string? currency = null,
            string? productCode = null)
        {
            return new Account
            {
                Name = "Test name",
                Product = new Product
                {
                    Id = $"Test Id",
                    Name = $"Test Product Name",
                    ProductCode = productCode ?? "Test Product Code"
                },
                Currency = currency ?? "Test Currency",
                Identifications = new Identifications
                {
                    ResourceId = accountResourceId ?? $"ResourceId_{Guid.NewGuid()}",
                    DomesticAccountNumber = $"Test DomesticAccountNumber",
                    Iban = "Test IBAN",
                    Bic = "Test BIC"
                },
                Balances = new[]
                {
                    new Balance { Amount = "100.125", Type = "booked" },
                    new Balance { Amount = "200.135", Type = "available" }
                },
                Aliases = hasAliases
                    ? new[]
                      {
                          new Alias
                          {
                              Type = Constants.AliasTypes.Swish,
                              Id = "0701234567",
                              ProductId = Constants.ProductIds.SwishCorporate,
                              Name = hasSwissAliasName ? "SwishCorporate Name" : null
                          },
                          new Alias
                          {
                              Type = Constants.AliasTypes.Swish,
                              Id = "0701234568",
                              ProductId = Constants.ProductIds.SwishForMerchants,
                              Name = hasSwissAliasName ? "SwishForMerchants Name" : null
                          },
                          new Alias
                          {
                              Type = Constants.AliasTypes.BankGiro,
                              Id = "Test Bg Id"
                          }
                      }
                    : null
            };
        }

        public static GetAccountReservedAmountsResponse CreateGetAccountReservedAmountsResponse(
            bool hasReservation = true)
        {
            return new GetAccountReservedAmountsResponse
            {
                Result = new GetAccountReservedAmountsResult
                {
                    RetrievedDateTime = DateTime.Now,
                    Account = new Account
                    {
                        Identifications = new Identifications
                        {
                            ResourceId = "Test Resource ID",
                            DomesticAccountNumber = "Test DomesticAccountNumber"
                        }
                    },
                    AccountReservations = hasReservation
                    ? new[]
                        {
                            new AccountReservation
                            {
                                Origin = "Test Origin",
                                ReservationDate = "2022-03-01",
                                Amount = "100.125",
                                Currency = "SEK",
                                DescriptiveText = "Test DescriptiveText",
                                ControlId = "Test ControlId"
                            }
                        }
                    : Array.Empty<AccountReservation>()
                }
            };
        }

        public static GetAccountFutureEventsResponse CreateGetAccountFutureEventsResponse(
            bool hasFutureEvent = true,
            bool isPaymentTypeInternational = false,
            string? creditDebitIndicator = null,
            string? amount = null)
        {
            return new GetAccountFutureEventsResponse
            {
                Result = new GetAccountFutureEventsResult
                {
                    RetrievedDateTime = DateTime.Now,
                    Account = new Account
                    {
                        Identifications = new Identifications
                        {
                            ResourceId = "Test Resource ID",
                            DomesticAccountNumber = "Test DomesticAccountNumber"
                        }
                    },
                    FutureEvents = hasFutureEvent
                        ? new[]
                          {
                              new FutureEvent
                              {
                                  PaymentType = isPaymentTypeInternational ? Constants.FutureEventPaymentTypes.International : "Test payment type",
                                  PaymentDetailType = "Test payment detail type",
                                  PaymentRelatedId = "Test related ID",
                                  CreditAccount = "Test credit account",
                                  CreditorName = "Test creditor name",
                                  DebitAccount = "Test debit account",
                                  ValueDate = "2022-03-01",
                                  CreditDebitIndicator = creditDebitIndicator ?? "Test credit/debit indicator",
                                  Amount = amount ?? "100.125",
                                  Currency = "EUR",
                                  DescriptiveText = "Test descriptive text",
                                  Ocr = "Test OCR",
                                  BankPrefix = "Test bank prefix",
                                  Suti = "Test suti",
                                  IsEquivalentAmount = true,
                                  DeleteAllowed = true,
                                  ModifyAllowed = true
                              }
                          }
                        : Array.Empty<FutureEvent>()
                }
            };
        }
    }
}
