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
    public class ClienteService : IClienteService
    {
        private readonly IConfiguration _configuration;
        private readonly DBSEGUROSCHUBBContext _context;
        private string cadena;
        private readonly IMapper _mapper;
        private readonly DataImportService<ClienteDto, Cliente> _importService;
        private readonly DataImportDAOService _importDAOService;

        public ClienteService(DBSEGUROSCHUBBContext context, IConfiguration configuration, IMapper mapper)
        {
            this._context = context;
            this._configuration = configuration;
            //this.cadena = _configuration["ConnectioAutoMapperProfilesnStrings:cadenaSQL"];
            this.cadena = _configuration.GetConnectionString("cadenaSQL");
            this._importService = new DataImportService<ClienteDto, Cliente>(context, mapper);
            this._importDAOService = new DataImportDAOService(configuration);
            this._mapper = mapper;
        }

        public async Task<List<ClienteDto>> GetListClientes()
        {
            try
            {
                var clientes = await _context.Clientes
                    .Where(c => c.Estado == "A")
                    .ToListAsync();
                var listaClientes = clientes.Select(c => new ClienteDto
                {
                    ClienteId = c.ClienteId,
                    Cedula = c.Cedula,
                    NombreCliente = c.NombreCliente,
                    Telefono = c.Telefono,
                    Edad = c.Edad,
                    FechaCreacion = c.FechaCreacion,
                    Estado = c.Estado

                }).ToList();

                return listaClientes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClienteDto?> GetClienteById(int clienteId)
        {
            try
            {
                Cliente? cliente = await _context.Clientes
                .Where(c => c.ClienteId == clienteId && c.Estado == "A")
                .FirstOrDefaultAsync();

                return cliente == null
                    ? null
                    : new ClienteDto
                    {
                        Cedula = cliente.Cedula,
                        NombreCliente = cliente.NombreCliente,
                        Telefono = cliente.Telefono,
                        Edad = cliente.Edad,
                        FechaCreacion = cliente.FechaCreacion,
                        Estado = cliente.Estado
                    };

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ClienteDto> CreateCliente(ClienteDto modelo)
        {
            try
            {
                if (string.IsNullOrEmpty(modelo.NombreCliente)
                    || string.IsNullOrEmpty(modelo.Cedula)
                    || string.IsNullOrEmpty(modelo.Telefono)
                    || modelo.Edad <= 0)
                {
                    throw new ArgumentException("Los campos requeridos deben ser proporcionados.");
                }

                // Verificar si ya existe un cliente con la misma cédula
                if (await ClienteConCedulaExiste(modelo.Cedula))
                {
                    throw new InvalidOperationException("Ya existe un cliente con la misma cédula.");
                }

                var cliente = new Cliente
                {
                    Cedula = modelo.Cedula,
                    NombreCliente = modelo.NombreCliente,
                    Telefono = modelo.Telefono,
                    Edad = modelo.Edad
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                modelo.ClienteId = cliente.ClienteId;

                return modelo;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<bool> ClienteConCedulaExiste(string cedula)
        {
            return await _context.Clientes.AnyAsync(c => c.Cedula == cedula);
        }

        public Task<ClienteDto> GetClienteByCedula(string cedula)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateCliente(int clienteId, ClienteDto modelo)
        {


            try
            {
                if (string.IsNullOrEmpty(modelo.NombreCliente)
                    || string.IsNullOrEmpty(modelo.Cedula)
                    || string.IsNullOrEmpty(modelo.Telefono)
                    || modelo.Edad <= 0)
                {
                    throw new ArgumentException("ID y otros campos requeridos deben ser proporcionados.");
                }

                // Verificar si ya existe un cliente con la misma cédula
                if (await ClienteConCedulaExiste(modelo.Cedula))
                {
                    throw new InvalidOperationException("Ya existe un cliente con la misma cédula.");
                }

                var clienteExistente = await _context.Clientes.FindAsync(clienteId);
                if (clienteExistente == null)
                {
                    throw new ArgumentException($"No se encontró un cliente con el ID: {clienteId}");
                }

                clienteExistente.Cedula = modelo.Cedula;
                clienteExistente.NombreCliente = modelo.NombreCliente;
                clienteExistente.Telefono = modelo.Telefono;
                clienteExistente.Edad = modelo.Edad;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> InactivarCliente(int clienteId)
        {
            try
            {
                var clienteExistente = await _context.Clientes.FindAsync(clienteId);

                if (clienteExistente == null)
                {
                    return false;
                }

                clienteExistente.Estado = "I";

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private bool TieneCedulasDuplicadas(IEnumerable<Cliente> clientes)
        {
            var cedulas = clientes.Select(s => s.Cedula);
            return cedulas.Distinct().Count() != cedulas.Count();
        }

        //private async Task<bool> ExistenCedulasDuplicadasAsync(IEnumerable<Cliente> clientes)
        //{
        //    var cedulas = clientes.Select(s => s.Cedula).Distinct();
        //    var existentes = await _context.Clientes
        //                                    .Where(s => cedulas.Contains(s.Cedula))
        //                                    .Select(s => s.Cedula)
        //                                    .ToListAsync();
        //    return existentes.Any();
        //}
        private async Task<bool> ExistenCedulasDuplicadasAsync(IEnumerable<Cliente> clientes)
        {
            var cedulas = clientes.Select(s => s.Cedula);
            var cedulasDistintas = cedulas.Distinct();

            var duplicadasEnBD = await _context.Clientes
                .Where(s => cedulasDistintas.Contains(s.Cedula))
                .Select(s => s.Cedula)
                .ToListAsync();

            return duplicadasEnBD.Any();
        }


        private async Task<string> ValidarCedulasDuplicadasAsync(IEnumerable<Cliente> clientes)
        {
            if (TieneCedulasDuplicadas(clientes))
            {
                return "Error: Hay cedulas duplicadas en los datos proporcionados.";
            }

            if (await ExistenCedulasDuplicadasAsync(clientes))
            {
                return "Error: Se encontraron cedulas duplicadas en la base de datos.";
            }

            return null; // No hay errores de códigos duplicados
        }

        private List<ClienteDto> ConvertExcelToClienteDtoList(Stream fileStream)
        {
            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheet(1); // Asumiendo que los datos están en la primera hoja
            return worksheet.RowsUsed().Skip(1) // Saltar la fila de encabezados
                            .Select(MapRowToClienteDto)
                            .ToList();
        }

        private async Task<List<ClienteDto>> ConvertTxtToClienteDtoList(Stream fileStream)
        {
            var clientesDto = new List<ClienteDto>();
            using (var reader = new StreamReader(fileStream))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    clientesDto.Add(MapLineToClienteDto(line));
                }
            }
            return clientesDto;
        }

        public async Task<string> ImportarDesdeTxtAsync(Stream fileStream)
        {
            var clientesDto = await ConvertTxtToClienteDtoList(fileStream);
            var clientes = _mapper.Map<IEnumerable<Cliente>>(clientesDto);
            var cedulaDuplicadoError = await ValidarCedulasDuplicadasAsync(clientes);
            if (cedulaDuplicadoError != null)
            {
                return cedulaDuplicadoError;
            }

            try
            {
                await _importService.ImportData(clientesDto);
                return "Importación desde archivo TXT completada con éxito.";
            }
            catch (Exception ex)
            {
                return $"Error al importar desde TXT: {ex.Message}";
            }
        }

        private ClienteDto MapLineToClienteDto(string line)
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
            return new ClienteDto
            {
                Cedula = parts[0],
                NombreCliente = parts[1],
                Telefono = parts[2],
                Edad = int.Parse(parts[3]),
                FechaCreacion = DateTime.Now,
                Estado = estado
            };
        }

        public async Task<string> ImportarDesdeExcelAsync(Stream fileStream)
        {
            var clientesDto = ConvertExcelToClienteDtoList(fileStream);
            var clientes = _mapper.Map<IEnumerable<Cliente>>(clientesDto);
            var cedulaDuplicadaError = await ValidarCedulasDuplicadasAsync(clientes);
            if (cedulaDuplicadaError != null)
            {
                return cedulaDuplicadaError;
            }
            try
            {
                // Enviar DTOs directamente a DataImportService
                await _importService.ImportData(clientesDto);
                return "Importación desde archivo Excel completada con éxito.";
            }
            catch (Exception ex)
            {
                // Access the inner exception
                Exception innerException = ex.InnerException;

                // Include inner exception details in the error message
                return $"Error al importar desde Excel: {ex.GetType().Name} - {ex.Message}. Detalles: {innerException?.Message}";
            }
        }

        private ClienteDto MapRowToClienteDto(IXLRow row)
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
            return new ClienteDto
            {
                Cedula = row.Cell(1).Value.ToString(),
                NombreCliente = row.Cell(2).Value.ToString(),
                Telefono = row.Cell(3).Value.ToString(),
                Edad = int.Parse(row.Cell(4).Value.ToString()),
                FechaCreacion = DateTime.Now,
                Estado = estado
            };
        }

        private IEnumerable<IEnumerable<ClienteDto>> ObtenerLotes(IEnumerable<ClienteDto> source, int tamañoDelLote)
        {
            return source.Select((x, i) => new { Index = i, Value = x })
                         .GroupBy(x => x.Index / tamañoDelLote)
                         .Select(x => x.Select(v => v.Value));
        }

        public Dictionary<string, string> CrearMapeoDeColumnas()
        {
            var columnMappings = new Dictionary<string, string>
            {
                {"Cedula", "Cedula"},
                {"NombreCliente", "NombreCliente"},
                {"Telefono", "Telefono"},
                {"Edad", "Edad"},
                {"FechaCreacion", "FechaCreacion"},
                {"Estado", "Estado"}
            };
            return columnMappings;
        }

        public async Task<string> ADOImportarDesdeTxtAsync(Stream fileStream)
        {
            var clientesDto = await ConvertTxtToClienteDtoList(fileStream);
            var dataTable = ConvertDTOToDataTable(clientesDto);
            var error = await ValidarDuplicadosADO(dataTable);
            if (error != null)
            {
                return error;
            }
            return await _importDAOService.ImportarDatosAsync(dataTable, "Clientes", CrearMapeoDeColumnas());
        }

        public async Task<string> ADOImportarDesdeExcelAsync(Stream fileStream)
        {
            var clientesDto = ConvertExcelToClienteDtoList(fileStream);
            var dataTable = ConvertDTOToDataTable(clientesDto);
            var error = await ValidarDuplicadosADO(dataTable);
            if (error != null)
            {
                return error;
            }
            return await _importDAOService.ImportarDatosAsync(dataTable, "Clientes", CrearMapeoDeColumnas());
        }

        private async Task<string> ValidarDuplicadosADO(DataTable dataTable)
        {
            if (ADOExistenCedulasDuplicadasEnDataTable(dataTable))
            {
                return "Error: Hay cedulas duplicadas en los datos proporcionados.";
            }

            if (await ADOExistenCedulasDuplicadasEnBaseDeDatos(dataTable))
            {
                return "Error: Se encontraron cedulas duplicadas en la base de datos.";
            }

            return null; // No hay errores
        }

        private bool ADOExistenCedulasDuplicadasEnDataTable(DataTable dataTable)
        {
            var cedulas = dataTable.AsEnumerable().Select(row => row["Cedula"].ToString()).ToList();
            return cedulas.Count != cedulas.Distinct().Count();
        }

        private async Task<bool> ADOExistenCedulasDuplicadasEnBaseDeDatos(DataTable dataTable)
        {
            using (var connection = new SqlConnection(cadena))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("CheckForDuplicateCedulas", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    var cedulasTable = new DataTable();
                    cedulasTable.Columns.Add("Cedula", typeof(string));
                    foreach (DataRow row in dataTable.Rows)
                    {
                        cedulasTable.Rows.Add(row["Cedula"]);
                    }
                    command.Parameters.AddWithValue("@Cedula", cedulasTable);
                    command.Parameters["@Cedula"].SqlDbType = SqlDbType.Structured;
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

        private DataTable ConvertDTOToDataTable(IEnumerable<ClienteDto> clientesDto)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Cedula", typeof(string));
            dataTable.Columns.Add("NombreCliente", typeof(string));
            dataTable.Columns.Add("Telefono", typeof(string));
            dataTable.Columns.Add("Edad", typeof(int));
            dataTable.Columns.Add("FechaCreacion", typeof(DateTime));
            dataTable.Columns.Add("Estado", typeof(string));
            foreach (var cliente in clientesDto)
            {
                dataTable.Rows.Add(
                    cliente.Cedula,
                    cliente.NombreCliente,
                    cliente.Telefono,
                    cliente.Edad,
                    cliente.FechaCreacion,
                    cliente.Estado
                );
            }
            return dataTable;
        }
    }
}
