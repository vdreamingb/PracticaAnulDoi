--  Creare baza de date

CREATE DATABASE Beneficiari;
GO
USE Beneficiari;
GO

--  Tabele

CREATE TABLE Localitati (
    CodLoc  INT IDENTITY(1,1) PRIMARY KEY,
    NumeLoc VARCHAR(100) NOT NULL,
    Tip     VARCHAR(10)  NOT NULL CHECK (Tip IN ('Urban','Rural')),
    Judet   VARCHAR(60)  NOT NULL
);

CREATE TABLE Beneficiari (
    CodBen  INT IDENTITY(1,1) PRIMARY KEY,
    NrBen   INT          NOT NULL DEFAULT 0,
    Nume    VARCHAR(100) NOT NULL,
    Prenume VARCHAR(100) NOT NULL,
    Adresa  VARCHAR(200),
    Telefon VARCHAR(20),
    Email   VARCHAR(100) NOT NULL CHECK (Email LIKE '%@%.%'),
    CodLoc  INT NOT NULL FOREIGN KEY REFERENCES Localitati(CodLoc)
);
GO


INSERT INTO Localitati (NumeLoc, Tip, Judet) VALUES
('Chisinau',        'Urban', 'Chisinau'),
('Balti',           'Urban', 'Balti'),
('Cahul',           'Urban', 'Cahul'),
('Sangerei',        'Rural', 'Sangerei'),
('Bilicenii Vechi', 'Rural', 'Sangerei'),
('Floreni',         'Rural', 'Anenii Noi');

INSERT INTO Beneficiari (NrBen, Nume, Prenume, Adresa, Telefon, Email, CodLoc) VALUES
(1,  'Popescu',  'Andrei',  'str. Stefan cel Mare 1', '069123456', 'a.popescu@mail.md',  1),
(2,  'Lungu',    'Maria',   's. Bilicenii Vechi 14',  '078234567', 'm.lungu@mail.md',    5),
(3,  'Rusu',     'Ion',     'str. Independentei 5',   '060345678', 'i.rusu@mail.md',     2),
(4,  'Cojocaru', 'Elena',   'bd. Decebal 12',         '079456789', 'e.cojocaru@mail.md', 1),
(5,  'Botnaru',  'Vasile',  's. Floreni nr. 3',       '069567890', 'v.botnaru@mail.md',  6),
(6,  'Grosu',    'Tatiana', 'str. Trandafirilor 8',   '078678901', 't.grosu@mail.md',    3),
(0,  'Test',     'User',    'str. Test 0',            '000000000', 'test@test.md',       4);
GO

--  1. Adauga beneficiar nou

CREATE PROCEDURE sp_Adauga
    @NrBen INT, @Nume VARCHAR(100), @Prenume VARCHAR(100),
    @Adresa VARCHAR(200), @Telefon VARCHAR(20),
    @Email VARCHAR(100), @CodLoc INT
AS
    INSERT INTO Beneficiari (NrBen,Nume,Prenume,Adresa,Telefon,Email,CodLoc)
    VALUES (@NrBen,@Nume,@Prenume,@Adresa,@Telefon,@Email,@CodLoc);
GO

--  2. Exclude dupa cod

CREATE PROCEDURE sp_ExcludeDupaCod @CodBen INT
AS
    DELETE FROM Beneficiari WHERE CodBen = @CodBen;
GO

--  3. Exclude NrBen = 0

CREATE PROCEDURE sp_ExcludeNrBenZero
AS
    DELETE FROM Beneficiari WHERE NrBen = 0;
GO

--  4. Lista alfabetica

CREATE PROCEDURE sp_ListaAlfabetica
AS
    SELECT b.CodBen, b.Nume, b.Prenume, b.Telefon, b.Email, l.NumeLoc, l.Tip
    FROM Beneficiari b
    JOIN Localitati l ON b.CodLoc = l.CodLoc
    ORDER BY b.Nume;
GO

--  5. Raport Urban / Rural

CREATE PROCEDURE sp_RaportUrbanRural
AS
    SELECT
        SUM(CASE WHEN l.Tip='Urban' THEN 1 ELSE 0 END) AS NrUrban,
        SUM(CASE WHEN l.Tip='Rural' THEN 1 ELSE 0 END) AS NrRural
    FROM Beneficiari b
    JOIN Localitati l ON b.CodLoc = l.CodLoc;
GO

--  6. Beneficiari dupa localitate

CREATE PROCEDURE sp_DupaLocalitate @CodLoc INT
AS
    SELECT b.CodBen, b.Nume, b.Prenume, b.Telefon, b.Email
    FROM Beneficiari b
    WHERE b.CodLoc = @CodLoc
    ORDER BY b.Nume;
GO

--  7. Localitati cu cei mai multi beneficiari

CREATE PROCEDURE sp_TopLocalitati
AS
    SELECT TOP 3 l.NumeLoc, l.Tip, COUNT(*) AS NrBeneficiari
    FROM Beneficiari b
    JOIN Localitati l ON b.CodLoc = l.CodLoc
    GROUP BY l.NumeLoc, l.Tip
    ORDER BY NrBeneficiari DESC;
GO

--  8. Tabela beneficiari rurali

CREATE PROCEDURE sp_CreeazaRurali
AS
BEGIN
    IF OBJECT_ID('BeneficiariRurali') IS NOT NULL
        DROP TABLE BeneficiariRurali;

    SELECT b.CodBen, b.Nume, b.Prenume, b.Adresa, b.Telefon, b.Email, l.NumeLoc
    INTO BeneficiariRurali
    FROM Beneficiari b
    JOIN Localitati l ON b.CodLoc = l.CodLoc
    WHERE l.Tip = 'Rural';
END;
GO

--  9. Afisare contact (email, telefon, adresa)

CREATE PROCEDURE sp_Contact
AS
    SELECT b.Nume, b.Prenume, b.Email, b.Telefon, b.Adresa, l.NumeLoc
    FROM Beneficiari b
    JOIN Localitati l ON b.CodLoc = l.CodLoc
    ORDER BY b.Nume;
GO

--  Teste rapide

EXEC sp_ListaAlfabetica;
EXEC sp_RaportUrbanRural;
EXEC sp_Contact;