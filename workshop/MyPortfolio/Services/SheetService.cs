using System.Text.Json;
using MyPortfolio.Models;

namespace MyPortfolio.Services
{
    public class SheetService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://docs.google.com/spreadsheets/d/1nr8EWtSU3Y50oK7oeKvwIZ1tHBUMmOhiSjtFYYufSYI/gviz/tq?tqx=out:json";

        public SheetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// 인물 정보를 가져옵니다 (WEB_People 탭)
        /// </summary>
        public async Task<List<WebPerson>> GetWebPeopleAsync()
        {
            var rows = await GetRowsFromSheetAsync("WEB_People");
            var people = new List<WebPerson>();

            if (rows == null) return people;

            // 헤더 제외하고 데이터 파싱 (Skip 1)
            foreach (var row in rows.Value.EnumerateArray().Skip(1))
            {
                var c = row.GetProperty("c");

                var name = GetCellValue(c, 2);
                var photoUrl = GetCellValue(c, 13);

                 // 디버깅: PhotoUrl 값 출력
                Console.WriteLine($"👤 {name} -> PhotoUrl: [{photoUrl}] (length: {photoUrl?.Length ?? 0})");

                people.Add(new WebPerson
                {
                    PersonId   = GetCellValue(c, 0),
                    Category   = GetCellValue(c, 1),
                    Name       = GetCellValue(c, 2),
                    Org        = GetCellValue(c, 3),
                    Dept       = GetCellValue(c, 4),
                    SubOrg     = GetCellValue(c, 5),
                    Email      = GetCellValue(c, 6),
                    Education  = GetCellValue(c, 7),
                    Experience = GetCellValue(c, 8),
                    Github     = GetCellValue(c, 9),
                    Blog       = GetCellValue(c, 10),
                    Linkedin   = GetCellValue(c, 11),
                    Tags       = GetCellValue(c, 12),
                    PhotoUrl   = photoUrl
                });
            }

            Console.WriteLine($" [SheetService] Loaded {people.Count} people");
            return people;
        }

        /// <summary>
        /// 논문/출판 정보를 가져옵니다 (WEB_Publications 탭)
        /// </summary>
        public async Task<List<WebPublication>> GetWebPublicationsAsync()
        {
            var rows = await GetRowsFromSheetAsync("WEB_Publications");
            var publications = new List<WebPublication>();

            if (rows == null) return publications;

            foreach (var row in rows.Value.EnumerateArray().Skip(1))
            {
                var c = row.GetProperty("c");
                publications.Add(new WebPublication
                {
                    Pub_ID     = GetCellValue(c, 0),
                    Year       = GetCellValue(c, 1),
                    Title      = GetCellValue(c, 2),
                    Venue_Name = GetCellValue(c, 3),
                    Authors    = GetCellValue(c, 4),
                    Paper_Link = GetCellValue(c, 5),
                    Venue_Link = GetCellValue(c, 6),
                    Notes      = GetCellValue(c, 7)
                });
            }

            Console.WriteLine($" [SheetService] Loaded {publications.Count} publications");
            return publications;
        }

                public async Task<List<WebProject>> GetWebProjectsAsync()
        {
            var rows = await GetRowsFromSheetAsync("WEB_Projects");
            var projects = new List<WebProject>();

            if (rows == null) return projects;

            foreach (var row in rows.Value.EnumerateArray())
            {
                var c = row.GetProperty("c");
                var id = GetCellValue(c, 0);

                // 헤더 행 건너뛰기
                if (id == "Project_ID" || string.IsNullOrEmpty(id)) continue;

                projects.Add(new WebProject
                {
                    ProjectId    = id,
                    Title        = GetCellValue(c, 1),
                    Icon         = GetCellValue(c, 2),
                    TeamId       = GetCellValue(c, 3),
                    Overview     = GetCellValue(c, 4),
                    Achievements = GetCellValue(c, 5),
                    Milestones   = GetCellValue(c, 6)
                });
            }

            Console.WriteLine($" [SheetService] Loaded {projects.Count} projects");
            return projects;
        }

        // ... 기존 GetWebProjectsAsync 등 아래에 추가

        public async Task<List<WebPersonTeam>> GetWebPersonTeamsAsync()
        {
            var rows = await GetRowsFromSheetAsync("WEB_PersonTeam");
            var list = new List<WebPersonTeam>();

            if (rows == null) return list;

            foreach (var row in rows.Value.EnumerateArray())
            {
                var c = row.GetProperty("c");
                var personId = GetCellValue(c, 0);

                // 헤더 행 제외
                if (personId == "Person_ID" || string.IsNullOrEmpty(personId)) continue;

                list.Add(new WebPersonTeam
                {
                    PersonId = personId,
                    TeamId   = GetCellValue(c, 1),
                    TeamRole = GetCellValue(c, 2),
                    NameKR   = GetCellValue(c, 3)
                });
            }

            return list;
        }

        public async Task<List<WebPublicationAuthor>> GetWebPublicationAuthorsAsync()
        {
            var rows = await GetRowsFromSheetAsync("WEB_PublicationAuthor");
            var list = new List<WebPublicationAuthor>();

            if (rows == null) return list;

            foreach (var row in rows.Value.EnumerateArray().Skip(1))
            {
                var c = row.GetProperty("c");
                var pubId = GetCellValue(c, 0);
                
                if (string.IsNullOrEmpty(pubId)) continue;

                list.Add(new WebPublicationAuthor
                {
                    PubId = pubId,
                    AuthorOrder = GetCellValue(c, 1),
                    PersonId = GetCellValue(c, 2),
                    AuthorNameDisplay = GetCellValue(c, 3),
                    IsCoFirstAuthor = GetCellValue(c, 4),
                    IsCorresponding = GetCellValue(c, 5)
                });
            }

            Console.WriteLine($" [SheetService] Loaded {list.Count} publication authors");
            return list;
        }

        /// <summary>
        /// 공통 로직: 특정 시트 탭에서 JSON 데이터를 가져와 rows 배열을 반환합니다.
        /// </summary>
        private async Task<JsonElement?> GetRowsFromSheetAsync(string sheetName)
        {
            try
            {
                string url = $"{BaseUrl}&sheet={sheetName}";
                Console.WriteLine($"🟡 [SheetService] Requesting: {sheetName}");

                var raw = await _httpClient.GetStringAsync(url);

                // gviz wrapper 제거
                var start = raw.IndexOf('{');
                var end = raw.LastIndexOf('}');
                if (start < 0 || end < 0) return null;

                var json = raw.Substring(start, end - start + 1);
                var doc = JsonDocument.Parse(json);

                return doc.RootElement.GetProperty("table").GetProperty("rows");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [SheetService] Error in {sheetName}: {ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// 공통 로직: gviz 셀 구조에서 값을 안전하게 추출합니다.
        /// </summary>
        private string GetCellValue(JsonElement cArray, int index)
        {
            if (index < cArray.GetArrayLength() &&
                cArray[index].ValueKind != JsonValueKind.Null &&
                cArray[index].TryGetProperty("v", out var v))
            {
                return v.ToString() ?? "";
            }
            return "";
        }
    }
}