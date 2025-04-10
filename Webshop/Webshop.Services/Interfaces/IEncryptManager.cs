namespace Webshop.Services.Interfaces
{
    /// <summary>
    /// This interface is for the encryption.
    /// </summary>
    public interface IEncryptManager
    {
        /// <summary>
        /// This method requires a string key as parameter. It returns a string, which is Hashed by SHA256 protocol.
        /// </summary>
        /// <param name="key">The string that must be hashed.</param>
        /// <returns>Hashed string</returns>
        string Hash(string key);
    }
}
