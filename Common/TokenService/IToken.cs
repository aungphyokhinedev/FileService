namespace TokenService;
public interface IToken{
    TokenResponse getToken(Dictionary<string,object> claim);
    ValidateResponse validateToken(string token);
}