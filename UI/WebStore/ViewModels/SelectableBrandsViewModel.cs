using System.Collections.Generic;
using WebStore.Domain.ViewModels;

namespace WebStore.ViewModels
{
    public class SelectableBrandsViewModel
    {
        public IEnumerable<BrandViewModel> Brands { get; set; }
        public int? BrandId { get; set; }
    }
}
