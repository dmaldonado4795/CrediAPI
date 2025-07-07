using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;

namespace CrediAPI.Domain.Models
{
    public class PlazosModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Index(IsUnique = true)]
        public int Meses { get; set; }
    }
}