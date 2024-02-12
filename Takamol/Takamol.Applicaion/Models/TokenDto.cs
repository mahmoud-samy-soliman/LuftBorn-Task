namespace Takamol.Applicaion.Model;
public enum TokenResult
{
    success,
    wrong_user_name,
    wrong_password
}

public class TokenDto
{
    public TokenDto(TokenResult result,string token="", DateTime? expiry=null)
    {
        Token = token;
        Expiry = expiry ?? DateTime.Now; 

        Result = result;
    }
    
    public string Token { get; set; } = string.Empty;
    public DateTime Expiry { get; set; }
    public TokenResult Result { get; set; }

}
