using dotenv.net;

static class EnvironmentVariables
{
    private static Lazy<Dictionary<string, string>> variables = new Lazy<Dictionary<string, string>>(
        () => EnvironmentVariables.Setup()
    );

    private static Dictionary<string, string> Setup()
    {
        DotEnv.Load(
            new DotEnvOptions(
                ignoreExceptions: false,
                envFilePaths: new []{"./"},
                probeForEnv: true
            )
        );

        return new Dictionary<string, string>(DotEnv.Read());
    }

    public static string GetVariable(string key)
    {
        return EnvironmentVariables.variables.Value[key];
    }
}
