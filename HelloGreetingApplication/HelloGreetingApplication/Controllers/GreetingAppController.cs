using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using RepositoryLayer.Context;


namespace HelloGreetingApplication.Controllers
{

    ///<summary>
    ///Class providing API for HelloGreeting
    ///</summary>
    ///
    [ApiController]
    [Route("[controller]")]
    public class GreetingAppController : ControllerBase
    {
        private readonly IGreetingBL _greetingBL;
        private readonly GreetingContext _dbContext;
        private readonly ILogger<GreetingAppController> _logger;
        public GreetingAppController(GreetingContext dbContext, IGreetingBL greetingBL, ILogger<GreetingAppController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _greetingBL = greetingBL ?? throw new ArgumentNullException(nameof(greetingBL)); 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get method to get the greeting message
        /// </summary>
        ///<returns>"Hello,World!"</returns>

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("GET request received at GreetingAppController."); 
            
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "API Endpoint Hit";
            responseModel.Data = "Hello, World!";
            
            _logger.LogInformation("GET request received at GreetingAppController.");
            return Ok(responseModel);
        }

        /// <summary>
        /// Post Method to create a new record
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>

        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            _logger.LogInformation("POST request received: {@RequestModel}", requestModel);

            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = " API Endpoint Hit";
            responseModel.Data = $"Key:{requestModel.key},Value:{requestModel.value}";

            _logger.LogInformation("Returning response: {@Response}", responseModel);
            return Ok(responseModel);

        }

        /// <summary>
        /// PUT method to update an existing record 
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>

        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            _logger.LogInformation("PUT request received for Key: {Key}", requestModel.key);

            // Check if request is valid
            if (requestModel == null || string.IsNullOrEmpty(requestModel.key))
            {
                _logger.LogWarning("PUT request failed: Invalid request data.");
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid Request Data",
                    Data = null
                });
            }

            // Fetch the existing record from the database (Example: using Entity Framework)
            var existingRecord = _dbContext.Entries.FirstOrDefault(e => e.Key == requestModel.key);

            if (existingRecord == null)
            {
                _logger.LogWarning("PUT request failed: Record not found for Key: {Key}", requestModel.key);
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Record Not Found",
                    Data = null
                });
            }

            // Update the value
            existingRecord.Value = requestModel.value;
            _dbContext.SaveChanges(); // Save changes to the database

            _logger.LogInformation("Record updated successfully for Key: {Key}", requestModel.key);

            // Return success response
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Data Updated Successfully",
                Data = $"Updated Key: {requestModel.key}, Updated Value: {requestModel.value}"
            });
        }
        /// <summary>
        /// PATCH method to partially update an existing record
        /// </summary>
        /// <param name="key"></param>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPatch]
        public IActionResult Patch(string key, [FromBody] RequestModel requestModel)
        {
            if (string.IsNullOrEmpty(key) || requestModel == null)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid Request Data",
                    Data = null
                });
            }

            // Find the existing record
            var existingRecord = _dbContext.Entries.FirstOrDefault(e => e.Key == key);

            if (existingRecord == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Record Not Found",
                    Data = null
                });
            }

            // Update only the fields that are provided
            if (!string.IsNullOrEmpty(requestModel.value))
            {
                existingRecord.Value = requestModel.value;
            }

            _dbContext.SaveChanges();

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Data Partially Updated Successfully",
                Data = $"Updated Key: {key}, Updated Value: {existingRecord.Value}"
            });
        }

        /// <summary>
        /// DELETE method to remove an existing record
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        [HttpDelete("{key}")]
        public IActionResult Delete(string key)
        {
            _logger.LogInformation("DELETE request received for Key: {Key}", key);
            if (string.IsNullOrEmpty(key))
            {
                _logger.LogWarning("DELETE request failed: Invalid key provided.");
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid Request Data",
                    Data = null
                });
            }

            // Find the existing record
            var existingRecord = _dbContext.Entries.FirstOrDefault(e => e.Key == key);

            if (existingRecord == null)
            {
                _logger.LogWarning("DELETE request failed: Record not found for Key: {Key}", key);
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Record Not Found",
                    Data = null
                });
            }

            _dbContext.Entries.Remove(existingRecord);
            _dbContext.SaveChanges(); // Save changes

            _logger.LogInformation("Record deleted successfully for Key: {Key}", key);

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Record Deleted Successfully",
                Data = $"Deleted Key: {key}"
            });
        }
        ///<summary>
        ///UC2 Task - Get Greeting from Business Layer
        ///</summary>
        [HttpGet("greeting")]
        public IActionResult GetGreeting([FromQuery] string? firstName = null, [FromQuery] string? lastName = null)
        {
            _logger.LogInformation("GET request received at GreetingAppController.");

            string message;
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                message= $"Hello, {firstName} {lastName}!";
            }
            else if (!string.IsNullOrEmpty(lastName))
            {
                message = $"Hello {lastName}";
            }
            else if (!string.IsNullOrEmpty(firstName))
            {
                message = $"Hello {firstName}";
            }
            else
            {
                message = "Hello, World!";
            }

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting Generated Successfully",
                Data = message
            });
        }



    }
}
