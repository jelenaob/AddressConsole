using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressConsole
{
    public class AddressLookup
    {
        public string Code { get; set; }
        public string MemberId { get; set; }
        public string Line1 { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string AddressId { get; set; }
        public string OutputAddress { get; set; }
        public bool SelectedFlag { get; set; }
    }
}
