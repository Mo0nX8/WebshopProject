namespace Webshop.Services.Interfaces
{
    /// <summary>
    /// This interface is for validations.
    /// </summary>
    public interface IValidationManager
    {
        /// <summary>
        /// This method requires a key as paramether. This method checks if the key is available, if not, returns error codes.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string IsAvailable(string key);
    }
}
