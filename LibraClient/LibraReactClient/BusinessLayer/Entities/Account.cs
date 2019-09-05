using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraReactClient.BusinessLayer.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        public string Owner { get; set; }
        public byte[] Address { get; set; }
        public string AddressHashed { get; set; }
        public string UserUID { get; set; }
        
    }
}
