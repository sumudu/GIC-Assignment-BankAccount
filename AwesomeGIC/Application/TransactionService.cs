using AwesomeGIC.Application.Interfaces;
using AwesomeGIC.Domain;
using AwesomeGIC.Domain.Enum;
using AwesomeGIC.Infrastructure.Interfaces;
using System.Text;

namespace AwesomeGIC.Application
{
    public class TransactionService : ITransactionService
    {
        private readonly IBankAccountRepository _iBankAccountRepository;
        private readonly IInterestService _interestService;
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(IBankAccountRepository bankAccountRepository, IInterestService interestService, ITransactionRepository transactionRepository)
        {
            _iBankAccountRepository = bankAccountRepository;
            _interestService = interestService;
            _transactionRepository = transactionRepository;
        }

        public void InputTransaction(string accountNumber, decimal amount, TransactionType type, DateTime date)
        {
            var account = _iBankAccountRepository.GetByAccountNumber(accountNumber) ?? new BankAccount(accountNumber);
            var transactionId = GenerateTransactionId(date);

            if (type == TransactionType.Deposit)
                account.Deposit(amount, date, transactionId);
            else
                account.Withdraw(amount, date, transactionId);

            _iBankAccountRepository.Save(account);
        }

        public string GenerateTransactionId(DateTime date)
        {
            var sameDateTransactions = _transactionRepository.GetAllTransactions().Where(t => t.Date.Date == date.Date);
            int transactionCount = sameDateTransactions.Count() + 1;
            string transactionId = $"{date:yyyyMMdd}-{transactionCount:D2}";
            return transactionId;
        }

        public string PrintTransactionsByAccount(string accountNumber)
        {
            var account = _iBankAccountRepository.GetByAccountNumber(accountNumber);
            if (account == null)
                throw new InvalidOperationException("Account not found.");

            var statementBuilder = new StringBuilder();
            statementBuilder.AppendLine($"Account: {accountNumber}");
            statementBuilder.AppendLine("| Date     | Txn Id      | Type | Amount   |");

            foreach (var transaction in account.Transactions)
            {
                statementBuilder.AppendLine(string.Format("| {0,-8:yyyyMMdd} | {1,-11} | {2,-4} | {3,8:F2} |",
                 transaction.Date, transaction.TransactionId, (transaction.Type == TransactionType.Deposit ? "D" : "W"), transaction.Amount));
            }

            return statementBuilder.ToString();
        }

        public string PrintStatement(string accountNumber, int year, int month)
        {
            var account = _iBankAccountRepository.GetByAccountNumber(accountNumber);
            if (account == null)
                throw new InvalidOperationException("Account not found.");

            var transactionsBefore = account.Transactions
                  .Where(t => t.Date.Year <= year && t.Date.Month < month)
                .OrderBy(t => t.Date)
                .ToList();

            decimal initialBalance = 0m;
            foreach (var transaction in transactionsBefore)
            {
                initialBalance += (transaction.Type == TransactionType.Deposit ? transaction.Amount : -transaction.Amount);
            }

            var transactionsToCalculate = account.Transactions
              .Where(t => t.Date.Year == year && t.Date.Month == month)
            .OrderBy(t => t.Date)
            .ToList();

            var interestRules = _interestService.GetAllRules().Where(x => x.IsActive).ToList();
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var interest = _interestService.CalculateMonthlyInterest(transactionsToCalculate, startDate, endDate, interestRules, initialBalance);

            var statementBuilder = new StringBuilder();
            statementBuilder.AppendLine($"Account: {accountNumber}");
            statementBuilder.AppendLine("| Date     | Txn Id      | Type |  Amount  |  Balance |");

            decimal runningBalance = initialBalance;
            foreach (var transaction in transactionsToCalculate)
            {
                runningBalance += (transaction.Type == TransactionType.Deposit ? transaction.Amount : -transaction.Amount);
                statementBuilder.AppendLine(string.Format("| {0,-8:yyyyMMdd} | {1,-11} | {2,-4} | {3,8:F2} | {4,8:F2} |",
                      transaction.Date, transaction.TransactionId, (transaction.Type == TransactionType.Deposit ? "D" : "W"), transaction.Amount, runningBalance));
            }

            runningBalance += interest;
            statementBuilder.AppendLine(string.Format("| {0,-8:yyyyMMdd} | {1,-11} | {2,-4} | {3,8:F2} | {4,8:F2} |",
                 endDate, "", "I", interest, runningBalance));

            return statementBuilder.ToString();
        }

    }
}
