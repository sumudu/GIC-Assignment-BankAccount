using AwesomeGIC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC.Application.Interfaces
{
    public interface IInterestService
    {
        void AddInterestRule(string ruleId, decimal rate, DateTime effectiveDate);
        string PrintInterestDetails();
        List<InterestRule> GetAllRules();
        decimal CalculateMonthlyInterest(List<Transaction> transactions, DateTime startDate, DateTime endDate, List<InterestRule> interestRules, decimal initialBalance);
    }
}
