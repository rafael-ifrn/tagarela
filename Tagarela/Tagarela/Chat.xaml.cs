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
using System.Windows.Media;

namespace Tagarela
{
    public partial class Chat : PhoneApplicationPage
    {

        private string channelName = "tagarelaIFRN";
        private string servidor = "https://meumsn.herokuapp.com";
        private string uri = "";
        private string sessao = "";
        private string email = "";


        public Chat()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dic = NavigationContext.QueryString;
            if (dic.ContainsKey("email"))
            {
                email = dic["email"];
            }

            if (dic.ContainsKey("sessao"))
            {
                sessao = dic["sessao"];
            }

            if (dic.ContainsKey("nick"))
            {
                tbNick.Text = dic["nick"];
            }
            
        }

        private void tbClear_Click(object sender, RoutedEventArgs e)
        {
            //Limpar Chat
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


        private async void enviarMensagem(object sender, RoutedEventArgs e)
        {
            if (sessao == "" || email == "")
            {
                MessageBox.Show(String.Format("Não é possível enviar a mensagem."));
            }
            else {

                Model.EnvioMensagem a = new Model.EnvioMensagem
                {
                    session = sessao,
                    amigo = email,
                    text = txtMensagem.Text
                };

                var response = await requisicaoHttp().PostAsync("/api/chat", conteudoJSON(JsonConvert.SerializeObject(a)));

                var resposta = response.Content.ReadAsStringAsync().Result;
                //MessageBox.Show(String.Format("Resposta: {0}", resposta));


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Result aResposta = JsonConvert.DeserializeObject<Result>(resposta);

                    if (aResposta.result)
                    {
                        //MessageBox.Show(String.Format("mensagem enviada."));
                        Mensagem mensagem = new Mensagem();
                        mensagem.text = a.text;
                        mensagem.time = "agora";
                        //recuperar a data
                        adicionarMensagem(mensagem, "ENVIADO");
                        txtMensagem.Text = "";
                    }
                    else {
                        MessageBox.Show(String.Format("Mensagem não aceita."));
                    }
                }
                else {
                    ErroPadrao aResposta = JsonConvert.DeserializeObject<ErroPadrao>(resposta);
                    MessageBox.Show(aResposta.message);
                }
            }

        }

        private async void recuperarMensagens()
        {
            if (sessao == "" || email == "")
            {
                MessageBox.Show(String.Format("Não é possível receber a mensagem."));
            }
            else {

                Model.ReceberMensagem a = new Model.ReceberMensagem
                {
                    session = sessao,
                    amigo = email,
                };

                var response = await requisicaoHttp().PutAsync("/api/chat", conteudoJSON(JsonConvert.SerializeObject(a)));

                var resposta = response.Content.ReadAsStringAsync().Result;
                //MessageBox.Show(String.Format("Resposta: {0}", resposta));


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    List<Messages> aResposta = JsonConvert.DeserializeObject<List<Messages>>(resposta);

                    if (aResposta.Count > 0)
                    {
                        string mensagens = "";
                        foreach (Messages mensagem in aResposta) {
                            //mensagens += mensagem.messages.text +"\n\n";
                            adicionarMensagem(mensagem.messages, "RECEBIDO");
                        }
                        //MessageBox.Show(String.Format(mensagens));

                    }
                    else {
                        //MessageBox.Show(String.Format("Não existe mensagem para o usuário."));
                    }
                }
                else {
                    ErroPadrao aResposta = JsonConvert.DeserializeObject<ErroPadrao>(resposta);
                    MessageBox.Show(aResposta.message);
                }
            }
        }


        private void btSync_Click(object sender, EventArgs e) {
            recuperarMensagens();
        }

        private async void btBloquear_Click(object sender, EventArgs e)
        {

            if (email == null || email == "")
            {
                MessageBox.Show(String.Format("Usuário não está selecionado."));
            }
            else {

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
                        //amigos.Remove(selecionado);
                        NavigationService.GoBack();
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


        private void adicionarMensagem(Mensagem mensagem, string origem) {

            Grid g = new Grid();
            g.MaxWidth = 395;
            g.VerticalAlignment = VerticalAlignment.Top;
            g.Margin = new Thickness(10, 10, 10, 0);

            TextBlock mensa = new TextBlock();
            mensa.Text = mensagem.text;
            mensa.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00));
            mensa.VerticalAlignment = VerticalAlignment.Top;
            mensa.TextWrapping = TextWrapping.Wrap;
            mensa.Padding = new Thickness(5, 3, 5, 0);

            TextBlock outros = new TextBlock();
            outros.Text = mensagem.time;
            outros.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00));
            outros.VerticalAlignment = VerticalAlignment.Top;
            outros.TextWrapping = TextWrapping.Wrap;
            outros.FontSize = 10.6;
            outros.Padding = new Thickness(5, 0, 0, 3);


            if (origem.Equals("ENVIADO"))
            {
                g.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xD6, 0xFF, 0x9C));
                g.HorizontalAlignment = HorizontalAlignment.Right;

                mensa.HorizontalAlignment = HorizontalAlignment.Right;

                outros.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else {
                g.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xB0, 0xE4, 0xF7));
                g.HorizontalAlignment = HorizontalAlignment.Left;

                mensa.HorizontalAlignment = HorizontalAlignment.Left;

                outros.HorizontalAlignment = HorizontalAlignment.Left;
            }


            StackPanel stk = new StackPanel();
            stk.Children.Add(mensa);
            stk.Children.Add(outros);
            g.Children.Add(stk);

            stkMensagens.Children.Add(g);
        }
    }
}