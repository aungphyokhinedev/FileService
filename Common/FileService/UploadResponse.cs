namespace FileService;
public class UploadResponse : IResponse {
    public List<IDictionary<string,object>> data {get;set;}
    public int code { get;set; }
    public string? message { get;set; }
}