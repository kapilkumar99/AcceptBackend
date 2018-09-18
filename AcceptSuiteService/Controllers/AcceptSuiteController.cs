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
		#region Create Transaction for Accept JS / Accept UI

		/// <summary>
		///  Method Implementation to create payment transaction for Accept JS and Accept UI
		/// </summary>
		/// <param name="apiLoginId"></param>
		/// <param name="apiTransactionKey"></param>
		/// <param name="token"></param>
		/// <returns>
		/// <param name="Status" value="True/False"></param>
		/// <param name="Value" value="if status true returns transaction id"></param>
		/// <param name="Message" value="if status false returns error message"></param>
		/// </returns>
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

						if (profileResponse.messages.resultCode.ToString().ToLower() == "error")
							objAcceptResponse.Message = profileResponse.messages.message[0].code + " " +
							                            profileResponse.messages.message[0].text;
						else
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

		#endregion

		#region Accept Hosted

		/// <summary>
		/// Method implementation is to retrieve token.
		/// </summary>
		/// <param name="apiLoginId"></param>
		/// <param name="apiTransactionKey"></param>
		/// <param name="iFrameCommunicatorUrl"></param>
		/// <returns>
		/// <param name="Status" value="True/False"></param>
		/// <param name="Value" value="if status true returns token"></param>
		/// <param name="Message" value="if status false returns error message"></param>
		/// </returns>
		[HttpGet("AcceptHosted")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public ActionResult<AcceptResponse> AcceptHosted(string apiLoginId, string apiTransactionKey, string iFrameCommunicatorUrl,string customerId= null)
		{
			AcceptResponse objAcceptResponse = new AcceptResponse();

			try
			{
				ProxyMethod();

				ANetApiResponse response = GetAnAcceptPaymentPage.Run(apiLoginId, apiTransactionKey, iFrameCommunicatorUrl, customerId);


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

		#endregion

		#region Accept Customer

		/// <summary>
		/// Method implementaion is to retrieve token for Accept Customer
		/// </summary>
		/// <param name="apiLoginId"></param>
		/// <param name="apiTransactionKey"></param>
		/// <param name="customerId"></param>
		/// <returns>
		/// <param name="Status" value="True/False"></param>
		/// <param name="Value" value="if status true returns token"></param>
		/// <param name="Message" value="if status false returns error message"></param>
		/// </returns>
		[HttpGet("AcceptCustomer")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public ActionResult<AcceptResponse> AcceptCustomer(string apiLoginId, string apiTransactionKey, string customerId,string iFrameCommunicatorUrl)
		{
			AcceptResponse objAcceptResponse = new AcceptResponse();

			try
			{

				ProxyMethod();

				ANetApiResponse response = GetAcceptCustomerProfilePage.Run(apiLoginId, apiTransactionKey, customerId, iFrameCommunicatorUrl);


				if (response != null)
				{
					if (response.messages.resultCode.ToString() == "Ok")
					{
						objAcceptResponse.Status = true;
						objAcceptResponse.Value = ((AuthorizeNet.Api.Contracts.V1.getHostedProfilePageResponse)response).token;					

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

		#endregion

		#region Validate Customer

		/// <summary>
		/// Method implementation is to validate customer
		/// </summary>
		/// <param name="apiLoginId"></param>
		/// <param name="apiTransactionKey"></param>
		/// <param name="customerId"></param>
		/// <returns>
		/// <param name="Status" value="True/False"></param>
		/// <param name="Value" value="if status true returns Success Message"></param>
		/// <param name="Message" value="if status false returns error message"></param>
		/// </returns>
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

		#endregion

		#region ProxyMethod

		/// <summary>
		/// Method Implementation is to set the values retrieved from System Environment Variables.
		/// </summary>
		private void ProxyMethod()
		{
			ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
			ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment.HttpUseProxy =
				AuthorizeNet.Environment.getBooleanProperty(Constants.HttpsUseProxy);

			//if HttpUseProxy is true than set the values for Proxy.
			if (ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment.HttpUseProxy)
			{
				//proxy username
				ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment.HttpsProxyUsername =
					AuthorizeNet.Environment.GetProperty(Constants.HttpsProxyUsername);
				//proxy password
				ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment.HttpsProxyPassword =
					AuthorizeNet.Environment.GetProperty(Constants.HttpsProxyPassword);
				//proxy host
				ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment.HttpProxyHost =
					AuthorizeNet.Environment.GetProperty(Constants.HttpsProxyHost);
				//proxy port
				ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment.HttpProxyPort =
					AuthorizeNet.Environment.getIntProperty(Constants.HttpsProxyPort);
			}
		}

		#endregion

	}

	public class AcceptResponse
	{
		public string Value { get; set; }//if status is true sets the response value
		public string Message { get; set; } //if status is false sets the error message
		public bool Status = false; //if status is true than the response is success

	}

}

