using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;

public class GreetingBL : IGreetingBL
{
    private readonly GreetingContext _dbContext;

    private readonly ILogger<GreetingBL> _logger;
    private readonly IGreetingRL _greetingRL;
    public string GetGreeting(string? firstName, string?lastName)
    {
        _logger.LogInformation("GreetingBL: Returning greeting message.");
        string message;

        if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
        {
            message = $"Hello, {firstName} {lastName}!";
        }
        else if (!string.IsNullOrEmpty(firstName))
        {
            message = $"Hello, {firstName}!";
        }
        else if (!string.IsNullOrEmpty(lastName))
        {
            message = $"Hello, Mr./Ms. {lastName}!";
        }
        else
        {
            message = "Hello, World!";
        }

        // Save the greeting message in the repository
        SaveGreeting(message);

        return message;
    }

    public void SaveGreeting(string message)
    {
        _greetingRL.SaveGreeting(message);

    }
    public GreetingBL(GreetingContext dbContext , ILogger<GreetingBL> logger, IGreetingRL greetingRL)
    {
        _dbContext = dbContext;
        _logger = logger;
        _greetingRL = greetingRL;
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
