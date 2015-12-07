using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Tagarela.Resources;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Phone.Notification;
using System.Net.Http;

namespace Tagarela
{
    public partial class MainPage : PhoneApplicationPage
    {

        private string channelName = "tagarelaIFRN";
        private string servidor = "https://meumsn.herokuapp.com";
        private string uriNotificacao = "";

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dic = NavigationContext.QueryString;
            if (dic.ContainsKey("username"))
            {
                txtLogin.Text = dic["username"];
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


        private async void btnLogin_Click(object sender, EventArgs e)
        {
            
            canalNotificacao();
            int tentativa = 1;
            while (uriNotificacao == "")
            {
                canalNotificacao();
                tentativa++;
            }



            //MessageBox.Show(uriNotificacao); 
            Model.Login a = new Model.Login
            {
                username = txtLogin.Text,
                password = txtSenha.Password,
            };

            // MessageBox.Show(String.Format("O canal URI é {0}", uri));
            var response = await requisicaoHttp().PostAsync("/api/user/login", conteudoJSON(JsonConvert.SerializeObject(a)));

            var resposta = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {

                Model.Dados sessao = JsonConvert.DeserializeObject<Model.Dados>(resposta);

                if (sessao.session.email != "" && sessao.session.email != null)
                {
                    NavigationService.Navigate(new Uri("/Usuario.xaml?sessao=" + sessao.session._id + "&nick=" + sessao.session.nick + "&pedidos=" + sessao.session.pedidos.Length, UriKind.Relative));
                } else {
                    MessageBox.Show("Login ou senha incorretos");
                }

            } else {
                Model.ErroPadrao aResposta = JsonConvert.DeserializeObject<Model.ErroPadrao>(resposta);
                MessageBox.Show(aResposta.message);
            }

        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Registrar.xaml", UriKind.Relative));
        }


        private void canalNotificacao()
        {
            // Recupera o canal se existir
            HttpNotificationChannel httpChannel = HttpNotificationChannel.Find(channelName);

            // Verifica se o canal existe
            if (httpChannel == null)
            {
                // Canal não existe. Instancia novo canal
                httpChannel = new HttpNotificationChannel(channelName);

                // Delegates para atualização, erro e recebimento de mensagem
                httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);
                httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ErrorOccurred);
                httpChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(httpChannel_ShellToastNotificationReceived);

                // Abre o canal e efetiva a ligação dos recebimentos
                httpChannel.Open();
                httpChannel.BindToShellToast();

            }
            else
            {
                // Canal existe
                // Delegates para atualização, erro e recebimento de mensagem
                httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);
                httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ErrorOccurred);
                httpChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(httpChannel_ShellToastNotificationReceived);
                // Mostra dados do canal
                System.Diagnostics.Debug.WriteLine(httpChannel.ChannelUri.ToString());
                //MessageBox.Show(String.Format("O canal URI é {0}", httpChannel.ChannelUri.ToString()));
                uriNotificacao = httpChannel.ChannelUri.ToString();
                // Atualiza serviço de usuários
                //atualizarUsuario(txtUsuario.Text, httpChannel.ChannelUri.ToString());
            }
        }

        private void httpChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                uriNotificacao = e.ChannelUri.ToString();
                // Mostra dados do canal
                System.Diagnostics.Debug.WriteLine(e.ChannelUri.ToString());
                // MessageBox.Show(String.Format("O canal URI é {0}", e.ChannelUri.ToString()));
                // Atualiza serviço de usuários
                //atualizarUsuario(txtUsuario.Text, e.ChannelUri.ToString());
            });
        }

        private void httpChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Mostra dados do erro
            Dispatcher.BeginInvoke(() => MessageBox.Show(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}",
                e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData)));
        }

        private void httpChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            string relativeUri = string.Empty;
            message.AppendFormat("Toast Recebido {0}:\n", DateTime.Now.ToShortTimeString());

            // Parse out the information that was part of the message.
            foreach (string key in e.Collection.Keys)
                message.AppendFormat("{0}: {1}\n", key, e.Collection[key]);

            // Display a dialog of all the fields in the toast.
            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(message.ToString());
                /*listMsg.Items.Add(
                    e.Collection["wp:Text1"] + ": " + e.Collection["wp:Text2"]); */
            });
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}