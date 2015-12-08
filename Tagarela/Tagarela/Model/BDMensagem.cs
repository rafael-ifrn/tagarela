using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Tagarela.Model
{
    [Table]
    public class BDMensagem
    {
        [Column(IsPrimaryKey = true)]
        public int id { get; set; }
        [Column]
        public string mensagem { get; set; }
        [Column]
        public string emailOrigem { get; set; }
        [Column]
        public string emailDestino { get; set; }
        [Column]
        public string time { get; set; }
    }
}
