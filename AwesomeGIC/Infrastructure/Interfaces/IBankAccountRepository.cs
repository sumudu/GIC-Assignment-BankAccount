using AwesomeGIC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC.Infrastructure.Interfaces
{
    public interface IBankAccountRepository
    {
        public BankAccount GetByAccountNumber(string accountNumber);
        public void Save(BankAccount account);
    }
}
