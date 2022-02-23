using AplusExtension;
using MassTransit;
using Newtonsoft.Json;
using TokenService;

namespace AuthenticationService;
public class ServiceAuthenticate : IAuthenticate
{
    private IRequestClient<TokenServiceContract> client;
    public ServiceAuthenticate(IRequestClient<TokenServiceContract> _client)
    {
        client = _client;
    }

    public async Task<string> getTokenAsync(string uid, string role)
    {
        var claim = new Claim {
            uid = uid,
            role = role,
            expiration = DateTimeOffset.Now.AddMinutes(15)
        };
        var data = claim.ToDictionary();
        var contract = new TokenContract(data);
        var result = await client.GetResponse<SignedToken>(contract);
        if(result.Message.response.code == StatusCodes.Status200OK)
        return result.Message.response.token;
        else throw new Exception("Invalid Claim");
       
    }

    public async Task<Claim> validateTokenAsync(string token)
    {
        var result = await client.GetResponse<Payload>(new TokenContract(token));
        if (result.Message.response.code == StatusCodes.Status200OK)
        {
            var payload = result.Message.response.payload;
            return  payload.toObject<Claim>();     
        }
        else
        {
            throw new Exception("Authorization Fial");
        }
    }

    
    
}