using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SDSE1_s_Sidekick
{
    public partial class Sidekick : Form
    {
        public Sidekick()
        {
            InitializeComponent();

            //Read UMDIMAGE's Path from "Config.txt"
            if (File.Exists("Config.txt") == true)
                LoadPath();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Liquid-S");
        }

        private void LoadPath() // Read UMDIMAGE's Path from "Config.txt".
        {
            using (FileStream ConfigTXT = new FileStream("Config.txt", FileMode.Open, FileAccess.Read))
            using (StreamReader DP = new StreamReader(ConfigTXT, Encoding.Default))
                textBox1.Text = DP.ReadLine();
        }

        private void SetDATA01Path() // Save UMDIMAGE's Path in the textbox and into "Config.txt".
        {
            FolderBrowserDialog UMDIMAGEPath = new FolderBrowserDialog();

            if (UMDIMAGEPath.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = UMDIMAGEPath.SelectedPath; // Write the Path inside the textbox.

                using (FileStream ConfigTXT = new FileStream("Config.txt", FileMode.Create, FileAccess.Write))
                using (StreamWriter UMDP = new StreamWriter(ConfigTXT, Encoding.Default))
                    UMDP.WriteLine(UMDIMAGEPath.SelectedPath);
            }
        }

        private void Button1_Click(object sender, EventArgs e) // "Set DATA01's Path..."
        {
            SetDATA01Path();
        }

        private void button2_Click(object sender, EventArgs e) // COMPUTE PROGRESS.
        {
            if (textBox1.Text == "" || textBox1.Text == null || textBox1.Text.Contains("Click on \"Set ")) //BUILD TEXT FILES
                SetDATA01Path();

            label2.Text = "Wait..."; // Change "Ready!" to "Wait..."
            label2.Refresh(); // Refresh the Status label.

            TranslationStatus Form2 = new TranslationStatus(textBox1.Text);
            Form2.Show();

            label2.Text = "Ready!"; // Change the "Status" to "Ready!".
        }
    }
}
