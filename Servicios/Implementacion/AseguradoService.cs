using BackendConsultorioSeguros.DTOs;
using BackendConsultorioSeguros.Models;
using BackendConsultorioSeguros.Servicios.Contrato;
using BackendConsultorioSeguros.Utility;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BackendConsultorioSeguros.Servicios.Implementacion
{
    public class AseguradoService : IAseguradoService
    {
        private readonly IConfiguration _configuration;
        private DBSEGUROSCHUBBContext _context;
        private string cadena;
        public AseguradoService(DBSEGUROSCHUBBContext context, IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration;
            this.cadena = _configuration["ConnectionStrings:cadenaSQL"];
        }

        public async Task<List<AseguradoDto>> GetListAsegurados()
        {
            List<AseguradoDto> listaAsegurados = new();

            using (SqlConnection db = new SqlConnection(cadena))
            {
                try
                {
                    await db.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("ObtenerAseguradosActivos", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                AseguradoDto asegurado = new AseguradoDto();
                                asegurado.AseguradoId = int.Parse(reader["AseguradoId"].ToString().Trim());
                                asegurado.ClienteId = int.Parse(reader["ClienteId"].ToString().Trim());
                                asegurado.SeguroId = int.Parse(reader["SeguroId"].ToString().Trim());
                                asegurado.Cliente = new ClienteDto
                                {
                                    ClienteId = int.Parse(reader["ClienteId"].ToString().Trim()),
                                    Cedula = reader["ClienteCedula"].ToString().Trim(),
                                    NombreCliente = reader["NombreCliente"].ToString().Trim(),
                                    Telefono = reader["ClienteTelefono"].ToString().Trim(),
                                    Edad = int.Parse(reader["ClienteEdad"].ToString().Trim()),
                                    FechaCreacion = DateTime.Parse(reader["ClienteFechaCreacion"].ToString().Trim()),
                                    Estado = reader["ClienteEstado"].ToString().Trim()
                                };
                                asegurado.Seguro = new SeguroDto
                                {
                                    SeguroId = int.Parse(reader["SeguroId"].ToString().Trim()),
                                    NombreSeguro = reader["NombreSeguro"].ToString().Trim(),
                                    CodigoSeguro = reader["CodigoSeguro"].ToString().Trim(),
                                    SumaAsegurada = decimal.Parse(reader["SumaAsegurada"].ToString().Trim()),
                                    Prima = decimal.Parse(reader["Prima"].ToString().Trim()),
                                    FechaCreacion = DateTime.Parse(reader["FechaCreacion"].ToString().Trim()),
                                    Estado = reader["Estado"].ToString().Trim()
                                };
                                asegurado.FechaCreacion = DateTime.Parse(reader["FechaCreacion"].ToString().Trim());
                                asegurado.Estado = reader["Estado"].ToString().Trim();

                                listaAsegurados.Add(asegurado);
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
            return listaAsegurados;
        }


        public async Task<AseguradoDto> GetAseguradoById(int aseguradoId)
        {
            AseguradoDto asegurado = null;
            using (SqlConnection db = new SqlConnection(cadena))
            {
                try
                {
                    await db.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("ObtenerAseguradoPorId", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@AseguradoId", aseguradoId));

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                asegurado = new AseguradoDto();
                                asegurado.AseguradoId = int.Parse(reader["AseguradoId"].ToString().Trim());
                                asegurado.ClienteId = int.Parse(reader["ClienteId"].ToString().Trim());
                                asegurado.SeguroId = int.Parse(reader["SeguroId"].ToString().Trim());
                                asegurado.Cliente = new ClienteDto
                                {
                                    ClienteId = int.Parse(reader["ClienteId"].ToString().Trim()),
                                    Cedula = reader["ClienteCedula"].ToString().Trim(),
                                    NombreCliente = reader["NombreCliente"].ToString().Trim(),
                                    Telefono = reader["ClienteTelefono"].ToString().Trim(),
                                    Edad = int.Parse(reader["ClienteEdad"].ToString().Trim()),
                                    FechaCreacion = DateTime.Parse(reader["ClienteFechaCreacion"].ToString().Trim()),
                                    Estado = reader["ClienteEstado"].ToString().Trim()
                                };
                                asegurado.Seguro = new SeguroDto
                                {
                                    SeguroId = int.Parse(reader["SeguroId"].ToString().Trim()),
                                    NombreSeguro = reader["NombreSeguro"].ToString().Trim(),
                                    CodigoSeguro = reader["CodigoSeguro"].ToString().Trim(),
                                    SumaAsegurada = decimal.Parse(reader["SumaAsegurada"].ToString().Trim()),
                                    Prima = decimal.Parse(reader["Prima"].ToString().Trim()),
                                    FechaCreacion = DateTime.Parse(reader["FechaCreacion"].ToString().Trim()),
                                    Estado = reader["Estado"].ToString().Trim()
                                };
                                asegurado.FechaCreacion = DateTime.Parse(reader["FechaCreacion"].ToString().Trim());
                                asegurado.Estado = reader["Estado"].ToString().Trim();
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
            return asegurado;
        }

        public async Task<CrearAseguradoDto> CreateAsegurado(CrearAseguradoDto asegurado)
        {
            if (!ValidateFields.Validate(asegurado))
            {
                throw new ArgumentException("Error de validación. Por favor, verifique los campos.");
            }

            using (SqlConnection db = new SqlConnection(cadena))
            {
                try
                {
                    await db.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("InsertarAsegurado", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ClienteId", asegurado.ClienteId));
                        cmd.Parameters.Add(new SqlParameter("@SeguroId", asegurado.SeguroId));

                        SqlParameter mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, -1);
                        mensajeParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(mensajeParam);

                        await cmd.ExecuteNonQueryAsync();

                        string mensaje = mensajeParam.Value.ToString();
                        if (!string.IsNullOrEmpty(mensaje) && mensaje != "OK")
                        {
                            throw new ApplicationException(mensaje);
                        }

                        return asegurado;
                    }
                }
                catch (Exception)
                {
                    await db.CloseAsync();
                    throw;
                }
            }
        }

        public async Task<CrearAseguradoDto> UpdateAsegurado(int aseguradoId, CrearAseguradoDto asegurado)
        {
            if (!ValidateFields.Validate(asegurado))
            {
                throw new ArgumentException("Error de validación. Por favor, verifique los campos.");
            }
            using (SqlConnection db = new SqlConnection(cadena))
            {
                try
                {
                    await db.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("ActualizarAsegurado", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros al comando
                        cmd.Parameters.AddWithValue("@AseguradoId", aseguradoId);
                        cmd.Parameters.AddWithValue("@ClienteId", asegurado.ClienteId);
                        cmd.Parameters.AddWithValue("@SeguroId", asegurado.SeguroId);

                        SqlParameter mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, -1);
                        mensajeParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(mensajeParam);

                        await cmd.ExecuteNonQueryAsync();

                        string mensaje = mensajeParam.Value.ToString();

                        if (!string.IsNullOrEmpty(mensaje) && mensaje != "OK")
                        {
                            throw new ApplicationException(mensaje);
                        }
                        return asegurado;
                    }
                }
                catch (Exception)
                {
                    await db.CloseAsync();
                    throw;
                }
            }
        }

        public async Task<bool> InactivarAsegurado(int aseguradoId)
        {
            using (SqlConnection db = new SqlConnection(cadena))
            {
                try
                {
                    await db.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("InactivarAsegurado", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros al comando
                        cmd.Parameters.AddWithValue("@AseguradoId", aseguradoId);

                        // Agregar parámetro de salida para el mensaje
                        SqlParameter mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, -1);
                        mensajeParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(mensajeParam);

                        await cmd.ExecuteNonQueryAsync();

                        // Obtener el mensaje de salida
                        string mensaje = mensajeParam.Value.ToString();

                        // Puedes manejar el mensaje como desees
                        if (mensaje.ToLower() == "eliminación lógica realizada correctamente.")
                        {
                            // Logica adicional si es necesario
                            return true;
                        }
                        else
                        {
                            // Logica adicional si es necesario
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    await db.CloseAsync();
                    throw;
                }
            }
        }




    }
}
