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


        private async void recuperaPendentes(string sessao)
        {

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(servidor);

            var response = await httpClient.GetAsync("/api/amigo/" + sessao + "/pendentes");
            var str = response.Content.ReadAsStringAsync().Result;

            List<Model.Amigo> amigos = JsonConvert.DeserializeObject<List<Model.Amigo>>(str);

            if (amigos != null)
            {
                foreach (Model.Amigo amigo in amigos)
                {
                    pendentes.Add(amigo.ToString());
                }
            }
        }



        private async void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(servidor);

            Model.AdicionarAmigo a = new Model.AdicionarAmigo
            {
                session = sessao,
                amigo = txtAmigo.Text,
            };

            string s = JsonConvert.SerializeObject(a);

            var content = new StringContent(s, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/api/amigo", content);

            var str = response.Content.ReadAsStringAsync().Result;

//            MessageBox.Show(String.Format("Resposta: {0}", str));

            Model.AdicionarAmigoResposta resposta = JsonConvert.DeserializeObject<Model.AdicionarAmigoResposta>(str);

            if (resposta.result)
            {
                MessageBox.Show(String.Format("Solicitação de amizade realizada com sucesso."));
                NavigationService.GoBack();
            }
            else {
                MessageBox.Show(String.Format("Não foi possível fazer solicitação."));
            }

        }
    }
}