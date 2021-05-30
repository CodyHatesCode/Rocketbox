using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rocketbox
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            lblVersion.Content += string.Format("v{0} ({1})", RbUtility.RB_VERSION, RbUtility.RB_BUILD_DATE);

            tbLicenseInfo.Text = File.ReadAllText("LICENSE");
            tbAcknowledgements.Text = File.ReadAllText("icons/LICENSE.txt");

            lblLink.Click += (sender, args) => { new ShellCommand(lblLink.NavigateUri.ToString()).Execute(string.Empty); this.Close(); };
            this.KeyDown += (sender, args) => { if (args.Key == Key.Escape) this.Close(); };
        }
    }
}
