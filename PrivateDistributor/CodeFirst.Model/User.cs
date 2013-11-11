using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Model
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(30)]
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(30)]
        [StringLength(30)]
        public string DisplayName { get; set; }

        [MinLength(40)]
        [MaxLength(40)]
        [StringLength(40)]
        public string AuthCode { get; set; }

        [MinLength(50)]
        [MaxLength(50)]
        [StringLength(50)]
        public string SessionKey { get; set; }

        public UserType UserType { get; set; }


        public ICollection<string> Mails { get; set; }
        public ICollection<string> Phones { get; set; }
        public ICollection<string> MoreContacts { get; set; }

        public string Location { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Car> Cars { get; set; }

        public User()
        {
            this.Cars = new HashSet<Car>();
            this.Mails = new HashSet<string>();
            this.Phones = new HashSet<string>();
            this.MoreContacts = new HashSet<string>();
        }
    } 
}
