using AplusExtension;
using DataService;
using MassTransit;

namespace FileService.DataAccess
{
    public class ServiceDataAccess : IDataAccess
    {
        //for dataservice
        private IRequestClient<DataServiceContract> _data;
        

        

        public ServiceDataAccess(IRequestClient<DataServiceContract>  data)
        {
            _data = data;
        }


        public async Task<DataService.Response> SaveFile(File file)
        {
            /*var data = new Dictionary<string, object>{
                        {"file_name" , file.FileName },
                        {"file_type" , file.FileType },
                        {"user_id" , file.UserId},
                        {"file_url" , file.FileUrl },
                        {"size" , file.Size },
                        {"access_type" , file.AccessType },
                    };*/
            
            var contract = new Query("files").Insert(file).Contract();
            var result = await _data.GetResponse<ResultData>(contract);
            return (DataService.Response)result.Message.response;
        }

         public async Task<DataService.Response> DeleteFile(File file)
        {
            var contract = new Query("files").Delete().Where("user_id = @uid and file_url = @url",new {uid = file.UserId, url = file.FileUrl}).Contract();        
            var result = await _data.GetResponse<ResultData>(contract);

            return (DataService.Response)result.Message.response;
        }
    }
}