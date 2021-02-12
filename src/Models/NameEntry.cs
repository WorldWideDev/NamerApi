using System;
using System.ComponentModel.DataAnnotations;

namespace NamesApi.Models
{
    public class NameEntry
    {
        [Key]
        public int Id {get;set;}
        [Required]
        public string Name {get;set;}
        [Required]
        [Range(0.0,1.0)]
        public float Weight {get;set;}
        public DateTime? CreatedAt {get;set;} = DateTime.Now;
        public DateTime? UpdatedAt {get;set;} = null;
    }
}