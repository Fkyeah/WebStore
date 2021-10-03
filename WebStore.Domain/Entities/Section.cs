﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    [Table("Products")]
    public class Section : NamedEntity, IOrderedEntity
    {
        [Column("Section_order_id")]
        public int Order { get; set; }
        
        public int? ParentId { get; set; }
        
        [ForeignKey(nameof(ParentId))]
        public Section Parent { get; set; }
        
        public ICollection<Product> Products { get; set; }
    }
}
