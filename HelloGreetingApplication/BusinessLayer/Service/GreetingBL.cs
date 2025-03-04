using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using RepositoryLayer.Context;
using System.Linq;

public class GreetingBL : IGreetingBL
{
    private readonly GreetingContext _dbContext;

    private readonly ILogger<GreetingBL> _logger;

   

    public string GetGreeting()
    {
        _logger.LogInformation("GreetingBL: Returning greeting message.");
        return "Hello, World!";
    }

    public GreetingBL(GreetingContext dbContext , ILogger<GreetingBL> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public ResponseModel<string> GetGreetingMessage()
    {
        return new ResponseModel<string>
        {
            Success = true,
            Message = "API Endpoint Hit",
            Data = "Hello, World!"
        };
    }

    public ResponseModel<string> CreateRecord(RequestModel requestModel)
    {
        var response = new ResponseModel<string>
        {
            Success = true,
            Message = "API Endpoint Hit",
            Data = $"Key: {requestModel.key}, Value: {requestModel.value}"
        };
        return response;
    }

    public ResponseModel<string> UpdateRecord(RequestModel requestModel)
    {
        var existingRecord = _dbContext.Entries.FirstOrDefault(e => e.Key == requestModel.key);

        if (existingRecord == null)
        {
            return new ResponseModel<string>
            {
                Success = false,
                Message = "Record Not Found",
                Data = null
            };
        }

        existingRecord.Value = requestModel.value;
        _dbContext.SaveChanges();

        return new ResponseModel<string>
        {
            Success = true,
            Message = "Data Updated Successfully",
            Data = $"Updated Key: {requestModel.key}, Updated Value: {requestModel.value}"
        };
    }

    public ResponseModel<string> PatchRecord(string key, RequestModel requestModel)
    {
        var existingRecord = _dbContext.Entries.FirstOrDefault(e => e.Key == key);

        if (existingRecord == null)
        {
            return new ResponseModel<string>
            {
                Success = false,
                Message = "Record Not Found",
                Data = null
            };
        }

        if (!string.IsNullOrEmpty(requestModel.value))
        {
            existingRecord.Value = requestModel.value;
        }

        _dbContext.SaveChanges();

        return new ResponseModel<string>
        {
            Success = true,
            Message = "Data Partially Updated Successfully",
            Data = $"Updated Key: {key}, Updated Value: {existingRecord.Value}"
        };
    }

    public ResponseModel<string> DeleteRecord(string key)
    {
        var existingRecord = _dbContext.Entries.FirstOrDefault(e => e.Key == key);

        if (existingRecord == null)
        {
            return new ResponseModel<string>
            {
                Success = false,
                Message = "Record Not Found",
                Data = null
            };
        }

        _dbContext.Entries.Remove(existingRecord);
        _dbContext.SaveChanges();

        return new ResponseModel<string>
        {
            Success = true,
            Message = "Record Deleted Successfully",
            Data = $"Deleted Key: {key}"
        };
    }
}
