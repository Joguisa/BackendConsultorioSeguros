
CREATE PROCEDURE InactivarSeguro
    @SeguroId INT
AS
BEGIN
    UPDATE Seguros
    SET Estado = 'I'
    WHERE SeguroId = @SeguroId;
END;

