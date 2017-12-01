using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    class LexicalAnalyzer : IAutomatAnalyzer
    {
        public List<string> errors { get; set; } = new List<string>();

        public List<Lexeme> output = new List<Lexeme>();
        public List<Lexeme> Identifiers = new List<Lexeme>();
        public Dictionary<string, int> Constants = new Dictionary<string, int>();
        public bool modeDeclaration = true;

        public int typeCode = 0;
        public Dictionary<string, int> DataTypes = new Dictionary<string, int>()
        {
            ["program"] = 0,
            ["int"] = 1,
            ["label"] = 2
        };
        
        public string lex = "";
        public int LineNumber = 1;

        public LexicalAutomatRule[] Rules { get; } = new[]
        {
            new LexicalAutomatRule {Alpha = 1,  Label = "abcdefghijklmnopqrstuvwxyz",  Beta = 2 },
            new LexicalAutomatRule {Alpha = 1,  Label = "0123456789",                  Beta = 3 },
            new LexicalAutomatRule {Alpha = 1,  Label = ";:,+-*/^()[]?",               Beta = 1 , Equal = "exit"},
            new LexicalAutomatRule {Alpha = 1,  Label = "=",                           Beta = 4 },
            new LexicalAutomatRule {Alpha = 1,  Label = ">",                           Beta = 5 },
            new LexicalAutomatRule {Alpha = 1,  Label = "<",                           Beta = 6 },
            new LexicalAutomatRule {Alpha = 1,  Label = "!",                           Beta = 7 },
            new LexicalAutomatRule {Alpha = 2,  Label = "abcdefghijklmnopqrstuvwxyz",  Beta = 2,                    NotEqual = "exit" },
            new LexicalAutomatRule {Alpha = 2,  Label = "0123456789",                  Beta = 3,                    NotEqual = "exit" },
            new LexicalAutomatRule {Alpha = 3,  Label = "0123456789",                  Beta = 3,                    NotEqual = "exit" },
            new LexicalAutomatRule {Alpha = 4,  Label = "=",                                       Equal = "exit",  NotEqual = "exit" },
            new LexicalAutomatRule {Alpha = 5,  Label = "=",                                       Equal = "exit",  NotEqual = "exit" },
            new LexicalAutomatRule {Alpha = 5,  Label = ">",                                       Equal = "exit",  NotEqual = "exit" },
            new LexicalAutomatRule {Alpha = 6,  Label = "=",                                       Equal = "exit",  NotEqual = "exit" },
            new LexicalAutomatRule {Alpha = 6,  Label = "<",                                       Equal = "exit",  NotEqual = "exit" },
            new LexicalAutomatRule {Alpha = 7,  Label = "=",                                       Equal = "exit" },
        };
        public List<string> Start(string sourceCode)
        {
            errors.Clear();
           
            LineNumber = 1;
            lex = "";

            if (!sourceCode.Contains("start"))
            {
                errors.Add(String.Format("Синтаксична помилка: не вистачає 'start' у рядку {1}", lex, LineNumber));
            }

            int currAlpha = 1;

            foreach (char ch in sourceCode)
            {
                try
                {
                    var currRules = Rules.Where(x => x.Alpha == currAlpha);
                    var currRule = currRules.FirstOrDefault(x => x.Label.Contains(ch));

                    if (currRule == null)
                    {
                        if (ch == '\n')
                        {
                            if (lex != "")
                                AddLex(lex, LineNumber);
                            lex = "";
                            currAlpha = 1;
                            LineNumber++;
                            continue;
                        }
                        if (ch == ' ' || ch == '\t')
                        {
                            if (lex != "")
                                AddLex(lex, LineNumber);
                            lex = "";
                            currAlpha = 1;
                            continue;
                        }

                        currAlpha = NotEqualFunc(currRules);
                        currRules = Rules.Where(x => x.Alpha == currAlpha);
                        currRule = currRules.FirstOrDefault(x => x.Label.Contains(ch));
                    }
                    lex += ch;
                    currAlpha = EqualFunc(currRule);
                }
                catch (Exception e)
                {
                    errors.Add(String.Format($"{e.Message} у рядку {LineNumber}"));
                    return errors;
                }
            }
            if (lex != "")
                AddLex(lex, LineNumber);

            return errors;
        }
        public int EqualFunc(LexicalAutomatRule currRule)
        {
            var str = currRule.Equal;
            if (str == "exit")
            {
                if (lex != "")
                    AddLex(lex, LineNumber);
                lex = "";
                return 1;
            }
            return (int)currRule.Beta;
        }
        public int NotEqualFunc(IEnumerable<LexicalAutomatRule> currRules)
        {
            string str = currRules.First().NotEqual;
            if (str == "exit")
            {
                if (lex != "")
                    AddLex(lex, LineNumber);
                lex = "";
                return 1;
            }
            else
            {
                throw new ArgumentException($"Лексична помилка \'{lex}\'");
            }
        }
        public void AddLex(string lexeme, int lineNumber)
        {
            string ClassNumber;
            int number;
            int LexemCode;
            if (lexeme == "start")
                modeDeclaration = false;

            if (DataTypes.ContainsKey(lexeme))
                DataTypes.TryGetValue(lexeme, out typeCode);

            try
            {
                if (LexemsTable.Table.TryGetValue(lexeme, out LexemCode))
                {
                    output.Add(new Lexeme(lexeme, lineNumber, (Codes)LexemCode, ""));
                }
                else
                {
                    if (Char.IsLetter(lexeme[0]))
                    {
                        LexemCode = 38;
                        if (modeDeclaration)
                        {
                            if (IdentifiersContainsLexeme(lexeme, out number))
                            {
                                throw new ArgumentException($"Повторне оголошення ідентифікатора \'{lexeme}\'");
                            }
                            else
                            {
                                Identifiers.Add(new Lexeme(lexeme, Identifiers.Count + 1, typeCode));
                                ClassNumber = Identifiers.Count.ToString();
                                output.Add(new Lexeme(lexeme, lineNumber, (Codes)LexemCode, ClassNumber));
                            }
                        }
                        else if (IdentifiersContainsLexeme(lexeme, out number))
                        {
                            ClassNumber = number.ToString();
                            output.Add(new Lexeme(lexeme, lineNumber, (Codes)LexemCode, ClassNumber));
                        }
                        else
                        {
                            throw new ArgumentException($"Неоголошений ідентифікатор \'{lexeme}\'");
                        }
                    }
                    else if (Char.IsNumber(lexeme[0]))
                    {
                        LexemCode = 39;
                        if (Constants.ContainsKey(lexeme))
                            Constants.TryGetValue(lexeme, out number);
                        else
                            Constants.Add(lexeme, Constants.Count + 1);

                        Constants.TryGetValue(lexeme, out number);
                        ClassNumber = number.ToString();
                        output.Add(new Lexeme(lexeme, lineNumber, (Codes)LexemCode, ClassNumber));
                    }
                    else
                    {
                        throw new ArgumentException($"Лексична помилка \'{lexeme}\'");
                    }
                }
            }
            catch (Exception e)
            {
                errors.Add(String.Format($"{e.Message} у рядку {lineNumber}"));
            }
        }
        public bool IdentifiersContainsLexeme(string lex, out int i)
        {
            i = 0;
            foreach (Lexeme id in Identifiers)
            {
                i++;
                if (id.Lexem == lex)
                    return true;
            }
            i = 0;
            return false;
        }
    }
}
