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
            string connstr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\" + DATABASE_NAME + ";Integrated Security=True";
            string provider = "System.Data.SqlClient";

            return Database.OpenConnectionString(connstr, provider);
        }


        public static Tuple<byte[], byte[]> HashAndSalt(string password) {
            byte[] s = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(s);

            byte[] h = new Rfc2898DeriveBytes(password, s, 10000).GetBytes(20);

            return new Tuple<byte[], byte[]>(h, s);
        }


        public static byte[] HashWithSalt(string password, byte[] salt) {
            return new Rfc2898DeriveBytes(password, salt, 10000).GetBytes(20);
        }


        /// <summary>
        /// Attempts to register an user to the website.
        /// </summary>
        /// <param name="username">The username of the new user.</param>
        /// <param name="password">The password of the new user.</param>
        /// <param name="email">The email adress of the new user.</param>
        /// <param name="session">The current session.</param>
        /// <param name="error">Output parameter for any error messages that may have been produced.</param>
        /// <returns>Whether or not the registration was successfull.</returns>
        public static bool RegisterUser(string username, string password, string email, HttpSessionStateBase session, out string error) {
            Database db = DBManager.Connect();

            dynamic qryAllowed = db.QuerySingle("SELECT * FROM AllowedEmails WHERE Email=@0", email);
            if (qryAllowed == null) {
                error = "Het email adres staat niet in de lijst van toegestane email adressen.";
                return false;
            }

            dynamic qryUsed = db.QuerySingle("SELECT * FROM Users WHERE Email=@0", email);
            if (qryUsed != null) {
                error = "Het email adres is reeds in gebruik.";
                return false;
            }

            int group = qryAllowed.GroupID;
            var pwd = DBManager.HashAndSalt(password);
            Guid id = Guid.NewGuid();

            int changes = db.Execute(@"
                BEGIN
                    IF NOT EXISTS (SELECT * FROM USERS WHERE Username=@2)
                    BEGIN
                        INSERT INTO USERS VALUES (@0, @1, @2, @3, @4, @5)
                    END
                END
                ",
                id,
                group,
                username,
                email,
                pwd.Item2,
                pwd.Item1
            );

            if (changes == 0) {
                error = "Kon de gebruikersinformatie niet in de database opslaan.";
                return false;
            } else {
                return DBManager.LoginUser(username, password, session, out error);
            }
        }


        /// <summary>
        /// Attempts to login an user to the website.
        /// </summary>
        /// <param name="username">The users username.</param>
        /// <param name="password">The users password.</param>
        /// <param name="session">The current session.</param>
        /// <param name="error">Output parameter for any error messages that may have been produced.</param>
        /// <returns>Whether or not the user was logged in.</returns>
        public static bool LoginUser(string username, string password, HttpSessionStateBase session, out string error) {
            Database db = DBManager.Connect();

            dynamic user = db.QuerySingle("SELECT * FROM Users WHERE Username=@0", username);
            if (user == null) {
                error = "Er is geen gebruiker met de naam " + username + " geregistreerd.";
                return false;
            }

            byte[] hash = DBManager.HashWithSalt(password, user.PwdSalt);

            bool matches = true;
            for (int i = 0; i < hash.Length; i++) if (hash[i] != user.PwdHash[i]) matches = false;

            if (!matches) {
                error = "Er is een onjuist wachtwoord ingevoerd.";
                return false;
            }

            session["UserSession"] = new User(username, user.Email, Guid.Parse(user.UUID), (UserGroup)user.UserGroup);
            error = "Geen fouten";
            return true;
        }


        public static void LogoutUser(HttpSessionStateBase session) {
            session.Remove("UserSession");
        }


        public static User GetLoggedInUser(HttpSessionStateBase session) {
            return (User)session["UserSession"];
        }

        
        public static Topic CreateTopic(string name, string description) {
            throw new NotImplementedException();
        }


        public static List<Topic> GetTopics() {
            throw new NotImplementedException();
        }


        public static void PostQuestion(string title, string content, List<Topic> topics, DateTime creation, Guid user) {
            throw new NotImplementedException();
        }


        public static void PostAnswer(string content, DateTime creation, Guid question, Guid user) {
            throw new NotImplementedException();
        }


        public static void MarkAsSolution(Guid question, Guid answer) {
            throw new NotImplementedException();
        }
    }
}