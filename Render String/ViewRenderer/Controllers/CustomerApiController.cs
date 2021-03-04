using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Westwind.Web.Mvc;

namespace ViewRendererSamples.Controllers
{
public class CustomerController : ApiController
{
    [HttpGet]
    public HttpResponseMessage Customer(int id = 0)
    {
        var customer = new Customer()
        {
            Id = id,
            Name = "Jimmy Roe",
            Address = "123 Nowwhere Lane\r\nAnytown, USA",
            Email = "jroe@doeboy.com"
        };

        string accept = Request.Headers
                                .GetValues("Accept")
                                .FirstOrDefault();

        if (!string.IsNullOrEmpty(accept) &&
            accept.ToLower().Contains("text/html"))
        {
            var html = ViewRenderer
                        .RenderView(
                              "~/views/samples/customerapi.cshtml",
                              customer);
            var message=new HttpResponseMessage(HttpStatusCode.OK);
            message.Content =new StringContent(html, Encoding.UTF8,
                                                "text/html");
            return message;
        }

        return Request.CreateResponse<Customer>(HttpStatusCode.OK, 
                                                customer);
    }

}

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

    }
}
