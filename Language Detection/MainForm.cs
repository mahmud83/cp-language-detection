using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Language_Detection
{
    public partial class MainForm : Form
    {
        string endpoint;
        string apikey;

        int ngram = 3;

        IFeatureExtractor fe;
        IClassifier cs;
        ISnippetParser sp;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Read the Code Project article linked in README.md to figure out how to get the URL and API Key for this application
            if (!File.Exists("api.url") || string.IsNullOrEmpty(endpoint = File.ReadAllText("api.url")))
            {
                endpoint = ShowDialog("Please enter the endpoint of your Azure Web Service. ", "Language Detector");
                File.WriteAllText("api.url", endpoint);
            }
            if (!File.Exists("api.key") || string.IsNullOrEmpty(apikey = File.ReadAllText("api.key")))
            {
                apikey = ShowDialog("Please enter the API Key for your Azure Web Service. ", "Language Detector");
                File.WriteAllText("api.key", apikey);
            }

            // Initialize our chosen feature extractor and classifier
            fe = new LetterGroupFeatureExtractor(ngram);
            cs = new AzureClassifier(fe, endpoint, apikey);
            sp = new CodeSnippetParser();

            // Create a training set off of a lot of code samples
            // ExtractTrainingSetFromFolder();
        }

        private void ExtractTrainingSetFromFolder()
        {
            ISnippetParser fsp = new FolderSnippetParser();
            List<CodeSnippet> snippets = fsp.ExtractLabeledCodeSnippets("manual");
            Encoding utf8 = Encoding.GetEncoding("UTF-8", new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback());

            using (StreamWriter w = new StreamWriter("cp-training-30k.csv", false, utf8))
            {
                w.WriteLine("Language,Snippet");
                Parallel.ForEach(snippets, s =>
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string f in fe.ExtractFeatures(s.Snippet))
                    {
                        // The lines can't be longer than 32k or they'll wrap for some reason
                        if (sb.Length < 32700)
                        {
                            sb.Append(f);
                            sb.Append(" ");
                        }
                    }

                    lock (w)
                    {
                        string snippet = sb.ToString().Replace(",", "");

                        // We don't want to train off of small snippets
                        if (snippet.Length >= 256)
                        {
                            w.WriteLine(s.Language.ToLower() + "," + snippet);
                        }
                    }
                });
            }
        }

        private string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                ClientSize = new Size(444, 85),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 9, Top = 9, Width = 303, Text = text };
            TextBox textBox = new TextBox() { Left = 12, Top = 25, Width = 423 };
            Button confirmation = new Button() { Text = "Save", Left = 360, Top = 51, Width = 75, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void btnDetect_Click(object sender, EventArgs e)
        {
            if (!workerDetect.IsBusy)
            {
                workerDetect.RunWorkerAsync(rtbSnippet.Text);

                rtbSnippet.Enabled = false;
                btnDetect.Enabled = false;
            }
        }

        private void workerDetect_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = cs.GetClassification(e.Argument as string);
        }

        private void workerDetect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Buest Guess: " + e.Result.ToString(), "Language Detector", MessageBoxButtons.OK, MessageBoxIcon.Information);

            rtbSnippet.Enabled = true;
            btnDetect.Enabled = true;
        }

        private void btnScore_Click(object sender, EventArgs e)
        {
            if (ofdInputFile.ShowDialog() == DialogResult.OK)
            {
                if (sfdReport.ShowDialog() == DialogResult.OK)
                {
                    if (!workerScoreFile.IsBusy)
                    {
                        workerScoreFile.RunWorkerAsync(new FileArgs() { SourceFile = ofdInputFile.FileName, DestinationFile = sfdReport.FileName });

                        btnScore.Enabled = false;
                    }
                }
            }
        }

        private void workerScoreFile_DoWork(object sender, DoWorkEventArgs e)
        {
            FileArgs files = e.Argument as FileArgs;
            List<CodeSnippet> snippets = sp.ExtractLabeledCodeSnippets(File.ReadAllText(files.SourceFile));
            
            ClassifierResult result = cs.ScoreClassifier(snippets, files.DestinationFile);

            e.Result = new ScoreArgs { Result = result, Files = files };
        }

        private void workerScoreFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ScoreArgs result = e.Result as ScoreArgs;
            MessageBox.Show("Results saved to " + result.Files.DestinationFile, "Language Detector", MessageBoxButtons.OK, MessageBoxIcon.Information);

            rtbSnippet.Text = result.Files.SourceFile + 
                "\r\n\r\nTotal: " + result.Result.Total + 
                "\r\nCorrect: " + result.Result.Correct + 
                "\r\nIncorrect: " + result.Result.Incorrect + 
                "\r\nAccuracy: " + result.Result.Accuracy.ToString("0.000") + "%";

            btnScore.Enabled = true;
        }

        private void btnPrep_Click(object sender, EventArgs e)
        {
            if (ofdInputFile.ShowDialog() == DialogResult.OK)
            {
                if (sfdReport.ShowDialog() == DialogResult.OK)
                {
                    if (!workerPrep.IsBusy)
                    {
                        workerPrep.RunWorkerAsync(new FileArgs() { SourceFile = ofdInputFile.FileName, DestinationFile = sfdReport.FileName });

                        btnPrep.Enabled = false;
                    }
                }
            }
        }

        private void workerPrep_DoWork(object sender, DoWorkEventArgs e)
        {
            FileArgs files = e.Argument as FileArgs;
            List<CodeSnippet> snippets = sp.ExtractLabeledCodeSnippets(File.ReadAllText(files.SourceFile));

            using (StreamWriter w = new StreamWriter(files.DestinationFile))
            {
                w.WriteLine("Language,Snippet");
                foreach (var s in snippets)
                {
                    string features = fe.ExtractFeatures(s.Snippet).Aggregate((c, n) => c + " " + n).Replace(",", "");
                    w.WriteLine(s.Language.ToLower() + "," + features);
                }
            }

            e.Result = files;
        }

        private void workerPrep_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FileArgs files = e.Result as FileArgs;
            MessageBox.Show("Preprocessed data saved to " + files.DestinationFile, "Language Detector", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnPrep.Enabled = true;
        }
    }

    class FileArgs
    {
        public string SourceFile { get; set; }

        public string DestinationFile { get; set; }
    }

    class ScoreArgs
    {
        public ClassifierResult Result { get; set; }

        public FileArgs Files { get; set; }
    }
}
