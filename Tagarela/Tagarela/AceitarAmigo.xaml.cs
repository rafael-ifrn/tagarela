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



        private async void recuperaPedidos(string sessao)
        {

            var response = await requisicaoHttp().GetAsync("/api/amigo/" + sessao + "/pedidos");
            var resposta = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {

                List<Amigo> amigos = JsonConvert.DeserializeObject<List<Amigo>>(resposta);

                if (amigos != null)
                {
                    foreach (Amigo amigo in amigos)
                    {
                        if (!pedidos.Contains(amigo.email))
                        {
                            pedidos.Add(amigo.email);
                        }
                    }
                }
            } else
            {
                Model.ErroPadrao aResposta = JsonConvert.DeserializeObject<Model.ErroPadrao>(resposta);
                MessageBox.Show(aResposta.message);
            }
        }


        private void btVoltar_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void btAceitar_Click(object sender, EventArgs e)
        {
            
            if (lbPedidos.SelectedItem == null)
            {
                MessageBox.Show(String.Format("Selecione um usuário."));
            }
            else {

                string selecionado = lbPedidos.SelectedItem.ToString();

                Model.AdicionarAmigo a = new Model.AdicionarAmigo
                {
                    session = sessao,
                    amigo = selecionado
                };

                var response = await requisicaoHttp().PutAsync("/api/amigo", conteudoJSON(JsonConvert.SerializeObject(a)));

                var resposta = response.Content.ReadAsStringAsync().Result;
                //MessageBox.Show(String.Format("Resposta: {0}", resposta));


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    AdicionarAmigoResposta aResposta = JsonConvert.DeserializeObject<AdicionarAmigoResposta>(resposta);

                    if (aResposta.result)
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
                else {
                    ErroPadrao aResposta = JsonConvert.DeserializeObject<ErroPadrao>(resposta);
                    MessageBox.Show(aResposta.message);
                }
            }

        }
    }
}