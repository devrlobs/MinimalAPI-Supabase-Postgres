namespace MinimalAPI_Supabase_Postgres;

public class SignInRequest
{
    public string Email {get;set;} = string.Empty;
    public string Password {get;set;} = string.Empty;
}