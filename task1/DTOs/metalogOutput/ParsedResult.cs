namespace task1.DTOs.metalogOutput;

public record ParsedFile(
    string Filepath,
    List<PaymentTransaction?> ParsedTransactions)
{
    public int FoundErrorsAmount =>
        ParsedTransactions
            .Count(x => x is null);
    public int ParsedLinesAmount =>
        ParsedTransactions
            .Count(x => x is not null);
}
