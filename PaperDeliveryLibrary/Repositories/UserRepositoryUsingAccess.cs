using Microsoft.Extensions.Options;
using PaperDeliveryLibrary.Models;
using PaperDeliveryLibrary.ProjectOptions;
using PaperDeliveryWpf.Repositories;
using System.Data.OleDb;
using System.Net;
using System.Runtime.Versioning;

namespace PaperDeliveryLibrary.Repositories;

/// <summary>
/// This class is providing members inherited from <see cref="IUserRepository"/>.
/// <para></para>
/// This class is explicit addressing an Access database.
/// <para></para>
/// ConnectionStrings for AcccessDb from www.connectionstrings.com
/// <br></br>Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\myFolder\\myAccessFile.accdb;
/// <br></br>Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\myFolder\\myAccessFile.accdb;Persist Security Info=False;
/// <br></br>Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\myFolder\\myAccessFile.accdb;Jet OLEDB:Database Password=MyDbPassword;
/// <br></br>this is standard value: Persist Security Info=False
/// </summary>
[SupportedOSPlatform("windows")]
public class UserRepositoryUsingAccess : IUserRepository
{
    private readonly IOptions<DatabaseOptionsUsingAccess> _databaseOptions;

    public UserRepositoryUsingAccess(IOptions<DatabaseOptionsUsingAccess> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public bool Authenticate(NetworkCredential networkCredential)
    {
        bool output = false;

        // Validate parameters.
        ArgumentNullException.ThrowIfNullOrWhiteSpace(networkCredential.UserName, nameof(networkCredential.UserName));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(networkCredential.Password, nameof(networkCredential.Password));
        ArgumentNullException.ThrowIfNull(_databaseOptions, nameof(DatabaseOptionsUsingAccess));

        // Setup connection strings.
        string validatedPath = ValidatePath(_databaseOptions.Value);
        string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={validatedPath};Jet OLEDB:Database Password={_databaseOptions.Value.DatabasePassword};";

        string validatedTable = ValidateTable(_databaseOptions.Value, connectionString);
        string queryString = $"SELECT * FROM {validatedTable} WHERE Login = '{networkCredential.UserName}'";

        // Read from database.
        UserModel userModel = new();
        using var connection = new OleDbConnection(connectionString);
        OleDbCommand command = new(queryString, connection);
        try
        {
            connection.Open();

            using OleDbDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // Values can not be null.
                userModel.Id = (int)reader["ID"];
                userModel.Login = (string)reader["Login"];
                userModel.Password = (string)reader["Password"];
                userModel.DisplayName = (string)reader["DisplayName"];
                userModel.AccessLevel = (int)reader["AccessLevel"];

                // Values can be null.
                userModel.Email = DBNull.Value.Equals(reader["Email"]) ? string.Empty : (string)reader["Email"];
            }

            reader.Close();
            connection.Close();
        }
        catch (OleDbException ex)
        {
            if (ex.ErrorCode == -2147217843)
            {
                throw new UnauthorizedAccessException($"Invalid Password! No access to {validatedPath}! " +
                    $"Error code: {ex.ErrorCode}");
            }

            throw new UnauthorizedAccessException($"No access to {validatedPath}! Error code: {ex.ErrorCode}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected exception while accessing the database! Message: {ex.Message}");
        }

        // Validate password.
        if (networkCredential.Password == userModel.Password)
        {
            output = true;
        }

        return output;
    }

    public UserModel? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public UserModel? GetByUserName(string userName)
    {
        // Validate parameters.
        ArgumentNullException.ThrowIfNullOrWhiteSpace(userName, nameof(userName));
        ArgumentNullException.ThrowIfNull(_databaseOptions, nameof(DatabaseOptionsUsingAccess));

        // Setup connection strings.
        string validatedPath = ValidatePath(_databaseOptions.Value);
        string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={validatedPath};Jet OLEDB:Database Password={_databaseOptions.Value.DatabasePassword};";

        string validatedTable = ValidateTable(_databaseOptions.Value, connectionString);
        string queryString = $"SELECT * FROM {validatedTable} WHERE Login = '{userName}'";

        // Read from database.
        UserModel? output = new();
        using var connection = new OleDbConnection(connectionString);
        OleDbCommand command = new(queryString, connection);
        try
        {
            connection.Open();

            using OleDbDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // Values can not be null.
                output.Id = (int)reader["ID"];
                output.Login = (string)reader["Login"];
                output.Password = (string)reader["Password"];
                output.DisplayName = (string)reader["DisplayName"];
                output.AccessLevel = (int)reader["AccessLevel"];

                // Values can be null.
                output.Email = DBNull.Value.Equals(reader["Email"]) ? string.Empty : (string)reader["Email"];
            }

            reader.Close();
            connection.Close();
        }
        catch (OleDbException ex)
        {
            if (ex.ErrorCode == -2147217843)
            {
                throw new UnauthorizedAccessException($"Invalid Password! No access to {validatedPath}! " +
                    $"Error code: {ex.ErrorCode}");
            }

            throw new UnauthorizedAccessException($"No access to {validatedPath}! Error code: {ex.ErrorCode}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected exception while accessing the database! Message: {ex.Message}");
        }

        // Return UserModel.
        return output;
    }

    public void Add(UserModel user)
    {
        throw new NotImplementedException();
    }

    public void Update(UserModel user)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public UserModel? Login(string login, string password)
    {
        // Validate parameters.
        ArgumentNullException.ThrowIfNullOrWhiteSpace(login, nameof(login));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(password, nameof(password));
        ArgumentNullException.ThrowIfNull(_databaseOptions, nameof(DatabaseOptionsUsingAccess));

        // Setup connection strings.
        string validatedPath = ValidatePath(_databaseOptions.Value);
        string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={validatedPath};Jet OLEDB:Database Password={_databaseOptions.Value.DatabasePassword};";

        string validatedTable = ValidateTable(_databaseOptions.Value, connectionString);
        string queryString = $"SELECT * FROM {validatedTable} WHERE Login = '{login}'";

        // Read from database.
        UserModel? output = new();
        using var connection = new OleDbConnection(connectionString);
        OleDbCommand command = new(queryString, connection);
        try
        {
            connection.Open();

            using OleDbDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // Values can not be null.
                output.Id = (int)reader["ID"];
                output.Login = (string)reader["Login"];
                output.Password = (string)reader["Password"];
                output.DisplayName = (string)reader["DisplayName"];
                output.AccessLevel = (int)reader["AccessLevel"];

                // Values can be null.
                output.Email = DBNull.Value.Equals(reader["Email"]) ? string.Empty : (string)reader["Email"];
            }

            reader.Close();
            connection.Close();
        }
        catch (OleDbException ex)
        {
            if (ex.ErrorCode == -2147217843)
            {
                throw new UnauthorizedAccessException($"Invalid Password! No access to {validatedPath}! " +
                    $"Error code: {ex.ErrorCode}");
            }

            throw new UnauthorizedAccessException($"No access to {validatedPath}! Error code: {ex.ErrorCode}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected exception while accessing the database! Message: {ex.Message}");
        }

        // Validate password.
        if (password != output.Password)
        {
            output = null;
        }

        // Return UserModel.
        return output;
    }

    private static string ValidateTable(DatabaseOptionsUsingAccess databaseOptions, string connectionString)
    {
        string output = databaseOptions.DatabaseTable;

        var listOfTables = CaptureDatabaseTables(connectionString);

        if (!listOfTables.Contains(output))
        {
            throw new ArgumentException("Table not found in database.", databaseOptions.DatabaseTable);
        }

        return output;
    }

    private static string ValidatePath(DatabaseOptionsUsingAccess databaseOptions)
    {
        string ouptput = databaseOptions.DatabasePath;

        if (!File.Exists(ouptput))
        {
            throw new ArgumentException("Database not found.", databaseOptions.DatabasePath);
        }

        return ouptput;
    }

    /// <summary>
    /// This method is capturing the user tables from the database provided by the configuration file.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns>A list of user tables. The list is empty, if no user tables are found.</returns>
    /// <exception cref="Exception">If an unexpected exception is thrown.</exception>
    private static List<string> CaptureDatabaseTables(string connectionString)
    {
        List<string> output = [];

        using var connection = new OleDbConnection(connectionString);
        try
        {
            connection.Open();

            //var dbSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, []); // optional syntax
            var dbOtherSchemaTables = connection.GetSchema("Tables");

            if (dbOtherSchemaTables == null)
            {
                return output;
            }
            if (dbOtherSchemaTables.Rows.Count == 0)
            {
                return output;
            }

            for (int i = 0; i < dbOtherSchemaTables.Rows.Count; i++)
            {
                output.Add(dbOtherSchemaTables.Rows[i][2].ToString()!);
            }

            List<int> rowsToRemove = [];

            for (int i = 0; i < output.Count; i++)
            {
                if (output[i].Contains("MSys") || output[i].Contains("USys"))
                {
                    rowsToRemove.Add(i);
                }
            }

            for (int i = rowsToRemove.Count - 1; i >= 0; i--)
            {
                output.RemoveAt(rowsToRemove[i]);
            }
            connection.Close();
        }
        catch (Exception ex)
        {
            string message = $"An unexpected exception was thrown in <{nameof(CaptureDatabaseTables)}>.";
            throw new Exception(message, ex);
        }

        return output;
    }

}
