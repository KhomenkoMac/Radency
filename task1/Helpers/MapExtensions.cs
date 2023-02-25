using System.Text.RegularExpressions;
using task1.DTOs;

namespace task1.Helpers;

public static class MapExtensions
{
    public static PaymentTransaction ToPaymentTransaction(this GroupCollection valuesFromColumns)
    {
        const string FirstnameColumnName = "first_name";
        const string LastnameColumnName = "last_name";
        const string AccountnameColumnName = "account_number";
        const string DateColumnName = "date";
        const string PaymentColumnName = "payment";
        const string ServiceColumnName = "service";

        return new PaymentTransaction
        {
            Firstname = valuesFromColumns[FirstnameColumnName].Value,
            Lastname = valuesFromColumns[LastnameColumnName].Value,
            Address = valuesFromColumns[AccountnameColumnName].Value,
            Payment = decimal.Parse(valuesFromColumns[PaymentColumnName].Value),
            Date = DateOnly.Parse(valuesFromColumns[DateColumnName].Value),
            AccountNumber = long.Parse(valuesFromColumns[AccountnameColumnName].Value),
            Service = valuesFromColumns[ServiceColumnName].Value
        };
    }
}
