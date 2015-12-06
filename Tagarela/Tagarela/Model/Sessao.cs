using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagarela.Model
{
    class Sessao
    {
        public string _id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string nick { get; set; }
        public string[] bloqueados { get; set; }
        public string[] pedidos { get; set; }
        public string[] amigos { get; set; }
    }
}
