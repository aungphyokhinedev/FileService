

namespace AuthenticationService;
public interface IAuthenticate {
    Task<string> getTokenAsync(string uid, string role);  
    Task<Claim> validateTokenAsync(string token);
   
}


 