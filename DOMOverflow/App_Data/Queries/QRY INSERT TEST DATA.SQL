-- ┌────────────────┬───────────────────┬─────────────────────────────┐
-- │ Username:      │ Password:         │ User group:                 │
-- ├────────────────┼───────────────────┼─────────────────────────────┤
-- │ TestUser01     │ TestUserPW        │ Deelnemer Domoticaproject   │
-- │ TestUser02     │ TestUserPW        │ Niet geverifieerd           │
-- │ TestUser03     │ TestUserPW        │ Student NHL-Stenden         │
-- │ TestUser04     │ TestUserPW        │ Docent  NHL-Stenden         │
-- │ TestAdmin      │ TestAdminPW       │ Websitebeheerder            │
-- └────────────────┴───────────────────┴─────────────────────────────┘
-- Don't forget to call QRY DELETE TEST DATA before using the database in production.


-- Users
INSERT INTO Users VALUES(
     'a6cbea09-e17c-48b6-9afe-182b785e87a1',
     1,
     'TestUser01',
     'test_user_01@test.com',
     0xe8cbd26717a27f6c788e326f1f50db91,
     0xfa54605607699d066f01b7ce628d51bdc2965c4b
);

INSERT INTO Users VALUES(
     '5c7114a0-3566-4fff-93c3-f066b0184422',
     0,
     'TestUser02',
     'test_user_02@test.com',
     0xe12031b7a068a3de760878df9d90d991,
     0x2ec9dc63019127b18243147d8f80d0457cbb22c5
);

INSERT INTO Users VALUES(
     '3a170bfa-5e33-413e-9df1-67db22c5f9fe',
     2,
     'TestUser03',
     'test_user_03@test.com',
     0x227de177e2730056f92b89c0a5002354,
     0x2aa65915eb59c40bde5525854e1a3f03eca769ef
);

INSERT INTO Users VALUES(
     '20305112-fc5c-4706-88c4-6aa7cc1841f7',
     3,
     'TestUser04',
     'test_user_04@test.com',
     0x7c48d32ef6556c717acf1afeba2705a3,
     0x96e8c5203cfa6f19135d582095486d551c469b7f
);

INSERT INTO Users VALUES(
     'ed838947-d86f-41b0-9542-3917986c45b7',
     4,
     'TestAdmin',
     'test_admin@test.com',
     0x2a7d4078f463c137a7eee1512ab9fb15,
     0xe6b48dfa63afaa80e6ac641c72e5a396690ac2db
);


-- Posts
INSERT INTO Posts VALUES(     -- Q1
     'f6057eac-07ac-476f-994c-fd037dd27402',
     'a6cbea09-e17c-48b6-9afe-182b785e87a1',
     'QUESTION',
     CURRENT_TIMESTAMP,
     'Waddup bois het is TestUser01 hier met een groot probleem!\n
     Ik wil dat mijn computer ding A doet maar mijn computer doet ding B en ik vindt het helemaal niet leuk!\n
     Pls geef snel antwoord, als mijn computer morgen nog niet ding A doet krijg ik een 1!!!!!!!',
     -3
);

INSERT INTO Questions VALUES(
     'f6057eac-07ac-476f-994c-fd037dd27402',
     NULL,     -- Set later to prevent FK issues.
     'PLS HALP MIJN COMPUTER DOET DING A NIET!!!!!1!!!!!!11!!1!!11!een'
);


INSERT INTO Posts VALUES(     -- Q2
     '8c596511-3a44-4e00-b0f0-d3c49c785b0a',
     '20305112-fc5c-4706-88c4-6aa7cc1841f7',
     'QUESTION',
     CURRENT_TIMESTAMP,
     'Een twee drie vier, Sibbele hier\n
     vijf zes, in wiskunde geef ik les,\n
     zeven acht, omdat ik andere vakken veracht,\n
     negen tien, heeft iemand mijn arduino gezien?',
     225
);

INSERT INTO Questions VALUES(
     '8c596511-3a44-4e00-b0f0-d3c49c785b0a',
     NULL,
     'Arduino'
);


INSERT INTO Posts VALUES(     -- Q1.A1
     'ae3608fb-3983-476c-b13c-0f7b322fd700',
     '3a170bfa-5e33-413e-9df1-67db22c5f9fe',
     'ANSWER',
     CURRENT_TIMESTAMP,
     'Ik weet wel hoe je ding A doet maar ik ga het je alleen vertellen voor 15 euro.',
     -40
);

INSERT INTO Answers VALUES(
     'ae3608fb-3983-476c-b13c-0f7b322fd700',
     '8c596511-3a44-4e00-b0f0-d3c49c785b0a'
);


INSERT INTO Posts VALUES(     -- Q1.A2
     'a8d53ed1-8e7b-4896-a185-c7139163adb1',
     '20305112-fc5c-4706-88c4-6aa7cc1841f7',
     'ANSWER',
     CURRENT_TIMESTAMP,
     'Om ding A te doen heb je 20 jaar programmeer-ervaring nodig\n
     Dat gaat je niet meer lukken voor morgen lol.',
     5
);

INSERT INTO Answers VALUES(
     'a8d53ed1-8e7b-4896-a185-c7139163adb1',
     '8c596511-3a44-4e00-b0f0-d3c49c785b0a'
);


INSERT INTO Posts VALUES(     -- Q2.A1
     '6182d8b7-34e7-467a-b24d-ba34c74f44ae',
     'a6cbea09-e17c-48b6-9afe-182b785e87a1',
     'ANSWER',
     CURRENT_TIMESTAMP,
     'pls meneer rapmeister sibbele kunt u mijn wiskundehuiswerk nakijken???',
     0
);

INSERT INTO Answers VALUES(
     '6182d8b7-34e7-467a-b24d-ba34c74f44ae',
     '8c596511-3a44-4e00-b0f0-d3c49c785b0a'
);


-- Topics
INSERT INTO Topics VALUES (
     'd5b8db4c-3e3b-47ac-813f-365161d022eb',
     'Arduino',
     'Wat als je een computer had\n
     maar hij was klein en je kon er\n
     geen CRYSIS op spelen...'
);

INSERT INTO Topics VALUES (
     '0df9b06e-8a7f-4095-91b3-e086f1b2191a',
     'Epic Sibbele Rap Posts',
     '*mic-drop*'
);

INSERT INTO Topics VALUES (
     '9e8e51e1-48a7-4d03-a386-0653829c28fd',
     'Ding A',
     'Ding A is echt zo''n klote-\n
     technologie degene die\n
     gekozen heeft om hierin les\n
     te gaan geven moet worden\n
     ontslagen.'
);


-- Set answer to question
UPDATE Questions SET Answer = 'a8d53ed1-8e7b-4896-a185-c7139163adb1' WHERE UUID = 'f6057eac-07ac-476f-994c-fd037dd27402';


-- Add topics to questions
INSERT INTO QuestionTopics VALUES (
     'f6057eac-07ac-476f-994c-fd037dd27402',
     '9e8e51e1-48a7-4d03-a386-0653829c28fd'
);

INSERT INTO QuestionTopics VALUES (
     '8c596511-3a44-4e00-b0f0-d3c49c785b0a',
     'd5b8db4c-3e3b-47ac-813f-365161d022eb'
);

INSERT INTO QuestionTopics VALUES (
     '8c596511-3a44-4e00-b0f0-d3c49c785b0a',
     '0df9b06e-8a7f-4095-91b3-e086f1b2191a'
);


-- Reports
INSERT INTO Reports VALUES (
     '24848d8e-b4a2-4b28-9c5b-6484be9f374e',
     '8c596511-3a44-4e00-b0f0-d3c49c785b0a',
     'a6cbea09-e17c-48b6-9afe-182b785e87a1',
     CURRENT_TIMESTAMP,
     'Sibbele heeft mij een 1 gegeven op wiskunde\n
     kunnen jullie hem bannen van de website?'
);

INSERT INTO Reports VALUES (
     'ada50d02-1bbc-4990-914a-ba9cfe3b5bc3',
     'a8d53ed1-8e7b-4896-a185-c7139163adb1',
     '3a170bfa-5e33-413e-9df1-67db22c5f9fe',
     CURRENT_TIMESTAMP,
     'Kunnen jullie deze post verwijderen?\n
     ik sta op het punt 15 euries binnen\n
     te harken en daar kunnen jullie wel in\n
     meedelen als je weet wat ik bedoel ;)'
);