using System.ComponentModel.DataAnnotations;

namespace AwesomeGIC.Domain
{
    public class InterestRule
    {
        [Key]
        public string RuleId { get; private set; }
        public DateTime EffectiveDate { get; private set; }
        public decimal Rate { get; private set; }
        public bool IsActive { get; set; } = true;

        public InterestRule(string ruleId, DateTime effectiveDate, decimal rate)
        {
            if (rate <= 0 || rate >= 100)
                throw new ArgumentException("Interest rate must be between 0 and 100.");

            RuleId = ruleId;
            EffectiveDate = effectiveDate;
            Rate = rate;
        }
    }
}
