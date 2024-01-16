using AutoMapper;
using BackendConsultorioSeguros.DTOs;
using BackendConsultorioSeguros.Helpers;
using BackendConsultorioSeguros.Models;
using BackendConsultorioSeguros.Servicios.Contrato;
using ClosedXML.Excel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BackendConsultorioSeguros.Servicios.Implementacion
{
    public class SeguroService : ISeguroService
    {
        private readonly IConfiguration _configuration;
        private DBSEGUROSCHUBBContext _context;
        private string cadena;
        private readonly IMapper _mapper;
        private readonly DataImportService<SeguroDto, Seguro> _importService;
        private readonly DataImportDAOService _importDAOService;
        public SeguroService(DBSEGUROSCHUBBContext context, IConfiguration configuration, IMapper mapper)
        {
            this._context = context;
            this._configuration = configuration;
            //this.cadena = _configuration["ConnectioAutoMapperProfilesnStrings:cadenaSQL"];
            this.cadena = _configuration.GetConnectionString("cadenaSQL");
            this._importService = new DataImportService<SeguroDto, Seguro>(context, mapper);
            this._importDAOService = new DataImportDAOService(configuration);
            this._mapper = mapper;
        }

        public async Task<List<SeguroDto>> GetListSeguros()
        {
            List<SeguroDto> listaSeguro = new();

            using (SqlConnection db = new SqlConnection(cadena))
            {
                try
                {
                    await db.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("ObtenerSegurosActivos", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                SeguroDto seguro = new SeguroDto();
                                seguro.SeguroId = int.Parse(reader["SeguroId"].ToString().Trim());
                                seguro.NombreSeguro = reader["NombreSeguro"].ToString().Trim();
                                seguro.CodigoSeguro = reader["CodigoSeguro"].ToString().Trim();
                                seguro.SumaAsegurada = decimal.Parse(reader["SumaAsegurada"].ToString().Trim());
                                seguro.Prima = decimal.Parse(reader["Prima"].ToString().Trim());
                                seguro.FechaCreacion = DateTime.Parse(reader["FechaCreacion"].ToString().Trim());
                                seguro.Estado = reader["Estado"].ToString().Trim();

                                listaSeguro.Add(seguro);
                            }
                            await db.CloseAsync();
                        }
                    }
                }
                catch (Exception)
                {
                    await db.CloseAsync();
                    throw;
                }
            }
            return listaSeguro;
        }

        public async Task<SeguroDto> GetSeguroById(int seguroId)
        {
            SeguroDto seguro = null;

            using (SqlConnection db = new SqlConnection(cadena))
            {
                try
                {
                    await db.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("ObtenerSeguroPorId", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@SeguroId", seguroId);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                seguro = new SeguroDto();
                                seguro.SeguroId = int.Parse(reader["SeguroId"].ToString().Trim());
                                seguro.NombreSeguro = reader["NombreSeguro"].ToString().Trim();
                                seguro.CodigoSeguro = reader["CodigoSeguro"].ToString().Trim();
                                seguro.SumaAsegurada = decimal.Parse(reader["SumaAsegurada"].ToString().Trim());
                                seguro.Prima = decimal.Parse(reader["Prima"].ToString().Trim());
                                seguro.FechaCreacion = DateTime.Parse(reader["FechaCreacion"].ToString().Trim());
                                seguro.Estado = reader["Estado"].ToString().Trim();
                            }
                            await db.CloseAsync();
                        }
                    }
                }
                catch (Exception)
                {
                    await db.CloseAsync();
                    throw;
                }
            }

            return seguro;
        }

        public Task<SeguroDto> GetSeguroByCodigo(string codigoSeguro)
        {
            throw new NotImplementedException();
        }

        public async Task<SeguroDto> CreateSeguro(SeguroDto seguro)
        {

            //if (!ValidateFields.Validate(seguro))
            //{
            //    throw new ArgumentException("Error de validación. Por favor, verifique los campos.");
            //}

            using (SqlConnection db = new SqlConnection(cadena))
            {
                try
                {
                    await db.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("InsertarSeguro", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@NombreSeguro", seguro.NombreSeguro);
                        cmd.Parameters.AddWithValue("@CodigoSeguro", seguro.CodigoSeguro);
                        cmd.Parameters.AddWithValue("@SumaAsegurada", seguro.SumaAsegurada);
                        cmd.Parameters.AddWithValue("@Prima", seguro.Prima);

                        SqlParameter mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, -1);
                        mensajeParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(mensajeParam);

                        await cmd.ExecuteNonQueryAsync();

                        string mensaje = mensajeParam.Value.ToString();
                        if (!string.IsNullOrEmpty(mensaje) && mensaje != "OK")
                        {
                            throw new ApplicationException(mensaje);
                        }
                        return seguro;
                    }
                }
                catch (Exception)
                {
                    await db.CloseAsync();
                    throw;
                }
            }
        }

        public async Task<SeguroDto> UpdateSeguro(int seguroId, SeguroDto seguro)
        {
            //if (!ValidateFields.Validate(seguro))
            //{
            //    throw new ArgumentException("Error de validación. Por favor, verifique los campos.");
            //}

            using (SqlConnection db = new SqlConnection(cadena))
            {
                try
                {
                    await db.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("ActualizarSeguro", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@SeguroId", seguroId);
                        cmd.Parameters.AddWithValue("@NombreSeguro", seguro.NombreSeguro);
                        cmd.Parameters.AddWithValue("@CodigoSeguro", seguro.CodigoSeguro);
                        cmd.Parameters.AddWithValue("@SumaAsegurada", seguro.SumaAsegurada);
                        cmd.Parameters.AddWithValue("@Prima", seguro.Prima);

                        SqlParameter mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, -1);
                        mensajeParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(mensajeParam);

                        await cmd.ExecuteNonQueryAsync();

                        string mensaje = mensajeParam.Value.ToString();
                        if (!string.IsNullOrEmpty(mensaje) && mensaje != "OK")
                        {
                            throw new ApplicationException(mensaje);
                        }
                        return seguro;
                    }
                }
                catch (Exception)
                {
                    await db.CloseAsync();
                    throw;
                }
            }
            
        }

        public async Task<bool> InactivarSeguro(int seguroId)
        {
            using (SqlConnection db = new SqlConnection(cadena))
            {
                try
                {
                    await db.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("InactivarSeguro", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@SeguroId", seguroId);

                        await cmd.ExecuteNonQueryAsync();

                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    await db.CloseAsync();
                }
            }
        }

        private bool TieneCodigosDuplicados(IEnumerable<Seguro> seguros)
        {
            var codigos = seguros.Select(s => s.CodigoSeguro);
            return codigos.Distinct().Count() != codigos.Count();
        }

        private async Task<bool> ExistenCodigosDuplicadosAsync(IEnumerable<Seguro> seguros)
        {
            var codigos = seguros.Select(s => s.CodigoSeguro).Distinct();
            var existentes = await _context.Seguros
                                           .Where(s => codigos.Contains(s.CodigoSeguro))
                                           .Select(s => s.CodigoSeguro)
                                           .ToListAsync();
            return existentes.Any();
        }

        private async Task<string> ValidarCodigosDuplicadosAsync(IEnumerable<Seguro> seguros)
        {
            if (TieneCodigosDuplicados(seguros))
            {
                return "Error: Hay códigos duplicados en los datos proporcionados.";
            }

            if (await ExistenCodigosDuplicadosAsync(seguros))
            {
                return "Error: Se encontraron códigos de seguro duplicados en la base de datos.";
            }

            return null; // No hay errores de códigos duplicados
        }

        private List<SeguroDto> ConvertExcelToSeguroDtoList(Stream fileStream)
        {
            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheet(1); // Asumiendo que los datos están en la primera hoja
            return worksheet.RowsUsed().Skip(1) // Saltar la fila de encabezados
                            .Select(MapRowToSeguroDto)
                            .ToList();
        }

        private async Task<List<SeguroDto>> ConvertTxtToSeguroDtoList(Stream fileStream)
        {
            var segurosDto = new List<SeguroDto>();
            using (var reader = new StreamReader(fileStream))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    segurosDto.Add(MapLineToSeguroDto(line));
                }
            }
            return segurosDto;
        }

        public async Task<string> ImportarDesdeTxtAsync(Stream fileStream)
        {
            var segurosDto = await ConvertTxtToSeguroDtoList(fileStream);
            var seguros = _mapper.Map<IEnumerable<Seguro>>(segurosDto);
            var codigoDuplicadoError = await ValidarCodigosDuplicadosAsync(seguros);
            if (codigoDuplicadoError != null)
            {
                return codigoDuplicadoError;
            }

            try
            {
                await _importService.ImportData(segurosDto);
                return "Importación desde archivo TXT completada con éxito.";
            }
            catch (Exception ex)
            {
                return $"Error al importar desde TXT: {ex.Message}";
            }
        }
        private SeguroDto MapLineToSeguroDto(string line)
        {
            var parts = line.Split(',');

            string estado = "A";
            if (parts.Length > 4)
            {
                estado = parts[4];
            }
            if (estado != "A" && estado != "I")
            {
                estado = "I";
            }
            return new SeguroDto
            {
                CodigoSeguro = parts[0],
                NombreSeguro = parts[1],
                SumaAsegurada = decimal.Parse(parts[2]),
                Prima = decimal.Parse(parts[3]),
                FechaCreacion = DateTime.Now,
                Estado = estado
            };
        }

        public async Task<string> ImportarDesdeExcelAsync(Stream fileStream)
        {
            var segurosDto = ConvertExcelToSeguroDtoList(fileStream);
            var seguros = _mapper.Map<IEnumerable<Seguro>>(segurosDto);
            var codigoDuplicadoError = await ValidarCodigosDuplicadosAsync(seguros);
            if (codigoDuplicadoError != null)
            {
                return codigoDuplicadoError;
            }
            try
            {
                // Enviar DTOs directamente a DataImportService
                await _importService.ImportData(segurosDto);
                return "Importación desde archivo Excel completada con éxito.";
            }
            catch (Exception ex)
            {
                return $"Error al importar desde Excel: {ex.Message}";
            }
        }

        private SeguroDto MapRowToSeguroDto(IXLRow row)
        {
            string estado = "I"; // Valor predeterminado
            if (row.Cell(5).IsEmpty())
            {
                estado = "A";
            }
            else
            {
                string estadoValue = row.Cell(5).Value.ToString();
                if (estadoValue == "A" || estadoValue == "I")
                {
                    estado = estadoValue; // Utiliza el valor si es "A" o "I".
                }
            }
            return new SeguroDto
            {
                CodigoSeguro = row.Cell(1).Value.ToString(),
                NombreSeguro = row.Cell(2).Value.ToString(),
                SumaAsegurada = decimal.Parse(row.Cell(3).Value.ToString()),
                Prima = decimal.Parse(row.Cell(4).Value.ToString()),
                FechaCreacion = DateTime.Now,
                Estado = estado
            };
        }

        private IEnumerable<IEnumerable<SeguroDto>> ObtenerLotes(IEnumerable<SeguroDto> source, int tamañoDelLote)
        {
            return source.Select((x, i) => new { Index = i, Value = x })
                         .GroupBy(x => x.Index / tamañoDelLote)
                         .Select(x => x.Select(v => v.Value));
        }

        public Dictionary<string, string> CrearMapeoDeColumnas()
        {
            var columnMappings = new Dictionary<string, string>
            {
                {"CodigoSeguro", "CodigoSeguro"},
                {"NombreSeguro", "NombreSeguro"},
                {"SumaAsegurada", "SumaAsegurada"},
                {"Prima", "Prima"},
                {"FechaCreacion", "FechaCreacion"},
                {"Estado", "Estado"}
            };
            return columnMappings;
        }

        public async Task<string> ADOImportarDesdeTxtAsync(Stream fileStream)
        {
            var segurosDto = await ConvertTxtToSeguroDtoList(fileStream);
            var dataTable = ConvertDTOToDataTable(segurosDto);
            var error = await ValidarDuplicadosADO(dataTable);
            if (error != null)
            {
                return error;
            }
            //return await RealizarImportacion(segurosDto, "TuTablaDestino");
            return await _importDAOService.ImportarDatosAsync(dataTable, "Seguros", CrearMapeoDeColumnas());
        }
        public async Task<string> ADOImportarDesdeExcelAsync(Stream fileStream)
        {
            var segurosDto = ConvertExcelToSeguroDtoList(fileStream);
            var dataTable = ConvertDTOToDataTable(segurosDto);
            var error = await ValidarDuplicadosADO(dataTable);
            if (error != null)
            {
                return error;
            }
            //return await RealizarImportacion(segurosDto, "TuTablaDestino");
            return await _importDAOService.ImportarDatosAsync(dataTable, "Seguros", CrearMapeoDeColumnas());
        }

        private async Task<string> ValidarDuplicadosADO(DataTable dataTable)
        {
            if (ADOExistenCodigosDuplicadosEnDataTable(dataTable))
            {
                return "Error: Hay códigos duplicados en los datos proporcionados.";
            }

            if (await ADOExistenCodigosDuplicadosEnBaseDeDatos(dataTable))
            {
                return "Error: Se encontraron códigos de seguro duplicados en la base de datos.";
            }

            return null; // No hay errores
        }

        private bool ADOExistenCodigosDuplicadosEnDataTable(DataTable dataTable)
        {
            var codigos = dataTable.AsEnumerable().Select(row => row["CodigoSeguro"].ToString()).ToList();
            return codigos.Count != codigos.Distinct().Count();
        }

        private async Task<bool> ADOExistenCodigosDuplicadosEnBaseDeDatos(DataTable dataTable)
        {
            using (var connection = new SqlConnection(cadena))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("CheckForDuplicateCodes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    var codesTable = new DataTable();
                    codesTable.Columns.Add("CodigoSeguro", typeof(string));
                    foreach (DataRow row in dataTable.Rows)
                    {
                        codesTable.Rows.Add(row["CodigoSeguro"]);
                    }
                    command.Parameters.AddWithValue("@Codes", codesTable);
                    command.Parameters["@Codes"].SqlDbType = SqlDbType.Structured;
                    var existsParam = new SqlParameter("@Exists", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(existsParam);

                    await command.ExecuteNonQueryAsync();

                    return (int)existsParam.Value == 1;
                }
            }
        }

        private DataTable ConvertDTOToDataTable(IEnumerable<SeguroDto> segurosDto)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("CodigoSeguro", typeof(string)); // VARCHAR(10)
            dataTable.Columns.Add("NombreSeguro", typeof(string)); // VARCHAR(60)
            dataTable.Columns.Add("SumaAsegurada", typeof(decimal)); // DECIMAL(15, 2)
            dataTable.Columns.Add("Prima", typeof(decimal)); // DECIMAL(15, 2)
            dataTable.Columns.Add("FechaCreacion", typeof(DateTime)); // DATETIME
            dataTable.Columns.Add("Estado", typeof(string)); // VARCHAR(1)

            foreach (var seguro in segurosDto)
            {
                dataTable.Rows.Add(
                    seguro.CodigoSeguro,
                    seguro.NombreSeguro,
                    seguro.SumaAsegurada,
                    seguro.Prima,
                    seguro.FechaCreacion == default(DateTime) ? (object)DBNull.Value : seguro.FechaCreacion,
                    seguro.Estado
                );
            }
            return dataTable;
        }
    }
}
