using AwesomeGIC.Domain.Enum;

namespace AwesomeGIC.Application.Interfaces
{
    internal interface ITransactionService
    {
        void InputTransaction(string accountNumber, decimal amount, TransactionType type, DateTime date);
        string PrintTransactionsByAccount(string accountNumber);
        string PrintStatement(string accountNumber, int year, int month);
    }
}
