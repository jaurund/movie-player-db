namespace Backend.Database
{
    public static class Secrets
    {
        public static string GetSecret(string key)
        {
            // Retrieve secret from configuration or secret manager
            return "your-secret";
        }
    }
}