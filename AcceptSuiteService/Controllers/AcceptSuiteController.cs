using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNet.Utilities;
using Microsoft.AspNetCore.Mvc;
using net.authorize.sample;

namespace AcceptSuiteService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AcceptSuiteController : ControllerBase
	{

		[HttpGet("AcceptJS")]
		public ActionResult<string> AcceptJs(string token)
		{

			ProxyMethod();

			ANetApiResponse profileResponse = CreateAnAcceptPaymentTransaction.Run("42a6v35CanG9", "43xmRuVC68tD8879", token);
			//return profileResponse.messages.message[0].code + " " + profileResponse.messages.message[0].text;

			//string message = profileResponse.messages.message[0].code + " " + profileResponse.messages.message[0].text;

			string message = string.Empty;

			if (profileResponse != null)
			{
				if (profileResponse.messages.resultCode.ToString() == "Ok")
					message = "Successfully created transaction with Transaction ID: " +
					          ((AuthorizeNet.Api.Contracts.V1.createTransactionResponse) profileResponse)
					          .transactionResponse.transId;
				else
				{
					message = ((AuthorizeNet.Api.Contracts.V1.createTransactionResponse) profileResponse)
					          .transactionResponse
					          .errors[0].errorCode +
					          ((AuthorizeNet.Api.Contracts.V1.createTransactionResponse) profileResponse)
					          .transactionResponse.errors[0].errorText;

				}
			}			

			Response.StatusCode = (int)HttpStatusCode.OK;
			return Content(message, MediaTypeNames.Text.Plain);
		}

		[HttpGet("AcceptHosted")]
		public ActionResult<string> AcceptHosted()
		{
			ProxyMethod();

			//var response = GetAnAcceptPaymentPage.Run("42a6v35CanG9", "43xmRuVC68tD8879");

			ANetApiResponse response = GetAnAcceptPaymentPage.Run("42a6v35CanG9", "43xmRuVC68tD8879");

			string message = string.Empty;


			//validate
			if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
			{
				 message = ((AuthorizeNet.Api.Contracts.V1.getHostedPaymentPageResponse)response).token;
			}
			else if (response != null)
			{
				 message = "Failed to get hosted payment page Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text;
				
			}

			Response.StatusCode = (int)HttpStatusCode.OK;
			return Content(message, MediaTypeNames.Text.Plain);

		}

		[HttpGet("AcceptCustomer")]
		public ActionResult<string> AcceptCustomer()
		{
			ANetApiResponse profileResponse = GetAcceptCustomerProfilePage.Run("78BZ5Xprry", "8s2F95Q7brhHd7Tn", "1813212446");
			return profileResponse.messages.ToString();
		}

		[HttpGet("ValidateCustomer")]
		public ActionResult<string> ValidateCustomer(string customerId)
		{
			try
			{
				//1813212446
				ANetApiResponse profileResponse = GetCustomerProfile.Run("78BZ5Xprry", "8s2F95Q7brhHd7Tn", customerId);
				string message = profileResponse.messages.message[0].code + " " + profileResponse.messages.message[0].text;

				Response.StatusCode = (int)HttpStatusCode.OK;
				return Content(message, MediaTypeNames.Text.Plain);


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return e.Message + " Stack Trace " + e.StackTrace;

			}

		}

		[HttpGet("Test")]
		public ActionResult<string> Test()
		{
			return "Hello World !!!";
		}


		private void ProxyMethod()
		{
			ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
			ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment.HttpUseProxy =
				AuthorizeNet.Environment.getBooleanProperty(Constants.HttpsUseProxy);

			if (ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment.HttpUseProxy)
			{
				ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment.HttpsProxyUsername =
					AuthorizeNet.Environment.GetProperty(Constants.HttpsProxyUsername);
				ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment.HttpsProxyPassword =
					AuthorizeNet.Environment.GetProperty(Constants.HttpsProxyPassword);
				ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment.HttpProxyHost =
					AuthorizeNet.Environment.GetProperty(Constants.HttpsProxyHost);
				ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment.HttpProxyPort =
					AuthorizeNet.Environment.getIntProperty(Constants.HttpsProxyPort);
			}
		}
	}

}

