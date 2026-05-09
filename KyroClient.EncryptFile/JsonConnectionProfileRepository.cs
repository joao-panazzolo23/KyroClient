// using System.Text.Json;
// using KyroClient.Core.Secrets.Services;
//
// namespace KyroClient.EncryptFile;
//
// public sealed class JsonConnectionProfileRepository : IConnectionProfileRepository
// {
//     private readonly string _filePath;
//     private readonly ISecretStore _secretStore;
//     private readonly JsonSerializerOptions _jsonOptions;
//
//     private JsonConnectionProfileRepository(
//         string filePath,
//         ISecretStore secretStore,
//         JsonSerializerOptions jsonOptions)
//     {
//         _filePath = filePath;
//         _secretStore = secretStore;
//         _jsonOptions = jsonOptions;
//     }
//
//     public static JsonConnectionProfileRepository Create(ISecretStore secretStore)
//     {
//         var dir = Path.Combine(
//             Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
//             "sqlclient");
//
//         Directory.CreateDirectory(dir);
//
//         var jsonOptions = new JsonSerializerOptions
//         {
//             WriteIndented = true,
//             Converters = { new ConnectionOptionsJsonConverter() }
//         };
//
//         return new JsonConnectionProfileRepository(
//             Path.Combine(dir, "profiles.json"),
//             secretStore,
//             jsonOptions);
//     }
//
//     public async Task<IReadOnlyList<ConnectionProfile>> GetAllAsync(CancellationToken ct = default)
//     {
//         if (!File.Exists(_filePath))
//             return [];
//
//         var json = await File.ReadAllTextAsync(_filePath, ct);
//         var dtos = JsonSerializer.Deserialize<List<ConnectionProfileDto>>(json, _jsonOptions) ?? [];
//
//         var profiles = new List<ConnectionProfile>();
//         foreach (var dto in dtos)
//         {
//             var password = await _secretStore.GetAsync(dto.SecretKey);
//             profiles.Add(dto.ToProfile(password));
//         }
//
//         return profiles;
//     }
//
//     public async Task SaveAsync(ConnectionProfile profile, CancellationToken ct = default)
//     {
//         var all = await LoadDtosAsync(ct);
//
//         var secretKey = $"sqlclient:profile:{profile.Id}";
//         await _secretStore.SaveAsync(secretKey, profile.Options.GetPassword() ?? string.Empty);
//
//         var dto = ConnectionProfileDto.FromProfile(profile, secretKey);
//         var existing = all.FindIndex(x => x.Id == profile.Id);
//
//         if (existing >= 0)
//             all[existing] = dto;
//         else
//             all.Add(dto);
//
//         await PersistAsync(all, ct);
//     }
//
//     public async Task DeleteAsync(Guid id, CancellationToken ct = default)
//     {
//         var all = await LoadDtosAsync(ct);
//         var dto = all.FirstOrDefault(x => x.Id == id);
//
//         if (dto is null) return;
//
//         await _secretStore.DeleteAsync(dto.SecretKey);
//         all.RemoveAll(x => x.Id == id);
//         await PersistAsync(all, ct);
//     }
//
//     private async Task<List<ConnectionProfileDto>> LoadDtosAsync(CancellationToken ct)
//     {
//         if (!File.Exists(_filePath))
//             return [];
//
//         var json = await File.ReadAllTextAsync(_filePath, ct);
//         return JsonSerializer.Deserialize<List<ConnectionProfileDto>>(json, _jsonOptions) ?? [];
//     }
//
//     private async Task PersistAsync(List<ConnectionProfileDto> dtos, CancellationToken ct)
//     {
//         var json = JsonSerializer.Serialize(dtos, _jsonOptions);
//         await File.WriteAllTextAsync(_filePath, json, ct);
//     }
// }