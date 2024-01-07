namespace BLL.Validation.Exceptions
{
    public class ConnectionException : Exception
    {
        /// <summary>
        /// Ctor with single param 
        /// </summary>
        /// <param name="message">Exception message</param>
        public ConnectionException(string message) : base(message) { }
    }
}
