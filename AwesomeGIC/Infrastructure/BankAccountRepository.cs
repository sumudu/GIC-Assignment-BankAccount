using AwesomeGIC.Domain;
using AwesomeGIC.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AwesomeGIC.Infrastructure
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly BankAccountContext _context;

        public BankAccountRepository(BankAccountContext context)
        {
            _context = context;
        }

        public BankAccount GetByAccountNumber(string accountNumber)
        {
            return _context.BankAccounts.Include(account => account.Transactions.OrderBy(x=>x.Date)).FirstOrDefault(a => a.AccountNumber == accountNumber);
        }

        public void Save(BankAccount account)
        {
            if (_context.BankAccounts.Any(a => a.AccountNumber == account.AccountNumber))
                _context.BankAccounts.Update(account);
            else
                _context.BankAccounts.Add(account);

            _context.SaveChanges();
        }
    }
}
