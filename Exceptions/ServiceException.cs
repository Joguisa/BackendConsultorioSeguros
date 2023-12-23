namespace BackendConsultorioSeguros.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(string error) : base(error)
        {

        }
    }
}
