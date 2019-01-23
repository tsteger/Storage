using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public int Price { get; set; }
        [DataType(DataType.Date)]
        public DateTime Orderdate { get; set; }
        [MaxLength(20)]
        public string Category { get; set; }
        [MaxLength(20)]
        public string Shelf { get; set; }
        public int Count { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
    }
}
