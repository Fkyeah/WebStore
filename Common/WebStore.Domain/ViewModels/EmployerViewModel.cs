using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels
{
    [Display(Name = "Сотрудники")]
    public class EmployerViewModel
    {
        [Display(Name = "Порядковый номер")]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя обязательно")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2-х до 50-ти символов")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Фамилия обязательна")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от 2-х до 50-ти символов")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Отчество обязательно")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "Отчество должно быть от 2-х до 50-ти символов")]
        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Возраст обязателен")]
        [Range(minimum: 18, maximum: 75, ErrorMessage = "Возраст должен быть от 18 до 75")]
        public int Age { get; set; }
    }
}
