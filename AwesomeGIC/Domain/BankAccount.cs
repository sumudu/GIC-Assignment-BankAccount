using AwesomeGIC.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace AwesomeGIC.Domain
{
    public class BankAccount
    {
        [Key]
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }
        public List<Transaction> Transactions { get; private set; }

        public BankAccount(string accountNumber)
        {
            AccountNumber = accountNumber;
            Transactions = new List<Transaction>();
        }

        public void Deposit(decimal amount, DateTime date, string transactionId)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive");
            AddTransaction(TransactionType.Deposit, amount, date, transactionId);
        }

        public void Withdraw(decimal amount, DateTime date, string transactionId)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive");
            if (Balance < amount) throw new InvalidOperationException("Insufficient balance.");
            AddTransaction(TransactionType.Withdrawal, amount, date, transactionId);
        }

        private void AddTransaction(TransactionType type, decimal amount, DateTime date, string transactionId)
        {
            var transaction = new Transaction(type, amount, date, this, transactionId);
            Transactions.Add(transaction);
            Balance += (type == TransactionType.Deposit) ? amount : -amount;
        }

        public IReadOnlyList<Transaction> GetTransactions() => Transactions.AsReadOnly();
    }
}
