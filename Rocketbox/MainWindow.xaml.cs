﻿using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using NHotkey;
using NHotkey.Wpf;

namespace Rocketbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon _trayIcon;

        public MainWindow()
        {
            InitializeComponent();
            RbData.LoadData();

            _trayIcon = new System.Windows.Forms.NotifyIcon();
            _trayIcon.Icon = new Icon(@"icons/rocket.ico");
            _trayIcon.Visible = true;
            _trayIcon.Click += (sender, e) => { this.Show(); };

            HotkeyManager.Current.AddOrReplace("ShowRb", Key.OemTilde, ModifierKeys.Windows, OnHotkey);

            responseText.Text = string.Empty;
            textConsole.Text = string.Empty;

            Invoker.Invoke(textConsole.Text);

            // Event assignments:
            //  Window will close if unfocused
            //  Other window items will update if a key is pressed
            //  Text box will focus when focused (loaded)
            this.Deactivated += (sender, e) => this.Hide();
            this.KeyDown += KeyPress;
            this.Loaded += (sender, e) => textConsole.Focus();

            textConsole.TextChanged += TextUpdated;
        }

        private void TextUpdated(object sender, TextChangedEventArgs e)
        {
            // Invoker will process the command and return its result
            Invoker.Invoke(textConsole.Text);
            responseText.Text = Invoker.GetResponse();

            // Set an icon if the icon file listed in the command exists
            string icon = Invoker.GetIcon();
            if(icon != null && icon.Trim() != string.Empty)
            {
                if(RbUtility.IconExists(icon))
                {
                    iconView.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\icons\\" + icon));
                }
            }
            else
            {
                iconView.Source = null;
            }
        }

        private void KeyPress(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
                case Key.Enter:
                    if(Invoker.Execute())
                    {
                        this.Hide();
                    }
                    else
                    {
                        responseText.Text = Invoker.GetResponse();
                    }
                    break;
            }
        }

        private void OnHotkey(object sender, HotkeyEventArgs e)
        {
            this.Show();
        }
    }
}
