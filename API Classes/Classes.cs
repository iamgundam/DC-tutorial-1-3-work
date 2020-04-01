using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Classes
{
    public class DataIntermed
    {
        public uint acct;
        public uint pin;
        public string fname;
        public string lname;
        public int bal;

        public DataIntermed(uint inAcct, uint inPin, string inFName, string inLName, int inBal)
        {
            acct = inAcct;
            pin = inPin;
            fname = inFName;
            lname = inLName;
            bal = inBal;
        }
    }

    public class SearchData
    {
        public string searchStr;

        public SearchData(string inSearchStr)
        {
            searchStr = inSearchStr;
        }
    }
}
