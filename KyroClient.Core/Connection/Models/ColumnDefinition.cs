namespace KyroClient.Core.Connection.Models;

public sealed record ColumnDefinition(string Name, Type ClrType, string DbTypeName);