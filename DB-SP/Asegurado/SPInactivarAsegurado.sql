USE DBSEGUROSCHUBB
GO
SET ANSI_NULLS ON
GO
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[InactivarAsegurado]') AND TYPE IN (N'P', N'PC'))
BEGIN
  EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InactivarAsegurado] AS'
END

GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[InactivarAsegurado]
  @AseguradoId INT,
  @mensaje NVARCHAR(MAX) OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  BEGIN TRY
    BEGIN TRANSACTION

    -- Verificar si el asegurado ya está inactivo
    IF NOT EXISTS (SELECT 1 FROM Asegurados WHERE AseguradoId = @AseguradoId AND Estado = 'I')
    BEGIN
      -- Realizar la eliminación lógica si el asegurado existe
      IF EXISTS (SELECT 1 FROM Asegurados WHERE AseguradoId = @AseguradoId)
      BEGIN
        UPDATE Asegurados
        SET Estado = 'I'
        WHERE AseguradoId = @AseguradoId;

        SET @mensaje = 'Eliminación lógica realizada correctamente.';
      END
      ELSE
      BEGIN
        SET @mensaje = 'No se encontró el asegurado con el ID especificado.';
      END
    END
    ELSE
    BEGIN
      SET @mensaje = 'El asegurado ya está inactivo.';
    END

    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;

    SET @mensaje = 'Error ' + ERROR_MESSAGE();
  END CATCH
END;
