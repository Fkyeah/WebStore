using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
