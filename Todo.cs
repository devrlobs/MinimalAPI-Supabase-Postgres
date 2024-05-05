using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MinimalAPI_Supabase_Postgres;

[Table("Todo")]
public class Todo : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("is_done")]
    public bool IsDone { get; set; }
    [Column("created_by")]
    public string CreatedBy {get;set;} = string.Empty;
}