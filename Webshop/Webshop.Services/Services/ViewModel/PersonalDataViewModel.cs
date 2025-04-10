namespace Webshop.Services.Services.ViewModel
{
    /// <summary>
    /// This helps adding personal data to a view through model.
    /// </summary>
    public class PersonalDataViewModel
    {
        /// <summary>
        /// This property contains an user's id.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// This property contains an user's username.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// This property contains an user's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// This is a hidden property used for handling the password change check.
        /// </summary>
        public bool PasswordChanged { get; set; }
    }
}
