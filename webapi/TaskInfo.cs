namespace WebApi;

public class TaskInfo
{
    public TaskInfo(string id, string data)
    {
        Id = id;
        Data = data;
    }

    public string Id { get; init; }

    public string Data { get; set; }

    public int LastSentCharacterNumber { get; set; } = -1;
}