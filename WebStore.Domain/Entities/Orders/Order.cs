using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities.Orders
{
    public class Order : BaseEntity
    {
        [Required]
        public User User { get; set; }

        [Required]
        [MaxLength(100)]
        public string Phone { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Address { get; set; }
        
        public string Description { get; set; }
        
        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;
        [Required]
        public IEnumerable<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
    public class OrderItem : BaseEntity
    {
        [Required]
        public Product Product { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        public Order Order { get; set; }
        
        [NotMapped]
        public decimal TotalPrice => Price * Quantity;
        [Required]
        public int Quantity { get; set; }
    }
}
