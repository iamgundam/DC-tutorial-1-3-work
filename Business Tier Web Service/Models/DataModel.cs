using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Data_Tier;

namespace Business_Tier_Web_Service.Models
{
    public class DataModel
    {
        private DataServerInterface data;

        public DataModel()
        {
            //Open connection to data tier server
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/DataService";

            ChannelFactory<Data_Tier.DataServerInterface> dataFactory;
            dataFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            data = dataFactory.CreateChannel();
        }

        public int GetNumEntries()
        {
            return data.GetNumEntries();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out string fName, out string lName, out int bal)
        {
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

            while (!found && !(index == data.GetNumEntries())) //While not found AND index not at end of array. +1 added to index in last iteration.
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

        }//end SearchByLastName
    }//end class
}//end namespace