using EmployeePortal.Application.Infrastructures;
using EmployeePortal.Application.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace EmployeePortal.Infrastructure.Services
{
    public class CsvService : ICsvService
    {
        public FileDetail Export<T>(List<T> data, string fileName)
        {
            if (data == null || !data.Any())
                throw new ArgumentException("The list cannot be null or empty.", nameof(data));

            var sb = new StringBuilder();

            // Get properties of the generic type
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Add CSV header
            sb.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Add object data
            foreach (var item in data)
            {
                var values = properties.Select(p =>
                {
                    var value = p.GetValue(item, null);
                    return EscapeCsvValue(value?.ToString() ?? string.Empty);
                });

                sb.AppendLine(string.Join(",", values));
            }

            return new FileDetail
            {
                FileContents = Encoding.UTF8.GetBytes(sb.ToString()),
                ContentType = "text/csv",
                FileName = fileName + ".csv",
            };
        }


        public ImportResponse<T> Import<T>(IFormFile file, char delimiter = ',') where T : class, new()
        {
            var response = new ImportResponse<T>
            {
                ErrorMessages = new List<string>(),
                SkippedRows = new List<int>(),
                Data = new List<T>()
            };

            if (file == null || file.Length == 0)
            {
                response.Success = false;
                response.ErrorMessages.Add("File is empty.");
                return response;
            }

            try
            {
                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream);

                var headers = reader.ReadLine()?.Split(delimiter);
                if (headers == null || headers.Length == 0)
                {
                    response.Success = false;
                    response.ErrorMessages.Add("CSV file does not contain headers.");
                    return response;
                }

                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                          .ToDictionary(p => p.Name.ToLower(), p => p);

                int rowNumber = 0;
                while (!reader.EndOfStream)
                {
                    rowNumber++;
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var values = line.Split(delimiter);
                    if (values.Length != headers.Length)
                    {
                        response.ErrorMessages.Add($"Row {rowNumber} has a mismatched column count.");
                        response.SkippedRows.Add(rowNumber);
                        continue;
                    }

                    var item = new T();
                    bool hasErrors = false;

                    for (int i = 0; i < headers.Length; i++)
                    {
                        var header = headers[i].Trim().ToLower();
                        if (!properties.TryGetValue(header, out var property)) continue;

                        var value = values[i].Trim();
                        //if (string.IsNullOrEmpty(value)) continue;

                        try
                        {
                            var convertedValue = Convert.ChangeType(value, property.PropertyType, CultureInfo.InvariantCulture);
                            property.SetValue(item, convertedValue);
                        }
                        catch (Exception ex)
                        {
                            response.ErrorMessages.Add($"Row {rowNumber} - {header} is invalid value '{value}'");
                            hasErrors = true;
                        }
                    }

                    if (hasErrors)
                    {
                        response.SkippedRows.Add(rowNumber);
                    }
                    else
                    {
                        response.Data.Add(item);
                    }
                }

                response.Success = response.ErrorMessages.Count == 0;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages.Add($"An unexpected error occurred: {ex.Message}");
            }

            return response;
        }


        // Escapes special characters in CSV values (e.g., commas, quotes)
        private string EscapeCsvValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Contains(",") || value.Contains("\""))
            {
                value = value.Replace("\"", "\"\""); // Escape double quotes
                return $"\"{value}\""; // Wrap value in quotes
            }

            return value;
        }

    }
}
