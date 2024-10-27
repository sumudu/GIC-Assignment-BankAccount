using AwesomeGIC.Domain;

namespace AwesomeGIC.Infrastructure.Interfaces
{
    public interface ITransactionRepository
    {
        List<Transaction> GetAllTransactions();
    }
}
