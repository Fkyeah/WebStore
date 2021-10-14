using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebStore.Infrastructure.Convensions
{
    public class TestConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            // Возможность производить различные действия с controller
        }
    }
}
