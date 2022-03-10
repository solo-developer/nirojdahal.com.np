namespace Personal.Domain.Services.Interface
{
    public interface ICryptoGraphyService
    {
        string DecryptString(string key, string cipher);
        
    }
}
