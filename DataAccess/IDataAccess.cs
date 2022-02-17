using DataService;

namespace FileService;
public interface IDataAccess {
     Task<DataService.Response>  SaveFile(File file);

     Task<DataService.Response> DeleteFile(File file);
}