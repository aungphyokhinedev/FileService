namespace TokenService;
public interface ValidateToken {
    string token {get;}
}

public interface Payload {
    ValidateResponse response {get;}
}