using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;


namespace Translator
{
    public partial class MainForm : Form
    {
        private LexicalTablesForm lexicalTablesForm;
        private LexicalAnalyzer lexicalAnalyzer;
        private SyntaxAnalyzer syntaxAnalyzer;
        private SyntaxAnalyzerAutomat automat;
        public MainForm()
        {
            InitializeComponent();
            numberedRTB1.RichTextBox.Select();
            numberedRTB1.RichTextBox.TextChanged += NumberedRTB_TextChanged;
        }
        private void BuildErrorsMessage(List<string> errors)
        {
            foreach (string error in errors)
                richTextBox2.Text += error + "\n";
        }
        private void lexicalAnalyzerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lexicalTablesForm = new LexicalTablesForm(numberedRTB1.RichTextBox.Text);
            lexicalTablesForm.Show();
        }
        private void NumberedRTB_TextChanged(object sender, EventArgs e)
        {
            string sourceCode = numberedRTB1.RichTextBox.Text;
            List<string> lexicalErrors = new List<string>();
            List<string> syntaxErrors = new List<string>();

            lexicalAnalyzer = new LexicalAnalyzer();
            syntaxAnalyzer = new SyntaxAnalyzer();
            automat = new SyntaxAnalyzerAutomat();

            richTextBox2.Text = "";
            
            lexicalErrors = lexicalAnalyzer.Start(sourceCode);
            if (lexicalErrors.Count == 0)
            {
                //syntaxErrors = syntaxAnalyzer.Main(lexicalAnalyzer.output);
                syntaxErrors = automat.Start(lexicalAnalyzer.output);
            }
            foreach (string error in lexicalErrors)
                richTextBox2.Text += error + "\n";
            if (syntaxErrors.Count != 0)
            {
                BuildErrorsMessage(syntaxErrors);
            }
        }
        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "C:";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.Title = "Select a Text File";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader read = new StreamReader(File.OpenRead(openFileDialog1.FileName));
                numberedRTB1.RichTextBox.Text = read.ReadToEnd();
                read.Dispose();
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "C:";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.Title = "Save a Text File";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter write = new StreamWriter(File.Create(saveFileDialog1.FileName));
                write.Write(numberedRTB1.RichTextBox.Text);
                write.Dispose();
            }
        }
    }
}