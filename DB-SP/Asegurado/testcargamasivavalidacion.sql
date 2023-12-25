USE DBSEGUROSCHUBB;
GO

CREATE TYPE dbo.CodigoSeguroType AS TABLE 
(
    CodigoSeguro VARCHAR(100)
)
go 

CREATE PROCEDURE CheckForDuplicateCodes
    @Codes dbo.CodigoSeguroType READONLY,
    @Exists INT OUTPUT
AS
BEGIN
    SET @Exists = 0;

    IF EXISTS (
        SELECT 1
        FROM [dbo].[Seguros]
        WHERE CodigoSeguro IN (SELECT CodigoSeguro FROM @Codes)
    )
    BEGIN
        SET @Exists = 1;
    END
END






