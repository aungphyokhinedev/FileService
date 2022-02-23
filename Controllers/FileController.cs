using AuthenticationService;
using DataService;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Controllers;

[ApiController]
public class FileController : ControllerBase
{

    private readonly ILogger<FileController> _logger;
    private IDataAccess _data;
    private IFileUpload _fileupload;

    public FileController(ILogger<FileController> logger,IDataAccess data, IFileUpload upload)
    {
        _logger = logger;
        _data = data;
        _fileupload = upload;
    }

    [HttpPost]
    [Route("upload/{accesstype}")]
    [Allowed("*")]

    public async Task<UploadResponse> Upload(IFormFile file, string accesstype)
    {
        var claim = HttpContext.Claim();
        var uploadresult = await _fileupload.uploadAsync(file);
        if(uploadresult.code == StatusCodes.Status200OK){
            var fileinfo = uploadresult.data;
            fileinfo.UserId = int.Parse(claim.uid);
            fileinfo.AccessType = accesstype;
            var saveresult = await _data.SaveFile(fileinfo);
            return new UploadResponse {
                code = saveresult.code,
                message = saveresult.message, 
                data = saveresult.rows   
                
            };
        }
        else{
            return new UploadResponse{
                code = uploadresult.code,
                message = uploadresult.message
            };
        }

       
    }

    
    

    
    [HttpDelete]
    [Route("file/{url}")]
    [Allowed("*")]
    public async Task<Response> Remove(string url)
    {
          var claim = HttpContext.Claim();
          File file = new File{
              UserId = int.Parse(claim.uid),
              FileUrl = url
          };
          var deleteresult = await _data.DeleteFile(file);
          return deleteresult;
      
    }

    
    
}
