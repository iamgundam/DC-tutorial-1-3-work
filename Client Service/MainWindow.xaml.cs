using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Business_Tier;

namespace Client_Service
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusinessServerInterface foob;

        public MainWindow()
        {
            InitializeComponent();

            //Open connection to Business tier
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8101/BusinessService";

            ChannelFactory<Business_Tier.BusinessServerInterface> businessFactory;
            businessFactory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            foob = businessFactory.CreateChannel();
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            string fName = "", lName = "";
            int bal = 0;
            uint acctNo = 0, pin = 0;

            index = Int32.Parse(IndexBox.Text);
            foob.GetValuesForEntry(index, out acctNo, out pin, out fName, out lName, out bal);
            FNameBox.Text = fName;
            LNameBox.Text = lName;
            AcctNoBox.Text = acctNo.ToString();
            PinBox.Text = pin.ToString("D4");
            BalanceBox.Text = bal.ToString();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string fName = "", lName = SearchBox.Text;
            int bal = 0;
            uint acctNo = 0, pin = 0;

            foob.SearchByLastName(lName, out fName, out acctNo, out pin, out bal);
            FNameBox.Text = fName;
            LNameBox.Text = lName;
            AcctNoBox.Text = acctNo.ToString();
            PinBox.Text = pin.ToString("D4");
            BalanceBox.Text = bal.ToString();
        }
    }
}
