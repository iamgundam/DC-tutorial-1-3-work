using Data_Tier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier
{
    class BusinessProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Wel...");

            ServiceHost host;
            NetTcpBinding tcp = new NetTcpBinding();

            host = new ServiceHost(typeof(BusinessServer));
            host.AddServiceEndpoint(typeof(BusinessServerInterface), tcp, "net.tcp://0.0.0.0:8101/BusinessService");
            host.Open();

            Console.WriteLine("...come! Business Tier online.");
            Console.ReadLine();

            host.Close();
        }
    }

    [ServiceContract]
    public interface BusinessServerInterface
    {
        [OperationContract]
        int GetNumEntries();

        [OperationContract]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out string fName, out string lName, out int bal);

        [OperationContract]
        void SearchByLastName(string lName, out string fName, out uint acctNo, out uint pin, out int balance);

    }//end BusinesserverInterface

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class BusinessServer : BusinessServerInterface
    {
        private DataServerInterface data;
        private uint logCount;

        public BusinessServer()
        {
            //Open connection to Data tier server
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/DataService";

            ChannelFactory<Data_Tier.DataServerInterface> dataFactory;
            dataFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            data = dataFactory.CreateChannel();

            logCount = 0;
        }

        public int GetNumEntries()
        {
            Log("Accessed total number of database entries.");
            return data.GetNumEntries();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out string fName, out string lName, out int bal)
        {
            Log(String.Concat("Retrieved values at index ", index.ToString()));
            data.GetValuesForEntry(index, out acctNo, out pin, out fName, out lName, out bal);
        }

        public void SearchByLastName(string lName, out string fName, out uint acctNo, out uint pin, out int balance)
        {
            bool found = false;
            int index = 0;

            string searchFName, searchLName;
            uint searchAcctNo, searchPin;
            int searchBalance;

            //Pre-assign default values if not found
            fName = "";
            acctNo = 0;
            pin = 0;
            balance = 0;

            while(!found && !(index == data.GetNumEntries())) //While not found AND index not at end of array. +1 added to index in last iteration.
            {
                //Retrieve values at index
                data.GetValuesForEntry(index, out searchAcctNo, out searchPin, out searchFName, out searchLName, out searchBalance);

                //Check lName for equivalency
                if (String.Equals(lName, searchLName))
                {
                    //Apply values to out variables.
                    fName = searchFName;
                    acctNo = searchAcctNo;
                    pin = searchPin;
                    balance = searchBalance;

                    //Exit loop.
                    found = true;
                }

                index += 1;
            }

            Log(String.Concat("Searched database for ", lName));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(string logString)
        {
            logCount += 1;
            Console.WriteLine(String.Concat(logCount.ToString(), ": ", logString));
        }

    }//end class
}//end namespace
