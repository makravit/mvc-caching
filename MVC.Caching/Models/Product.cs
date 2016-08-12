using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Caching.Models
{
    public class Product
    {
        public int Id { get; set; }


        [Required]
        [StringLength(50)]
        public string Name { get; set; }


        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}