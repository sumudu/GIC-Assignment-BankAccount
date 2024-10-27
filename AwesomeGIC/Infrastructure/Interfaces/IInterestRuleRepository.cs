using AwesomeGIC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC.Infrastructure.Interfaces
{
    public interface IInterestRuleRepository
    {
        public InterestRule GetLatestRule(DateTime date);
        public void Save(InterestRule rule);
        public List<InterestRule> GetAllRules();
    }
}
