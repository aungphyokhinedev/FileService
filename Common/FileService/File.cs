namespace FileService;

public class File {
    public int Id {get;set;}
    public string FileName{get;set;}
    public string FileType{get;set;}
    public string FileUrl{get;set;}
    public int UserId{get;set;}

    public string AccessType {get;set;}
    public long Size{get;set;}

    
}