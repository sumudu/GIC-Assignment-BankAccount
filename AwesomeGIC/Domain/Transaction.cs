using AwesomeGIC.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC.Domain
{
    public class Transaction
    {
        public string TransactionId { get; private set; }
        public TransactionType Type { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }
        public BankAccount Account { get; private set; }

        private Transaction() { }
        public Transaction(TransactionType type, decimal amount, DateTime date, BankAccount account, string transactionId)
        {
            Type = type;
            Amount = amount;
            Date = date;
            Account = account;
            TransactionId = transactionId;
        }


    }

}
