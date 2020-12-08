using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Models.Products
{
    public class PostProductRequest : IValidatableObject
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public decimal? Cost { get; set; }
        [Range(1,int.MaxValue)]
        public int Count { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Cost < 0)
            {
                yield return new ValidationResult("No negative costs", new string[] { nameof(Cost) });
            }
        }
    }
}
