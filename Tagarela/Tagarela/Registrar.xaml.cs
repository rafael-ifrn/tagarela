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
using Newtonsoft.Json;
using System.Text;

namespace Tagarela
{
    public partial class Registrar : PhoneApplicationPage
    {

        private string channelName = "tagarelaIFRN";
        private string servidor = "https://meumsn.herokuapp.com";
        private string uri = "";

        public Registrar()
        {
            InitializeComponent();
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


        private void btVoltar_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }


        private async void btnCadastrar_Click(object sender, EventArgs e)
        {
            Model.Autenticacao a = new Model.Autenticacao
            {
                username = txtEmail.Text,
                password = txtSenha.Password,
                nick = txtNick.Text
            };

            var response = await requisicaoHttp().PostAsync("/api/user/create", conteudoJSON(JsonConvert.SerializeObject(a)));

            var resposta = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (resposta.IndexOf("{\"session") == 0)
                {
                    NavigationService.Navigate(new Uri("/MainPage.xaml?username=" + txtEmail.Text, UriKind.Relative));
                }
            } else
            {
                Model.ErroPadrao aResposta = JsonConvert.DeserializeObject<Model.ErroPadrao>(resposta);
                MessageBox.Show(aResposta.message);
            }
        }
    }
}