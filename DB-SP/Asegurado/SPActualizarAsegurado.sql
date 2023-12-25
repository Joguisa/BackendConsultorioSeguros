USE DBSEGUROSCHUBB
GO
SET ANSI_NULLS ON
GO
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[ActualizarAsegurado]') AND TYPE IN (N'P', N'PC'))
BEGIN
    EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ActualizarAsegurado] AS'
END

GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[ActualizarAsegurado]
    @AseguradoId INT,
    @ClienteId INT,
    @SeguroId INT,
    @mensaje NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION

        -- Verificar si ya existe otro registro con el mismo ClienteId y SeguroId
        IF NOT EXISTS (SELECT 1 FROM Asegurados WHERE ClienteId = @ClienteId AND SeguroId = @SeguroId AND AseguradoId <> @AseguradoId)
        BEGIN
            -- Verificar si el cliente y el seguro tienen estado 'A'
            IF EXISTS (SELECT 1 FROM Clientes WHERE ClienteId = @ClienteId AND Estado = 'A') AND
               EXISTS (SELECT 1 FROM Seguros WHERE SeguroId = @SeguroId AND Estado = 'A')
            BEGIN
                -- Realizar la actualización
                UPDATE Asegurados
                SET ClienteId = @ClienteId,
                    SeguroId = @SeguroId
                WHERE AseguradoId = @AseguradoId;

                SET @mensaje = 'OK';
            END
            ELSE
            BEGIN
                SET @mensaje = 'El cliente o el seguro tienen estado inactivo (I).';
            END
        END
        ELSE
        BEGIN
            SET @mensaje = 'Ya existe un registro con el mismo ClienteId y SeguroId.';
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

        SET @mensaje = 'Error ' + ERROR_MESSAGE();
    END CATCH
END;
