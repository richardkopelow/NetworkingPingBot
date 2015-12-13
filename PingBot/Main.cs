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
            TestTimer.Enabled = true;
            DateTime time = DateTime.Now;
            DateTime nextHour = new DateTime(time.Year, time.Month, time.Day, time.Hour + 1, 0, 0);
            int interval=3600000-(time.Millisecond+1000*(time.Second+60*(time.Minute)));
            TestTimer.Interval = interval;

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
            runTests();
        }

        private void TestTimer_Tick(object sender, EventArgs e)
        {
            TestTimer.Interval = 3600000;
            runTests();
        }
        private async void runTests()
        {
            await floodTest();
            await sequentialTest();

        }
        private async Task floodTest(int bufferSize)
        {
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
                    };
                    byte[] buffer = new byte[bufferSize];
                    ping.SendPingAsync(ipAddress,TimeOut,buffer);
                    floodRunningTotal++;
                }
            }
            await Task.Run((Action)waitForFloodFinish);
        }
        private void waitForFloodFinish()
        {
            while (floodRunningTotal>0)
            {

            }
        }

        private async Task sequentialTest(int bufferSize)
        {
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
                    };

                    byte[] buffer = new byte[bufferSize];
                    await ping.SendPingAsync(ipAddress,TimeOut,buffer);
                    sequentialDataGridView.DataSource = null;
                    sequentialDataGridView.DataSource = sequentialReplies;
                }
            }
        }
    }
}
