namespace WebAPI.Models.Email;

public class MailSettings
{
    private readonly IConfiguration _configuration;

    public MailSettings(IConfiguration configuration)
    {
        _configuration = configuration;
        DisplayName = _configuration["MailDisplayName"];
        From = _configuration["MailFrom"];
        UserName = _configuration["MailUsername"];
        Password = _configuration["MailPassword"];
        Host = _configuration["MailHost"];
        Port = Convert.ToInt32(_configuration["MailPort"]);
        UseSSL = _configuration.GetSection("Mailsettings").GetValue<bool>("UseSSl");
        UseStartTls = _configuration.GetSection("Mailsettings").GetValue<bool>("UseStartTls");
    }

    public string? DisplayName { get; set; } 
    public string? From { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Host { get; set; }
    public int Port { get; set; }
    public bool UseSSL { get; set; }
    public bool UseStartTls { get; set; }
    
}
