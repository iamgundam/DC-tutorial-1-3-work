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
using RestSharp;
using API_Classes;
using Newtonsoft.Json;
using System.Threading;

namespace Client_Service
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestClient client;

        public MainWindow()
        {
            InitializeComponent();

            string URL = "https://localhost:44317/";
            client = new RestClient(URL);
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;

            //Get index to search for
            try
            {
                index = Int32.Parse(IndexBox.Text);
                //Fetch values at index
                RestRequest req = new RestRequest(String.Concat("api/getvalues/", index.ToString()));
                IRestResponse resp = client.Get(req);

                //Deserialize JSON returned by the response to our request
                DataIntermed result = JsonConvert.DeserializeObject<DataIntermed>(resp.Content);

                //Set values using DataIntermed object.
                FNameBox.Text = result.fname;
                LNameBox.Text = result.lname;
                AcctNoBox.Text = result.acct.ToString();
                PinBox.Text = result.pin.ToString("D4");
                BalanceBox.Text = result.bal.ToString();
            }
            catch(FormatException e1)
            {
                IndexBox.Text = "";
            }
        }
        /*
        // Same thread search
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //Use API class search template
            SearchData search = new SearchData();
            search.searchStr = SearchBox.Text;

            //Set URL request for search POST function, and add SearchData
            RestRequest req = new RestRequest(String.Concat("api/search/"));
            req.AddJsonBody(search);

            //Do the request
            IRestResponse resp = client.Post(req);

            //Deserialize and display.
            DataIntermed result = JsonConvert.DeserializeObject<DataIntermed>(resp.Content);

            FNameBox.Text = result.fname;
            LNameBox.Text = result.lname;
            AcctNoBox.Text = result.acct.ToString();
            PinBox.Text = result.pin.ToString("D4");
            BalanceBox.Text = result.bal.ToString();
        }
        */ 

        //Async search
        // Delegate for async search
        private delegate DataIntermed SearchRequest(string searchName);
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string lName = SearchBox.Text;

            //Sleep GUI to wait for results
            SearchBox.IsReadOnly = true;
            IndexBox.IsReadOnly = true;
            GoButton.IsEnabled = false;
            SearchButton.IsEnabled = false;

            //Set delegate target function
            SearchRequest sOp = AsyncSearchFunction;

            //Set callback function to run when async task finishes running
            AsyncCallback callback = this.OnSearchComplete;

            //Begin async task, searching
            sOp.BeginInvoke(lName, callback, null);
        }
        private DataIntermed AsyncSearchFunction(string searchName)
        {
            //Use API class search template
            SearchData search = new SearchData();
            search.searchStr = searchName;

            //Set URL request for search POST function, and add SearchData
            RestRequest req = new RestRequest(String.Concat("api/search/"));
            req.AddJsonBody(search);

            //Do the request
            IRestResponse resp = client.Post(req);

            //Deserialize and send to callback.
            return JsonConvert.DeserializeObject<DataIntermed>(resp.Content);
        }
        public void OnSearchComplete(IAsyncResult asyncResult)
        {
            SearchRequest sOp;

            //Retrieve async task from import
            AsyncResult asyncObj = (AsyncResult)asyncResult;

            //Check if the task has already been ended, before attempting to end
            if(asyncObj.EndInvokeCalled == false)
            {
                //Retrieve delegate
                sOp = (SearchRequest)asyncObj.AsyncDelegate;

                //End task, retrieve out variables
                DataIntermed result = sOp.EndInvoke(asyncObj);

                //Throws an InvalidOperationException without using dispatcher.
                this.Dispatcher.Invoke(() =>
                {
                    FNameBox.Text = result.fname;
                    LNameBox.Text = result.lname;
                    AcctNoBox.Text = result.acct.ToString();
                    PinBox.Text = result.pin.ToString("D4");
                    BalanceBox.Text = result.bal.ToString();

                    //Wake up GUI
                    SearchBox.IsReadOnly = false;
                    IndexBox.IsReadOnly = false;
                    GoButton.IsEnabled = true;
                    SearchButton.IsEnabled = true;
                });
            }

            //Clean up.
            asyncObj.AsyncWaitHandle.Close();
        }
        


    }//end class

}//end namespace
