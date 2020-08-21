using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_Classes;
using Business_Tier_Web_Service.Models;

namespace Business_Tier_Web_Service.Controllers
{
    public class SearchController : ApiController
    {
        // POST api/<controller>
        [Route("api/search")]
        [HttpPost]
        public DataIntermed Post(SearchData value)
        {
            DataModel data = new DataModel();

            uint acct, pin;
            string fName, lName;
            int bal;

            lName = value.searchStr;
            data.SearchByLastName(lName, out fName, out acct, out pin, out bal);
            DataIntermed output = new DataIntermed();
            output.acct = acct;
            output.pin = pin;
            output.fname = fName;
            output.lname = lName;
            output.bal = bal;

            return output;
        }

    }
}