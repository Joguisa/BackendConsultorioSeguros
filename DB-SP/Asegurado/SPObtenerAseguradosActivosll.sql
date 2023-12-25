USE DBSEGUROSCHUBB
GO

SET ANSI_NULLS ON
GO
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[ObtenerAseguradosActivos]') AND TYPE IN (N'P', N'PC'))
BEGIN
    EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ObtenerAseguradosActivos] AS'
END

GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[ObtenerAseguradosActivos]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        A.AseguradoId,
        A.ClienteId,
        A.SeguroId,
        A.Estado,
        A.FechaCreacion,
        C.Cedula AS ClienteCedula,
        C.NombreCliente AS NombreCliente,
        C.Telefono AS ClienteTelefono,
        C.Edad AS ClienteEdad,
		C.FechaCreacion AS ClienteFechaCreacion,
		C.Estado AS ClienteEstado,
        S.NombreSeguro AS NombreSeguro,
        S.CodigoSeguro AS CodigoSeguro,
        S.SumaAsegurada AS SumaAsegurada,
        S.Prima AS Prima,
		S.FechaCreacion AS FechaCreacion,
		S.Estado AS Estado
    FROM Asegurados A
    INNER JOIN Clientes C ON A.ClienteId = C.ClienteId
    INNER JOIN Seguros S ON A.SeguroId = S.SeguroId
    WHERE A.Estado = 'A';
END;
