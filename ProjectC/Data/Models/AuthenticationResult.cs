namespace ProjectC.Data.Models
{
    public class AuthenticationResult
    {
        public bool WasSuccessful { get; set; } = true;
        public string? ErrorMessage { get; set;} = null;
    }
}
