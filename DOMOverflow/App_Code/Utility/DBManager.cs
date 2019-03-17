using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using WebMatrix.Data;

/// <summary>
/// Wrapper class for managing connections to the database.
/// </summary>
namespace DOMOverflow {
    public static class DBManager {
        public const string DATABASE_NAME = "DOMOverflow.mdf";

        public static Database Connect() {
            string connstr  = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\" + DATABASE_NAME + ";Integrated Security=True";
            string provider = "System.Data.SqlClient";

            return Database.OpenConnectionString(connstr, provider);
        }


        public static Tuple<byte[], byte[]> HashAndSalt(string password) {
            byte[] s = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(s);

            byte[] h = new Rfc2898DeriveBytes(password, s, 10000).GetBytes(20);

            return new Tuple<byte[], byte[]>(h, s);
        }
    }
}