using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using GSES2.Domain.Models;

namespace GSES2.Repository;
public class Repository : IRepository
{
    private const string csvFile = "DB.csv";
    public async Task<IEnumerable<string>> GetSubscribedEmailsAsync()
    {
        List<Email> records;
        using (var reader = new StreamReader(csvFile))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            records = csv.GetRecords<Email>().ToList();
        }

        return records.Select(n => n.EmailRecord);
    }

    public async Task SubscribeEmailAsync(string email)
    {
        var configEmails = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false
        };

        using (var stream = File.Open(csvFile, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, configEmails))
        {
            csv.WriteField(email);
            csv.NextRecord();
        }
    }

    public async Task<bool> IsEmailExists(string email)
    {
        IEnumerable<Email> records;
        using (var reader = new StreamReader(csvFile))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            records = csv.GetRecords<Email>().ToList();
        }

        return records.Any(n => n.EmailRecord == email);
    }
}
