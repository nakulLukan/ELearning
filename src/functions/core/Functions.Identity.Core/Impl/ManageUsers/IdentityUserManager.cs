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
            string? email;
            bool isEmailVerified;
            GetEmailDetailsFromUserAttributes(userAttributes, out email, out isEmailVerified);
            string? phoneNumber;
            bool isPhoneNumberVerified;
            GetPhoneNumberDetailsFromUserAttributes(userAttributes, out phoneNumber, out isPhoneNumberVerified);

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
            if (email != null)
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@NormalizedEmail", email.ToUpperInvariant());
            }
            else
            {
                command.Parameters.AddWithValue("@Email", DBNull.Value);
                command.Parameters.AddWithValue("@NormalizedEmail", DBNull.Value);
            }
            command.Parameters.AddWithValue("@FullName", fullName);
            command.Parameters.AddWithValue("@EmailConfirmed", isEmailVerified);
            command.Parameters.AddWithValue("@PhoneNumberConfirmed", isPhoneNumberVerified);

            if (phoneNumber != null)
            {
                command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
            }
            else
            {
                command.Parameters.AddWithValue("@PhoneNumber", DBNull.Value);
            }

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

    private static void GetPhoneNumberDetailsFromUserAttributes(IDictionary<string, string> userAttributes, out string? phoneNumber, out bool isPhoneNumberVerified)
    {
        phoneNumber = null;
        isPhoneNumberVerified = false;
        if (userAttributes.ContainsKey("phone_number"))
        {
            phoneNumber = userAttributes["phone_number"];
        }

        if (userAttributes.ContainsKey("phone_number_verified"))
        {
            isPhoneNumberVerified = bool.Parse(userAttributes["phone_number_verified"]);
        }
    }

    private static void GetEmailDetailsFromUserAttributes(IDictionary<string, string> userAttributes, out string? email, out bool isEmailVerified)
    {
        email = null;
        isEmailVerified = false;
        if (userAttributes.ContainsKey("email"))
        {
            email = userAttributes["email"];
        }

        if (userAttributes.ContainsKey("email_verified"))
        {
            isEmailVerified = bool.Parse(userAttributes["email_verified"]);
        }
    }
}
