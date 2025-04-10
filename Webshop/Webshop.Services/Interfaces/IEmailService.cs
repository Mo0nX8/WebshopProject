using Webshop.EntityFramework.Data;

namespace Webshop.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for sending email services related to orders and password resets.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an order confirmation email asynchronously.
        /// </summary>
        /// <param name="order">The order object containing order details.</param>
        /// <param name="email">The recipient's email address.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task SendOrderEmailAsync(Orders order, string email);

        /// <summary>
        /// Sends a password reset email asynchronously.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The subject of the reset email.</param>
        /// <param name="body">The body content of the reset email.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task SendResetEmailAsync(string email, string subject, string body);
    }
}
