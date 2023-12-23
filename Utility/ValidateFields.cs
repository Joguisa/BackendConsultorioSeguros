namespace BackendConsultorioSeguros.Utility
{
    public class ValidateFields
    {
        private static List<string> validationErrors = new List<string>();

        public static bool Validate<T>(T entity)
        {
            validationErrors.Clear();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(entity);

                if (IsRequired(property) && IsNullOrEmpty(value))
                {
                    validationErrors.Add($"El campo '{property.Name}' es obligatorio y no puede estar vacío.");
                }
            }

            return validationErrors.Count == 0;
        }

        public static List<string> GetValidationErrors()
        {
            return validationErrors;
        }

        private static bool IsRequired(System.Reflection.PropertyInfo property)
        {
            return true;
        }

        private static bool IsNullOrEmpty(object value)
        {
            return value == null || (value is string stringValue && string.IsNullOrEmpty(stringValue));
        }
    }

}
