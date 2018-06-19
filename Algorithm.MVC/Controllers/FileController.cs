using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Algorithm.MVC.Controllers
{
    [EnableCors(
        origins: "*",
        headers: "*",
        methods: "*")]
    public class FileController : ApiController
    {
        

        public IEnumerable<string> GetFiles()
        {
            var s = new List<string>()
                    {
                        "Amr",
                        "Fayez",
                        "Mahmoud"
                    };
            return s;
        }
    }
}
