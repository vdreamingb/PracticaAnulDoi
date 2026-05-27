CREATE TRIGGER trg_BlockZeroNrBen
ON Beneficiari
INSTEAD OF INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM inserted WHERE NrBen = 0)
    BEGIN
        RAISERROR('NrBen nu poate fi zero.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Handle INSERT
    IF NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO Beneficiari (NrBen, Nume, Prenume, Adresa, Telefon, Email, CodLoc)
        SELECT NrBen, Nume, Prenume, Adresa, Telefon, Email, CodLoc
        FROM inserted;
    END
    ELSE
    -- Handle UPDATE
    BEGIN
        UPDATE b
        SET
            b.NrBen     = i.NrBen,
            b.Nume      = i.Nume,
            b.Prenume   = i.Prenume,
            b.Adresa    = i.Adresa,
            b.Telefon   = i.Telefon,
            b.Email     = i.Email,
            b.CodLoc    = i.CodLoc
        FROM Beneficiari b
        INNER JOIN inserted i ON b.CodBen = i.CodBen;
    END
END;
