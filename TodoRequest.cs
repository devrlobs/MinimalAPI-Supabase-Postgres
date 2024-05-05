namespace MinimalAPI_Supabase_Postgres;

public class TodoRequest
{
    public string Name { get; set; } = string.Empty;
    public bool IsDone { get; set; }
}