using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lets2Chat.Models
{
    public class Message 
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Descricao { get; set; }
        public DateTime Datetime { get; set; }
        public string TargetName { get; set; }
        public string UserId { get; set; }
        public AppUser appUser { get; set; }        
        public string TargetId { get; set; }
        public AppUser targetUser { get; set; }
    }
}
