using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressConsole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace FindAdrTest
    {
        public class Item
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public string Text { get; set; }
            public string Highlight { get; set; }
            public string Description { get; set; }
        }

        public class AddressResponse
        {
            public List<Item> Items { get; set; }
        }
    }

}
