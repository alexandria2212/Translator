using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Translator.Codes;

namespace Translator
{
    class SyntaxAnalyzer
    {
        public bool found;
        public List<Lexeme> lexems;
        public int i = 0;
        public List<string> errors = new List<string>();
        
        public List<string> Main(List<Lexeme> Lexems)
        {
            lexems = Lexems;
            errors.Clear();
            found = false;
            try
            {
                if (lexems[i++].LexemCode == CODE_PROGRAM)
                    if (lexems[i++].LexemCode == CODE_IDENTIFIER)
                        if (lexems[i++].LexemCode == CODE_VAR)
                        {
                            if (VarList())
                                if (lexems[i++].LexemCode == CODE_START)
                                    if (OperList())
                                        if (lexems[i++].LexemCode == CODE_END)
                                            if (lexems.Count == i)
                                                return errors;
                                            else throw new ArgumentException($"Синтаксична помилка: лексеми після 'end'");
                                        else throw new ArgumentException($"Синтаксична помилка: не вистачає 'end'");
                        }
                        else throw new ArgumentException($"Синтаксична помилка: не вистачає 'var'");
                    else throw new ArgumentException($"Синтаксична помилка: невірне ім'я програми");
                else throw new ArgumentException($"Синтаксична помилка: не вистачає 'program'");
            }
            catch (ArgumentOutOfRangeException e)
            {
                errors.Add("Синтаксична помилка: не вистачає 'end'");
            }
            catch (Exception e)
            {
                errors.Add(String.Format($"{e.Message} у рядку {lexems[--i].LineNumber}"));
            }
            return errors;
        }
        public bool VarList()
        {
            found = false;
            if (Var())
            {
                i--;
                if (lexems[i++].LexemCode == CODE_SEMICOLON)
                {
                    found = true;
                    while (lexems[i++].LexemCode != CODE_START && found == true)
                    {
                        i--;
                        if (Var())
                        {
                            i--;
                            if (lexems[i++].LexemCode == CODE_SEMICOLON)
                                found = true;
                            else found = false;
                        }
                        else
                        {
                            found = false;

                        }
                    }
                }
                else found = false;
            }
            else found = false;
            i--;
            return found;
        }
        public bool Var()
        {
            found = false;
            var lex = lexems[i++].LexemCode;
            if (lex == CODE_INT || lex == CODE_LABEL)
                if (lexems[i++].LexemCode == CODE_COLON)
                    if (IdList())
                        found = true;
                    else
                    {
                        found = false;
                    }
                else
                {
                    found = false;
                    throw new ArgumentException($"Синтаксична помилка: не вистачає ':'");
                }
            else
            {
                found = false;
                throw new ArgumentException($"Синтаксична помилка: невірний тип ідентифікатора");
            }

            return found;
        }
        public bool IdList()
        {
            found = false;
            if (lexems[i++].LexemCode == CODE_IDENTIFIER)
            {
                found = true;
                while (lexems[i++].LexemCode == CODE_COMMA && found == true)
                {
                    if (lexems[i++].LexemCode == CODE_IDENTIFIER && found == true)
                        found = true;
                    else
                    {
                        found = false;
                        throw new ArgumentException($"Синтаксична помилка: потрібний ідентифікатор");
                    }
                }
                if (lexems[--i].LexemCode != CODE_SEMICOLON && found == true)
                    throw new ArgumentException($"Синтаксична помилка: потрібно ','");
                i++;
            }
            else throw new ArgumentException($"Синтаксична помилка: потрібний ідентифікатор");
            return found;
        }
        public bool OperList()
        {
            found = false;
            if (Operator())
            {
                if (lexems[i++].LexemCode == CODE_SEMICOLON)
                {
                    found = true;
                    while (lexems[i++].LexemCode != CODE_END && found == true)
                    {
                        i--;
                        if (Operator())
                            if (lexems[i++].LexemCode == CODE_SEMICOLON)
                                found = true;
                            else
                            {
                                throw new ArgumentException($"Синтаксична помилка: невірний оператор");
                            }
                        else found = false;
                    }
                    i--;
                }
                else
                {
                    throw new ArgumentException($"Синтаксична помилка: невірний оператор");
                }
            }
            else found = false;
            return found;
        }
        public bool Operator()
        {
            found = false;

            if (lexems[i++].LexemCode == CODE_IDENTIFIER)
            {
                if (lexems[i++].LexemCode == CODE_ASSIGN)
                    if (Vuraz())
                        return true;
                    else return false;
                else
                {
                    i--;
                    if (lexems[i++].LexemCode == CODE_COLON)
                        return true;
                    else
                    {
                        throw new ArgumentException($"Синтаксична помилка: невірний синтаксис присвоювання");
                    }
                }
            }
            else i--;

            if (lexems[i++].LexemCode == CODE_CIN)
            {
                if (lexems[i++].LexemCode == CODE_RIGHT_QUOTES)
                    if (lexems[i++].LexemCode == CODE_IDENTIFIER)
                    {
                        found = true;
                        while (lexems[i++].LexemCode == CODE_RIGHT_QUOTES && found == true)
                        {
                            if (lexems[i++].LexemCode == CODE_IDENTIFIER)
                                found = true;
                            else
                            {
                                throw new ArgumentException($"Синтаксична помилка: потрібен ідентифікатор");
                            }
                        }
                        i--;
                        return found;
                    }
                    else
                    {
                        throw new ArgumentException($"Синтаксична помилка: потрібен ідентифікатор");
                    }
                else
                {
                    throw new ArgumentException($"Синтаксична помилка: не вистачає '>>'");
                }
            }
            else i--;

            if (lexems[i++].LexemCode == CODE_COUT)
            {
                if (lexems[i++].LexemCode == CODE_LEFT_QUOTES)
                    if (lexems[i++].LexemCode == CODE_IDENTIFIER)
                    {
                        found = true;
                        while (lexems[i++].LexemCode == CODE_LEFT_QUOTES && found == true)
                        {
                            if (lexems[i++].LexemCode == CODE_IDENTIFIER)
                                found = true;
                            else
                            {
                                throw new ArgumentException($"Синтаксична помилка: потрібен ідентифікатор");
                            }
                        }
                        i--;
                        return found;
                    }
                    else
                    {
                        throw new ArgumentException($"Синтаксична помилка: потрібен ідентифікатор");
                    }
                else
                {
                    throw new ArgumentException($"Синтаксична помилка: не вистачає '<<'");
                }
            }
            else i--;

            if (lexems[i++].LexemCode == CODE_FOR)
            {
                if (lexems[i++].LexemCode == CODE_IDENTIFIER)
                    if (lexems[i++].LexemCode == CODE_ASSIGN)
                    {
                        if (Vuraz())
                            if (lexems[i++].LexemCode == CODE_BY)
                            {
                                if (Vuraz())
                                    if (lexems[i++].LexemCode == CODE_WHILE)
                                    {
                                        if (Vidn())
                                            if (lexems[i++].LexemCode == CODE_DO)
                                            {
                                                if (Operator())
                                                    return true;
                                            }
                                            else
                                            {
                                                throw new ArgumentException($"Синтаксична помилка: не вистачає 'do'");
                                            }
                                    }
                                    else
                                    {
                                        throw new ArgumentException($"Синтаксична помилка: не вистачає 'while'");
                                    }
                            }
                            else
                            {
                                throw new ArgumentException($"Синтаксична помилка: не вистачає 'by'");
                            }
                    }
                    else
                    {
                        throw new ArgumentException($"Синтаксична помилка: не вистачає '='");
                    }
                else
                {
                    throw new ArgumentException($"Синтаксична помилка: не вистачає ідентифікатора");
                }
                return false;
            }
            else i--;

            if (lexems[i++].LexemCode == CODE_IF)
            {
                if (Vidn())
                {
                    if (lexems[i++].LexemCode == CODE_THEN)
                        if (lexems[i++].LexemCode == CODE_GOTO)
                            if (lexems[i++].LexemCode == CODE_IDENTIFIER)
                                return true;
                            else throw new ArgumentException($"Синтаксична помилка: не вистачає мітки");
                        else throw new ArgumentException($"Синтаксична помилка: не вистачає 'goto'");
                    else throw new ArgumentException($"Синтаксична помилка: не вистачає 'then'");
                }
                else return false;
            }
            else i--;

            if (lexems[i++].LexemCode == CODE_LEFT_SQUARE_BRACKET)
            {
                if (lexems[i++].LexemCode == CODE_IDENTIFIER)
                    if (lexems[i++].LexemCode == CODE_ASSIGN)
                    {
                        if (Vidn())
                            if (lexems[i++].LexemCode == CODE_QUESTION)
                            {
                                if (Vuraz())
                                    if (lexems[i++].LexemCode == CODE_COLON)
                                    {
                                        if (Vuraz())
                                            if (lexems[i++].LexemCode == CODE_RIGHT_SQUARE_BRACKET) // ]
                                                return true;
                                            else throw new ArgumentException($"Синтаксична помилка: не вистачає ']'");
                                    }
                                    else throw new ArgumentException($"Синтаксична помилка: не вистачає ':'");
                            }
                            else throw new ArgumentException($"Синтаксична помилка: не вистачає '?'");
                    }
                    else throw new ArgumentException($"Синтаксична помилка: не вистачає '='");
                else throw new ArgumentException($"Синтаксична помилка: не вистачає ідентифікатора");
                return false;
            }
            else i--;
            throw new ArgumentException($"Синтаксична помилка: невірний оператор");
        }
        public bool Vuraz()
        {
            found = false;
            if (lexems[i++].LexemCode != CODE_MINUS)
                i--;
            if (Term())
            {
                found = true;
                var lex = lexems[i++].LexemCode;
                while ((lex == CODE_PLUS || lex == CODE_MINUS) && found == true)
                {
                    if (Term())
                        found = true;
                    else found = false;
                    lex = lexems[i++].LexemCode;
                }
                i--;
            }
            if (!found)
            {
                throw new ArgumentException($"Синтаксична помилка: невірний синтаксис виразу");
            }
            return found;
        }
        public bool Term()
        {
            found = false;
            if (Mnoz())
            {
                found = true;
                var lex = lexems[i++].LexemCode;
                while ((lex == CODE_MULTIPLY || lex == CODE_DIVISION) && found == true)
                {
                    if (Mnoz())
                        found = true;
                    else found = false;
                    lex = lexems[i++].LexemCode;
                }
                i--;
            }
            return found;
        }
        public bool Mnoz()
        {
            found = false;
            if (PervVuraz())
            {
                found = true;
                while (lexems[i++].LexemCode == CODE_POWER && found == true)
                {
                    if (PervVuraz())
                        found = true;
                    else found = false;
                }
                i--;
            }
            return found;
        }
        public bool PervVuraz()
        {
            found = false;
            if (lexems[i++].LexemCode == CODE_LEFT_BRACKET)
                if (Vuraz())
                    if (lexems[i++].LexemCode == CODE_RIGHT_BRACKET)
                        found = true;
                    else
                    {
                        throw new ArgumentException($"Синтаксична помилка: не вистачає ')'");
                    }
                else found = false;
            else i--;

            if (lexems[i++].LexemCode == CODE_IDENTIFIER)
                found = true;
            else i--;

            if (lexems[i++].LexemCode == CODE_CONST)
                found = true;
            else i--;
            return found;
        }
        public bool Vidn()
        {
            found = false;
            if (Vuraz())
            {
                found = false;
                var lex = lexems[i++].LexemCode;
                if (lex == CODE_LESS_THAN || lex == CODE_LESS_THAN_EQUAL || lex == CODE_MORE_THAN
                    || lex == CODE_MORE_THAN_EQUAL || lex == CODE_EQUAL || lex == CODE_NOT_EQUAL)
                {
                    if (Vuraz())
                        found = true;
                }
                else
                    throw new ArgumentException($"Синтаксична помилка: не вистачає оператора відношення");
            }
            return found;
        }
    }
}
