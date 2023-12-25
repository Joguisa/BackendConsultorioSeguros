using Microsoft.Data.SqlClient;
using System.Data;

namespace BackendConsultorioSeguros.Helpers
{
    public class DataImportDAOService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DataImportDAOService(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._connectionString = _configuration["ConnectionStrings:cadenaSQL"];
        }

        public async Task<string> ImportarDatosAsync(DataTable dataTable, string nombreTabla, Dictionary<string, string> columnMappings)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var transaction = connection.BeginTransaction();

                try
                {
                    await BulkInsertAsync(dataTable, nombreTabla, columnMappings, connection, transaction);
                    transaction.Commit();
                    return "Importación completada con éxito.";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return $"Error al importar: {ex.Message}";
                }
            }
        }

        private async Task BulkInsertAsync(DataTable dataTable, string destinationTableName, Dictionary<string, string> columnMappings, SqlConnection connection, SqlTransaction transaction)
        {
            using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = destinationTableName;
                foreach (var mapping in columnMappings)
                {
                    bulkCopy.ColumnMappings.Add(mapping.Key, mapping.Value);
                }
                bulkCopy.BatchSize = 1000; // Ajusta según tus necesidades
                await bulkCopy.WriteToServerAsync(dataTable);
            }
        }
    }
}
