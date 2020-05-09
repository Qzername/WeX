using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class Messages
    {
        public ulong id;
        public string ismessagesonline;
        public string text;
        public ulong channelid;
    }

    public class MainConfig
    {
        public ulong serverid;
        public ulong muteroleid;
    }

    public class MarriageItem
    {
        public ulong serverid;
        public ulong userid;
        public ulong user2id;
        public string date;
    }
}
