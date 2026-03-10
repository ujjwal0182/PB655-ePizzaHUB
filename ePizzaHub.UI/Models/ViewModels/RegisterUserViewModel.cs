namespace ePizzaHub.UI.Models.ViewModels
{
    public class RegisterUserViewModel
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
    }
}
