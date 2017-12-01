using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    class LexemsTable
    {
        public static Dictionary<string, int> Table = new Dictionary<string, int>()
        {
            #region lexems
            ["program"] = 1,
            ["var"] = 2,
            ["start"] = 3,
            ["end"] = 4,
            ["int"] = 5,
            ["label"] = 6,
            ["cin"] = 7,
            ["cout"] = 8,
            ["for"] = 9,
            ["by"] = 10,
            ["while"] = 11,
            ["do"] = 12,
            ["if"] = 13,
            ["then"] = 14,
            ["goto"] = 15,
            [";"] = 16,
            [":"] = 17,
            [","] = 18,
            ["="] = 19,
            [">>"] = 20,
            ["<<"] = 21,
            ["+"] = 22,
            ["-"] = 23,
            ["*"] = 24,
            ["/"] = 25,
            ["^"] = 26,
            ["("] = 27,
            [")"] = 28,
            ["<"] = 29,
            ["<="] = 30,
            [">"] = 31,
            [">="] = 32,
            ["=="] = 33,
            ["!="] = 34,
            ["?"] = 35,
            ["["] = 36,
            ["]"] = 37
            #endregion
        };
        
    }
}
