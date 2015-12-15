using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PingBot
{
    public partial class SettingsDialogue : Form
    {
        public Main MainWindow { get; set; }

        public SettingsDialogue()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            MainWindow.MinBufferLength = int.Parse(minBufferBox.Text);
            MainWindow.MaxBufferLength = int.Parse(maxBufferBox.Text);

            string targetsString = pingTargetsBox.Text.Replace("\r", "");
            MainWindow.PingTargets = new List<string>(targetsString.Split('\n'));

            using (StreamWriter sw=new StreamWriter(Application.StartupPath+"\\config"))
            {
                StringBuilder listSB = new StringBuilder();
                foreach (string item in MainWindow.PingTargets)
                {
                    listSB.Append(',');
                    listSB.Append(item);
                }
                string text = string.Format("{0},{1}{2}", MainWindow.MinBufferLength,MainWindow.MaxBufferLength,listSB.ToString());
                sw.Write(text);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void SettingsDialogue_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void SettingsDialogue_Load(object sender, EventArgs e)
        {
            minBufferBox.Text = MainWindow.MinBufferLength.ToString();
            maxBufferBox.Text = MainWindow.MaxBufferLength.ToString();
            StringBuilder sb = new StringBuilder();
            foreach (string item in MainWindow.PingTargets)
            {
                sb.Append(item);
                sb.Append('\n');
            }
            sb.Remove(sb.Length - 1, 1);
            pingTargetsBox.Text = sb.ToString();
        }
    }
}
