USE Beneficiari
Go
CREATE PROCEDURE sp_StatisticiDashboard
AS
    SELECT
        COUNT(*)                                                        AS Total,
        SUM(CASE WHEN l.Tip = 'Urban' THEN 1 ELSE 0 END)               AS NrUrban,
        SUM(CASE WHEN l.Tip = 'Rural' THEN 1 ELSE 0 END)               AS NrRural,
        CAST(SUM(CASE WHEN l.Tip = 'Urban' THEN 1 ELSE 0 END) * 100.0
             / COUNT(*) AS INT)                                         AS ProcUrban,
        CAST(SUM(CASE WHEN l.Tip = 'Rural' THEN 1 ELSE 0 END) * 100.0
             / COUNT(*) AS INT)                                         AS ProcRural
    FROM Beneficiari b
    JOIN Localitati l ON b.CodLoc = l.CodLoc;
GO