﻿#pragma checksum "C:\Users\rafael\Documents\GitHub\tagarela\Tagarela\Tagarela\AdicionarAmigo.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3083448C9AA57CEE72CACD6CA0183521"
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
    
    
    public partial class AdicionarAmigo : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock tbTitulo;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock tbLogin;
        
        internal System.Windows.Controls.TextBox txtAmigo;
        
        internal System.Windows.Controls.TextBlock tbPedido;
        
        internal System.Windows.Controls.ListBox lbPendentes;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Tagarela;component/AdicionarAmigo.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.tbTitulo = ((System.Windows.Controls.TextBlock)(this.FindName("tbTitulo")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.tbLogin = ((System.Windows.Controls.TextBlock)(this.FindName("tbLogin")));
            this.txtAmigo = ((System.Windows.Controls.TextBox)(this.FindName("txtAmigo")));
            this.tbPedido = ((System.Windows.Controls.TextBlock)(this.FindName("tbPedido")));
            this.lbPendentes = ((System.Windows.Controls.ListBox)(this.FindName("lbPendentes")));
        }
    }
}

