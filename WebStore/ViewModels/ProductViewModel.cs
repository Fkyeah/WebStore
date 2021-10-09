using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.ViewModels
{
    public class ProductViewModel : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        public string Section { get; set; }
        public string? Brand { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}
