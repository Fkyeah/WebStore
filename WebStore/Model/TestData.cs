using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Model
{
    public class TestData 
    {
        public static List<Employer> EmployerList { get; } = new List<Employer>()
        {
            new Employer { Id = 1, Name = "Дмитрий", LastName = "Тихонов", Patronymic = "Игоревич", Age = 26 },
            new Employer { Id = 2, Name = "Иван", LastName = "Иванов", Patronymic = "Иванович", Age = 23 },
            new Employer { Id = 3, Name = "Петр", LastName = "Петров", Patronymic = "Петрович", Age = 24 },
            new Employer { Id = 4, Name = "Сидр", LastName = "Сидоров", Patronymic = "Сидорович", Age = 25 }
        };
    }
}
