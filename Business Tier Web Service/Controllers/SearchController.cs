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
    public class SearchController : ApiController
    {
        private DataModel data;

        public SearchController()
        {
            data = new DataModel();
        }

        // POST api/<controller>
        public DataIntermed Post(SearchData find)
        {
            uint acct, pin;
            string fName, lName;
            int bal;

            lName = find.searchStr;
            data.SearchByLastName(lName, out fName, out acct, out pin, out bal);
            DataIntermed output = new DataIntermed(acct, pin, fName, lName, bal);

            return output;
        }

    }
}