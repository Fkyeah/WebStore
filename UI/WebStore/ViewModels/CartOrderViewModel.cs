using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public class CartOrderViewModel
    {
        public CartViewModel Cart { get; set; }
        public OrderViewModel Order { get; set; }
    }
}
