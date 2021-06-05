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

            lblVersion.Content += string.Format("v{0} ({1})", RbGlobals.RB_VERSION, RbGlobals.RB_BUILD_DATE);

            tbLicenseInfo.Text = File.ReadAllText(RbGlobals.LICENSE_FILE_NAME);
            tbThirdPartyLicenses.Text = File.ReadAllText(RbGlobals.THIRD_PARTY_LICENSE_FILE_NAME);

            lblLink.Click += (sender, args) => { new ShellCommand(lblLink.NavigateUri.ToString()).Execute(string.Empty); this.Close(); };
            this.KeyDown += (sender, args) => { if (args.Key == Key.Escape) this.Close(); };
        }
    }
}
