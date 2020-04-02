using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Data_Classes;

namespace Data_Tier
{
    class ServerRun
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Wel...");

            ServiceHost host;
            NetTcpBinding tcp = new NetTcpBinding();

            host = new ServiceHost(DataServer.get()) ;
            host.AddServiceEndpoint(typeof(DataServerInterface), tcp, "net.tcp://0.0.0.0:8100/DataService");
            host.Open();

            Console.WriteLine("...come! System online.");
            Console.ReadLine();

            host.Close();
        }
    }

    [ServiceContract]
    public interface DataServerInterface
    {
        [OperationContract]
        int GetNumEntries();

        [OperationContract]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out string fName, out string lName, out int bal);

    }//end DataServerInterface

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext =false, InstanceContextMode = InstanceContextMode.Single)]
    internal class DataServer : DataServerInterface
    {
        private static DataServer instance;
        private DbClass db;

        private DataServer()
        {
            db = new DbClass();
        }

        public static DataServer get()
        {
            if(instance == null)
            {
                instance = new DataServer();
            }

            return instance;
        }

        public int GetNumEntries()
        {
            return db.GetNumRecords();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out string fName, out string lName, out int bal)
        {
            acctNo = db.GetAcctNo(index);
            pin = db.GetPin(index);
            fName = db.GetFName(index);
            lName = db.GetLName(index);
            bal = db.GetBalance(index);
        }
    }//end DataServer

}//end namespace
