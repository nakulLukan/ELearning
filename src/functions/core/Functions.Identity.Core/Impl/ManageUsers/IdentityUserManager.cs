using Functions.Identity.Core.Contracts.ManageUsers;
using Npgsql;

namespace Functions.Identity.Core.Impl.ManageUsers;

public class IdentityUserManager : IIdentityUserManager
{
    private readonly string _connectionString;
    private NpgsqlConnection connection;

    public IdentityUserManager()
    {
        _connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")!;
    }

    public async Task<string> AddUser(IDictionary<string, string> userAttributes)
    {
        try
        {
            string email = userAttributes["email"];
            bool isEmailVerified = bool.Parse(userAttributes["email_verified"]);
            string sub = userAttributes["sub"];
            string fullName = userAttributes["name"];
            connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            string insertQuery = @"INSERT INTO ""AspNetUsers"" (""Id"", ""IsAdmin"", ""AccountCreatedOn"",""IsActive"") 
                                    VALUES 
                                    (@UserId, @IsAdmin, @AccountCreatedOn, @IsActive);
                                    INSERT INTO ""ApplicationUserOtherDetails"" (""PhoneNumber"", ""UserId"", ""Email"", ""NormalizedEmail"", ""EmailConfirmed"", ""PhoneNumberConfirmed"", ""FullName"")
                                    VALUES
                                    (@PhoneNumber, @UserId, @Email, @NormalizedEmail, @EmailConfirmed, @PhoneNumberConfirmed, @FullName)";


            // SQL query to insert user record
            using var command = new NpgsqlCommand(insertQuery, connection);
            // Add parameters to the query
            command.Parameters.AddWithValue("@UserId", sub);
            command.Parameters.AddWithValue("@IsAdmin", false);
            command.Parameters.AddWithValue("@AccountCreatedOn", DateTime.UtcNow);
            command.Parameters.AddWithValue("@IsActive", isEmailVerified);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@NormalizedEmail", email.ToUpperInvariant());
            command.Parameters.AddWithValue("@FullName", fullName);
            command.Parameters.AddWithValue("@EmailConfirmed", isEmailVerified);
            command.Parameters.AddWithValue("@PhoneNumberConfirmed", false);
            command.Parameters.AddWithValue("@PhoneNumber", DBNull.Value);

            // Execute the query
            await command.ExecuteNonQueryAsync();
            return sub;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to user account. Message: {0}\nStackTrace: {1}", ex.Message, ex.StackTrace);
            throw;
        }
    }
}
