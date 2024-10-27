using AwesomeGIC.Application.Interfaces;
using AwesomeGIC.Domain;
using AwesomeGIC.Domain.Enum;
using AwesomeGIC.Infrastructure.Interfaces;
using System.Text;


namespace AwesomeGIC.Application
{
    public class InterestService : IInterestService
    {
        private readonly IInterestRuleRepository _interestRuleRepository;
        private readonly IBankAccountRepository _bankAccountRepository;

        public InterestService(IInterestRuleRepository interestRuleRepository, IBankAccountRepository bankAccountRepository)
        {
            _interestRuleRepository = interestRuleRepository;
            _bankAccountRepository = bankAccountRepository;
        }

        public void AddInterestRule(string ruleId, decimal rate, DateTime effectiveDate)
        {
            var rule = new InterestRule(ruleId, effectiveDate, rate);
            _interestRuleRepository.Save(rule);
        }

        public string PrintInterestDetails()
        {
            var interestRules = _interestRuleRepository.GetAllRules().Where(x => x.IsActive).OrderBy(y => y.EffectiveDate);
            if (interestRules == null)
                throw new InvalidOperationException("No Interest details found.");

            var ruleBuilder = new StringBuilder();
            ruleBuilder.AppendLine($"Interest rules:");
            ruleBuilder.AppendLine("| Date     | RuleId | Rate(%) |");

            foreach (var rule in interestRules)
            {
                ruleBuilder.AppendLine(string.Format("| {0,-8:yyyyMMdd} | {1,-6} | {2,7:F2} |",
                rule.EffectiveDate, rule.RuleId, rule.Rate));
            }

            return ruleBuilder.ToString();
        }
        public List<InterestRule> GetAllRules()
        {
            var rules = _interestRuleRepository.GetAllRules();
            return rules;
        }
        public decimal CalculateMonthlyInterest(List<Transaction> transactions, DateTime startDate, DateTime endDate, List<InterestRule> interestRules, decimal initialBalance)
        {
            decimal totalInterest = 0m;

            var orderedTransactions = transactions.OrderBy(t => t.Date).ToList();

            foreach (var rule in interestRules.OrderBy(r => r.EffectiveDate))
            {
                var periodStartDate = startDate > rule.EffectiveDate ? startDate : rule.EffectiveDate;
                var periodEndDate = interestRules.SkipWhile(r => r != rule).Skip(1).FirstOrDefault()?.EffectiveDate.AddDays(-1) ?? endDate;

                if (periodEndDate < startDate || periodStartDate > endDate)
                    continue;

                for (var date = periodStartDate; date <= periodEndDate; date = date.AddDays(1))
                {
                    var dailyTransactions = orderedTransactions.Where(t => t.Date == date);
                    foreach (var txn in dailyTransactions)
                    {
                        initialBalance += (txn.Type == TransactionType.Deposit ? txn.Amount : -txn.Amount);
                    }

                    totalInterest += (initialBalance * (rule.Rate / 100)) / 365;
                }
            }

            return Math.Round(totalInterest, 2);
        }
    }

}
