namespace Language_Detection
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.rtbSnippet = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDetect = new System.Windows.Forms.Button();
            this.workerDetect = new System.ComponentModel.BackgroundWorker();
            this.btnScore = new System.Windows.Forms.Button();
            this.ofdInputFile = new System.Windows.Forms.OpenFileDialog();
            this.sfdReport = new System.Windows.Forms.SaveFileDialog();
            this.workerScoreFile = new System.ComponentModel.BackgroundWorker();
            this.btnPrep = new System.Windows.Forms.Button();
            this.workerPrep = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // rtbSnippet
            // 
            this.rtbSnippet.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbSnippet.Location = new System.Drawing.Point(12, 25);
            this.rtbSnippet.Name = "rtbSnippet";
            this.rtbSnippet.Size = new System.Drawing.Size(645, 378);
            this.rtbSnippet.TabIndex = 0;
            this.rtbSnippet.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Code Snippet";
            // 
            // btnDetect
            // 
            this.btnDetect.Location = new System.Drawing.Point(577, 409);
            this.btnDetect.Name = "btnDetect";
            this.btnDetect.Size = new System.Drawing.Size(80, 23);
            this.btnDetect.TabIndex = 2;
            this.btnDetect.Text = "Detect";
            this.btnDetect.UseVisualStyleBackColor = true;
            this.btnDetect.Click += new System.EventHandler(this.btnDetect_Click);
            // 
            // workerDetect
            // 
            this.workerDetect.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerDetect_DoWork);
            this.workerDetect.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerDetect_RunWorkerCompleted);
            // 
            // btnScore
            // 
            this.btnScore.Location = new System.Drawing.Point(12, 409);
            this.btnScore.Name = "btnScore";
            this.btnScore.Size = new System.Drawing.Size(80, 23);
            this.btnScore.TabIndex = 3;
            this.btnScore.Text = "Score File";
            this.btnScore.UseVisualStyleBackColor = true;
            this.btnScore.Click += new System.EventHandler(this.btnScore_Click);
            // 
            // ofdInputFile
            // 
            this.ofdInputFile.FileName = "LanguageSamples.txt";
            // 
            // sfdReport
            // 
            this.sfdReport.FileName = "ScoreResults.csv";
            this.sfdReport.Filter = "CSV Files|*.csv";
            // 
            // workerScoreFile
            // 
            this.workerScoreFile.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerScoreFile_DoWork);
            this.workerScoreFile.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerScoreFile_RunWorkerCompleted);
            // 
            // btnPrep
            // 
            this.btnPrep.Location = new System.Drawing.Point(98, 409);
            this.btnPrep.Name = "btnPrep";
            this.btnPrep.Size = new System.Drawing.Size(80, 23);
            this.btnPrep.TabIndex = 4;
            this.btnPrep.Text = "Prep File";
            this.btnPrep.UseVisualStyleBackColor = true;
            this.btnPrep.Click += new System.EventHandler(this.btnPrep_Click);
            // 
            // workerPrep
            // 
            this.workerPrep.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerPrep_DoWork);
            this.workerPrep.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerPrep_RunWorkerCompleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 444);
            this.Controls.Add(this.btnPrep);
            this.Controls.Add(this.btnScore);
            this.Controls.Add(this.btnDetect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbSnippet);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Programming Language Detector - Scott Clayton";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbSnippet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDetect;
        private System.ComponentModel.BackgroundWorker workerDetect;
        private System.Windows.Forms.Button btnScore;
        private System.Windows.Forms.OpenFileDialog ofdInputFile;
        private System.Windows.Forms.SaveFileDialog sfdReport;
        private System.ComponentModel.BackgroundWorker workerScoreFile;
        private System.Windows.Forms.Button btnPrep;
        private System.ComponentModel.BackgroundWorker workerPrep;
    }
}

