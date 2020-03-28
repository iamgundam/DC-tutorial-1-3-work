using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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

        //Delegate for async search
        private delegate void SearchOperation(string lName, out string fName, out uint acctNo, out uint pin, out int balance);
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string fName, lName = SearchBox.Text;
            int bal;
            uint acctNo, pin;

            //Sleep GUI to wait for results
            SearchBox.IsReadOnly = true;
            IndexBox.IsReadOnly = true;
            GoButton.IsEnabled = false;
            SearchButton.IsEnabled = false;

            //Set delegate target function
            SearchOperation sOp = foob.SearchByLastName;

            //Set callback function to run when async task finishes running
            AsyncCallback callback = this.OnSearchComplete;

            //Begin async task, searching
            sOp.BeginInvoke(lName, out fName, out acctNo, out pin, out bal, callback, null);
        }
        public void OnSearchComplete(IAsyncResult asyncResult)
        {
            SearchOperation sOp;
            string fName, lName = SearchBox.Text;
            int bal;
            uint acctNo, pin;

            //Retrieve async task from import
            AsyncResult asyncObj = (AsyncResult)asyncResult;

            //Check if the task has already been ended, before attempting to end
            if(asyncObj.EndInvokeCalled == false)
            {
                //Retrieve delegate
                sOp = (SearchOperation)asyncObj.AsyncDelegate;

                //End task, retrieve out variables
                sOp.EndInvoke(out fName, out acctNo, out pin, out bal, asyncObj);

                //Display results.
                FNameBox.Text = fName;
                LNameBox.Text = lName;
                AcctNoBox.Text = acctNo.ToString();
                PinBox.Text = pin.ToString("D4");
                BalanceBox.Text = bal.ToString();

                //Wake up GUI
                SearchBox.IsReadOnly = false;
                IndexBox.IsReadOnly = false;
                GoButton.IsEnabled = true;
                SearchButton.IsEnabled = true;
            }

            //Clean up.
            asyncObj.AsyncWaitHandle.Close();
        }

    }//end class
}//end namespace
