namespace task1.DTOs.metalogOutput;

public record class ParsedInfoStats(
    int ParsedLinesAmount,
    int ParseErrorsAmount,
    List<string> InvalidFilesAsText
);
