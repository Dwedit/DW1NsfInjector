namespace DW1NsfInjector
{
    partial class Form1
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
            this.dw1RomTextBox = new System.Windows.Forms.TextBox();
            this.nsfFileTextBox = new System.Windows.Forms.TextBox();
            this.outputFileNameTextBox = new System.Windows.Forms.TextBox();
            this.lblDw1Rom = new System.Windows.Forms.Label();
            this.lblNsf = new System.Windows.Forms.Label();
            this.lblSaveAs = new System.Windows.Forms.Label();
            this.browseButton1 = new System.Windows.Forms.Button();
            this.browseButton2 = new System.Windows.Forms.Button();
            this.browseButton3 = new System.Windows.Forms.Button();
            this.lblNsfInformation = new System.Windows.Forms.Label();
            this.goButton = new System.Windows.Forms.Button();
            this.lblTrackNumber = new System.Windows.Forms.Label();
            this.trackNumberTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // dw1RomTextBox
            // 
            this.dw1RomTextBox.Location = new System.Drawing.Point(134, 6);
            this.dw1RomTextBox.Name = "dw1RomTextBox";
            this.dw1RomTextBox.Size = new System.Drawing.Size(307, 20);
            this.dw1RomTextBox.TabIndex = 1;
            // 
            // nsfFileTextBox
            // 
            this.nsfFileTextBox.Location = new System.Drawing.Point(134, 32);
            this.nsfFileTextBox.Name = "nsfFileTextBox";
            this.nsfFileTextBox.Size = new System.Drawing.Size(307, 20);
            this.nsfFileTextBox.TabIndex = 4;
            // 
            // outputFileNameTextBox
            // 
            this.outputFileNameTextBox.Location = new System.Drawing.Point(134, 58);
            this.outputFileNameTextBox.Name = "outputFileNameTextBox";
            this.outputFileNameTextBox.Size = new System.Drawing.Size(307, 20);
            this.outputFileNameTextBox.TabIndex = 7;
            // 
            // lblDw1Rom
            // 
            this.lblDw1Rom.AutoSize = true;
            this.lblDw1Rom.Location = new System.Drawing.Point(12, 9);
            this.lblDw1Rom.Name = "lblDw1Rom";
            this.lblDw1Rom.Size = new System.Drawing.Size(116, 13);
            this.lblDw1Rom.TabIndex = 0;
            this.lblDw1Rom.Text = "&Dragon Warrior 1 ROM";
            // 
            // lblNsf
            // 
            this.lblNsf.AutoSize = true;
            this.lblNsf.Location = new System.Drawing.Point(37, 35);
            this.lblNsf.Name = "lblNsf";
            this.lblNsf.Size = new System.Drawing.Size(91, 13);
            this.lblNsf.TabIndex = 3;
            this.lblNsf.Text = ".&NSF File to Inject";
            // 
            // lblSaveAs
            // 
            this.lblSaveAs.AutoSize = true;
            this.lblSaveAs.Location = new System.Drawing.Point(81, 61);
            this.lblSaveAs.Name = "lblSaveAs";
            this.lblSaveAs.Size = new System.Drawing.Size(47, 13);
            this.lblSaveAs.TabIndex = 6;
            this.lblSaveAs.Text = "Save &As";
            // 
            // browseButton1
            // 
            this.browseButton1.Location = new System.Drawing.Point(447, 3);
            this.browseButton1.Name = "browseButton1";
            this.browseButton1.Size = new System.Drawing.Size(75, 23);
            this.browseButton1.TabIndex = 2;
            this.browseButton1.Text = "Browse...";
            this.browseButton1.UseVisualStyleBackColor = true;
            this.browseButton1.Click += new System.EventHandler(this.browseButton1_Click);
            // 
            // browseButton2
            // 
            this.browseButton2.Location = new System.Drawing.Point(447, 30);
            this.browseButton2.Name = "browseButton2";
            this.browseButton2.Size = new System.Drawing.Size(75, 23);
            this.browseButton2.TabIndex = 5;
            this.browseButton2.Text = "Browse..";
            this.browseButton2.UseVisualStyleBackColor = true;
            this.browseButton2.Click += new System.EventHandler(this.browseButton2_Click);
            // 
            // browseButton3
            // 
            this.browseButton3.Location = new System.Drawing.Point(447, 56);
            this.browseButton3.Name = "browseButton3";
            this.browseButton3.Size = new System.Drawing.Size(75, 23);
            this.browseButton3.TabIndex = 8;
            this.browseButton3.Text = "Browse...";
            this.browseButton3.UseVisualStyleBackColor = true;
            this.browseButton3.Click += new System.EventHandler(this.browseButton3_Click);
            // 
            // lblNsfInformation
            // 
            this.lblNsfInformation.AutoSize = true;
            this.lblNsfInformation.Location = new System.Drawing.Point(12, 117);
            this.lblNsfInformation.Name = "lblNsfInformation";
            this.lblNsfInformation.Size = new System.Drawing.Size(86, 13);
            this.lblNsfInformation.TabIndex = 12;
            this.lblNsfInformation.Text = "NSF Information:";
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(183, 84);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(75, 23);
            this.goButton.TabIndex = 11;
            this.goButton.Text = "GO!";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // lblTrackNumber
            // 
            this.lblTrackNumber.AutoSize = true;
            this.lblTrackNumber.Location = new System.Drawing.Point(29, 89);
            this.lblTrackNumber.Name = "lblTrackNumber";
            this.lblTrackNumber.Size = new System.Drawing.Size(99, 13);
            this.lblTrackNumber.TabIndex = 9;
            this.lblTrackNumber.Text = "NSF Track Number";
            // 
            // trackNumberTextBox
            // 
            this.trackNumberTextBox.Location = new System.Drawing.Point(134, 86);
            this.trackNumberTextBox.Name = "trackNumberTextBox";
            this.trackNumberTextBox.Size = new System.Drawing.Size(43, 20);
            this.trackNumberTextBox.TabIndex = 10;
            this.trackNumberTextBox.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 188);
            this.Controls.Add(this.trackNumberTextBox);
            this.Controls.Add(this.lblTrackNumber);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.lblNsfInformation);
            this.Controls.Add(this.browseButton3);
            this.Controls.Add(this.browseButton2);
            this.Controls.Add(this.browseButton1);
            this.Controls.Add(this.lblSaveAs);
            this.Controls.Add(this.lblNsf);
            this.Controls.Add(this.lblDw1Rom);
            this.Controls.Add(this.outputFileNameTextBox);
            this.Controls.Add(this.nsfFileTextBox);
            this.Controls.Add(this.dw1RomTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Dragon Warrior 1 NSF Injector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox dw1RomTextBox;
        private System.Windows.Forms.TextBox nsfFileTextBox;
        private System.Windows.Forms.TextBox outputFileNameTextBox;
        private System.Windows.Forms.Label lblDw1Rom;
        private System.Windows.Forms.Label lblNsf;
        private System.Windows.Forms.Label lblSaveAs;
        private System.Windows.Forms.Button browseButton1;
        private System.Windows.Forms.Button browseButton2;
        private System.Windows.Forms.Button browseButton3;
        private System.Windows.Forms.Label lblNsfInformation;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.Label lblTrackNumber;
        private System.Windows.Forms.TextBox trackNumberTextBox;
    }
}

