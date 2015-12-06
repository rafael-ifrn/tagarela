using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagarela.Model
{
    class Amigo
    {
        public string email { get; set; }
        public string nick { get; set; }
        public string status { get; set; }

        public override string ToString() {
            return nick + " <" + email + ">";
        }
    }
}
