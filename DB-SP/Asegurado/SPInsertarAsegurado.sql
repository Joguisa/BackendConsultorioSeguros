USE DBSEGUROSCHUBB
GO

SET ANSI_NULLS ON
GO
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[InsertarAsegurado]') AND TYPE IN (N'P', N'PC'))
BEGIN
    EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertarAsegurado] AS'
END

GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[InsertarAsegurado]
    @ClienteId INT,
    @SeguroId INT,
    @mensaje NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION

        IF EXISTS (SELECT 1 FROM Clientes WHERE ClienteId = @ClienteId AND Estado = 'A') AND
           EXISTS (SELECT 1 FROM Seguros WHERE SeguroId = @SeguroId AND Estado = 'A')
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM Asegurados WHERE ClienteId = @ClienteId AND SeguroId = @SeguroId)
            BEGIN
                INSERT INTO Asegurados (ClienteId, SeguroId)
                VALUES (@ClienteId, @SeguroId);
                SET @mensaje = 'OK';
            END
            ELSE
            BEGIN
                SET @mensaje = 'Ya existe un registro con el mismo ClienteId y SeguroId.';
            END
        END
        ELSE
        BEGIN
            -- Indicar que el cliente o el seguro está inactivo
            SET @mensaje = 'El cliente o el seguro está inactivo.';
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @mensaje = 'Error ' + ERROR_MESSAGE();
    END CATCH
END;
