﻿#pragma checksum "C:\Users\rafael\Documents\GitHub\tagarela\Tagarela\Tagarela\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1772CFD24B0F384F44BE49650F25E347"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Tagarela {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock tbLogin;
        
        internal System.Windows.Controls.TextBox txtLogin;
        
        internal System.Windows.Controls.TextBlock tbSenha;
        
        internal System.Windows.Controls.TextBox txtSenha;
        
        internal System.Windows.Controls.Button btnLogin;
        
        internal System.Windows.Controls.Button btnRegistrar;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Tagarela;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.tbLogin = ((System.Windows.Controls.TextBlock)(this.FindName("tbLogin")));
            this.txtLogin = ((System.Windows.Controls.TextBox)(this.FindName("txtLogin")));
            this.tbSenha = ((System.Windows.Controls.TextBlock)(this.FindName("tbSenha")));
            this.txtSenha = ((System.Windows.Controls.TextBox)(this.FindName("txtSenha")));
            this.btnLogin = ((System.Windows.Controls.Button)(this.FindName("btnLogin")));
            this.btnRegistrar = ((System.Windows.Controls.Button)(this.FindName("btnRegistrar")));
        }
    }
}

