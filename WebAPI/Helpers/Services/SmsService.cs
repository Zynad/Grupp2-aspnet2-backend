using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WebAPI.Models.Sms;

namespace WebAPI.Helpers.Services;

public class SmsService
{
    private readonly IConfiguration _configuration;

    public SmsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> SendSmsAsync(string phoneNo,string smsBody)
    {
        try
        {
            var accountSid = _configuration["PhoneAccountSID"];
            var authToken = _configuration["PhoneAuthToken"];
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber(phoneNo));
            messageOptions.From = _configuration["PhoneFromNumber"];
            messageOptions.Body = smsBody;

            var message = await MessageResource.CreateAsync(messageOptions);
            if(message != null) 
            {
                return true;
            }

        }
        catch { }
        return false;
    }
}
