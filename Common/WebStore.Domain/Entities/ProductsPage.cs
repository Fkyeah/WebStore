using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Domain.Entities
{
    public class ProductsPage
    {
        public ProductsPage(IEnumerable<Product> products, int totalCount)
        {
            Products = products;
            TotalCount = totalCount;
        }
        public IEnumerable<Product> Products { get; set; }
        public int TotalCount { get; set; }

    }
}
