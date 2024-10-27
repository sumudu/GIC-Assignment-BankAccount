using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC.Domain.Enum
{
    public enum TransactionType
    {
        [Description("D")]
        Deposit,
        [Description("W")]
        Withdrawal
    }
}
