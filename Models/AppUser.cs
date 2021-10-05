using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lets2Chat.Models
{
    public class AppUser : IdentityUser 
    {
        public List<Message> Messages { get; set; }
    }
}
