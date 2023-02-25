namespace task1.DTOs;

public class PaymentTransaction
{
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Address { get; set; } = null!;
    public decimal Payment { get; set; }
    public DateOnly Date { get; set; }
    public long AccountNumber { get; set; }
    public string Service { get; set; } = null!;
}
