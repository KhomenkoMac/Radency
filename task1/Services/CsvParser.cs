using System.Text.RegularExpressions;
using task1.DTOs;
using task1.DTOs.metalogOutput;

namespace task1.Services;


public class CsvParser
{
    const string ValidRecordPattern = @"(?<first_name>[A-Za-z]+),\s(?<last_name>[A-Za-z]*)\,\s""(?<address>[a-zA-Z]+,\s[a-zA-Z]+\s\d+,\s\d+)"",\s(?<payment>\d+\.\d+),\s(?<date>\d+\-\d+\-\d+),\s(?<account_number>\d+),\s(?<service>[A-Za-z]+)";

    const string FirstnameColumnName = "first_name";
    const string LastnameColumnName = "last_name";
    const string AddressColumnName = "address";
    const string AccountnameColumnName = "account_number";
    const string DateColumnName = "date";
    const string PaymentColumnName = "payment";
    const string ServiceColumnName = "service";

    private readonly Regex _regex = new(ValidRecordPattern);

    public async Task<ParsedFile> Parse(string filepath)
    {
        var readCsvRecords = await File.ReadAllLinesAsync(filepath);

        readCsvRecords = readCsvRecords
            .Select(x => x.Replace('”', '"').Replace('“', '"'))
            .ToArray()[1..];

        var amountOfRecordsInFile = readCsvRecords.Length;

        var fetchedTransactions = readCsvRecords.Select(x =>
        {
            if (!_regex.IsMatch(x)) return null;
            var hello = _regex.Match(x);
            var valuesFromColumns = hello.Groups;
            return new PaymentTransaction
            {
                Firstname = valuesFromColumns[FirstnameColumnName].Value,
                Lastname = valuesFromColumns[LastnameColumnName].Value,
                Address = valuesFromColumns[AddressColumnName].Value,
                Payment = decimal.Parse(valuesFromColumns[PaymentColumnName].Value),
                Date = DateOnly.ParseExact(valuesFromColumns[DateColumnName].Value, "yyyy-dd-MM"),
                AccountNumber = long.Parse(valuesFromColumns[AccountnameColumnName].Value),
                Service = valuesFromColumns[ServiceColumnName].Value
            };

        }).ToList();

        return new ParsedFile(filepath, fetchedTransactions!);
    }
}
