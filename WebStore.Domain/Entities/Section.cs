using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    class Section : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
    }
}
