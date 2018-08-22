using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNET.Api.Controllers;
using AuthorizeNET.Api.Contracts.V1;
using AuthorizeNET.Api.Controllers.Bases;

namespace net.authorize.sample.MobileInappTransactions
{
    public class CreateAnApplePayTransaction
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, Decimal Amount)
        {
            Console.WriteLine("Create Apple Pay Transaction Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNET.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            //set up data based on transaction
            var opaqueData = new opaqueDataType { dataDescriptor = "COMMON.APPLE.INAPP.PAYMENT", dataValue = "eyJkYXRhIjoiMHRTbWRvOGwrbkxYRE5DQWJkdmRBY1wveHNzVGFRYmdpaUpzemlnbEh1aTZzcmI1UHRaSjJQU3RibElmYUNGQVdBSGhXSVFsd1J6Ujg0V3FnOUhxN0o0QVZWMW5wQ2xUOEV2M2hjMlMxSVwvZEZPd1NxYWliQm45ejBFTTg3ZThNWVwvVGdtVmNYSDJ4XC80S1JFM08rZFIrcjNvT2VFV1BMM2Qydlo5WGJ1ZWk4enV4Y21zTG9xM1J4Vzc3WW1RclFCdUhDdUlvdXlEVGcyZmtRV2dleHY5ZTJLMmxIQ0lZa3Bicis3M0UzczZMRitaXC9OYk9acU9Cc0JFaWRrNEdQVU5VcllqQVpLWjRrcThOMVwvdloxU0RsaW1iaGFBKzNlempWWHJYWEowZXFIVnJBc1VHZVpyaGxUYjA5RWdya3cwNWhhbkRuNHdCWG91QU5NbTZhUFZVMWVNTTFcL2dYdm16Mmg4Tm1GbFJxRGljWkk2WmNHRXRoWElsVVpUaXlvQTVOWHFKbzJZaWlWcW8wNGJaemlOM3dVQjQ1b2k4eU1PTVlqaG9IYUZlcnFzUWFLaEU2bFA4bXdaVFV4IiwidmVyc2lvbiI6IkVDX3YxIiwiaGVhZGVyIjp7ImFwcGxpY2F0aW9uRGF0YSI6Ijk0ZWUwNTkzMzVlNTg3ZTUwMWNjNGJmOTA2MTNlMDgxNGYwMGE3YjA4YmM3YzY0OGZkODY1YTJhZjZhMjJjYzIiLCJ0cmFuc2FjdGlvbklkIjoiYzFjYWY1YWU3MmYwMDM5YTgyYmFkOTJiODI4MzYzNzM0Zjg1YmYyZjljYWRmMTkzZDFiYWQ5ZGRjYjYwYTc5NSIsImVwaGVtZXJhbFB1YmxpY0tleSI6Ik1JSUJTekNDQVFNR0J5cUdTTTQ5QWdFd2dmY0NBUUV3TEFZSEtvWkl6ajBCQVFJaEFQXC9cL1wvXC84QUFBQUJBQUFBQUFBQUFBQUFBQUFBXC9cL1wvXC9cL1wvXC9cL1wvXC9cL1wvXC9cL1wvXC9NRnNFSVBcL1wvXC9cLzhBQUFBQkFBQUFBQUFBQUFBQUFBQUFcL1wvXC9cL1wvXC9cL1wvXC9cL1wvXC9cL1wvXC84QkNCYXhqWFlxanFUNTdQcnZWVjJtSWE4WlIwR3NNeFRzUFk3emp3K0o5SmdTd01WQU1TZE5naUc1d1NUYW1aNDRST2RKcmVCbjM2UUJFRUVheGZSOHVFc1FrZjR2T2JsWTZSQThuY0RmWUV0NnpPZzlLRTVSZGlZd3BaUDQwTGlcL2hwXC9tNDduNjBwOEQ1NFdLODR6VjJzeFhzN0x0a0JvTjc5UjlRSWhBUFwvXC9cL1wvOEFBQUFBXC9cL1wvXC9cL1wvXC9cL1wvXC8rODV2cXRweGVlaFBPNXlzTDhZeVZSQWdFQkEwSUFCT2lVUkpScVA0WThTV3FXaVk3THJOVWtiMCtpTW1XVHdLdmJyWmtDanlUZTJSd2ZZc2lIU3ArM2Y0UnZqYVE5TjVWeFhzaEo2b0lYSlY3UmJIWmJxKzg9IiwicHVibGljS2V5SGFzaCI6IlFmMEZmdFd1c0FOZmN5c2NwbVJ1b2MxOEJyS1ByeHdQdmJQOEVUUTVNcTA9In0sInNpZ25hdHVyZSI6Ik1JSURRZ1lKS29aSWh2Y05BUWNDb0lJRE16Q0NBeThDQVFFeEN6QUpCZ1VyRGdNQ0dnVUFNQXNHQ1NxR1NJYjNEUUVIQWFDQ0Fpc3dnZ0luTUlJQmxLQURBZ0VDQWhCY2wrUGYzK1U0cGsxM25WRDlud1FRTUFrR0JTc09Bd0lkQlFBd0p6RWxNQ01HQTFVRUF4NGNBR01BYUFCdEFHRUFhUUJBQUhZQWFRQnpBR0VBTGdCakFHOEFiVEFlRncweE5EQXhNREV3TmpBd01EQmFGdzB5TkRBeE1ERXdOakF3TURCYU1DY3hKVEFqQmdOVkJBTWVIQUJqQUdnQWJRQmhBR2tBUUFCMkFHa0Fjd0JoQUM0QVl3QnZBRzB3Z1o4d0RRWUpLb1pJaHZjTkFRRUJCUUFEZ1kwQU1JR0pBb0dCQU5DOCtrZ3RnbXZXRjFPempnRE5yalRFQlJ1b1wvNU1LdmxNMTQ2cEFmN0d4NDFibEU5dzRmSVhKQUQ3RmZPN1FLaklYWU50MzlyTHl5N3hEd2JcLzVJa1pNNjBUWjJpSTFwajU1VWM4ZmQ0ZnpPcGszZnRaYVFHWE5MWXB0RzFkOVY3SVM4Mk91cDlNTW8xQlBWclhUUEhOY3NNOTlFUFVuUHFkYmVHYzg3bTByQWdNQkFBR2pYREJhTUZnR0ExVWRBUVJSTUUrQUVIWldQcld0SmQ3WVo0MzFoQ2c3WUZTaEtUQW5NU1V3SXdZRFZRUURIaHdBWXdCb0FHMEFZUUJwQUVBQWRnQnBBSE1BWVFBdUFHTUFid0J0Z2hCY2wrUGYzK1U0cGsxM25WRDlud1FRTUFrR0JTc09Bd0lkQlFBRGdZRUFiVUtZQ2t1SUtTOVFRMm1GY01ZUkVJbTJsK1hnOFwvSlh2K0dCVlFKa09Lb3NjWTRpTkRGQVwvYlFsb2dmOUxMVTg0VEh3TlJuc3ZWM1BydjdSVFk4MWdxMGR0Qzh6WWNBYUFrQ0hJSTN5cU1uSjRBT3U2RU9XOWtKazIzMmdTRTdXbEN0SGJmTFNLZnVTZ1FYOEtYUVl1WkxrMlJyNjNOOEFwWHNYd0JMM2NKMHhnZUF3Z2QwQ0FRRXdPekFuTVNVd0l3WURWUVFESGh3QVl3Qm9BRzBBWVFCcEFFQUFkZ0JwQUhNQVlRQXVBR01BYndCdEFoQmNsK1BmMytVNHBrMTNuVkQ5bndRUU1Ba0dCU3NPQXdJYUJRQXdEUVlKS29aSWh2Y05BUUVCQlFBRWdZQ0FNdzhVMmsxODM3ZW9mVkFreVVSK2E2bkt3VVNzYWthXC90VEd6QUswOWY4dHdsdDFmQTR0RVJBK21wTEppUWZkU1NZXC9mN2ZQNTJRb001UGhCZFYzTFBRaktSOFo1TjNKUzdGbU5Kd1ZTbndRY3gzYVJuaFR2d0RNRHMrZVJtSm5JK0VcL242Y05wYXpMeDVMRzRFMGplMFwvclJkbEhXYWxEK1NCTW4xQW5YY2c9PSJ9" }; 

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = opaqueData };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString() ,    // authorize and capture transaction
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
                        Console.WriteLine("Successfully created an Apple pay transaction with Transaction ID: " + response.transactionResponse.transId);
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
