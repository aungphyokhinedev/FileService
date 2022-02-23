namespace  FileService;
public class LocalFileUpload : IFileUpload
{
    private IWebHostEnvironment _hostingEnvironment;
    private readonly IConfiguration _config;
    public LocalFileUpload(IWebHostEnvironment environment, IConfiguration config)
    {
        _hostingEnvironment = environment;
        _config = config;
    }

    private bool isSizeValid(IFormFile file)
    {
        var maxsize = _config.GetValue<int>("Upload:MaxSize");
        if (file.Length == 0)
        {
            throw new Exception("Empty File");
        }
        else if (file.Length > (maxsize * 1000))
        {
            throw new Exception("Exceed upload file size limit");
        }
        return true;
    }

    public async Task<FileUploadResult> uploadAsync(IFormFile file)
    {
        try
        {

            string uploaddir = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads");

            if (isSizeValid(file))
            {
                string filePrefix = Guid.NewGuid().ToString();
                string uploadName = filePrefix + file.FileName;
                string filePath = Path.Combine(uploaddir, uploadName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    return new FileUploadResult
                    {
                        code = StatusCodes.Status200OK,
                        data = new File
                        {
                            FileName = file.FileName,
                            Size = file.Length,
                            FileType = file.ContentType,
                            FileUrl = uploadName
                        }
                    };
                }
            }
            else
            {
                return new FileUploadResult
                {
                    code = StatusCodes.Status204NoContent,
                    message = "Empty File",

                };
            }

        }
        catch (Exception ex)
        {
            return new FileUploadResult
            {
                code = StatusCodes.Status500InternalServerError,
                message = ex.Message,

            };
        }
    }
}