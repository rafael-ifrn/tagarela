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
        ObservableCollection<String> amigos = new ObservableCollection<string>();

        List<Model.Amigo> pedidos;

        private string channelName = "tagarelaIFRN";
        private string servidor = "https://meumsn.herokuapp.com";
        private string uri = "";
        private string sessao = "";

        public Usuario()
        {
            InitializeComponent();

           // this.NavigationCacheMode = NavigationCacheMode.Required;
            lbAmigos.ItemsSource = amigos;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dic = NavigationContext.QueryString;
            if (dic.ContainsKey("sessao"))
            {
                sessao = dic["sessao"];
                tbNick.Text = dic["nick"];

                recuperaAmigos(sessao);
                recuperaPedidos(sessao);
            }
            if (dic.ContainsKey("status"))
            {
                tbStatus.Text = dic["status"];
            }
        }



        private HttpClient requisicaoHttp() {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(servidor);

            return httpClient;
        }

        private StringContent conteudoJSON(string json) {
            return new StringContent(json, Encoding.UTF8, "application/json");
        }


        private async void recuperaAmigos(string sessao) {

            var response = await requisicaoHttp().GetAsync("/api/amigo/"+ sessao);
            var resposta = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                List<Model.Amigo> amigosR = JsonConvert.DeserializeObject<List<Model.Amigo>>(resposta);

                amigos.Clear();

                if (amigosR != null)
                {
                    foreach (Model.Amigo amigo in amigosR)
                    {
                        if (!amigos.Contains(amigo.ToString()))
                        {
                            amigos.Add(amigo.ToString());
                        }
                    }
                }
            }
            else {
                Model.ErroPadrao aResposta = JsonConvert.DeserializeObject<Model.ErroPadrao>(resposta);
                MessageBox.Show(aResposta.message);
            }
        }


        private async void recuperaPedidos(string sessao)
        {

            var response = await requisicaoHttp().GetAsync("/api/amigo/"+ sessao + "/pedidos");
            var resposta = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                pedidos = JsonConvert.DeserializeObject<List<Model.Amigo>>(resposta);
                if (pedidos.Count > 0)
                {
                    tbSolicitacao.Visibility = Visibility.Visible;
                }

            }
            else {
                Model.ErroPadrao aResposta = JsonConvert.DeserializeObject<Model.ErroPadrao>(resposta);
                MessageBox.Show(aResposta.message);
            }
        }


        private void btAdicionar_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdicionarAmigo.xaml?sessao=" + sessao, UriKind.Relative));
        }

        private void btConfiguracaoes_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Configuracoes.xaml?sessao=" + sessao, UriKind.Relative));
        }

        private void btAceitar_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AceitarAmigo.xaml?sessao=" + sessao, UriKind.Relative));
        }

        private async void btBloquear_Click(object sender, EventArgs e)
        {

            if (lbAmigos.SelectedItem == null)
            {
                MessageBox.Show(String.Format("Selecione um usuário."));
            }
            else {
                string selecionado = lbAmigos.SelectedItem.ToString();
                string email = "";
                email = selecionado.Substring(selecionado.LastIndexOf('<') + 1, selecionado.Length - (selecionado.LastIndexOf('<') + 2));
                //MessageBox.Show(String.Format("email: {0}", email));

                Model.AdicionarAmigo a = new Model.AdicionarAmigo
                {
                    session = sessao,
                    amigo = email
                };

                var response = await requisicaoHttp().PostAsync("/api/amigo/bloquear", conteudoJSON(JsonConvert.SerializeObject(a)));

                var resposta = response.Content.ReadAsStringAsync().Result;
                //MessageBox.Show(String.Format("Resposta: {0}", resposta));


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Model.AdicionarAmigoResposta aResposta = JsonConvert.DeserializeObject<Model.AdicionarAmigoResposta>(resposta);

                    if (aResposta.result)
                    {
                        MessageBox.Show(String.Format("Amigo bloqueado com sucesso."));
                        amigos.Remove(selecionado);
                    }
                    else {
                        MessageBox.Show(String.Format("Não foi possível bloquear o amigo."));
                    }
                }
                else {
                    Model.ErroPadrao aResposta = JsonConvert.DeserializeObject<Model.ErroPadrao>(resposta);
                    MessageBox.Show(aResposta.message);
                }

            }
        }

        private void btDesbloquear_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/DesbloquearAmigo.xaml?sessao=" + sessao, UriKind.Relative));
        }
    }
}