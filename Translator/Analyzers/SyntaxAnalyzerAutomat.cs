using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Translator.Codes;

namespace Translator
{
    class SyntaxAnalyzerAutomat : IAutomatAnalyzer
    {
        public List<string> errors { get; set; } = new List<string>();
        public Stack<int> stack = new Stack<int>();

        public List<string> Start(List<Lexeme> lexems)
        {
            int currAlpha = 1;
            errors.Clear();
            foreach (var lex in lexems)
            {
                try
                {
                    if (currAlpha == 0)
                    {
                        throw new ArgumentException($"Синтаксична помилка: оператор після end");
                    }
                    var currRules = Rules.Where(x => x.Alpha == currAlpha);
                    var currRule = currRules.FirstOrDefault(x => x.Code == lex.LexemCode);
                    while (currRule == null)
                    {
                        currAlpha = NotEqualFunc(currRules);
                        currRules = Rules.Where(x => x.Alpha == currAlpha);
                        currRule = currRules.FirstOrDefault(x => x.Code == lex.LexemCode);
                    }
                    currAlpha = EqualFunc(currRule);
                }
                catch (Exception e)
                {
                    errors.Add(String.Format($"{e.Message} у рядку {lex.LineNumber}"));
                    return errors;
                }
            }
            if (currAlpha != (int)CODE_NOTHING)
            {
                errors.Add(String.Format("Синтаксична помилка: не вистачає end"));
            }
            return errors;
        }
        public int EqualFunc(SyntaxAutomatRule currRule)
        {
            var str = currRule.Equal;
            if (str == "exit")
            {
                try
                {
                    return stack.Pop();
                }
                catch
                {
                    return 0;
                }
            }
            if (currRule.Stack != null)
            {
                stack.Push((int)currRule.Stack);
            }
            return (int)currRule.Beta;
        }
        public int NotEqualFunc(IEnumerable<SyntaxAutomatRule> currRules)
        {
            string str = currRules.First().NotEqual;
            if (str == "exit")
            {
                try
                {
                    return stack.Pop();
                }
                catch
                {
                    return 0;
                }
            }
            if (str == "error")
            {
                var listLexems = currRules.Select(x => x.Code);
                var errorMessage = string.Join(", ", listLexems);
                errorMessage = errorMessage.Replace("CODE_", "");
                errorMessage = errorMessage.ToLower();
                throw new ArgumentException($"Синтаксична помилка: не вистачає {errorMessage}");
            }

            var numbers = str.Split().Select(int.Parse).ToArray();
            var newAlpha = numbers.First();
            var newStack = numbers.Last();

            stack.Push(newStack);

            return newAlpha;
        }
        public SyntaxAutomatRule[] Rules { get; } = new[]
        {
            new SyntaxAutomatRule {Alpha = 1,  Code = CODE_PROGRAM,               Beta = 2 },
            new SyntaxAutomatRule {Alpha = 2,  Code = CODE_IDENTIFIER,            Beta = 3 },
            new SyntaxAutomatRule {Alpha = 3,  Code = CODE_VAR,                   Beta = 7,   Stack = 4 },
            new SyntaxAutomatRule {Alpha = 4,  Code = CODE_START,                 Beta = 12,  Stack = 5 },
            new SyntaxAutomatRule {Alpha = 5,  Code = CODE_SEMICOLON,             Beta = 6 },
            new SyntaxAutomatRule {Alpha = 6,  Code = CODE_END,                                           Equal = "exit", NotEqual = "12 5"},
            new SyntaxAutomatRule {Alpha = 7,  Code = CODE_INT,                   Beta = 8 },
            new SyntaxAutomatRule {Alpha = 7,  Code = CODE_LABEL,                 Beta = 8 },
            new SyntaxAutomatRule {Alpha = 8,  Code = CODE_COLON,                 Beta = 9 },
            new SyntaxAutomatRule {Alpha = 9,  Code = CODE_IDENTIFIER,            Beta = 10 },
            new SyntaxAutomatRule {Alpha = 10, Code = CODE_COMMA,                 Beta = 9 },
            new SyntaxAutomatRule {Alpha = 10, Code = CODE_SEMICOLON,             Beta = 11 },
            new SyntaxAutomatRule {Alpha = 11, Code = CODE_INT,                   Beta = 8,                               NotEqual = "exit" },
            new SyntaxAutomatRule {Alpha = 11, Code = CODE_LABEL,                 Beta = 8,                               NotEqual = "exit" },
            new SyntaxAutomatRule {Alpha = 12, Code = CODE_IDENTIFIER,            Beta = 13 },
            new SyntaxAutomatRule {Alpha = 12, Code = CODE_CIN,                   Beta = 15 },
            new SyntaxAutomatRule {Alpha = 12, Code = CODE_COUT,                  Beta = 18 },
            new SyntaxAutomatRule {Alpha = 12, Code = CODE_FOR,                   Beta = 21 },
            new SyntaxAutomatRule {Alpha = 12, Code = CODE_IF,                    Beta = 39,  Stack = 27 },
            new SyntaxAutomatRule {Alpha = 12, Code = CODE_LEFT_SQUARE_BRACKET,   Beta = 30 },
            new SyntaxAutomatRule {Alpha = 13, Code = CODE_ASSIGN,                Beta = 35,  Stack = 14 },
            new SyntaxAutomatRule {Alpha = 13, Code = CODE_COLON,                                         Equal = "exit" },
            new SyntaxAutomatRule {Alpha = 14, Code = CODE_NOTHING,                                                       NotEqual = "exit" },
            new SyntaxAutomatRule {Alpha = 15, Code = CODE_RIGHT_QUOTES,          Beta = 16 },
            new SyntaxAutomatRule {Alpha = 16, Code = CODE_IDENTIFIER,            Beta = 17 },
            new SyntaxAutomatRule {Alpha = 17, Code = CODE_RIGHT_QUOTES,          Beta = 16,                              NotEqual = "exit" },
            new SyntaxAutomatRule {Alpha = 18, Code = CODE_LEFT_QUOTES,           Beta = 19 },
            new SyntaxAutomatRule {Alpha = 19, Code = CODE_IDENTIFIER,            Beta = 20 },
            new SyntaxAutomatRule {Alpha = 20, Code = CODE_LEFT_QUOTES,           Beta = 19,                              NotEqual = "exit" },
            new SyntaxAutomatRule {Alpha = 21, Code = CODE_IDENTIFIER,            Beta = 22 },
            new SyntaxAutomatRule {Alpha = 22, Code = CODE_ASSIGN,                Beta = 35,  Stack = 23 },
            new SyntaxAutomatRule {Alpha = 23, Code = CODE_BY,                    Beta = 35,  Stack = 24 },
            new SyntaxAutomatRule {Alpha = 24, Code = CODE_WHILE,                 Beta = 39,  Stack = 25 },
            new SyntaxAutomatRule {Alpha = 25, Code = CODE_DO,                    Beta = 12,  Stack = 26 },
            new SyntaxAutomatRule {Alpha = 26, Code = CODE_NOTHING,                                                       NotEqual = "exit" },
            new SyntaxAutomatRule {Alpha = 27, Code = CODE_THEN,                  Beta = 28 },
            new SyntaxAutomatRule {Alpha = 28, Code = CODE_GOTO,                  Beta = 29 },
            new SyntaxAutomatRule {Alpha = 29, Code = CODE_IDENTIFIER,                                    Equal = "exit" },
            new SyntaxAutomatRule {Alpha = 30, Code = CODE_IDENTIFIER,            Beta = 31 },
            new SyntaxAutomatRule {Alpha = 31, Code = CODE_ASSIGN,                Beta = 39,  Stack = 32 },
            new SyntaxAutomatRule {Alpha = 32, Code = CODE_QUESTION,              Beta = 35,  Stack = 33 },
            new SyntaxAutomatRule {Alpha = 33, Code = CODE_COLON,                 Beta = 35,  Stack = 34 },
            new SyntaxAutomatRule {Alpha = 34, Code = CODE_RIGHT_SQUARE_BRACKET,                          Equal = "exit" },
            new SyntaxAutomatRule {Alpha = 35, Code = CODE_MINUS,                 Beta = 36 },
            new SyntaxAutomatRule {Alpha = 35, Code = CODE_IDENTIFIER,            Beta = 37 },
            new SyntaxAutomatRule {Alpha = 35, Code = CODE_CONST,                 Beta = 37 },
            new SyntaxAutomatRule {Alpha = 35, Code = CODE_LEFT_BRACKET,          Beta = 35,  Stack = 38 },
            new SyntaxAutomatRule {Alpha = 36, Code = CODE_IDENTIFIER,            Beta = 37 },
            new SyntaxAutomatRule {Alpha = 36, Code = CODE_CONST,                 Beta = 37 },
            new SyntaxAutomatRule {Alpha = 36, Code = CODE_LEFT_BRACKET,          Beta = 35,  Stack = 38 },
            new SyntaxAutomatRule {Alpha = 37, Code = CODE_PLUS,                  Beta = 36,                              NotEqual = "exit" },
            new SyntaxAutomatRule {Alpha = 37, Code = CODE_MINUS,                 Beta = 36,                              NotEqual = "exit" },
            new SyntaxAutomatRule {Alpha = 37, Code = CODE_MULTIPLY,              Beta = 36,                              NotEqual = "exit" },
            new SyntaxAutomatRule {Alpha = 37, Code = CODE_DIVISION,              Beta = 36,                              NotEqual = "exit" },
            new SyntaxAutomatRule {Alpha = 37, Code = CODE_POWER,                 Beta = 36,                              NotEqual = "exit" },
            new SyntaxAutomatRule {Alpha = 38, Code = CODE_RIGHT_BRACKET,         Beta = 37 },
            new SyntaxAutomatRule {Alpha = 39, Code = CODE_NOTHING,                                                       NotEqual = "35 40" },
            new SyntaxAutomatRule {Alpha = 40, Code = CODE_LESS_THAN,             Beta = 35,  Stack = 41 },
            new SyntaxAutomatRule {Alpha = 40, Code = CODE_LESS_THAN_EQUAL,       Beta = 35,  Stack = 41 },
            new SyntaxAutomatRule {Alpha = 40, Code = CODE_MORE_THAN,             Beta = 35,  Stack = 41 },
            new SyntaxAutomatRule {Alpha = 40, Code = CODE_MORE_THAN_EQUAL,       Beta = 35,  Stack = 41 },
            new SyntaxAutomatRule {Alpha = 40, Code = CODE_EQUAL,                 Beta = 35,  Stack = 41 },
            new SyntaxAutomatRule {Alpha = 40, Code = CODE_NOT_EQUAL,             Beta = 35,  Stack = 41 },
            new SyntaxAutomatRule {Alpha = 41, Code = CODE_NOTHING,                                                       NotEqual = "exit" }
        };
    }
}
