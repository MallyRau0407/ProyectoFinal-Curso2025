namespace Project.WebApi.Models.Penalties;

public class CreatePenaltyRequest
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
}
