CREATE PROCEDURE ObtenerSeguroPorId
    @SeguroId INT
AS
BEGIN
    SELECT * FROM Seguros (nolock) WHERE SeguroId = @SeguroId AND Estado = 'A';
END;