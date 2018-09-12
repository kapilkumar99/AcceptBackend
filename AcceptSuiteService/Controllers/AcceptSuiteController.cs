using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNet.Utilities;
using Microsoft.AspNetCore.Mvc;
using net.authorize.sample;
using Newtonsoft.Json.Linq;

namespace AcceptSuiteService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AcceptSuiteController : ControllerBase
	{

		[HttpGet("AcceptJS")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public ActionResult<AcceptResponse> AcceptJs(string apiLoginId, string apiTransactionKey, string token)
		{

			AcceptResponse objAcceptResponse = new AcceptResponse();

			try
			{

				ProxyMethod();

				ANetApiResponse profileResponse = CreateAnAcceptPaymentTransaction.Run(apiLoginId, apiTransactionKey, token);


				if (profileResponse != null)
				{


					if (profileResponse.messages.resultCode.ToString() == "Ok")
					{
						objAcceptResponse.Status = true;
						objAcceptResponse.Value = ((AuthorizeNet.Api.Contracts.V1.createTransactionResponse)profileResponse)
							.transactionResponse.transId;

					}
					else
					{
						objAcceptResponse.Status = false;
						objAcceptResponse.Message = ((AuthorizeNet.Api.Contracts.V1.createTransactionResponse)profileResponse)
								  .transactionResponse
								  .errors[0].errorCode +
								  ((AuthorizeNet.Api.Contracts.V1.createTransactionResponse)profileResponse)
								  .transactionResponse.errors[0].errorText;

					}

				}
				else
				{
					objAcceptResponse.Status = false;
					return NotFound();
				}

			}
			catch (Exception e)
			{
				objAcceptResponse.Status = false;

				objAcceptResponse.Message = "Error occured while executing payment. " + e.Message;
			}


			return objAcceptResponse;
		}

		[HttpGet("AcceptHosted")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public ActionResult<AcceptResponse> AcceptHosted(string apiLoginId, string apiTransactionKey, string iFrameCommunicatorUrl)
		{
			AcceptResponse objAcceptResponse = new AcceptResponse();

			try
			{

				ProxyMethod();

				ANetApiResponse response = GetAnAcceptPaymentPage.Run(apiLoginId, apiTransactionKey, iFrameCommunicatorUrl);


				if (response != null)
				{



					if (response.messages.resultCode.ToString() == "Ok")
					{
						objAcceptResponse.Status = true;
						objAcceptResponse.Value =
							((AuthorizeNet.Api.Contracts.V1.getHostedPaymentPageResponse)response).token;

					}
					else
					{
						objAcceptResponse.Status = false;
						objAcceptResponse.Message = "Failed to get hosted payment page Error: " +
													response.messages.message[0].code + "  " +
													response.messages.message[0].text;

					}

				}
				else
				{
					objAcceptResponse.Status = false;
					return NotFound();
				}
			}
			catch (Exception e)
			{
				objAcceptResponse.Status = false;

				objAcceptResponse.Message = "Error occured while executing payment. " + e.Message;
			}


			return objAcceptResponse;

		}


		[HttpGet("AcceptCustomer")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public ActionResult<AcceptResponse> AcceptCustomer(string apiLoginId, string apiTransactionKey, string customerId)
		{
			AcceptResponse objAcceptResponse = new AcceptResponse();

			try
			{

				ProxyMethod();

				ANetApiResponse response = GetAcceptCustomerProfilePage.Run(apiLoginId, apiTransactionKey, customerId);


				if (response != null)
				{
					if (response.messages.resultCode.ToString() == "Ok")
					{
						objAcceptResponse.Status = true;
						objAcceptResponse.Value = "";
						//((AuthorizeNet.Api.Contracts.V1.getHostedPaymentPageResponse)response).token;

					}
					else
					{
						objAcceptResponse.Status = false;
						objAcceptResponse.Message = "Failed to get hosted payment page Error: " +
													response.messages.message[0].code + "  " +
													response.messages.message[0].text;

					}

				}
				else
				{
					objAcceptResponse.Status = false;
					return NotFound();
				}
			}
			catch (Exception e)
			{
				objAcceptResponse.Status = false;

				objAcceptResponse.Message = "Error occured while executing payment. " + e.Message;
			}


			return objAcceptResponse;
		}


		[HttpGet("ValidateCustomer")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public ActionResult<AcceptResponse> ValidateCustomer(string apiLoginId, string apiTransactionKey, string customerId)
		{
			AcceptResponse objAcceptResponse = new AcceptResponse();

			try
			{

				ProxyMethod();

				ANetApiResponse response = GetCustomerProfile.Run(apiLoginId, apiTransactionKey, customerId);


				if (response != null)
				{

					if (response.messages.resultCode.ToString() == "Ok")
					{
						objAcceptResponse.Status = true;
						objAcceptResponse.Value = response.messages.message[0].code + " " + response.messages.message[0].text;

					}
					else
					{
						objAcceptResponse.Status = false;
						objAcceptResponse.Message = "Error: " +
													response.messages.message[0].code + "  " +
													response.messages.message[0].text;

					}

				}
				else
				{
					objAcceptResponse.Status = false;
					return NotFound();
				}
			}
			catch (Exception e)
			{
				objAcceptResponse.Status = false;

				objAcceptResponse.Message = "Error . " + e.Message;
			}


			return objAcceptResponse;

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

	public class AcceptResponse
	{
		public string Value { get; set; }
		public string Message { get; set; }
		public bool Status = false;

	}

}

