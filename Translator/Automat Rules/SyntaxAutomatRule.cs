using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    class SyntaxAutomatRule : IAutomatRule
    {
        public int Alpha { get; set; }
        public Codes Code { get; set; }
        public int? Beta { get; set; }
        public int? Stack { get; set; }
        public string Equal { get; set; }
        public string NotEqual { get; set; } = "error";
    }
}
