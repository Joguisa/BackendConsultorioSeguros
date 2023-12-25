CREATE PROCEDURE ObtenerSegurosActivos
AS
BEGIN
    SELECT * FROM Seguros (nolock) WHERE Estado = 'A';
END;