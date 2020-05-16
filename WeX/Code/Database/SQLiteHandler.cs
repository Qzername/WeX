using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;

namespace Database
{
    public static class SQLiteHandler
    {
        static string path;

        static SQLiteHandler()
        {
            path = "Data Source=./Databases/ServerConfigs.s3db;Version=3;";
        }

        public static bool NoServer(ulong serverid)
        {
            using var con = new SQLiteConnection(path);
            con.Open();

            string stm = "SELECT id FROM WelcomeMessages WHERE id=" + serverid;

            using var cmd = new SQLiteCommand(stm, con);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            int id = 0;
            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
            }

            if (id == 0)
                return true;
            else
                return false;
        }

        public static void NewServer(ulong serverid)
        {
            using var con = new SQLiteConnection(path);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "INSERT INTO WelcomeMessages(id, ismessageonline, text, channelid) VALUES(@id, @yes, @no, 0);";
            cmd.Parameters.AddWithValue("@id", serverid);
            cmd.Parameters.AddWithValue("@yes", "false");
            cmd.Parameters.AddWithValue("@no", "Welcome to [server], [user]");
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO ByeMessages(id, ismessageonline, text, channelid) VALUES(@id, @yes, @no, 0);";
            cmd.Parameters.AddWithValue("@id", serverid);
            cmd.Parameters.AddWithValue("@yes", "false");
            cmd.Parameters.AddWithValue("@no", "[user] has left from [server]");
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO mainconfig(serverid, muteroleid, autoroleid, logchannelid, prefix) VALUES(@id, 0, 0, 0, 'null');";
            cmd.Parameters.AddWithValue("@id", serverid);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public static Messages GetMessage(ulong serverid, bool isWelcome)
        {
            if (NoServer(serverid))
                NewServer(serverid);

            Messages mess = new Messages();

            using var con = new SQLiteConnection(path);
            con.Open();

            string stm;

            if(isWelcome)
                stm = "SELECT * FROM WelcomeMessages WHERE id=" + serverid;
            else
                stm = "SELECT * FROM ByeMessages WHERE id=" + serverid;

            using var cmd = new SQLiteCommand(stm, con);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                mess.id = Convert.ToUInt64(rdr.GetInt64(0));
                mess.ismessagesonline = rdr.GetString(1);
                mess.text = rdr.GetString(2);
                mess.channelid = Convert.ToUInt64(rdr.GetInt64(3));
            }
            return mess;
        }

        public static MainConfig GetMessage(ulong serverid)
        {
            MainConfig mess = new MainConfig();

            using var con = new SQLiteConnection(path);
            con.Open();

            string stm;

            stm = "SELECT * FROM mainconfig WHERE serverid=" + serverid;

            using var cmd = new SQLiteCommand(stm, con);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                mess.serverid = Convert.ToUInt64(rdr.GetInt64(0));
                mess.muteroleid = Convert.ToUInt64(rdr.GetInt64(1));
                mess.autoroleid = Convert.ToUInt64(rdr.GetInt64(2));
                mess.logchannelid = Convert.ToUInt64(rdr.GetInt64(3));
                mess.prefix = rdr.GetString(4);
            }
            return mess;
        }

        public static void Update(Messages mess, bool isWelcome)
        {
            using var con = new SQLiteConnection(path);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            if (isWelcome)
                cmd.CommandText = "UPDATE WelcomeMessages SET text = @text, ismessageonline = @message, channelid = @id WHERE id=@serverid";
            else
                cmd.CommandText = "UPDATE ByeMessages SET text = @text, ismessageonline = @message, channelid = @id WHERE id=@serverid";
            cmd.Parameters.AddWithValue("@id", mess.channelid);
            cmd.Parameters.AddWithValue("@serverid", mess.id);
            cmd.Parameters.AddWithValue("@message", mess.ismessagesonline);
            cmd.Parameters.AddWithValue("@text", mess.text);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public static void Update(MainConfig mess)
        {
            using var con = new SQLiteConnection(path);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "UPDATE mainconfig SET muteroleid = @muteroleid, autoroleid =@autoroleid, logchannelid = @logchannel, prefix=@prefix WHERE serverid = @id;";
            cmd.Parameters.AddWithValue("@id", mess.serverid);
            cmd.Parameters.AddWithValue("@muteroleid", mess.muteroleid);
            cmd.Parameters.AddWithValue("@autoroleid", mess.autoroleid);
            cmd.Parameters.AddWithValue("@logchannel", mess.logchannelid);
            cmd.Parameters.AddWithValue("@prefix", mess.prefix);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    
        public static class Marriage
        {
            public static bool NoInMarriage(ulong serverid, ulong userid)
            {
                using var con = new SQLiteConnection(path);
                con.Open();

                string stm = "SELECT user1 FROM Marriage WHERE serverid=" + serverid + " AND user1="+userid;

                using var cmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                int id = 0;
                while (rdr.Read())
                {
                    id = rdr.GetInt32(0);
                }

                if (id == 0)
                    return true;
                else
                    return false;
            }

            public static void NewPerson(ulong serverid, ulong userid)
            {
                using var con = new SQLiteConnection(path);
                con.Open();
                using var cmd = new SQLiteCommand(con);
                cmd.CommandText = "INSERT INTO Marriage(serverid, user1, user2, data) VALUES(@id, @yes, 0, 'none');";
                cmd.Parameters.AddWithValue("@id", serverid);
                cmd.Parameters.AddWithValue("@yes", userid);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }

            public static MarriageItem GetMarriage(ulong serverid, ulong userid)
            {
                MarriageItem mess = new MarriageItem();

                using var con = new SQLiteConnection(path);
                con.Open();

                string stm;

                stm = "SELECT * FROM Marriage WHERE serverid=" + serverid+" AND user1=" +userid;

                using var cmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    mess.serverid = Convert.ToUInt64(rdr.GetInt64(0));
                    mess.userid = Convert.ToUInt64(rdr.GetInt64(1));
                    mess.user2id = Convert.ToUInt64(rdr.GetInt64(2));
                    mess.date = rdr.GetString(3);
                }
                return mess;
            }

            public static void NewMarriage(ulong serverid, ulong user1id, ulong user2id)
            {
                if (NoInMarriage(serverid, user2id))
                    NewPerson(serverid, user2id);

                using var con = new SQLiteConnection(path);
                con.Open();
                using var cmd = new SQLiteCommand(con);
                cmd.CommandText = "UPDATE Marriage SET serverid = @serverid, user1 = @user1, user2 = @user2, data = @data WHERE serverid=@serverid AND user1 = @user1";
                cmd.Parameters.AddWithValue("@serverid", serverid);
                cmd.Parameters.AddWithValue("@user1", user1id);
                cmd.Parameters.AddWithValue("@user2", user2id);
                var dateAndTime = DateTime.Now;
                var date = dateAndTime.Date;
                cmd.Parameters.AddWithValue("@data", date.Day + " " +date.Month +" " +date.Year);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "UPDATE Marriage SET serverid = @serverid, user1 = @user2, user2 = @user1, data = @data WHERE serverid=@serverid AND user1 = @user2";
                cmd.Parameters.AddWithValue("@serverid", serverid);
                cmd.Parameters.AddWithValue("@user1", user1id);
                cmd.Parameters.AddWithValue("@user2", user2id);
                cmd.Parameters.AddWithValue("@data", DateTime.UtcNow.Date.ToString());
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }

            public static void BrokeMarriage(ulong serverid, ulong user1id, ulong user2id)
            {
                using var con = new SQLiteConnection(path);
                con.Open();
                using var cmd = new SQLiteCommand(con);
                cmd.CommandText = "UPDATE Marriage SET serverid = @serverid, user1 = @user1, user2 = 0, data = 'none' WHERE serverid=@serverid AND user1 = @user1";
                cmd.Parameters.AddWithValue("@serverid", serverid);
                cmd.Parameters.AddWithValue("@user1", user1id);
                cmd.Parameters.AddWithValue("@user2", user2id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "UPDATE Marriage SET serverid = @serverid, user1 = @user1, user2 = 0, data = 'none' WHERE serverid=@serverid AND user1 = @user2";
                cmd.Parameters.AddWithValue("@serverid", serverid);
                cmd.Parameters.AddWithValue("@user1", user1id);
                cmd.Parameters.AddWithValue("@user2", user2id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
