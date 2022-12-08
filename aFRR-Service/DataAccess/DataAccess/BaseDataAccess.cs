using DataAccess.Attributes;
using DataAccess.Interfaces;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess.DataAccess;

internal abstract class BaseDataAccess<T> : IBaseDataAccess<T> where T : class
{
    private readonly string _connetionString;

    // ValueNames = "value1, value2"
    private string ValueNames => GetJoinedStrings(TableColumns);

    // ValueParameters = "@value1, @value2"
    private string ValueParameters => GetJoinedStrings(TableColumns, prefix: "@");

    // ValueUpdates = "value1=@value1, value2=@value2"
    private string ValueUpdates => GetJoinedConditionStrings(TableColumns, prefix: "@");

    private protected BaseDataAccess(string connectionstring)
    {
        TableName = typeof(T).Name;
        AutoIncrementingIds = GetAllPropertyNamesWithAttribute(typeof(IsAutoIncrementingIDAttribute));
        PrimaryKeys = GetAllPropertyNamesWithAttribute(typeof(IsPrimaryKeyAttribute));
        TableColumns = typeof(T).GetProperties().Select(property => property.Name).Except(AutoIncrementingIds);
        _connetionString = connectionstring;

        string condition = GetJoinedConditionStrings(PrimaryKeys, separator: " AND", prefix: "@");
        UpdateCommand = $"UPDATE {TableName} SET {ValueUpdates} WHERE {condition};";

        condition = GetJoinedConditionStrings(PrimaryKeys, separator: " AND", prefix: "@");
        GetCommand = $"SELECT * FROM {TableName} WHERE {condition};";

        GetAllCommand = $"SELECT * FROM {TableName};";

        condition = GetJoinedConditionStrings(PrimaryKeys, separator: " AND", prefix: "@");
        DeleteCommand = $"DELETE FROM {TableName} WHERE {condition};";

        if (AutoIncrementingIds.Any())
        {
            InsertCommand = $"INSERT INTO {TableName} ({ValueNames}) OUTPUT INSERTED.Id VALUES ({ValueParameters});";
        }
        else
        {
            InsertCommand = $"INSERT INTO {TableName} ({ValueNames}) VALUES ({ValueParameters});";
        }
    }
    private protected IDbConnection CreateConnection() => new SqlConnection(_connetionString);

    // The name of the table
    private protected string TableName { get; set; }

    // List of all primary keys, can be multiple in compund keys
    private protected IEnumerable<string> PrimaryKeys { get; set; }

    // List of all auto incrementing IDs
    private protected IEnumerable<string> AutoIncrementingIds { get; set; }

    // All TableColumns, excluding any auto incrementing IDs, those can be gotten from AutoIncrementingIds and often from PrimaryKeys
    private protected IEnumerable<string> TableColumns { get; set; }

    //Commands
    private protected string InsertCommand { get; set; }
    private protected string GetCommand { get; set; }
    private protected string GetAllCommand { get; set; }
    private protected string UpdateCommand { get; set; }
    private protected string DeleteCommand { get; set; }

    public virtual async Task<int> CreateAsync(T entity)
    {
        // INSERT INTO Table (value1, value2) VALUES (1, 2); SELECT LAST_INSERT_ID();
        try
        {
            using var connection = CreateConnection();
            if (!AutoIncrementingIds.Any())
            {
                await connection.QuerySingleOrDefaultAsync(InsertCommand, entity);
                return await Task.FromResult(0);
            }
            else
            {
                return await connection.QuerySingleAsync<int>(InsertCommand, entity);
            }
        }
        catch (Exception exception)
        {
            throw new Exception($"Error during async creation of '{typeof(T).Name}'!\n" +
                $"Message was: '{exception.Message}'\n" +
                $"Table Name: {TableName}\n" +
                $"ValueNames: {ValueNames}\n" +
                $"Command: {InsertCommand}", exception);
        }
    }

    public virtual async Task<bool> DeleteAsync(params dynamic[] id)
    {
        var parameters = PrimaryKeys.Zip(id, (key, value) => new { key, value }).ToDictionary(x => x.key, x => x.value);
        // DELETE FROM Table WHERE Value1=1 AND Value2=2;
        try
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(DeleteCommand, parameters) > 0;
        }
        catch (Exception exception)
        {
            throw new Exception($"Error during async deletion of '{typeof(T).Name}' with Id:{id}!\n" +
                $"Message was: '{exception.Message}'\n" +
                $"Table Name: {TableName}\n" +
                $"Command: {DeleteCommand}", exception);
        }
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        // SELECT * FROM Table;
        try
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<T>(GetAllCommand);
        }
        catch (Exception exception)
        {
            throw new Exception($"Error getting all async of '{typeof(T).Name}'!\n" +
                $"Message was: '{exception.Message}'\n" +
                $"Table Name: {TableName}\n" +
                $"ValueNames: {ValueNames}\n" +
                $"Command: {GetAllCommand}", exception);
        }
    }

    public virtual async Task<T> GetAsync(params dynamic[] id)
    {
        var parameters = PrimaryKeys.Zip(id, (key, value) => new { key, value }).ToDictionary(x => x.key, x => x.value);
        // SELECT * FROM Table WHERE Value1=1 AND Value2=2;
        try
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(GetCommand, parameters);
        }
        catch (Exception exception)
        {
            throw new Exception($"Error getting asyng by params:`{id}` of '{typeof(T).Name}'!\n" +
                $"Message was: '{exception.Message}'\n" +
                $"Table Name: {TableName}\n" +
                $"ValueNames: {ValueNames}\n" +
                $"Command: {GetCommand}", exception);
        }
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        try
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(UpdateCommand, entity) > 0;
        }
        catch (Exception exception)
        {
            throw new Exception($"Error during async update of '{typeof(T).Name}'!\n" +
                $"Message was: '{exception.Message}'\n" +
                $"Table Name: {TableName}\n" +
                $"ValueNames: {ValueNames}\n" +
                $"Command: {UpdateCommand}", exception);
        }
    }

    private static IEnumerable<string> GetAllPropertyNamesWithAttribute(Type attribute)
    {
        return typeof(T).GetProperties()
            .Where(property => Attribute.IsDefined(property, attribute) == true)
            .Select(property => property.Name);
    }

    private static string GetJoinedStrings(IEnumerable<string> strings, string separator = ",", string prefix = "")
    {
        return string.Join($"{separator} ",
            strings.ToList().Select(property => $"{prefix}{property}"));
    }

    private static string GetJoinedConditionStrings(IEnumerable<string> values, string separator = ",", string prefix = "")
    {
        return string.Join($"{separator} ",
            values.Zip(values, (value1, value2) => value1 + $"={prefix}" + value2));
    }
    private static string GetJoinedConditionStrings(IEnumerable<string> values1, IEnumerable<string> values2, string separator = ",", string prefix = "")
    {
        return string.Join($"{separator} ",
            values1.Zip(values2, (value1, value2) => value1 + $"={prefix}" + value2));
    }
}