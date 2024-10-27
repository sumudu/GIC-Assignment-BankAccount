using AwesomeGIC.Domain;
using AwesomeGIC.Infrastructure.Interfaces;

namespace AwesomeGIC.Infrastructure
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankAccountContext _context;

        public TransactionRepository(BankAccountContext context)
        {
            _context = context;
        }

        public List<Transaction> GetAllTransactions()
        {
            return _context.Transactions.ToList();
        }
    }
}
