using AplusExtension;
using DataService;
using MassTransit;

namespace FileService.DataAccess
{
    public class ServiceDataAccess : IDataAccess
    {
        //for dataservice
        private IRequestClient<GetList> _list;
        private IRequestClient<AddData> _add;
        private IRequestClient<RemoveData> _remove;
        private IRequestClient<UpdateData> _update;

        

        public ServiceDataAccess(IRequestClient<RemoveData> remove,IRequestClient<GetList> list, IRequestClient<AddData> add,IRequestClient<UpdateData> update)
        {
            _list = list;
            _add = add;
            _remove = remove;
            _update = update;

        }


        public async Task<DataService.Response> SaveFile(File file)
        {
            var data = new Dictionary<string, object>{
                        {"file_name" , file.FileName },
                        {"file_type" , file.FileType },
                        {"user_id" , file.UserId},
                        {"file_url" , file.FileUrl },
                        {"size" , file.Size },
                        {"access_type" , file.AccessType },
                    };
           
            var parameters = data.toParameterList();

            var result = await _add.GetResponse<ResultData>(new {request = new CreateRequest {
                table = "files",
                data = parameters
            }});

            return (DataService.Response)result.Message.response;
        }

         public async Task<DataService.Response> DeleteFile(File file)
        {
            
            
                var result = await _remove.GetResponse<ResultData>(new {request = new RemoveRequest{
                 table ="files",
                
                 filter = new Filter{
                    where = "user_id = @uid and file_url = @url",
                    parameters = new Dictionary<string, object>{
                        {"uid" , file.UserId },
                        {"url", file.FileUrl}
                    }.toParameterList()
                }
             }});

            return (DataService.Response)result.Message.response;
        }
    }
}