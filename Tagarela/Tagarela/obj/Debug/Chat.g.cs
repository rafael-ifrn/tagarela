﻿#pragma checksum "C:\Users\rafael\Documents\GitHub\tagarela\Tagarela\Tagarela\Chat.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "55F71FA6EE2D6F7A8E44A78C3A197199"
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
    
    
    public partial class Chat : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Image image;
        
        internal System.Windows.Controls.TextBlock tbNick;
        
        internal System.Windows.Controls.Button tbClear;
        
        internal System.Windows.Controls.Grid grdMensagens;
        
        internal System.Windows.Controls.StackPanel stkMensagens;
        
        internal System.Windows.Controls.TextBox txtMensagem;
        
        internal System.Windows.Controls.Button button;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Tagarela;component/Chat.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.image = ((System.Windows.Controls.Image)(this.FindName("image")));
            this.tbNick = ((System.Windows.Controls.TextBlock)(this.FindName("tbNick")));
            this.tbClear = ((System.Windows.Controls.Button)(this.FindName("tbClear")));
            this.grdMensagens = ((System.Windows.Controls.Grid)(this.FindName("grdMensagens")));
            this.stkMensagens = ((System.Windows.Controls.StackPanel)(this.FindName("stkMensagens")));
            this.txtMensagem = ((System.Windows.Controls.TextBox)(this.FindName("txtMensagem")));
            this.button = ((System.Windows.Controls.Button)(this.FindName("button")));
        }
    }
}

