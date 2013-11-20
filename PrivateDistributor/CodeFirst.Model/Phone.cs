using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Model
{
    public class Phone
    {
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(30)]
        [StringLength(30)]
        public string Number { get; set; }
    }
}
