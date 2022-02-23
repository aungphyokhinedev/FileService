namespace TokenService;
public class ValidateResponse {
    public int code{get;set;}
    public string? message{get;set;}
    public Dictionary<string,object>? payload {get;set;}
}