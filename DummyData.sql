INSERT INTO public."Establishment" ("EstablishmentAddressId", "Type", "ReceiveTime")
VALUES  (1, 1, CURRENT_TIMESTAMP),
		(2, 2, CURRENT_TIMESTAMP),
		(3, 3, CURRENT_TIMESTAMP);

INSERT INTO public."EstablishmentAddress" ("Country", "City", "Street", "StreetNumber", "ReceiveTime", "EstablishmentId")
VALUES ('Lithuania', 'Vilnius', 'Naugarduko', '24', CURRENT_TIMESTAMP, 1),
       ('Lithuania', 'Kaunas', 'Laisvės', '24', CURRENT_TIMESTAMP, 2),
       ('Lithuania', 'Vilnius', 'Didlaukio', '59', CURRENT_TIMESTAMP, 3);

<<<<<<< HEAD
INSERT INTO public."Employee" ("Title", "EstablishmentId", "AddressId", "PersonalCode", "FirstName", "LastName", "BirthDate", "ReceiveTime", "Phone", "Email", "PasswordHash")
VALUES  (100, 1, 1, '50303060000', 'John', 'Krasinski', '2003-03-06', CURRENT_TIMESTAMP, '+37061234556', 'john.krasinski@gmail.com', '$2a$12$PhK89F7LE/YEyiVhd2wG1eoZoS4arsYu06WwOYIsH9IvKZw5Ev1cO'),
		(2, 1, 1, '40303060000', 'Emily', 'Black', '2000-04-20', CURRENT_TIMESTAMP, '+37067658110', 'emily.black@gmail.com', '$2a$12$PhK89F7LE/YEyiVhd2wG1eoZoS4arsYu06WwOYIsH9IvKZw5Ev1cO');
=======
INSERT INTO public."Employee" ("Title", "EstablishmentId", "AddressId", "PersonalCode", "FirstName", "LastName", "BirthDate", "ReceiveTime", "Phone", "Email")
VALUES  (100, 1, 1, '50303060000', 'John', 'Krasinski', '2003-03-06', CURRENT_TIMESTAMP, '+37061234556', 'john.krasinski@gmail.com'),
		(2, 1, 1, '40303060000', 'Emily', 'Black', '2000-04-20', CURRENT_TIMESTAMP, '+37067658110', 'emily.black@gmail.com');
>>>>>>> main
		
INSERT INTO public."EmployeeAddress" ("Country", "City", "Street", "StreetNumber", "HouseNumber", "ReceiveTime", "EmployeeId")
VALUES  ('Lithuania', 'Vilnius', 'Liepyno', '24', '1', CURRENT_TIMESTAMP, 1),
		('Lithuania', 'Vilnius', 'Kauno', '24', '2', CURRENT_TIMESTAMP, 2);
		
INSERT INTO public."Item" ("Name", "Cost", "Tax", "AlcoholicBeverage", "ReceiveTime") 
VALUES 
    ('Red Wine', 12.99, 1.21, TRUE, CURRENT_TIMESTAMP),
    ('Beer', 3.50, 0.70, TRUE, CURRENT_TIMESTAMP),
    ('Orange Juice', 2.99, 0.60, FALSE, CURRENT_TIMESTAMP),
    ('Vodka', 15.00, 3.00, TRUE, CURRENT_TIMESTAMP),
    ('Mineral Water', 1.20, 0.24, FALSE, CURRENT_TIMESTAMP),
    ('Whiskey', 25.00, 5.00, TRUE, CURRENT_TIMESTAMP),
    ('Soda', 1.50, 0.30, FALSE, CURRENT_TIMESTAMP),
    ('Gin', 18.50, 3.70, TRUE, CURRENT_TIMESTAMP),
    ('Apple Juice', 2.75, 0.55, FALSE, CURRENT_TIMESTAMP),
    ('Tequila', 22.00, 4.40, TRUE, CURRENT_TIMESTAMP);

INSERT INTO public."Storage" ("EstablishmentId", "ItemId", "Count") 
VALUES 
    (1, 1, 100),
    (1, 2, 150),
    (1, 3, 200),
    (1, 4, 50),
    (1, 5, 300),
    (1, 6, 75),
    (1, 7, 250),
    (1, 8, 60),
    (1, 9, 180),
    (1, 10, 40);
