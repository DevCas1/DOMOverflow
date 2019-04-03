using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            return (User) session["UserSession"];
        }


        /// <summary>
        /// Attempts to create a new topic.
        /// </summary>
        /// <param name="name">The topic's name.</param>
        /// <param name="description">The topic's description.</param>
        /// <param name="error">Output parameter for any error messages that may have been produced.</param>
        /// <returns>An object representation of the new topic, or null if there was an error.</returns>
        public static Topic CreateTopic(string name, string description, out string error) {
            Database db = DBManager.Connect();

            Guid id = Guid.NewGuid();

            int changes = db.Execute(@"
                BEGIN
                    IF NOT EXISTS (SELECT * FROM Topics WHERE UUID=@0)
                    BEGIN
                        INSERT INTO Topics VALUES (@0, @1, @2)
                    END
                END
                ",
                id.ToString(),
                name,
                description
            );

            if (changes == 0) {
                error = "De gegeven topic bestaat al.";
                return null;
            }

            error = "Geen fouten.";
            return new Topic(id, name, description);
        }


        /// <summary>
        /// Get a list of all topics that currently exist.
        /// </summary>
        /// <returns>A list of all currently existing topics.</returns>
        public static List<Topic> GetTopics() {
            Database db = DBManager.Connect();

            List<Topic> result = new List<Topic>();
            var qryTopics = db.Query("SELECT * FROM Topics");

            foreach (dynamic topic in qryTopics) result.Add(new Topic(Guid.Parse(topic.UUID), topic.TopicName, topic.TopicDesc));

            return result;
        }


        /// <summary>
        /// Attempt to insert a question into the database.
        /// </summary>
        /// <param name="question">The question to be inserted</param>
        /// <throws>An ExternalException if the database fails to complete one or more of the required queries.</throws>
        public static void PostQuestion(Question question) {
            Database db = DBManager.Connect();

            int changesPost = db.Execute(@"
                BEGIN
                    IF NOT EXISTS (SELECT * FROM Posts WHERE UUID=@0)
                    BEGIN
                        INSERT INTO Posts     VALUES (@0, @1, @2, @3, @4, @5);
                        INSERT INTO Questions VALUES (@0, @6, @7);
                    END
                END
                ",
                question.UUID.ToString(),
                question.PosterID.ToString(),
                question.Type.ToString(),
                question.PostDate,
                question.Content,
                question.Rating,
                question.Answer == null ? null : question.Answer.ToString(),
                question.Title
            );

            if (changesPost == 0) throw new ExternalException("Er is een fout opgetreden tijdens het toevoegen van de post " + question.UUID.ToString() + " in de database.");

            foreach (Topic topic in question.Topics) {
                int changesTopic = db.Execute("INSERT INTO QuestionTopics VALUES (@0, @1)", question.UUID.ToString(), topic.UUID.ToString());
                if (changesTopic == 0) throw new ExternalException("Er is een fout opgetreden tijdens het toevoegen van de topic " + topic.name + " aan de vraag " + question.UUID.ToString());
            }
        }


        /// <summary>
        /// Attempt to insert an answer into the database.
        /// </summary>
        /// <param name="answer">The answer to be inserted</param>
        /// <throws>An ExternalException if the database fails to complete one or more of the required queries.</throws>
        public static void PostAnswer(Answer answer) {
            Database db = DBManager.Connect();

            int changesPost = db.Execute(@"
                BEGIN
                    IF NOT EXISTS (SELECT * FROM Posts WHERE UUID=@0)
                    BEGIN
                        INSERT INTO Posts   VALUES (@0, @1, @2, @3, @4, @5);
                        INSERT INTO Answers VALUES (@0, @6);
                    END
                END
                ",
                answer.UUID.ToString(),
                answer.PosterID.ToString(),
                answer.Type.ToString(),
                answer.PostDate,
                answer.Content,
                answer.Rating,
                answer.Question.ToString()
            );

            if (changesPost == 0) throw new ExternalException("Er is een fout opgetreden tijdens het toevoegen van de post " + answer.UUID.ToString() + " in de database.");
        }


        /// <summary>
        /// Marks the given answer as the solution to the given question.
        /// </summary>
        /// <param name="question">The question</param>
        /// <param name="answer">The answer</param>
        /// <throws>An ExternalException if the database fails to complete one or more of the required queries.</throws>
        public static void MarkAsSolution(Question question, Answer answer) {
            Database db = DBManager.Connect();

            int changes = db.Execute("UPDATE Questions SET Answer=@0 WHERE UUID=@1", answer.UUID.ToString(), question.UUID.ToString());
            if (changes == 0) throw new ExternalException("Er is een fout opgetreden tijdens het markeren van het antwoord " + answer.UUID.ToString() + " op de vraag " + question.UUID.ToString());
        }

        
        /// <summary>
        /// Get the question with the given ID.
        /// </summary>
        /// <param name="id">The ID of the question.</param>
        /// <returns>The question, or null if no such question exists.</returns>
        public static Question GetQuestion(Guid id) {
            Database db = DBManager.Connect();

            dynamic qry = db.QuerySingle(@"
                SELECT 
                    Poster,  
                    PostDate, 
                    Content, 
                    Rating, 
                    Title, 
                    Answer
                FROM Posts INNER JOIN Questions ON Posts.UUID = Questions.UUID
                WHERE Posts.UUID = @0",
                id.ToString()
            );

            if (qry == null) return null;

            IEnumerable<dynamic> topicsQry = db.Query(@"
                SELECT
                    UUID,
                    TopicName,
                    TopicDesc
                FROM QuestionTopics INNER JOIN Topics ON QuestionTopics.Topic = Topics.UUID
                WHERE QuestionTopics.Question = @0
                ",
                id.ToString()
            );

            List<Topic> topics = new List<Topic>();
            foreach (dynamic topic in topicsQry) topics.Add(new Topic(Guid.Parse(topic.UUID), topic.TopicName, topic.TopicDesc));


            return new Question(
                id,
                Guid.Parse(qry.Poster),
                qry.PostDate,
                qry.Title,
                qry.Content,
                qry.Rating,
                qry.Answer == null ? null : Guid.Parse(qry.Answer),
                topics
            );
        }


        /// <summary>
        /// Get the answer with the given ID.
        /// </summary>
        /// <param name="id">The ID of the answer.</param>
        /// <returns>The answer, or null if no such answer exists.</returns>
        public static Answer GetAnswer(Guid id) {
            Database db = DBManager.Connect();

            dynamic qry = db.QuerySingle(@"
                SELECT 
                    Poster,  
                    PostDate, 
                    Content, 
                    Rating, 
                    Question
                FROM Posts INNER JOIN Answers ON Posts.UUID = Answers.UUID
                WHERE Posts.UUID = @0",
                id.ToString()
            );

            if (qry == null) return null;
            return new Answer(id, Guid.Parse(qry.Poster), Guid.Parse(qry.Question), qry.PostDate, qry.Content, qry.Rating);
        } 


        public static List<Answer> GetAnswersForQuestion(Question question) {
            Database db = DBManager.Connect();

            IEnumerable<dynamic> qry = db.Query(@"
                SELECT
                    Posts.UUID AS UUID,
                    Poster,  
                    PostDate, 
                    Content, 
                    Rating, 
                    Question
                FROM Posts INNER JOIN Answers ON Posts.UUID = Answers.UUID
                WHERE Answers.Question = @0",
                question.UUID.ToString()
            );

            List<Answer> results = new List<Answer>();
            foreach (dynamic answer in qry) results.Add(new Answer(Guid.Parse(answer.UUID), Guid.Parse(answer.Poster), Guid.Parse(answer.Question), answer.PostDate, answer.Content, answer.Rating));

            return results;
        }
    }
}