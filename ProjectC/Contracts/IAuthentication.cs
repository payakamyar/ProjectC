using ProjectC.Data.DTO;
using ProjectC.Data.Models;

namespace ProjectC.Contracts
{
    public interface IAuthentication
    {
        public AuthenticationResult Authenticate(UserLoginDTO user);
    }
}
