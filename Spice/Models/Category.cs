using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        //[Remote("Create", "Category", AdditionalFields = "Id",
             //   ErrorMessage = "Category name already exists")]
        [RegularExpression("[A-Za-z]*", ErrorMessage = "Invalid Name ")]
        [Display(Name ="CategoryName")]
        public string Name { get; set; }

    }
}
