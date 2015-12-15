using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PingBot
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        public List<string> PingTargets { get; set; }
        public int TimeOut { get; set; }
        public int MinBufferLength { get; set; }
        public int MaxBufferLength { get; set; }

        private int floodRunningTotal = 0;
        private List<PingStats> floodReplies;
        private List<PingStats> sequentialReplies;

        private void Form1_Load(object sender, EventArgs e)
        {
            TimeOut = 125;
            PingTargets = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(Application.StartupPath + "\\config"))
                {
                    string fileBody = sr.ReadToEnd();
                    string[] items = fileBody.Split(',');
                    MinBufferLength = int.Parse(items[0]);
                    MaxBufferLength = int.Parse(items[1]);
                    for (int i = 2; i < items.Length; i++)
                    {
                        PingTargets.Add(items[i]);
                    }
                }
            }
            catch (Exception)
            {
                PingTargets.Add("127.0.0.1");
                //PingTargets.Add("155.246.229.73");
                //PingTargets.Add("155.246.135.129");
                //PingTargets.Add("155.246.135.1");
                PingTargets.Add("google.com");
            }
        }

        private void TestTimer_Tick(object sender, EventArgs e)
        {
            TestTimer.Interval = 3600000;
            runTests();
        }
        private async void runTests()
        {
            await floodTest(MinBufferLength);
            await sequentialTest(MinBufferLength);

        }

        String date; //defines a variable for using when referencing the log file path to avoid spltting issues

        private async Task floodTest(int bufferSize)
        {
            string addresses = "IP Address: \t"; //define variables for use in log
            string statuses = "Status: \t";
            string rtts = "RTT: \t\t";
            string ttls = "TTL: \t\t";

            int rtt_total = 0; //define variables for calculating averages
            int ttl_total = 0;
            int count = 0;

            floodReplies = new List<PingStats>();
            foreach (string ipAddress in PingTargets)
            {
                for (int i = 0; i < 4; i++)
                {
                    Ping ping = new Ping();
                    ping.PingCompleted += (object sender, PingCompletedEventArgs e) =>
                    {
                        floodRunningTotal--;
                        PingStats stats = new PingStats();
                        stats.Address = ipAddress;
                        if (e.Reply!=null)
                        {
                            stats.RoundtripTime = e.Reply.RoundtripTime;
                            stats.Status = e.Reply.Status;
                            if (e.Reply.Options!=null)
                            {
                                stats.TTL = e.Reply.Options.Ttl;
                            }
                        }
                        else
                        {
                            stats.Status = IPStatus.Unknown;
                        }
                        floodReplies.Add(stats);
                        floodDataGridView.DataSource = null;
                        floodDataGridView.DataSource = floodReplies;

                        addresses = addresses + stats.Address + "\t"; //append the data in the strings to be used in the log
                        statuses = statuses + stats.Status + " \t";
                        rtts = rtts + stats.RoundtripTime + "\t\t";
                        ttls = ttls + stats.TTL + "\t\t";

                        rtt_total += (int) stats.RoundtripTime; //increment variables for finding the average
                        ttl_total += stats.TTL;
                        count++;
                    };
                    byte[] buffer = new byte[bufferSize];
                    ping.SendPingAsync(ipAddress,TimeOut,buffer);
                    floodRunningTotal++;
                }
            }
            await Task.Run((Action)waitForFloodFinish);

            double rtt_avg = (double)rtt_total / (double)count; //calculate averages
            double ttl_avg = (double)ttl_total / (double)count;

            DateTime localDate = DateTime.Now; //output log
            String fileFormat = "MMM d yyyy - HHmm";   // Use this format
            String headerFormat = "MMM d yyyy - HH:mm";
            date = localDate.ToString(fileFormat);
            String header = localDate.ToString(headerFormat);
            String floodLog = header + " Flood Test\r\n\r\n" + addresses + "\r\n" + statuses + "\r\n" + rtts + "\r\n" + ttls + "\r\n\r\n" + "Average RTT: " + rtt_avg + "\r\nAverage TTL: " + ttl_avg + "\r\n"; 
            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Network Ping Log for " + date + @".txt", floodLog);
        }
        private void waitForFloodFinish()
        {
            while (floodRunningTotal>0)
            {

            }
        }

        private async Task sequentialTest(int bufferSize)
        {
            string addresses = "IP Address: \t"; //define variables for use in log
            string statuses = "Status: \t";
            string rtts = "RTT: \t\t";
            string ttls = "TTL: \t\t";

            int rtt_total = 0; //define variables for calculating averages
            int ttl_total = 0;
            int count = 0;

            sequentialReplies = new List<PingStats>();
            foreach (string ipAddress in PingTargets)
            {
                for (int i = 0; i < 4; i++)
                {
                    Ping ping = new Ping();
                    ping.PingCompleted += (object sender, PingCompletedEventArgs e) =>
                    {
                        PingStats stats = new PingStats();
                        stats.Address = ipAddress;
                        if (e.Reply != null)
                        {
                            stats.RoundtripTime = e.Reply.RoundtripTime;
                            stats.Status = e.Reply.Status;
                            if (e.Reply.Options != null)
                            {
                                stats.TTL = e.Reply.Options.Ttl;
                            }
                        }
                        else
                        {
                            stats.Status = IPStatus.Unknown;
                        }
                        sequentialReplies.Add(stats);

                        addresses = addresses + stats.Address + "\t"; //append the data in the strings to be used in the log
                        statuses = statuses + stats.Status + " \t";
                        rtts = rtts + stats.RoundtripTime + "\t\t";
                        ttls = ttls + stats.TTL + "\t\t";

                        rtt_total += (int)stats.RoundtripTime; //increment variables for finding the average
                        ttl_total += stats.TTL;
                        count++;
                    };

                    byte[] buffer = new byte[bufferSize];
                    await ping.SendPingAsync(ipAddress,TimeOut,buffer);
                    sequentialDataGridView.DataSource = null;
                    sequentialDataGridView.DataSource = sequentialReplies;
                }
            }

            double rtt_avg = (double)rtt_total / (double)count; //calculate averages
            double ttl_avg = (double)ttl_total / (double)count;

            DateTime localDate = DateTime.Now; //output log
            String fileFormat = "MMM d yyyy - HHmm";   // Use this format
            String headerFormat = "MMM d yyyy - HH:mm";
            //String date = localDate.ToString(fileFormat);
            String header = localDate.ToString(headerFormat);
            String seqLog = header + " Sequential Test\r\n\r\n" + addresses + "\r\n" + statuses + "\r\n" + rtts + "\r\n" + ttls + "\r\n\r\n" + "Average RTT: " + rtt_avg + "\r\nAverage TTL: " + ttl_avg + "\r\n"; 
            using (StreamWriter sw = File.AppendText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Network Ping Log for " + date + @".txt"))
            {
                sw.WriteLine("\r\n\r\n" + seqLog);
            }
            //System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Network Ping Log for " + date + @".txt", seqLog);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsDialogue sd =new SettingsDialogue();
            sd.MainWindow = this;
            sd.ShowDialog();
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            TestTimer.Enabled = true;
            DateTime time = DateTime.Now;
            DateTime nextHour = new DateTime(time.Year, time.Month, time.Day, time.Hour + 1, 0, 0);
            int interval = 3600000 - (time.Millisecond + 1000 * (time.Second + 60 * (time.Minute)));
            TestTimer.Interval = interval;

            runTests();
        }
    }
}
