using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

namespace Tagarela.Model
{
    public class MensagemDataContext : DataContext
    {
        public static string DBConnStr = "Data Source=isostore:mensagem.sdf";

        public MensagemDataContext() : base(DBConnStr)
        {
        }

        public Table<BDMensagem> BDMensagens
        {
            get { return GetTable<BDMensagem>(); }
        }

        public static MensagemDataContext OpenMensagemDataContext()
        {
            MensagemDataContext dc = new MensagemDataContext();
            if (!dc.DatabaseExists()) dc.CreateDatabase();
            return dc;
        }

        //Cannot remove an entity that has not been attached
        public void DeletarMensagem(string email)
        {
            MensagemDataContext dc = new MensagemDataContext();

            IEnumerable<BDMensagem> mensagens = SelectMensagem(email);

            foreach (BDMensagem mensagem in mensagens)
            {
                dc.BDMensagens.DeleteOnSubmit(mensagem);
                
            }
        }

        public void InserirMensagem(BDMensagem mensagem)
        {
            mensagem.id = QtMensagem()+1;
            BDMensagens.InsertOnSubmit(mensagem);
            SubmitChanges();
        }

        public int QtMensagem() {

            MensagemDataContext dc = OpenMensagemDataContext();

            var query = (from x in dc.BDMensagens select x).ToList();
            var max_Query = 0;

            try
            {
                max_Query =(from m in dc.BDMensagens
                            select m.id).Max();
            }
            catch (InvalidOperationException ex) {
            }
            
            return max_Query;
        }

        public IEnumerable<BDMensagem> SelectMensagem(string email)
        {
            MensagemDataContext dc = OpenMensagemDataContext();

            return from msg in dc.BDMensagens
                where msg.emailDestino == email || msg.emailOrigem == email
                select msg;
        }
    }
}
