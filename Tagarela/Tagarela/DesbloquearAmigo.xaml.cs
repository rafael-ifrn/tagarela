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
using Tagarela.Model;

namespace Tagarela
{
    public partial class DesbloquearAmigo : PhoneApplicationPage
    {
        ObservableCollection<String> bloqueados = new ObservableCollection<string>();

        private string channelName = "tagarelaIFRN";
        private string servidor = "https://meumsn.herokuapp.com";
        private string uri = "";
        private string sessao = "";

        public DesbloquearAmigo()
        {
            InitializeComponent();
            lbBloqueados.ItemsSource = bloqueados;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dic = NavigationContext.QueryString;
            if (dic.ContainsKey("sessao"))
            {
                sessao = dic["sessao"];
                recuperaBloqueados(sessao);
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


        private async void recuperaBloqueados(string sessao)
        {

            var response = await requisicaoHttp().GetAsync("/api/amigo/" + sessao + "/bloqueados");
            var resposta = response.Content.ReadAsStringAsync().Result;

            List<Model.Amigo> amigos = JsonConvert.DeserializeObject<List<Model.Amigo>>(resposta);

            if (amigos != null)
            {
                foreach (Model.Amigo amigo in amigos)
                {
                    if (!bloqueados.Contains(amigo.email))
                    {
                        bloqueados.Add(amigo.email);
                    }
                }
            }
        }

        private void btVoltar_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }


        private async void btDesbloquear_Click(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(servidor);

            if (lbBloqueados.SelectedItem == null)
            {
                MessageBox.Show(String.Format("Selecione um usuário."));
            }
            else {
                string selecionado = lbBloqueados.SelectedItem.ToString();

                Model.AdicionarAmigo a = new Model.AdicionarAmigo
                {
                    session = sessao,
                    amigo = selecionado
                };

                //var response = await requisicaoHttp().PostAsync("/api/amigo", conteudoJSON(JsonConvert.SerializeObject(a)));
                var response = await requisicaoHttp().DeleteAsync("/api/amigo?session="+ sessao +"&amigo="+ selecionado);

                var resposta = response.Content.ReadAsStringAsync().Result;

                MessageBox.Show(String.Format("Resposta: {0}", resposta));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Model.AdicionarAmigoResposta aResposta = JsonConvert.DeserializeObject<Model.AdicionarAmigoResposta>(resposta);

                    if (aResposta.result)
                    {
                        MessageBox.Show(String.Format("Usuário desbloqueado com sucesso."));
                        if (lbBloqueados.Items.Count == 1)
                        {
                            NavigationService.GoBack();
                        }
                        else {
                            bloqueados.Remove(selecionado);
                        }
                    }
                    else {
                        MessageBox.Show(String.Format("Não foi possível desbloquear o usuário."));
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