using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using KyroClient.Core.Secrets.Services;

namespace KyroClient.EncryptFile;

public sealed class EncryptedFileSecretStore : ISecretStore
{
    private readonly string _filePath;
    private readonly byte[] _key;
    private Dictionary<string, string> _cache = [];

    private EncryptedFileSecretStore(string filePath, byte[] key, Dictionary<string, string> cache)
    {
        _filePath = filePath;
        _key = key;
        _cache = cache;
    }

    public static async Task<EncryptedFileSecretStore> CreateAsync(CancellationToken ct = default)
    {
        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "sqlclient");

        Directory.CreateDirectory(dir);

        var filePath = Path.Combine(dir, "secrets.dat");
        var key = DeriveKey();
        var cache = await LoadFromDiskAsync(filePath, key, ct);

        return new EncryptedFileSecretStore(filePath, key, cache);
    }

    public async Task SaveAsync(string key, string secret)
    {
        _cache[key] = secret;
        await PersistAsync();
    }

    public Task<string?> GetAsync(string key)
    {
        _cache.TryGetValue(key, out var value);
        return Task.FromResult(value);
    }

    public async Task DeleteAsync(string key)
    {
        _cache.Remove(key);
        await PersistAsync();
    }

    private static byte[] DeriveKey()
    {
        var machineId = Environment.MachineName + Environment.UserName;
        return SHA256.HashData(Encoding.UTF8.GetBytes(machineId));
    }

    private static async Task<Dictionary<string, string>> LoadFromDiskAsync(
        string filePath, byte[] key, CancellationToken ct)
    {
        if (!File.Exists(filePath))
            return [];

        var raw = await File.ReadAllBytesAsync(filePath, ct);
        var json = Decrypt(raw, key);
        return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? [];
    }

    private async Task PersistAsync()
    {
        var json = JsonSerializer.Serialize(_cache);
        var encrypted = Encrypt(json);
        await File.WriteAllBytesAsync(_filePath, encrypted);
    }

    private byte[] Encrypt(string plaintext)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV();

        using var ms = new MemoryStream();
        ms.Write(aes.IV);

        using var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(Encoding.UTF8.GetBytes(plaintext));
        cs.FlushFinalBlock();

        return ms.ToArray();
    }

    private static string Decrypt(byte[] ciphertext, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = ciphertext[..16];

        using var ms = new MemoryStream(ciphertext[16..]);
        using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}
