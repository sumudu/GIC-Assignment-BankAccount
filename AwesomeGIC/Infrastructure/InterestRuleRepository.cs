using AwesomeGIC.Domain;
using AwesomeGIC.Infrastructure.Interfaces;

namespace AwesomeGIC.Infrastructure
{
    public class InterestRuleRepository : IInterestRuleRepository
    {
        private readonly BankAccountContext _context;

        public InterestRuleRepository(BankAccountContext context)
        {
            _context = context;
        }

        public InterestRule GetLatestRule(DateTime date)
        {
            return _context.InterestRules.Where(r => r.EffectiveDate <= date)
                                         .OrderByDescending(r => r.EffectiveDate)
                                         .FirstOrDefault();
        }

        public List<InterestRule> GetAllRules()
        {
            return _context.InterestRules.ToList();
        }

        public void Save(InterestRule rule)
        {
            var existingRules = _context.InterestRules
                                     .Where(r => r.EffectiveDate.Date == rule.EffectiveDate.Date && r.IsActive)
                                     .ToList();

            foreach (var existingRule in existingRules)
            {
                existingRule.IsActive = false;
                _context.InterestRules.Update(existingRule);

            }
            _context.InterestRules.Add(rule);
            _context.SaveChanges();
        }
    }

}
