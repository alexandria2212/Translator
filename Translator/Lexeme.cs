using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    public class Lexeme
    {
        public string Lexem;
        public int LineNumber;
        public Codes LexemCode;
        public string ClassNumber;

        public int Elem;
        public string Type;

        public Lexeme(string lexem, int lineNumber, Codes lexemCode, string classNumber)
        {
            Lexem = lexem;
            LineNumber = lineNumber;
            LexemCode = lexemCode;
            ClassNumber = classNumber;
        }
        public Lexeme(string lexem, int elem, int type)
        {
            Lexem = lexem;
            Elem = elem;
            switch (type)
            {
                case 0: Type = "program"; break;
                case 1: Type = "int"; break;
                case 2: Type = "label"; break;
            }
        }
    }
}
