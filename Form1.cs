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

namespace DW1NsfInjector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            this.dw1RomTextBox.Text = RegistryUtility.GetSetting("Dw1RomPath", this.dw1RomTextBox.Text);
            this.nsfFileTextBox.Text = RegistryUtility.GetSetting("NsfPath", this.nsfFileTextBox.Text);
            this.outputFileNameTextBox.Text = RegistryUtility.GetSetting("OutputPath", this.outputFileNameTextBox.Text);
            this.trackNumberTextBox.Text = RegistryUtility.GetSetting("TrackNumber", this.trackNumberTextBox.Text);
            if (File.Exists(this.nsfFileTextBox.Text))
            {
                LoadNSFInformation(this.nsfFileTextBox.Text);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            RegistryUtility.SaveSetting("Dw1RomPath", this.dw1RomTextBox.Text);
            RegistryUtility.SaveSetting("NsfPath", this.nsfFileTextBox.Text);
            RegistryUtility.SaveSetting("OutputPath", this.outputFileNameTextBox.Text);
            RegistryUtility.SaveSetting("TrackNumber", this.trackNumberTextBox.Text);
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            string dw1RomPath = this.dw1RomTextBox.Text;
            string nsfPath = this.nsfFileTextBox.Text;
            string outputPath = this.outputFileNameTextBox.Text;
            int trackNumber = -1;
            if (int.TryParse(this.trackNumberTextBox.Text.Trim(), out trackNumber))
            {

            }
            else
            {
                trackNumber = -1;
            }
            if (trackNumber > 255 || trackNumber < 0)
            {
                MessageBox.Show("Track Number is invalid.  Must be 0-255.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.trackNumberTextBox.Text = trackNumber.ToString();
            if (!File.Exists(dw1RomPath))
            {
                MessageBox.Show("DW1 ROM File not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (!File.Exists(nsfPath))
            {
                MessageBox.Show(".NSF File not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            string outputExt = Path.GetExtension(outputPath);
            if (outputExt.ToLowerInvariant() != ".nes")
            {
                MessageBox.Show("Output File is not a .NES file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string outputDirectory = GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDirectory))
            {
                var dialogResult = MessageBox.Show("Output Directory does not exist.  Create it?", "Create Directory?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (DialogResult != DialogResult.Yes)
                {
                    return;
                }
                Directory.CreateDirectory(outputDirectory);
            }
            Injector.InjectNsf(dw1RomPath, nsfPath, outputPath, (byte)trackNumber);
            MessageBox.Show("NSF has been injected into the ROM.", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        bool DoFileDialog(Control control, string filter, bool open)
        {
            string inputFileName = control.Text;
            string fileName = DoFileDialog(inputFileName, filter, open);
            if (String.IsNullOrEmpty(fileName)) return false;
            control.Text = fileName;
            return true;
        }

        static string GetDirectoryName(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return "";
            }
            try
            {
                return Path.GetDirectoryName(path);
            }
            catch
            {
                return "";
            }
        }
        string DoFileDialog(string inputFileName, string filter, bool open)
        {
            string inputDirectoryName = GetDirectoryName(inputFileName);
            filter += "|All Files (*.*)|*.*";
            OpenFileDialog openFileDialog;
            SaveFileDialog saveFileDialog;
            FileDialog fileDialog;
            if (open)
            {
                openFileDialog = new OpenFileDialog();
                fileDialog = openFileDialog;
                if (File.Exists(inputFileName))
                {
                    fileDialog.FileName = inputFileName;
                    fileDialog.InitialDirectory = inputDirectoryName;
                }
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;
                openFileDialog.AutoUpgradeEnabled = true;
                openFileDialog.ValidateNames = true;
            }
            else
            {
                saveFileDialog = new SaveFileDialog();
                fileDialog = saveFileDialog;
                if (Directory.Exists(inputFileName))
                {
                    fileDialog.InitialDirectory = inputFileName;
                }
                else if (Directory.Exists(inputDirectoryName))
                {
                    fileDialog.InitialDirectory = inputDirectoryName;
                }
                saveFileDialog.OverwritePrompt = false;
                saveFileDialog.ValidateNames = true;
                saveFileDialog.AutoUpgradeEnabled = true;
                saveFileDialog.CheckPathExists = true;
            }
            fileDialog.Filter = filter;
            var dialogResult = fileDialog.ShowDialog();
            string result = null;

            if (dialogResult == DialogResult.OK)
            {
                result = fileDialog.FileName;
            }
            fileDialog.Dispose();
            return result;
        }
        
        private void browseButton1_Click(object sender, EventArgs e)
        {
            bool okay = DoFileDialog(this.dw1RomTextBox, "NES Files (*.nes)|*.nes", true);
            if (okay && this.outputFileNameTextBox.Text == "" && File.Exists(this.dw1RomTextBox.Text))
            {
                this.outputFileNameTextBox.Text = GetDirectoryName(this.dw1RomTextBox.Text);
            }
        }

        private void browseButton2_Click(object sender, EventArgs e)
        {
            bool okay = DoFileDialog(this.nsfFileTextBox, "NSF Files (*.nsf)|*.nsf", true);
            if (okay && File.Exists(this.nsfFileTextBox.Text))
            {
                LoadNSFInformation(this.nsfFileTextBox.Text);
            }
        }

        private void browseButton3_Click(object sender, EventArgs e)
        {
            DoFileDialog(this.outputFileNameTextBox, "NES Files (*.nes)|*.nes", false);
        }

        private void LoadNSFInformation(string fileName)
        {
            var nsf = new NSF();
            nsf.Load(fileName);
            this.lblNsfInformation.Text =
                "NSF Information:\r\n" +
                "Game Title: " + nsf.SongName + "\r\n" +
                "Artist: " + nsf.Artist + "\r\n" +
                "Copyright: " + nsf.Copyright + "\r\n" +
                "Number of songs: " + nsf.TotalSongs.ToString();
        }

    }
}
