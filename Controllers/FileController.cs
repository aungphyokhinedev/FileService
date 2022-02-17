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
    [TypeFilter(typeof(AuthenticateAttribute))]

    public async Task<UploadResponse> Upload(IFormFile file, string accesstype)
    {
      
        var uploadresult = await _fileupload.uploadAsync(file);
        if(uploadresult.code == ResultCode.OK){
            var fileinfo = uploadresult.data;
            fileinfo.UserId =int.Parse( Request.HttpContext.Items["__uid"].ToString());
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
     [TypeFilter(typeof(AuthenticateAttribute))]
    public async Task<Response> Remove(string url)
    {
          File file = new File{
              UserId = int.Parse( Request.HttpContext.Items["__uid"].ToString()),
              FileUrl = url
          };
          var deleteresult = await _data.DeleteFile(file);
          return deleteresult;
      
    }

    
    
}
