namespace MinimalAPI_Supabase_Postgres;

public class TodoResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDone { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}