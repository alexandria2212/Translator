using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    interface IAutomatRule
    {
        int Alpha { get; }
        int? Beta { get; }
        string Equal { get; }
        string NotEqual { get; }
    }
}
