
using DocumentFormat.OpenXml.Packaging;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Response;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace EmployeeSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private int id;
        private readonly ITestService _testService;
        private readonly ITestService _testService1;
        private readonly ITestService _testService2;

        public TestController(ITestService testService, ITestService test1, ITestService test2)
        {
            _testService = testService;
            _testService1 = test1;
            _testService2 = test2;
            if(ReferenceEquals(_testService, _testService1) && ReferenceEquals(_testService1, _testService2))
            {
                Console.WriteLine("Both objects are same");
            }
            else
            {
                Console.WriteLine("Different objects");
            }
            
        }

        [HttpGet]
        public ActionResult GetIp()
        {
            try
            {
                var context = HttpContext;

                // 1. Request Details
                Console.WriteLine($"Request Method: {context.Request.Method}");
                Console.WriteLine($"Request Path: {context.Request.Path}");
                Console.WriteLine($"Request Protocol: {context.Request.Protocol}");
                Console.WriteLine($"Request Host: {context.Request.Host}");
                Console.WriteLine($"Request ContentType: {context.Request.ContentType}");
                Console.WriteLine($"Request ContentLength: {context.Request.ContentLength}");
                Console.WriteLine($"Request QueryString: {context.Request.QueryString}");

                // Print all headers
                if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
                {
                    // Split the header value to get the scheme and token
                    var token = authorizationHeader.ToString().Split(' ').Last();

                    Console.WriteLine($"Extracted Token: {token}");
                }
                else
                {
                    Console.WriteLine("Authorization header not found.");
                }
                foreach (var header in context.Request.Headers)
                {
                    Console.WriteLine($"Request Header - {header.Key}: {header.Value}");
                }

                // 2. Response Details
                Console.WriteLine($"Response StatusCode: {context.Response.StatusCode}");
                Console.WriteLine($"Response ContentType: {context.Response.ContentType}");

                // Print all headers
                foreach (var header in context.Response.Headers)
                {
                    Console.WriteLine($"Response Header - {header.Key}: {header.Value}");
                }

                // 3. Connection Details
                Console.WriteLine($"Remote IP Address: {context.Connection.RemoteIpAddress}");
                Console.WriteLine($"Remote Port: {context.Connection.RemotePort}");
                Console.WriteLine($"Local IP Address: {context.Connection.LocalIpAddress}");
                Console.WriteLine($"Local Port: {context.Connection.LocalPort}");
                Console.WriteLine($"Connection ID: {context.Connection.Id}");
                Console.WriteLine($"Client Certificate: {(context.Connection.ClientCertificate != null ? context.Connection.ClientCertificate.Subject : "None")}");

                // 4. User Details
                Console.WriteLine($"Is Authenticated: {context.User.Identity.IsAuthenticated}");
                Console.WriteLine($"User Name: {context.User.Identity.Name}");
               
                Console.WriteLine($"Authentication Type: {context.User.Identity.AuthenticationType}");

                // Print all user claims
                foreach (var claim in context.User.Claims)
                {
                    Console.WriteLine($"User Claim - {claim.Type}: {claim.Value}");
                }

                // 5. Items Details
                foreach (var item in context.Items)
                {
                    Console.WriteLine($"Item Key: {item.Key}");

                    if (item.Value is null)
                    {
                        Console.WriteLine("Value: null");
                    }
                    else
                    {
                        Console.WriteLine($"Value Type: {item.Value.GetType()}");

                        // Check for specific types to print detailed information
                        if (item.Value is string)
                        {
                            Console.WriteLine($"String Value: {item.Value}");
                        }
                        else if (item.Value is int || item.Value is double || item.Value is float)
                        {
                            Console.WriteLine($"Numeric Value: {item.Value}");
                        }
                        else if (item.Value is DateTime dateTime)
                        {
                            Console.WriteLine($"DateTime Value: {dateTime.ToString("yyyy-MM-dd HH:mm:ss")}");
                        }
                        else if (item.Value is IEnumerable enumerable)
                        {
                            Console.WriteLine("Collection Value:");
                            foreach (var val in enumerable)
                            {
                                Console.WriteLine($"  - {val}");
                            }
                        }
                        else
                        {
                            // Use reflection to display properties of complex objects
                            Console.WriteLine("Complex Object Details:");
                            var properties = item.Value.GetType().GetProperties();
                            foreach (var prop in properties)
                            {
                                var propValue = prop.GetValue(item.Value, null);
                                Console.WriteLine($"  - {prop.Name}: {propValue}");
                            }
                        }
                    }
                }


                /*// 6. Session Details
                if (context.Session != null)
                {
                    Console.WriteLine($"Session ID: {context.Session.Id}");
                    foreach (var key in context.Session.Keys)
                    {
                        Console.WriteLine($"Session Key: {key}, Value: {context.Session.GetString(key)}");
                    }
                }
                else
                {
                    Console.WriteLine("No active session.");
                }*/

                // 7. Trace Identifier
                Console.WriteLine($"Trace Identifier: {context.TraceIdentifier}");

                // 8. WebSockets
                Console.WriteLine($"WebSocket Is Available: {context.WebSockets.IsWebSocketRequest}");

                // 9. Features
                foreach (var feature in context.Features)
                {
                    Console.WriteLine($"Feature: {feature.Key} - {feature.Value}");
                }

                // 10. Request Aborted
                Console.WriteLine($"Request Aborted: {context.RequestAborted.IsCancellationRequested}");


                var ip = HttpContext.Connection.RemoteIpAddress;
                Console.WriteLine("Ip : " + ip);
                var response = $"Your Ip Address is : {ip.ToString()}";
                return Ok(response);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ConvertToString")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<string>>> ConvertToString(IFormFile file)
        {
            var stream = file.OpenReadStream();
            var text = _testService.ConvertToTextAsync(stream);
            var response = new ApiResponse<string>
            {
                Success = true,
                Status = 200,
                Message = "conversion done",
                Data = text
            };
            return Ok(response);
            
        }

    }
}
