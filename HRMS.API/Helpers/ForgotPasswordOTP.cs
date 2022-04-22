using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.API.Helpers
{
    public class ForgotPasswordOTP
    {
        public async Task<bool> SendOtp(EmployeeDetail empDetail, string message,string BASEURL)
        {
            empDetail.ContactNumber = "9560435849";//empDetail.ContactNumber;;
            using var client = new HttpClient();
            client.BaseAddress = new Uri(BASEURL);
            var response = await client.GetAsync("SMSApi/send?userid=klbsotp&password=Klb@2020&sendMethod=quick&mobile=" + empDetail?.ContactNumber + "&msg=" + message + "&senderid=MODOTP&msgType=text&duplicatecheck=true&format=text");
            return response.IsSuccessStatusCode;
        }
        public string GetRandomOtp()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var stringChars = new char[5];

            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        public string GetOtpMessage(EmployeeDetail empDetail, string randomPassword,Company company) =>
             $"Dear {empDetail?.EmployeeName}.  Your OTP is {randomPassword}" +
                $" Do not share with any one for security. Regards {company.Name}.";
         

    }
}
