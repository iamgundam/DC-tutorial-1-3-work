using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Business_Tier_Web_Service.Models;
using API_Classes;

namespace Business_Tier_Web_Service.Controllers
{
    public class GetValuesController : ApiController
    {
        private DataModel data;

        public GetValuesController()
        {
            data = new DataModel();
        }

        // GET api/<controller>/5
        // Gets account values at import index.
        public DataIntermed Get(int index)
        {
            uint acct, pin;
            string fName, lName;
            int bal;

            data.GetValuesForEntry(index, out acct, out pin, out fName, out lName, out bal);
            DataIntermed output = new DataIntermed(acct, pin, fName, lName, bal);

            return output;
        }
    }
}