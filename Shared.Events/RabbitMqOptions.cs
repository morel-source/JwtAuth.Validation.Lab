namespace Shared.Events;

public record RabbitMqOptions(
    string Host,
    string UserName,
    string Password
)
{
    public RabbitMqOptions() : this(
        Host: "localhost",
        UserName: "guest",
        Password: "guest")
    {
    }
}