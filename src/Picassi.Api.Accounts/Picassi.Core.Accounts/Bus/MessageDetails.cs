using System;

namespace Picassi.Core.Accounts.Bus
{
    public class MessageDetails
    {
        public string Type { get; set; }

        public DateTime Time { get; set; }

        public string Payload { get; set; }
    }
}
