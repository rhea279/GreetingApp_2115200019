using ModelLayer.Model;
using System.Collections.Generic;

public interface IGreetingBL
{
    string GetGreeting(string? firstName, string? lastName);
    void SaveGreeting(string message);
    ResponseModel<string> CreateRecord(RequestModel requestModel);
    ResponseModel<string> UpdateRecord(RequestModel requestModel);
    ResponseModel<string> PatchRecord(string key, RequestModel requestModel);
    ResponseModel<string> DeleteRecord(string key);
}
