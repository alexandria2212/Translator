using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translator
{
    public partial class LexicalTablesForm : Form
    {
        private LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer();
        public LexicalTablesForm(string str)
        {
            InitializeComponent();
            dataGridView1.Columns.Clear();
            dataGridView2.Columns.Clear();
            dataGridView3.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();

            lexicalAnalyzer.output.Clear();
            lexicalAnalyzer.Identifiers.Clear();
            lexicalAnalyzer.Constants.Clear();

            List<string> lexicalErrors = lexicalAnalyzer.Start(str);
            #region ColumnAdd
            dataGridView1.Columns.Add("LineNumber", "№ рядку");
            dataGridView1.Columns.Add("Lexem", "Підрядок");
            dataGridView1.Columns.Add("LexemCode", "Код лексеми");
            dataGridView1.Columns.Add("ClassNumber", "№ елементу даного класу");

            dataGridView2.Columns.Add("Lexem", "Ідентифікатор");
            dataGridView2.Columns.Add("Code", "№ елементу");
            dataGridView2.Columns.Add("Type", "Тип");

            dataGridView3.Columns.Add("Lexem", "Константа");
            dataGridView3.Columns.Add("Code", "№ елементу");
            #endregion
            BuildLexemTable(lexicalAnalyzer);
            BuildConstantsTable(lexicalAnalyzer);
            BuildIdentifiersTable(lexicalAnalyzer);
        }
        private void BuildLexemTable(LexicalAnalyzer analyzer)
        {
            for (int i = 0; i < analyzer.output.Count; i++)
            {
                dataGridView1.Rows.Add(
                analyzer.output[i].LineNumber,
                analyzer.output[i].Lexem,
                (int)analyzer.output[i].LexemCode,
                analyzer.output[i].ClassNumber);
            }
        }
        private void BuildIdentifiersTable(LexicalAnalyzer analyzer)
        {
            foreach (Lexeme id in analyzer.Identifiers)
            {
                dataGridView2.Rows.Add(
                    id.Lexem,
                    id.Elem,
                    id.Type);
            }
        }
        private void BuildConstantsTable(LexicalAnalyzer analyzer)
        {
            foreach (KeyValuePair<string, int> keyValue in analyzer.Constants)
            {
                dataGridView3.Rows.Add(
                    keyValue.Key,
                    keyValue.Value.ToString());
            }
        }
    }
}
