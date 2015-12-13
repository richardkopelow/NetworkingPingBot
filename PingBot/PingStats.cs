using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PingBot
{
    class PingStats
    {
        public string Address { get; set; }
        public IPStatus Status { get; set; }
        public long RoundtripTime { get; set; }
        public int TTL { get; set; }
    }
}
