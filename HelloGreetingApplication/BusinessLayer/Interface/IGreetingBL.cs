using ModelLayer.Model;
using System.Collections.Generic;

public interface IGreetingBL
{
   
    ResponseModel<string> CreateRecord(RequestModel requestModel);
    ResponseModel<string> UpdateRecord(RequestModel requestModel);
    ResponseModel<string> PatchRecord(string key, RequestModel requestModel);
    ResponseModel<string> DeleteRecord(string key);
}
