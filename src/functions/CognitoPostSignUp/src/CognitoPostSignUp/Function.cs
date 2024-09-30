using Amazon.Lambda.CognitoEvents;
using Amazon.Lambda.Core;
using Functions.Identity.Core.Contracts.ManageUsers;
using Microsoft.Extensions.DependencyInjection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CognitoPostSignUp
{
    public class Function
    {
        private readonly IIdentityUserManager _identityUserManager;
        public Function()
        {
            var serviceProvider = Startup.ConfigureServices();
            _identityUserManager = serviceProvider.GetRequiredService<IIdentityUserManager>();
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input">The event for the Lambda function handler to process.</param>
        /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
        /// <returns></returns>
        public async Task<CognitoPostConfirmationEvent> FunctionHandler(CognitoPostConfirmationEvent input, ILambdaContext context)
        {
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(input.Request.UserAttributes));
            string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")!;
            var userId = await _identityUserManager.AddUser(input.Request.UserAttributes, connectionString);
            Console.WriteLine($"User '{userId}' added to database");
            return input;
        }
    }
}
