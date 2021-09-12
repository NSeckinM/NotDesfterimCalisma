using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotDefterim.Data
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime KayitZamani { get; set; } = DateTime.Now;

        //Nav prop Bir kullanıcının birden fazla notu olabilir.
        public virtual ICollection<Note> Notes { get; set; }

    }
}
