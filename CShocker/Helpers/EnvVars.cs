namespace CShocker.Helpers;

internal static class EnvVars
{
    public static readonly string OpenShockApiHost = Environment.GetEnvironmentVariable("OPENSHOCK_HOST") ?? "https://api.openshock.app";
    public static readonly string? OpenShockApiKey = Environment.GetEnvironmentVariable("OPENSHOCK_APIKEY");
    public static readonly string Name = Environment.GetEnvironmentVariable("OPENSHOCK_NAME") ?? "CShocker";
}