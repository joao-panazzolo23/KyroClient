using System.Data;

namespace KyroClient.Core.Connection.Models;

public abstract record QueryResult
{
    public record Rows(DataTable Table, TimeSpan Elapsed) : QueryResult;
    public record RowsAffected(int Count, TimeSpan Elapsed) : QueryResult;
    public record Failed(string Message) : QueryResult;
}