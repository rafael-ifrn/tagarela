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

        private async void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(servidor);


            Model.Autenticacao a = new Model.Autenticacao
            {
                username = txtEmail.Text,
                password = txtSenha.Text,
                nick = txtNick.Text
            };

            // MessageBox.Show(String.Format("O canal URI é {0}", uri));

            string s = JsonConvert.SerializeObject(a);

            var content = new StringContent(s, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/user/create", content);

            var str = response.Content.ReadAsStringAsync().Result;

            MessageBox.Show(String.Format("resposta do login é {0}", str));

            if (str.IndexOf("{\"session") == 0)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml?username="+ txtEmail.Text, UriKind.Relative));
            }
        }
    }
}