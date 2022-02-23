namespace  FileService;
public interface IFileUpload{
    Task<FileUploadResult> uploadAsync (IFormFile file);
}