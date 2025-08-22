using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace C_Analyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "C:\\Users\\baraa jabareen\\Downloads\\simple_types.c"; // Set your default path here
            label1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AnalyzeFunctionsAndnVariables(textBox1.Text);
            AnalyzeForAndWhile(textBox1.Text);
            AnalyzePrintfAndScanf(textBox1.Text);   // <--- add this
        }

        private void AnalyzeFunctionsAndnVariables(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File not found!");
                return;
            }

            // Safe to read file size now
            long fileSize = new FileInfo(filePath).Length;

            // Read & strip comments (block + line), then split
            string code = File.ReadAllText(filePath);
            code = StripComments(code);
            string[] lines = code.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            long lin = lines.Length;

            int functionCount = 0;
            int variableCount = 0;

            label1.Text = ""; // clear before appending

            foreach (string line in lines)
            {
                string trimmedLine = DeleteExtraText(line); // keeps up to first ';' and trims

                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith("#"))
                    continue;

                label1.Text += trimmedLine + "\n";

                // Function (simple heuristic: single-line "retType name(...) {")
                if (trimmedLine.EndsWith("{") &&
                    (trimmedLine.Contains("int ") || trimmedLine.Contains("void ") ||
                     trimmedLine.Contains("float ") || trimmedLine.Contains("char ") ||
                     trimmedLine.Contains("double ") || trimmedLine.Contains("long ")))
                {
                    functionCount++;
                }
                // Variable declaration (single-line; avoid prototypes/for/while by rejecting '(')
                else if (trimmedLine.EndsWith(";") && !trimmedLine.Contains("(") &&
                         (trimmedLine.Contains("int ") || trimmedLine.Contains("float ") ||
                          trimmedLine.Contains("char ") || trimmedLine.Contains("double ") ||
                          trimmedLine.Contains("long ")))
                {
                    // Count one per line (simple). If you want to count a,b,*p separately, use CountDeclarators().
                    variableCount++;
                    // variableCount += CountDeclarators(trimmedLine); // <- optional upgrade
                }
            }

            label2.Text = $"Functions: {functionCount}\n" +
                          $"Variables: {variableCount}\n" +
                          $"Lines: {lin}\n" +
                          $"File Size: {fileSize} bytes";
        }

        private void AnalyzeForAndWhile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File not found!");
                return;
            }

            string code = File.ReadAllText(filePath);
            code = StripComments(code);

            // for(init;cond;iter) on one line
            var forRx = new Regex(@"\bfor\b\s*\([^;]*;[^;]*;[^;]*\)", RegexOptions.Compiled);
            var whileRx = new Regex(@"\bwhile\b\s*\(", RegexOptions.Compiled);

            int forCount = forRx.Matches(code).Count;
            int whileCount = whileRx.Matches(code).Count;

            label2.Text += $"\nfor loops: {forCount}\nwhile loops: {whileCount}";
        }

        // Already good, reusing here for both analyses
        private string StripComments(string code)
        {
            code = Regex.Replace(code, @"/\*.*?\*/", "", RegexOptions.Singleline);  // block
            code = Regex.Replace(code, @"//.*?$", "", RegexOptions.Multiline);      // line
            return code;
        }

        // Keep only up to first ';' and trim; also removes trailing '//' if present
        private string DeleteExtraText(string line)
        {
            int commentIndex = line.IndexOf("//");
            if (commentIndex != -1) line = line.Substring(0, commentIndex);

            int semi = line.IndexOf(';');
            if (semi != -1) line = line.Substring(0, semi + 1);

            return line.Trim();
        }
        private void AnalyzePrintfAndScanf(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File not found!");
                return;
            }

            // Read, strip comments and string literals
            string code = File.ReadAllText(filePath);
            code = StripComments(code);
            code = RemoveStringLiterals(code);

            // Match calls like: printf(...), scanf(...)
            var printfRx = new Regex(@"\bprintf\s*\(", RegexOptions.Compiled);
            var scanfRx = new Regex(@"\bscanf\s*\(", RegexOptions.Compiled);

            int printfCount = printfRx.Matches(code).Count;
            int scanfCount = scanfRx.Matches(code).Count;

            // Append to the summary label
            label2.Text += $"\nprintf: {printfCount}\nscanf: {scanfCount}";
        }
        // Remove C/C++ string literals to avoid matching "printf" inside quotes
        private string RemoveStringLiterals(string code)
        {
            // Matches " ... " with support for escaped quotes \" and escaped backslashes \\
            return Regex.Replace(code, @"""(?:\\.|[^""\\])*""", "\"\"", RegexOptions.Singleline);
        }

    }
}
