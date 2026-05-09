// using KyroClient.Core.Connection.Strategies;
//
// namespace KyroClient.Core.Secrets.Models;
//
// internal sealed record ConnectionProfileDto
// {
//     public Guid Id { get; init; }
//     public required string Name { get; init; }
//     public required string ProviderName { get; init; }
//     public required string SecretKey { get; init; }   // ← referência pro secret store
//     public required IConnectionOptions Options { get; init; } // ← sem senha
//     public DateTime LastUsed { get; init; }
//
//     public static ConnectionProfileDto FromProfile(ConnectionProfile profile, string secretKey)
//         => new()
//         {
//             Id = profile.Id,
//             Name = profile.Name,
//             ProviderName = profile.ProviderName,
//             SecretKey = secretKey,
//             Options = profile.Options.WithoutPassword(),
//             LastUsed = profile.LastUsed
//         };
//
//     public ConnectionProfile ToProfile(string? password)
//         => new()
//         {
//             Id = Id,
//             Name = Name,
//             ProviderName = ProviderName,
//             Options = Options.WithPassword(password),
//             LastUsed = LastUsed
//         };
// }
//
// internal class ConnectionProfile
// {
//     public Guid Id { get; set; }
//     public string Name { get; set; }
//     public string ProviderName { get; set; }
//     public IConnectionOptions Options { get; set; }
// }
