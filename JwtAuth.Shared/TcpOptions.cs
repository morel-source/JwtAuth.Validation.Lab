namespace JwtAuth.Shared;

public record TcpOptions(string Host, int ListeningPort)
{
    public TcpOptions() : this(
        Host: "127.0.0.1",
        ListeningPort: 8888
    ) { }
}