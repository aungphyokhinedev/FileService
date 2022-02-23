namespace FileService;
public class FileUploadResult : IResponse{
    public File data {get;set;}
     public int code { get;set; }
    public string? message { get;set; }
}