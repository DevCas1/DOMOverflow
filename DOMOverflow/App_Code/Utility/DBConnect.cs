using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;

/// <summary>
/// Wrapper class for managing connections to the database.
/// </summary>
namespace DOMOverflow {
    public static class DBConnect {
        /// <summary>
        /// Connect to the given database.
        /// </summary>
        /// <param name="name">The name of the database to connect to.</param>
        /// <returns>The requested database.</returns>
        public static Database Connect(string name) {
            string connstr  = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\" + name + ";Integrated Security=True";
            string provider = "System.Data.SqlClient";

            return Database.OpenConnectionString(connstr, provider);
        }
    }
}