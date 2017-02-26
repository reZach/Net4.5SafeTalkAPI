using Net4._5SafeTalkAPI.Hubs;
using Net4._5SafeTalkAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Net4._5SafeTalkAPI.Controllers
{
    public class CacheController : ApiHubController<SafeTalkHub>, IRedisCacheCache
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(GetCache());
        }

        [HttpPost]
        public IHttpActionResult Delete()
        {
            DeleteCache();
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult TestConnectionId()
        {
            return Ok(ConnectionId);
        }
    }
}
