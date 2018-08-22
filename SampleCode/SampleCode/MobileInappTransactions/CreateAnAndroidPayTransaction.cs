using AuthorizeNET.Api.Contracts.V1;
using AuthorizeNET.Api.Controllers;
using AuthorizeNET.Api.Controllers.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.authorize.sample.MobileInappTransactions
{
    public class CreateAnAndroidPayTransaction
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, Decimal Amount)
        {
            Console.WriteLine("Create Android Pay Transaction Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNET.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            //set up data based on transaction
            var opaqueData = new opaqueDataType { dataDescriptor = "COMMON.ANDROID.INAPP.PAYMENT", dataValue = "ewoJInB1YmxpY0tleUhhc2giICAgIDogIlNiMHFVc0tpN1UwWjMrRm1Ed2xsa205UlVEUHZhY1BDVlVUdlk3OFVwWVk9IiwKCSJ2ZXJzaW9uIjogIjEuMCIsCgkiZGF0YSIgOiAiZXdvSkltVnVZM0o1Y0hSbFpFMWxjM05oWjJVaU9pQWllVzVCZGxob1ZVODJPVEpQY0drekwxRm5aVGRhVDB4a04yVXZTMGRNVEhOdFRUaGxlR1JDVFdob1pHZFdSMjVOS3pGNGF5OUNZMkZFVFVkWFdqaGFTVWRIVTBOQlQwNXJaelpZTTB0SmFtRjFjbVpLUmpJcmFrMHdRelp2V0d0RlNqZGlXa1J4ZUhRMldXUjViVWszZEhnMk4yNTJTa3czTm5ndmFYb3JjVzR2TUhWNFIyZEJZVmw1YzNCUVdWQkZhRTVHYkZkUVYxVmxiVEV3VlZKd1NqRTVkVVJwWmtaTFVqTmFTemRNVDA4M1ZYQTRlRkF4VnpodlFWWlVOV05hV1d0MFNIWmtkMjAwUm1SV05VTmFjV0ZXZDNKcU0wcG5kRUZyTnlJc0Nna2laWEJvWlcxbGNtRnNVSFZpYkdsalMyVjVJam9nSWsxR2EzZEZkMWxJUzI5YVNYcHFNRU5CVVZsSlMyOWFTWHBxTUVSQlVXTkVVV2RCUld0V2JGQklTeXREVFdJNE5TOXhSVFY2YW1kaGNYRTFlSGgyYTBzMVdERm5WV3BQU0RKSmJrTkZWamhCUzI5TlNFbFZaamxwUkZveVFWTXhTVEZZWkhZMlEzWktOa0kyTkVoUE1UbEJhRGx6T1doSFpYQlJQVDBpTEFvSkluUmhaeUk2SUNKM1RrMW1PVzFvWVdGb1IydDFWVGdyZG5CclN6UnBVa0o1YWtWaUwzUnNla3BLYVc5RVVGRjNNMVZCUFNJS2ZRPT0iCn0=" };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = opaqueData };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // authorize and capture transaction
                amount = Amount,
                payment = paymentType
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the contoller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            //validate
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if(response.transactionResponse.messages != null)
                    {
                        Console.WriteLine("Successfully created an Android pay transaction with Transaction ID: " + response.transactionResponse.transId);
                        Console.WriteLine("Response Code: " + response.transactionResponse.responseCode);
                        Console.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
                        Console.WriteLine("Description: " + response.transactionResponse.messages[0].description);
                    }
                    else
                    {
                        Console.WriteLine("Failed Transaction.");
                        if (response.transactionResponse.errors != null)
                        {
                            Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                            Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Failed Transaction.");
                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                    }
                    else
                    {
                        Console.WriteLine("Error Code: " + response.messages.message[0].code);
                        Console.WriteLine("Error message: " + response.messages.message[0].text);
                    }
                }
            }
            else
            {
                Console.WriteLine("Null Response.");
            }

            return response;

        }
    }
}
