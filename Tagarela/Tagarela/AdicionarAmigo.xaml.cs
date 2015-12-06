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
using System.Collections.ObjectModel;
using Tagarela.Model;

namespace Tagarela
{
    public partial class AdicionarAmigo : PhoneApplicationPage
    {
        ObservableCollection<String> pendentes = new ObservableCollection<string>();

        private string channelName = "tagarelaIFRN";
        private string servidor = "https://meumsn.herokuapp.com";
        private string uri = "";
        private string sessao = "";

        public AdicionarAmigo()
        {
            InitializeComponent();
            lbPendentes.ItemsSource = pendentes;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dic = NavigationContext.QueryString;
            if (dic.ContainsKey("sessao"))
            {
                sessao = dic["sessao"];
                recuperaPendentes(sessao);
            }

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


        private async void recuperaPendentes(string sessao)
        {

            var response = await requisicaoHttp().GetAsync("/api/amigo/" + sessao + "/pendentes");
            var resposta = response.Content.ReadAsStringAsync().Result;

            List<Model.Amigo> amigos = JsonConvert.DeserializeObject<List<Model.Amigo>>(resposta);

            if (amigos != null)
            {
                foreach (Model.Amigo amigo in amigos)
                {
                    pendentes.Add(amigo.ToString());
                }
            }
        }


        private void btVoltar_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void btnAdicionar_Click(object sender, EventArgs e)
        {

            Model.AdicionarAmigo a = new Model.AdicionarAmigo
            {
                session = sessao,
                amigo = txtAmigo.Text,
            };

            var response = await requisicaoHttp().PostAsync("/api/amigo", conteudoJSON(JsonConvert.SerializeObject(a)));

            var resposta = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Model.AdicionarAmigoResposta aResposta = JsonConvert.DeserializeObject<Model.AdicionarAmigoResposta>(resposta);

                if (aResposta.result)
                {
                    MessageBox.Show(String.Format("Solicitação de amizade realizada com sucesso."));
                    NavigationService.GoBack();
                }
                else {
                    MessageBox.Show(String.Format("Não foi possível fazer solicitação."));
                }
            }
            else {
                ErroPadrao aResposta = JsonConvert.DeserializeObject<ErroPadrao>(resposta);
                MessageBox.Show(aResposta.message);
            }

        }
    }
}