namespace AiTrip.Domain.Interfaces
{
    public interface ISecretVault
    {
        Task<string?> GetSecretAsync(string key);
        string? GetSecret(string key);
    }
}
