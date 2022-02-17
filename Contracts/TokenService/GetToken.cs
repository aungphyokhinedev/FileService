namespace TokenService;
public interface GetToken {
    Dictionary<string,object> payload {get;}
}

public interface SignedToken {
    TokenResponse response{get;}
   
}