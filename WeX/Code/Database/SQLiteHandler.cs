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

            cmd.CommandText = "INSERT INTO mainconfig(serverid, muteroleid) VALUES(@id, 0);";
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
            }
            return mess;
        }

        public static void Update(Messages mess, bool isWelcome, ulong id)
        {
            using var con = new SQLiteConnection(path);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            if (isWelcome)
                cmd.CommandText = "UPDATE WelcomeMessages SET text = @text, ismessageonline = @message, channelid = @id;";
            else
                cmd.CommandText = "UPDATE ByeMessages SET text = @text, ismessageonline = @message, channelid = @id;";
            cmd.Parameters.AddWithValue("@id", mess.channelid);
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
            cmd.CommandText = "UPDATE mainconfig SET serverid = @id, muteroleid = @muteroleid";
            cmd.Parameters.AddWithValue("@id", mess.serverid);
            cmd.Parameters.AddWithValue("@muteroleid", mess.muteroleid);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
