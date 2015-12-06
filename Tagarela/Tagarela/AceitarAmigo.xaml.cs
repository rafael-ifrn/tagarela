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
    public partial class AceitarAmigo : PhoneApplicationPage
    {
        ObservableCollection<String> pedidos = new ObservableCollection<string>();

        private string channelName = "tagarelaIFRN";
        private string servidor = "https://meumsn.herokuapp.com";
        private string uri = "";
        private string sessao = "";


        public AceitarAmigo()
        {
            InitializeComponent();
            lbPedidos.ItemsSource = pedidos;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dic = NavigationContext.QueryString;
            if (dic.ContainsKey("sessao"))
            {
                sessao = dic["sessao"];
                recuperaPedidos(sessao);
            }

        }


        private async void recuperaPedidos(string sessao)
        {

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(servidor);

            var response = await httpClient.GetAsync("/api/amigo/" + sessao + "/pedidos");
            var str = response.Content.ReadAsStringAsync().Result;

            List<Model.Amigo> amigos = JsonConvert.DeserializeObject<List<Model.Amigo>>(str);

            if (amigos != null)
            {
                foreach (Model.Amigo amigo in amigos)
                {
                    if (!pedidos.Contains(amigo.email))
                    {
                        pedidos.Add(amigo.email);
                    }
                }
            }
        }



        private async void btAceitar_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(servidor);

            string selecionado = lbPedidos.SelectedItem.ToString();
            if (selecionado == null || selecionado == "")
            {
                MessageBox.Show(String.Format("Selecione um usuário."));
            }
            else {

                Model.AdicionarAmigo a = new Model.AdicionarAmigo
                {
                    session = sessao,
                    amigo = selecionado
                };

                string s = JsonConvert.SerializeObject(a);

                var content = new StringContent(s, Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync("/api/amigo", content);

                var str = response.Content.ReadAsStringAsync().Result;

                MessageBox.Show(String.Format("solicitação {0},\n Resposta: {1}", s, str));

                Model.AdicionarAmigoResposta resposta = JsonConvert.DeserializeObject<Model.AdicionarAmigoResposta>(str);

                if (resposta.result)
                {
                    MessageBox.Show(String.Format("Amigo adicionado com sucesso."));
                    if (lbPedidos.Items.Count == 1)
                    {
                        NavigationService.GoBack();
                    }
                    else {
                        pedidos.Remove(selecionado);
                    }
                }
                else {
                    MessageBox.Show(String.Format("Não foi possível adicionar o amigo."));
                }
            }

        }
    }
}