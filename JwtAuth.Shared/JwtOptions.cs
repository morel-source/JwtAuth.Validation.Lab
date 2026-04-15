namespace JwtAuth.Shared;

public record JwtOptions(string Key)
{
    public JwtOptions() : this(Key: "ThisIsMySuperLongSecretKeyForTesting123")
    {
    }
}