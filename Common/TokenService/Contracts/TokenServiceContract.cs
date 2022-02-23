
using Newtonsoft.Json;

namespace  TokenService;

public enum ServiceTypes 
{
    GetToken,
    Validate ,

}

public interface TokenServiceContract{
    ServiceTypes type {get;set;}
    string request{get;set;}
}

public class TokenContract : TokenServiceContract
{
    public ServiceTypes type { get;set; }
    public string request { get;set; }

    public TokenContract(Dictionary<string, object> payload) {
        type = ServiceTypes.GetToken;
        request = JsonConvert.SerializeObject(payload);
    }

     public TokenContract(string token) {
        type = ServiceTypes.Validate;
        request = token;
    }
}

