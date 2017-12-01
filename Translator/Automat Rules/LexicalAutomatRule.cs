using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    class LexicalAutomatRule : IAutomatRule
    {
        public int Alpha { get; set; }
        public string Label { get; set; }
        public int? Beta { get; set; }
        public string Equal { get; set; } 
        public string NotEqual { get; set; } = "error";
    }
}
