CREATE PROCEDURE ActualizarSeguro
    @SeguroId INT,
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

        -- Validar campos
        IF @NombreSeguro IS NULL OR @CodigoSeguro IS NULL
        BEGIN
            SET @mensaje = 'NombreSeguro y CodigoSeguro son campos obligatorios.';
            RETURN;
        END

        -- Verificar si ya existe un seguro con el mismo código
        IF EXISTS (SELECT 1 FROM Seguros WHERE CodigoSeguro = @CodigoSeguro AND SeguroId <> @SeguroId)
        BEGIN
            SET @mensaje = 'Ya existe un seguro con el mismo código.';
            RETURN;
        END

        -- Actualizar seguro excluyendo fechaCreacion y estado
        UPDATE Seguros
        SET
            NombreSeguro = @NombreSeguro,
            CodigoSeguro = @CodigoSeguro,
            SumaAsegurada = @SumaAsegurada,
            Prima = @Prima
        WHERE SeguroId = @SeguroId AND Estado = 'A';

        SET @mensaje = COALESCE(@mensaje, 'OK');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SET @mensaje = 'Error ' + ERROR_MESSAGE();
    END CATCH;
END;
