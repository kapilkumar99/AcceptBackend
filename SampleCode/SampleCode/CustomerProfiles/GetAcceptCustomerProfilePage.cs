using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNet.Api.Controllers;

namespace net.authorize.sample
{
    public class GetAcceptCustomerProfilePage
    {
        public static ANetApiResponse Run(string ApiLoginId, string ApiTransactionKey, string customerProfileId,string hostedPaymentIFrameCommunicatorUrl)
        {
            Console.WriteLine("Get Accept Customer Profile Page sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginId,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

			settingType[] settings = new settingType[1];
		
            settings[0] = new settingType();
            settings[0].settingName = settingNameEnum.hostedProfileIFrameCommunicatorUrl.ToString();
			settings[0].settingValue = hostedPaymentIFrameCommunicatorUrl;			

			var request = new getHostedProfilePageRequest();
            request.customerProfileId = customerProfileId;
            request.hostedProfileSettings = settings;

            var controller = new getHostedProfilePageController(request);
            controller.Execute();

            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine(response.messages.message[0].code);
                Console.WriteLine(response.messages.message[0].text);
                Console.WriteLine("Token: " + response.token.ToString());
            }
            else if(response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                              response.messages.message[0].text);
            }

            return response;
        }
    }
}
