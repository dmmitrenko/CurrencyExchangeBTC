using System.Globalization;
using System.Net;
using CsvHelper;
using CsvHelper.Configuration;
using GSES2.Domain.Exceptions;
using GSES2.Domain.Models;

namespace GSES2.Repository;
public class Repository : IRepository
{
    private const string csvFile = "DB.csv";
    private string filePath = Path.Combine(AppContext.BaseDirectory, csvFile);
    public async Task<IEnumerable<string>> GetSubscribedEmailsAsync()
    {
        if (!File.Exists(filePath))
        {
            throw new DomainException("No emails to send!", (int)HttpStatusCode.BadRequest);
        }
        List<Email> records;
        using (var reader = new StreamReader(filePath))
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

        if (!File.Exists(filePath))
        {
            using (var stream = File.Open(filePath, FileMode.Create))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, configEmails))
            {
                csv.WriteField("EmailRecord"); // Write the header
                csv.NextRecord();
            }
        }

        using (var stream = File.Open(filePath, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, configEmails))
        {
            csv.WriteField(email);
            csv.NextRecord();
        }
    }

    public async Task<bool> IsEmailExists(string email)
    {
        if (!File.Exists(filePath))
        {
            return false;
        }

        IEnumerable<Email> records;
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            records = csv.GetRecords<Email>().ToList();
        }

        return records.Any(n => n.EmailRecord == email);
    }
}
