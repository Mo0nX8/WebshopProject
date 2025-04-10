using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.Validators
{
    /// <summary>
    /// This is the implementation for <see cref="IValidationManager"/>.
    /// Provides method for handling password validation.
    /// </summary>>
    public class PasswordValidator : IValidationManager
    {
        /// <summary>
        /// Checks if the provided password is available for registration.
        /// </summary>
        /// <param name="key">The password to validate.</param>
        /// <returns>
        /// Returns an error message if the password is invalid, otherwise returns "200" indicating success.
        /// </returns>
        public string IsAvailable(string key)
        {
            if (key == null)
            {
                return "Hiba! Nem adtál meg jelszót!";
            }
            if (!key.Any(char.IsDigit))
            {
                return "Hiba! A jelszónak tartalmaznia kell legaláb egy számot!";
            }
            if (key.Length < 8)
            {
                return "Hiba! A jelszónak legalább 8 karakter hosszúnak kell lennie!";
            }
            return "200";
        }
    }
}
