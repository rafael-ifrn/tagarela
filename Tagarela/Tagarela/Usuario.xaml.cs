using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace Tagarela
{
    public partial class Usuario : PhoneApplicationPage
    {
        ObservableCollection<String> perguntas = new ObservableCollection<string>();

        private string channelName = "tagarelaIFRN";
        private string servidor = "https://meumsn.herokuapp.com";
        private string uri = "";
        private string sessao = "";

        public Usuario()
        {
            InitializeComponent();

           // this.NavigationCacheMode = NavigationCacheMode.Required;
            perguntaList.ItemsSource = perguntas;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dic = NavigationContext.QueryString;
            if (dic.ContainsKey("sessao")) {
                sessao = dic["sessao"];
                tbNick.Text = dic["nick"];
                recuperaAmigos(sessao);
            } 
        }

        private async void recuperaAmigos(string sessao) {

            perguntas.Clear();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(servidor);

            var response = await httpClient.GetAsync("/api/amigo/"+ sessao);
            var str = response.Content.ReadAsStringAsync().Result;

            List<Model.Amigo> amigos = JsonConvert.DeserializeObject<List<Model.Amigo>>(str);

            if (amigos != null) {
                foreach (Model.Amigo amigo in amigos)
                {
                    if (!perguntas.Contains(amigo.ToString())) {
                        perguntas.Add(amigo.ToString());
                    }
                }
            }
        }

        private void btAdicionar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdicionarAmigo.xaml?sessao=" + sessao, UriKind.Relative));
        }

        private void btAceitar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AceitarAmigo.xaml?sessao=" + sessao, UriKind.Relative));
        }
    }
}