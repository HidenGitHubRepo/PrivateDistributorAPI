using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CodeFirst.Model
{
    public class NewUserAuthCode
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        public bool IsUsed { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(12)]
        [StringLength(12)]
        public virtual User AuthCodeCreator { get; set; }

        public virtual User NewUser { get; set; }
        
        [MinLength(40)]
        [MaxLength(40)]
        [StringLength(40)]
        public string AuthCode { get; set; }

        public virtual Company Company { get; set; }
        public virtual Type Type { get; set; }

        public NewUserAuthCode()
        {
            this.IsUsed = false;
        }
    }
}
