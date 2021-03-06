﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Model
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(30)]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(30)]
        [StringLength(30)]
        public string DisplayName { get; set; }

        public string Fax { get; set; }
        public string MoreInformation { get; set; }
        public string Location { get; set; }

        public virtual ICollection<Email> Mails { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public CompanyType CompanyType { get; set; }

        public Company()
        {
            this.Mails = new HashSet<Email>();
            this.Phones = new HashSet<Phone>();
            this.Users = new HashSet<User>();
        }
    }
}