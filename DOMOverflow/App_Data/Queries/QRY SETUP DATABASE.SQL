-- Create required tables.
DROP TABLE IF EXISTS Users;
CREATE TABLE Users(
     UUID      CHAR(36)       NOT NULL PRIMARY KEY,                        -- C# GUID
     UserGroup INT            NOT NULL DEFAULT(0),                         -- Foreign key: references UserGroups.GroupID. The default UserGroup is 'Niet Geverifieerd'. (0)
     Username  NVARCHAR(127)  NOT NULL,
     Email     NVARCHAR(127)  NOT NULL,
     PwdSalt   BINARY(16)     NOT NULL,                                    -- Salt to prevent lookup attacks on the password hash.
     PwdHash   BINARY(20)     NOT NULL                                     -- Salted PBKDF2 hash of the password.
);


DROP TABLE IF EXISTS UserGroups;                                           -- The group an user belongs to. (student, docent, etc.)
CREATE TABLE UserGroups(
     GroupID   INT            NOT NULL PRIMARY KEY IDENTITY(0, 1),
     GroupName NVARCHAR(127)  NOT NULL
);


DROP TABLE IF EXISTS Posts;
CREATE TABLE Posts(                                                        -- Base class for other post types.
     UUID      CHAR(36)       NOT NULL PRIMARY KEY,                        -- C# GUID
     Poster    CHAR(36)       NOT NULL,                                    -- Foreign key: references Users.UUID.
     PostType  VARCHAR(16)    NOT NULL,                                    -- Foreign key: references PostTypes.PostType.
     PostDate  DATETIME       NOT NULL,
     Content   NTEXT          NOT NULL,                                    -- The post's text body. Lines are seperated with UNIX-style line endings. (\n)
     Rating    INT            NOT NULL DEFAULT(0)                          -- The post's rating. (upvotes - downvotes) The default rating is 0.
);


DROP TABLE IF EXISTS PostTypes;                                            -- What kind of post the given post is. (question, answer, comment, etc.)
CREATE TABLE PostTypes(
     PostType  VARCHAR(16)    NOT NULL PRIMARY KEY
);


DROP TABLE IF EXISTS Questions;
CREATE TABLE Questions(                                                    -- Child class of Posts.
     UUID      CHAR(36)       NOT NULL PRIMARY KEY,                        -- Foreign key: references Posts.UUID. C# GUID. Used for inheritance.
     Answer    CHAR(36),                                                   -- Foreign key: references Answers.UUID. C# GUID. The answer that has been marked as correct, if one exists.
     Title     NVARCHAR(127)  NOT NULL
);


DROP TABLE IF EXISTS Answers;
CREATE TABLE Answers(                                                      -- Child class of Posts.
     UUID      CHAR(36)       NOT NULL PRIMARY KEY,                        -- Foreign key: references Posts.UUID. C# GUID. Used for inheritance.
     Question  CHAR(36)       NOT NULL,                                    -- Foreign key: references Questions.UUID. C# GUID. The question this answer belongs to.
);


DROP TABLE IF EXISTS Topics;
CREATE TABLE Topics(
     UUID      CHAR(36)       NOT NULL PRIMARY KEY,                        -- C# GUID.
     TopicName NVARCHAR(127)  NOT NULL,
     TopicDesc NTEXT          NOT NULL                                     -- Lines are seperated with UNIX-style line endings. (\n)
);


DROP TABLE IF EXISTS QuestionTopics;
CREATE TABLE QuestionTopics(                                               -- Which topics a question has.
     Question  CHAR(36)       NOT NULL,                                    -- Foreign key: references Questions.UUID. C# GUID.
     Topic     CHAR(36)       NOT NULL,                                    -- Foreign key: references Topics.UUID. C# GUID.
     PRIMARY KEY (Question, Topic)
);


DROP TABLE IF EXISTS Reports;
CREATE TABLE Reports(
     UUID      CHAR(36)       NOT NULL PRIMARY KEY,                        -- C# GUID.
     Post      CHAR(36)       NOT NULL,                                    -- Foreign key: references Posts.UUID. C# GUID.
     Reporter  CHAR(36)       NOT NULL,                                    -- Foreign key: references Users.UUID. C# GUID.
     RepDate   DATETIME       NOT NULL,
     RepText   NTEXT          NOT NULL                                     -- Lines are seperated with UNIX-style line endings. (\n)
);


DROP TABLE IF EXISTS AllowedEmails;
CREATE TABLE AllowedEmails(                                                -- Email adresses that are allowed to register for the website.
     Email     NVARCHAR(127)  NOT NULL PRIMARY KEY,
     GroupID   INT            NOT NULL                                     -- Foreign key: references UserGroups.GroupID.
);


-- Create foreign keys.
ALTER TABLE Users             ADD FOREIGN KEY (UserGroup)        REFERENCES UserGroups(GroupID);

ALTER TABLE Posts             ADD FOREIGN KEY (Poster)           REFERENCES Users(UUID);
ALTER TABLE Posts             ADD FOREIGN KEY (PostType)         REFERENCES PostTypes(PostType);

ALTER TABLE Questions         ADD FOREIGN KEY (UUID)             REFERENCES Posts(UUID);
ALTER TABLE Questions         ADD FOREIGN KEY (Answer)           REFERENCES Answers(UUID);

ALTER TABLE Answers           ADD FOREIGN KEY (UUID)             REFERENCES Posts(UUID);
ALTER TABLE Answers           ADD FOREIGN KEY (Question)         REFERENCES Questions(UUID);

ALTER TABLE QuestionTopics    ADD FOREIGN KEY (Question)         REFERENCES Questions(UUID);
ALTER TABLE QuestionTopics    ADD FOREIGN KEY (Topic)            REFERENCES Topics(UUID);

ALTER TABLE Reports           ADD FOREIGN KEY (Post)             REFERENCES Posts(UUID);
ALTER TABLE Reports           ADD FOREIGN KEY (Reporter)         REFERENCES Users(UUID);

ALTER TABLE AllowedEmails     ADD FOREIGN KEY (GroupID)          REFERENCES UserGroups(GroupID);

-- Insert required values into the database.
-- Do not change the ordering of these records.
INSERT INTO UserGroups VALUES ('Niet Geverifieerd');
INSERT INTO UserGroups VALUES ('Deelnemer Domoticaproject');
INSERT INTO UserGroups VALUES ('Student NHL-Stenden');
INSERT INTO UserGroups VALUES ('Docent NHL-Stenden');
INSERT INTO UserGroups VALUES ('Websitebeheerder');

INSERT INTO PostTypes VALUES ('QUESTION');
INSERT INTO PostTypes VALUES ('ANSWER');