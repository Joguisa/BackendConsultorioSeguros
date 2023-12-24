using BackendConsultorioSeguros.DTOs;
using BackendConsultorioSeguros.Models;
using BackendConsultorioSeguros.Servicios.Contrato;
using BackendConsultorioSeguros.Utility;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BackendConsultorioSeguros.Servicios.Implementacion
{
    public class SeguroService : ISeguroService
    {
        private readonly IConfiguration _configuration;
        private DBSEGUROSCHUBBContext _context;
        private string cadena;
        public SeguroService(DBSEGUROSCHUBBContext context, IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration;
            this.cadena = _configuration["ConnectionStrings:cadenaSQL"];
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

            if (!ValidateFields.Validate(seguro))
            {
                throw new ArgumentException("Error de validación. Por favor, verifique los campos.");
            }

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
            if (!ValidateFields.Validate(seguro))
            {
                throw new ArgumentException("Error de validación. Por favor, verifique los campos.");
            }

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
    }
}
