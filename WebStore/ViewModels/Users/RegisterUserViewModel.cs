using System.ComponentModel.DataAnnotations;


namespace WebStore.ViewModels.Users
{
    public class RegisterUserViewModel
    {
        [Required, MaxLength(50)]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
