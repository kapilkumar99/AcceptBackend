using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizeNET.Api.Contracts.V1;
using Microsoft.AspNetCore.Mvc;
using net.authorize.sample;

namespace AcceptSuiteService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcceptSuiteController : ControllerBase
    {
		
		[HttpGet("AcceptJS")]
		public ActionResult<string> AcceptJs()
        {
			ANetApiResponse profileResponse = CreateAnAcceptPaymentTransaction.Run("78BZ5Xprry", "8s2F95Q7brhHd7Tn", "token");
			return profileResponse.messages.ToString();
        }

	    [HttpGet("AcceptHosted")]
		public ActionResult<string> AcceptHosted()
	    {
		    ANetApiResponse profileResponse = GetAnAcceptPaymentPage.Run("78BZ5Xprry", "8s2F95Q7brhHd7Tn", 22);
		    return profileResponse.messages.ToString();
	    }

	    [HttpGet("AcceptCustomer")]
	    public ActionResult<string> AcceptCustomer()
	    {
		    ANetApiResponse profileResponse = GetAcceptCustomerProfilePage.Run("78BZ5Xprry", "8s2F95Q7brhHd7Tn", "1813212446");
		    return profileResponse.messages.ToString();
	    }

	    [HttpGet("ValidateCustomer")]
	    public ActionResult<string> ValidateCustomer()
	    {
		    ANetApiResponse profileResponse = GetCustomerProfile.Run("78BZ5Xprry", "8s2F95Q7brhHd7Tn", "1813212446");
		    return profileResponse.messages.ToString();
	    }

	}
}
