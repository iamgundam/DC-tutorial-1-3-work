using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Business_Tier_Web_Service.Models;

namespace Business_Tier_Web_Service.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/<controller>
        // Default get request, returns number of entries in database.
        public string Get()
        {
            DataModel data = new DataModel();
            return data.GetNumEntries().ToString();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}