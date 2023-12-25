
CREATE PROCEDURE InsertarSeguro
    @NombreSeguro VARCHAR(60),
    @CodigoSeguro VARCHAR(10),
    @SumaAsegurada DECIMAL(15, 2),
    @Prima DECIMAL(15, 2),
    @mensaje NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @mensaje = '';

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Seguros WHERE CodigoSeguro = @CodigoSeguro)
        BEGIN
            INSERT INTO Seguros (NombreSeguro, CodigoSeguro, SumaAsegurada, Prima)
            VALUES (@NombreSeguro, @CodigoSeguro, @SumaAsegurada, @Prima);

            SELECT SCOPE_IDENTITY() AS SeguroId;
        END
        ELSE
        BEGIN
            SET @mensaje = 'Ya existe un seguro con el mismo código.';
            RETURN
        END

		SET @mensaje = COALESCE(@mensaje, 'OK');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @mensaje = 'Error ' + ERROR_MESSAGE();
    END CATCH;
END;
