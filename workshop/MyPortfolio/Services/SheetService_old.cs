// using MyPortfolio.Models;
// using System.Text.RegularExpressions;

// namespace MyPortfolio.Services
// {
//     public class SheetService
//     {
//         private readonly HttpClient _httpClient;
//         private readonly string _csvUrl = "https://docs.google.com/spreadsheets/d/1nr8EWtSU3Y50oK7oeKvwIZ1tHBUMmOhiSjtFYYufSYI/export?format=csv&gid=0";

//         public SheetService(HttpClient httpClient)
//         {
//             _httpClient = httpClient;
//         }

//         public async Task<List<WebPerson>> GetWebPeopleAsync()
//         {
//             try
//             {
//                 var csvContent = await _httpClient.GetStringAsync(_csvUrl);
//                 var lines = csvContent.Split('\n', StringSplitOptions.RemoveEmptyEntries).Skip(1); // 헤더 스킵

//                 var people = new List<WebPerson>();
//                 foreach (var line in lines)
//                 {
//                     var columns = line.Split(',').Select(c => Regex.Unescape(c.Trim('"'))).ToArray();
//                     if (columns.Length < 2 || string.IsNullOrEmpty(columns[0])) continue;

//                     people.Add(new WebPerson
//                     {
//                         PersonId = columns.ElementAtOrDefault(0) ?? "",
//                         Category = columns.ElementAtOrDefault(1) ?? "",
//                         Name = columns.ElementAtOrDefault(2) ?? "",
//                         Org = columns.ElementAtOrDefault(3) ?? "",
//                         Dept = columns.ElementAtOrDefault(4) ?? "",
//                         SubOrg = columns.ElementAtOrDefault(5) ?? "",
//                         Email = ExtractEmail(columns.ElementAtOrDefault(6) ?? ""),
//                         Education = columns.ElementAtOrDefault(7) ?? "",
//                         Experience = columns.ElementAtOrDefault(8) ?? "",
//                         Github = ExtractUrl(columns.ElementAtOrDefault(9) ?? ""),
//                         Blog = ExtractUrl(columns.ElementAtOrDefault(10) ?? ""),
//                         Linkedin = ExtractUrl(columns.ElementAtOrDefault(11) ?? ""),
//                         Tags = columns.ElementAtOrDefault(12) ?? ""
//                     });
//                 }
//                 return people;
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"CSV load error: {ex.Message}");
//                 return new List<WebPerson>();
//             }
//         }

//         private static string ExtractEmail(string cellValue)
//         {
//             if (string.IsNullOrEmpty(cellValue)) return "";
//             var match = Regex.Match(cellValue, @"\[mailto:([^\]]+)\]");
//             return match.Success ? match.Groups[1].Value : cellValue;
//         }

//         private static string ExtractUrl(string cellValue)
//         {
//             if (string.IsNullOrEmpty(cellValue)) return "";
//             if (cellValue.StartsWith("http")) return cellValue;
//             var match = Regex.Match(cellValue, @"\[https?://[^\]]+\]");
//             return match.Success ? match.Groups[0].Value.Trim('[', ']') : cellValue;
//         }
//     }
// }
