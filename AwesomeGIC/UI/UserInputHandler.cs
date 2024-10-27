using AwesomeGIC.Application;
using AwesomeGIC.Application.Interfaces;
using AwesomeGIC.Domain.Enum;

namespace AwesomeGIC.UI
{
    internal class UserInputHandler
    {
        private readonly ITransactionService _transactionService;
        private readonly IInterestService _interestService;

        public UserInputHandler(ITransactionService transactionService, IInterestService interestService)
        {
            _transactionService = transactionService;
            _interestService = interestService;
        }

        public void Start()
        {
            Console.WriteLine(" ");
            Console.WriteLine("**********************************************************");
            Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
            bool appRunning = true;

            while (appRunning)
            {
                Console.WriteLine(" ");
                Console.WriteLine("Is there anything else you'd like to do?");
                Console.WriteLine("[T] Input transactions");
                Console.WriteLine("[I] Define interest rules");
                Console.WriteLine("[P] Print statement");
                Console.WriteLine("[Q] Quit");

                var selectedOption = Console.ReadLine();
                switch (selectedOption?.ToUpper())
                {
                    case "T":
                        HandleTransaction();
                        break;
                    case "I":
                        HandleInterestRule();
                        break;
                    case "P":
                        HandlePrintStatement();
                        break;
                    case "Q":
                        appRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid selection");
                        break;
                }
            }

            Console.WriteLine("Thank you for banking with AwesomeGIC Bank");
            Console.WriteLine("Have a nice day!");
            Console.ReadLine();
        }

        private void HandleTransaction()
        {
            Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format \r\n(or enter blank to go back to main menu):");

            var transactionInput = Console.ReadLine();

            if (ValidateTransactionInput(transactionInput, out string accountNumber, out DateTime txnDate, out TransactionType txnType, out decimal txnAmount))
            {
                try
                {
                    _transactionService.InputTransaction(accountNumber, txnAmount, txnType, txnDate);
                    var accountStatement = _transactionService.PrintTransactionsByAccount(accountNumber);
                    Console.WriteLine(" ");
                    Console.WriteLine(accountStatement);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private bool ValidateTransactionInput(string transactionInput, out string accountNumber, out DateTime txnDate, out TransactionType txnType, out decimal txnAmount)
        {
            accountNumber = null;
            txnDate = default;
            txnType = default;
            txnAmount = default;

            var parts = transactionInput.Split(' ');
            accountNumber = parts[1];

            if (parts.Length != 4)
            {
                Console.WriteLine("Invalid input. Please enter in format: <YYYYMMDD> <ACCT> <TYPE> <AMOUNT>.");
                return false;
            }

            if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out txnDate))
            {
                Console.WriteLine("Invalid date format. Use YYYYMMDD.");
                return false;
            }

            var typeInput = parts[2].ToUpper();
            if (typeInput == "D")
            {
                txnType = TransactionType.Deposit;
            }
            else if (typeInput == "W")
            {
                txnType = TransactionType.Withdrawal;
            }
            else
            {
                Console.WriteLine("Invalid transaction type. Use 'D' for Deposit or 'W' for Withdrawal.");
                return false;
            }

            if (!decimal.TryParse(parts[3], out txnAmount) || txnAmount <= 0 || decimal.Round(txnAmount, 2) != txnAmount)
            {
                Console.WriteLine("Invalid amount. Must be a positive number with up to two decimal places.");
                return false;
            }

            return true;
        }

        private void HandleInterestRule()
        {
            Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format \r\n(or enter blank to go back to main menu)");
            var interestRateInput = Console.ReadLine();

            if (ValidateInterestRuleInput(interestRateInput, out string rule, out DateTime ruleDate, out decimal interestRate))
            {
                _interestService.AddInterestRule(rule, interestRate, ruleDate);
                var interestDetails = _interestService.PrintInterestDetails();
                Console.WriteLine(" ");
                Console.WriteLine(interestDetails);
            }

            Console.WriteLine("Interest rule added.");
        }

        private bool ValidateInterestRuleInput(string interestRateInput, out string rule, out DateTime ruleDate, out decimal interestRate)
        {
            rule = null;
            ruleDate = default;
            interestRate = default;

            var parts = interestRateInput.Split(' ');
            if (parts.Length != 3)
            {
                Console.WriteLine("Invalid input. Please enter in format: <Date> <RuleId> <Rate in %>.");
                return false;
            }

            if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out ruleDate))
            {
                Console.WriteLine("Invalid date format. Use YYYYMMDD.");
                return false;
            }

            if (!decimal.TryParse(parts[2], out interestRate) || interestRate <= 0 || interestRate >= 100 || decimal.Round(interestRate, 2) != interestRate)
            {
                Console.WriteLine("Invalid amount. Must be a positive number with up to two decimal places.");
                return false;
            }

            rule = parts[1];

            return true;
        }

        private void HandlePrintStatement()
        {
            Console.WriteLine("Please enter account and month to generate the statement <Account> <Year><Month>\r\n(or enter blank to go back to main menu)");
            var printInput = Console.ReadLine();

            if (ValidatePrintInput(printInput, out string accountNumber, out int year, out int month))
            {
                var statement = _transactionService.PrintStatement(accountNumber, year, month);
                Console.WriteLine(statement);
            }
        }

        private bool ValidatePrintInput(string printInput, out string accountNumber, out int year, out int month)
        {
            year = default;
            month = default;
            var parts = printInput.Split(' ');
            accountNumber = parts[0];
            var yearMonth = parts[1];

            if (parts.Length != 2)
            {
                Console.WriteLine("Invalid input. Please enter in format: <Account> <Month>.");
                return false;
            }

            if (yearMonth.Length == 6
                && int.TryParse(yearMonth.Substring(0, 4), out year) && int.TryParse(yearMonth.Substring(4, 2), out month)
                && month >= 1 && month <= 12)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid year or month format.");
                return false;
            }
        }
    }
}
