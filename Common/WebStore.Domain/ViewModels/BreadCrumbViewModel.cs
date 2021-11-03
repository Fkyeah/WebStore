using WebStore.Domain.Entities;

namespace WebStore.Domain.ViewModels
{
    public class BreadCrumbViewModel
    {
        public Brand Brand { get; set; }
        public Section Section { get; set; }
        public string Product { get; set; }
    }
}
