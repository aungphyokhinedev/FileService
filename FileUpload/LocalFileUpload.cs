namespace FileService;
public class LocalFileUpload : IFileUpload
{
    private IWebHostEnvironment _hostingEnvironment;

    public LocalFileUpload(IWebHostEnvironment environment) {
        _hostingEnvironment = environment;
    }

    public async Task<FileUploadResult> uploadAsync(IFormFile file)
    {
        try{
        string uploaddir = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads");
        
            if (file.Length > 0) {

                string filePrefix = Guid.NewGuid().ToString();
                string uploadName = filePrefix + file.FileName;
                string filePath = Path.Combine(uploaddir, uploadName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create)) {
                    await file.CopyToAsync(fileStream);
                    return new FileUploadResult {
                        code = ResultCode.OK,
                        data =  new File{
                            FileName = file.FileName,
                            Size = file.Length,
                            FileType = file.ContentType,
                            FileUrl = uploadName
                        }
                    };
                }
            }
            else{
                return  new FileUploadResult{
                code = ResultCode.NoContent,
                message = "Empty File",

            }; 
            }
        
        }
        catch(Exception ex){
            return  new FileUploadResult{
                code = ResultCode.InternalServerError,
                message = ex.Message,

            }; 
        }
    }
}