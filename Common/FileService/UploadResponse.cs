namespace FileService;
public class UploadResponse : IResponse {
    public List<IDictionary<string,object>> data {get;set;}
}