using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Tagarela.Model;

namespace Tagarela
{
    public partial class Configuracoes : PhoneApplicationPage
    {

        private string channelName = "tagarelaIFRN";
        private string servidor = "https://meumsn.herokuapp.com";
        private string uri = "";
        private string sessao = "";


        public Configuracoes()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dic = NavigationContext.QueryString;
            if (dic.ContainsKey("sessao"))
            {
                sessao = dic["sessao"];
            }

        }


        private void btVoltar_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private HttpClient requisicaoHttp()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(servidor);

            return httpClient;
        }

        private StringContent conteudoJSON(string json)
        {
            return new StringContent(json, Encoding.UTF8, "application/json");
        }


        private async void btnAlterar_Click(object sender, EventArgs e)
        {
            Model.Configuracoes c = new Model.Configuracoes
            {
                session = sessao,
                nick = txtNick.Text,
                status = txtStatus.Text
            };

            var response = await requisicaoHttp().PutAsync("/api/user", conteudoJSON(JsonConvert.SerializeObject(c)));

            var resposta = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Model.ConfiguracoesResposta aResposta = JsonConvert.DeserializeObject<Model.ConfiguracoesResposta>(resposta);

                if (aResposta.username != "")
                {
                    MessageBox.Show(String.Format("Configurações realizadas com sucesso."));
                    NavigationService.Navigate(new Uri("/Usuario.xaml?sessao=" + sessao +"&nick="+ txtNick.Text +"&status="+ txtStatus.Text, UriKind.Relative));
                }
                else {
                    MessageBox.Show(String.Format("Não foi possível fazer alteração."));
                }
            }
            else {
                ErroPadrao aResposta = JsonConvert.DeserializeObject<ErroPadrao>(resposta);
                MessageBox.Show(aResposta.message);
            }
        }
    }
}