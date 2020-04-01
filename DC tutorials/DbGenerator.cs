using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Classes
{
    internal class DbGenerator
    {

        //Random generator fields
        private Random r = new Random();
        private string vowels = "aeiou";
        private string consonants = "bcdfghjklmnpqrstvwxyz";

        public void GetNextAccount(out uint acctNo, out uint pin, out string fName, out string lName, out int balance)
        {
            acctNo = (uint)r.Next(10000, 99999); //"Next" returns positive integer, safe cast
            pin = (uint)r.Next(0, 9999); //Four digit PIN
            fName = GetName();
            lName = GetName();
            balance = r.Next(-999999, 999999);
        }

        private string GetName()
        {
            string output = "";
            int a, b;

            //Concatenate 3 vowel + consonant pairs
            for(int ii = 0; ii < 3; ii++)
            {
                //Get two random numbers, retrieve element at that number, add to string.
                a = r.Next(0, 20);
                b = r.Next(0, 4);
                output = String.Concat(output, consonants[a], vowels[b]);
            }

            return output;
        }
    }//end DbGenerator

    internal class DataStruct
    {
        public uint acctNo;
        public uint pin;
        public string fName;
        public string lName;
        public int balance;

        public DataStruct()
        {
            acctNo = 0;
            pin = 0;
            fName = "blah";
            lName = "bleh";
            balance = 0;
        }

        public DataStruct(uint inAcctNo, uint inPin, string inFName, string inLName, int inBalance)
        {
            acctNo = inAcctNo;
            pin = inPin;
            fName = inFName;
            lName = inLName;
            balance = inBalance;
        }
    }//end DataStruct

    public class DbClass
    {
        private List<DataStruct> dbList; //List of database entries.
        private DbGenerator dbGen = new DbGenerator();

        //Constants
        private int INITIAL_ELEMENTS = 100000;

        public DbClass()
        {
            uint acctNo, pin;
            string fName, lName;
            int balance;

            dbList = new List<Data_Classes.DataStruct>();

            //Fills the database with randomly generated entries.
            for(int ii = 0; ii < INITIAL_ELEMENTS; ii++)
            {
                dbGen.GetNextAccount(out acctNo, out pin, out fName, out lName, out balance); //Generate values
                dbList.Add(new DataStruct(acctNo, pin, fName, lName, balance)); //Add to list a new DataStruct object.
            }
        }//end default

        public uint GetAcctNo(int index)
        {
            return dbList.ElementAt(index).acctNo;
        }

        public uint GetPin(int index)
        {
            return dbList.ElementAt(index).pin;
        }

        public string GetFName(int index)
        {
            return dbList.ElementAt(index).fName;
        }

        public string GetLName(int index)
        {
            return dbList.ElementAt(index).lName;
        }

        public int GetBalance(int index)
        {
            return dbList.ElementAt(index).balance;
        }

        public int GetNumRecords()
        {
            return dbList.Count;
        }

    }//end DbClass

}//end namespace
