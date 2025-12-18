using Indx.Api;
using Indx.CloudApi;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IndxCloudLoader
{
    internal static partial class Program
    {
        #region Private Fields

        private const string SearchControllerRoute = "api";

        // Choose your dataset here
        private const string dataset = "pokedex"; // Options: "pokedex", "tmdb"

        #endregion Private Fields

        #region Private Methods

        private static async Task<SystemStatus> Analyze(string dataSetName, string fileName, HttpClient client)
        {
            var fullFile = File.ReadAllText(fileName);
            var retval = await client.PostAsync(SearchControllerRoute + "/AnalyzeString/" + dataSetName, new StringContent(fullFile, Encoding.UTF8, "text/plain"));
            //   var retval = await client.PostAsJsonAsync(SearchControllerRoute + "/analyze", fileName);
            if (retval.IsSuccessStatusCode)
            {
                var dataAsString = await retval.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<SystemStatus>(dataAsString);
            }
            return null;
        }



        private static async Task<bool> ClearFields(string dataSetName, string[] fields, HttpClient client)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,
            };

            var res = await client.PutAsJsonAsync(SearchControllerRoute + "/ClearFieldSettings/" + dataSetName, fields, options);
            return res.IsSuccessStatusCode;
        }

        private static async Task<FilterProxy> CombineFilters(string dataSetName, CombinedFilterProxy combinedFilter, HttpClient client)
        {
            var respons = await client.PutAsJsonAsync<CombinedFilterProxy>(SearchControllerRoute + "/CombineFilters/" + dataSetName, combinedFilter);
            FilterProxy filterProxy = null;
            if (respons.IsSuccessStatusCode)
            {
                var dataAsString = await respons.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                filterProxy = JsonSerializer.Deserialize<FilterProxy>(dataAsString, options);
            }
            return filterProxy;
        }

        private static async Task<BoostProxy> CreateBoost(string dataSetName, BoostProxy boost, HttpClient client)
        {
            var respons = await client.PutAsJsonAsync<BoostProxy>(SearchControllerRoute + "/CreateBoost/" + dataSetName, boost);
            BoostProxy boostProxy = null;
            if (respons.IsSuccessStatusCode)
            {
                var dataAsString = await respons.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                boostProxy = JsonSerializer.Deserialize<BoostProxy>(dataAsString, options);
            }
            return boostProxy;
        }
        
        private static async Task<bool> CreateOrOpenDataSet(string route, string dataSetName, int configuration, HttpClient client)
        {
            var respons = await client.PutAsJsonAsync<string>(route + "/CreateOrOpen/" + dataSetName + "/"
                + configuration.ToString(), "");
            if (!respons.IsSuccessStatusCode)
                Console.WriteLine("CreateOrOpenDataSet response error:" + respons.ToString());
            return respons.IsSuccessStatusCode;
        }

        private static async Task<FilterProxy> CreateRangeFilter(string dataSetName, RangeFilterProxy rangeFilter, HttpClient client)
        {
            var respons = await client.PutAsJsonAsync<RangeFilterProxy>(SearchControllerRoute + "/CreateRangeFilter/" + dataSetName, rangeFilter);
            FilterProxy filterProxy = null;
            if (respons.IsSuccessStatusCode)
            {
                var dataAsString = await respons.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                filterProxy = JsonSerializer.Deserialize<FilterProxy>(dataAsString, options);
            }
            return filterProxy;
        }

        private static async Task<FilterProxy> CreateValueFilter(string dataSetName, ValueFilterProxy rangeFilter, HttpClient client)
        {
            var respons = await client.PutAsJsonAsync<ValueFilterProxy>(SearchControllerRoute + "/CreateValueFilter/" + dataSetName, rangeFilter);
            FilterProxy filterProxy = null;
            if (respons.IsSuccessStatusCode)
            {
                var dataAsString = await respons.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                filterProxy = JsonSerializer.Deserialize<FilterProxy>(dataAsString, options);
            }
            return filterProxy;
        }

        private static async Task<bool> DeleteDataSet(string route, string dataSetName, HttpClient client)
        {
            var res = await client.DeleteAsync(route + "/DeleteDataSet/" + dataSetName);
            return res.IsSuccessStatusCode;
        }

        private static async Task<bool> DeleteDocument(string dataSetName, long documentKey, HttpClient client)
        {
            var res = await client.DeleteAsync(SearchControllerRoute + "/" + dataSetName + "/" + documentKey.ToString());
            return res.IsSuccessStatusCode;
        }

        private static async Task<string[]> GetAllFields(string dataSetName, HttpClient client)
        {
            var respons = await client.GetAsync(SearchControllerRoute + "/GetAllFields/" + dataSetName);
            if (respons.IsSuccessStatusCode)
            {
                var dataAsString = await respons.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<string[]>(dataAsString, options);
            }
            else
            {
                Console.WriteLine("System status request failed, type any char to exit");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            return new string[0];
        }

        private static async Task<string[]> GetFacetableFields(string dataSetName, HttpClient client)
        {
            var respons = await client.GetAsync(SearchControllerRoute + "/GetFacetableFields/" + dataSetName);
            if (respons.IsSuccessStatusCode)
            {
                var dataAsString = await respons.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<string[]>(dataAsString, options);
            }
            else
            {
                Console.WriteLine("GetFacetableFields failed, type any char to exit");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            return new string[0];
        }

        private static async Task<string[]> GetFilterableFields(string dataSetName, HttpClient client)
        {
            var respons = await client.GetAsync(SearchControllerRoute + "/GetFilterableFields/" + dataSetName);
            if (respons.IsSuccessStatusCode)
            {
                var dataAsString = await respons.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<string[]>(dataAsString, options);
            }
            else
            {
                Console.WriteLine("GetFilterableFields failed, type any char to exit");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            return new string[0];
        }

        private static async Task<string[]> GetSearchableFields(string dataSetName, HttpClient client)
        {
            var respons = await client.GetAsync(SearchControllerRoute + "/GetSearchableFields/" + dataSetName);
            if (respons.IsSuccessStatusCode)
            {
                var dataAsString = await respons.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<string[]>(dataAsString, options);
            }
            else
            {
                Console.WriteLine("System status request failed, type any char to exit");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            return Array.Empty<string>();
        }

        private static async Task<string[]> GetJson(string dataSetName, long[] keys, HttpClient client)
        {
            string[] records = null;

            string jsonData = JsonSerializer.Serialize(keys);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(SearchControllerRoute + "/GetJson/" + dataSetName, content);
            if (response.IsSuccessStatusCode)
            {
                var dataAsString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                records = JsonSerializer.Deserialize<string[]>(dataAsString, options);
            }
            else
            {
                Console.WriteLine("GetUserDataSets request failed, type any char to exit");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            return records;
        }

        private static async Task<string[]> GetSortableFields(string dataSetName, HttpClient client)
        {
            var respons = await client.GetAsync(SearchControllerRoute + "/GetSortableFields/" + dataSetName);
            if (respons.IsSuccessStatusCode)
            {
                var dataAsString = await respons.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<string[]>(dataAsString, options);
            }
            else
            {
                Console.WriteLine("System status request failed, type any char to exit");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            return new string[0];
        }

        private static async Task<int> GetNumberOfJsonRecordsInDb(string dataSetName, HttpClient client)
        {
            var response = await client.GetAsync(SearchControllerRoute + "/GetNumberOfJsonRecordsInDb/" + dataSetName);
            if (response.IsSuccessStatusCode)
            {
                var dataAsString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<int>(dataAsString, options);
            }
            else
                return 0;
        }

        private static async Task<SystemStatus> GetStatus(string dataSetName, HttpClient client)
        {
            SystemStatus state = null;
            var respons = await client.GetAsync(SearchControllerRoute + "/GetStatus/" + dataSetName);
            if (respons.IsSuccessStatusCode)
            {
                var dataAsString = await respons.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                state = JsonSerializer.Deserialize<SystemStatus>(dataAsString, options);
            }
            else
            {
                Console.WriteLine("System status request failed, type any char to exit");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            return state;
        }

        private static async Task<string[]> GetUserDataSets(HttpClient client)
        {
            string[] sets = null;
            var respons = await client.GetAsync(SearchControllerRoute + "/GetUserDataSets");
            if (respons.IsSuccessStatusCode)
            {
                var dataAsString = await respons.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                sets = JsonSerializer.Deserialize<string[]>(dataAsString, options);
            }
            else
            {
                Console.WriteLine("GetUserDataSets request failed, type any char to exit");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            return sets;
        }

        private static async Task<bool> IndexDataSet(string dataSetName, HttpClient client)
        {
            var respons = await client.GetAsync(SearchControllerRoute + "/IndexDataSet/" + dataSetName);
            return respons.IsSuccessStatusCode;
        }

        private static async Task<bool> LoadFromDatabaseAsync(string dataSetName, HttpClient client)
        {
            HttpResponseMessage retval;
            retval = await client.GetAsync(SearchControllerRoute + "/LoadFromDatabase/" + dataSetName);
            return retval.IsSuccessStatusCode;
        }
        private static async Task<SystemStatus> AnalyzeStreamAsync(string dataSetName, string fileName, HttpClient client)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (var streamContent = new StreamContent(fileStream))
            {
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain"); // Use "application/json" if appropriate

                // Send the POST request with the stream
                var retval = await client.PostAsync(SearchControllerRoute + "/AnalyzeStreamAsync/" + dataSetName, streamContent);

                //   var retval = await client.PostAsJsonAsync(SearchControllerRoute + "/analyze", fileName);
                if (retval.IsSuccessStatusCode)
                {
                    var dataAsString = await retval.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<SystemStatus>(dataAsString);
                }
            }
            return null;
        }
        private static async Task<bool> LoadStreamAsync(string dataSetName, string fileName, HttpClient client)
        {
            HttpResponseMessage retval;
            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (var streamContent = new StreamContent(fileStream))
            {
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain"); // Use "application/json" if appropriate
                retval = await client.PutAsync(SearchControllerRoute + "/LoadStream/" + dataSetName, streamContent);
            }
            return retval.IsSuccessStatusCode;
        }

        private static async Task<bool> LoadString(string dataSetName, string fileNameAndPath, HttpClient client)
        {
            var fc = File.ReadAllText(fileNameAndPath);
            var res = await client.PutAsync(SearchControllerRoute + "/" + "LoadString" + "/" + dataSetName, new StringContent(fc, Encoding.UTF8, "text/plain"));
            return res.IsSuccessStatusCode;
        }

        private static async Task<Result> Search(CloudQuery q, string dataSetName, HttpClient client)
        {
            string jsonString = JsonSerializer.Serialize<CloudQuery>(q);
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IncludeFields = true
            };
            var response = await client.PostAsJsonAsync(SearchControllerRoute + "/Search/" + dataSetName, q, options);
            if (response.IsSuccessStatusCode)
            {
                var dataAsString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Result>(dataAsString, options);
            }
            return null;
        }

        private static async Task<bool> SetFacetableFields(string dataSetName, string[] fields, HttpClient client)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,
            };

            var res = await client.PutAsJsonAsync(SearchControllerRoute + "/SetFacetableFields/" + dataSetName, fields, options);
            return res.IsSuccessStatusCode;
        }

        private static async Task<bool> SetFilterableFields(string dataSetName, string[] fields, HttpClient client)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,
            };
            var res = await client.PutAsJsonAsync(SearchControllerRoute + "/SetFilterableFields/" + dataSetName, fields, options);
            return res.IsSuccessStatusCode;
        }

        private static async Task<bool> SetSearchableFields(string dataSetName, (string, int)[] fields, HttpClient client)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,
            };

            var res = await client.PutAsJsonAsync(SearchControllerRoute + "/SetSearchableFields/" + dataSetName, fields, options);
            return res.IsSuccessStatusCode;
        }

        private static async Task<bool> SetSortableFields(string dataSetName, string[] fields, HttpClient client)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,
            };

            var res = await client.PutAsJsonAsync(SearchControllerRoute + "/SetSortableFields/" + dataSetName, fields, options);
            return res.IsSuccessStatusCode;
        }

        private static async Task LoadDataset()
        {
            // Dataset configuration
            string dataSetName;
            string file;
            (string Name, int Weight)[] searchableFields;
            string[] filterableFields;
            string[] facetableFields;
            string[] sortableFields;

            if (dataset == "tmdb")
            {
                dataSetName = "tmdb";
                file = "data/tmdb_top10k.json";
                searchableFields = new[]
                {
                    ("title", (int)Weight.High),
                    ("original_title", (int)Weight.Med),
                    ("description", (int)Weight.Med),
                    ("actors", (int)Weight.Low)
                };
                filterableFields = new[] { "release_year", "vote_average", "vote_count_tier", "genres", "decade", "actors", "language" };
                facetableFields = new[] { "release_year", "vote_average", "vote_count_tier", "genres", "decade", "actors", "language" };
                sortableFields = new[] { "popularity", "vote_average" };
            }
            else if (dataset == "pokedex")
            {
                dataSetName = "pokedex";
                file = "data/pokedex.json";
                searchableFields = new[]
                {
                    ("name", (int)Weight.High),
                    ("type1", (int)Weight.Med),
                    ("type2", (int)Weight.Low)
                };
                filterableFields = new[] { "speed", "attack", "hp", "type1", "type2", "is_legendary" };
                facetableFields = new[] { "speed", "attack", "hp", "type1", "type2", "is_legendary" };
                sortableFields = new[] { "name", "speed" };
            }
            else
            {
                Console.WriteLine($"Unknown dataset: {dataset}");
                return;
            }

            HttpClient client = new();
            if (Debugger.IsAttached)
                client.Timeout = TimeSpan.FromMinutes(5);
            SetBearerToken(client, bearerToken);

            Console.WriteLine($"Loading dataset: {dataSetName}");

            //var success = await DeleteDataSet(SearchControllerRoute, dataSetName, client);
            var success = await CreateOrOpenDataSet(SearchControllerRoute, dataSetName, 400, client);
            var status1 = await GetStatus(dataSetName, client);
            var status0 = await Analyze(dataSetName, file, client);
            //var status0 = await AnalyzeStreamAsync(dataSetName, file, client);

            status1 = await GetStatus(dataSetName, client);

            var proceed = true;

            var list = await GetAllFields(dataSetName, client);
            Console.WriteLine($"All fields: {string.Join(", ", list)}");

            // Set searchable fields with weights
            var myres = await SetSearchableFields(dataSetName, searchableFields, client);
            Console.WriteLine($"SetSearchableFields result: {myres}");

            // Set filterable fields
            var myres2 = await SetFilterableFields(dataSetName, filterableFields, client);
            Console.WriteLine($"SetFilterableFields result: {myres2}");

            // Set facetable fields
            var facetsResult = await SetFacetableFields(dataSetName, facetableFields, client);
            Console.WriteLine($"SetFacetableFields result: {facetsResult}");

            // Set sortable fields
            var sortres = await SetSortableFields(dataSetName, sortableFields, client);
            Console.WriteLine($"SetSortableFields result: {sortres}");

            // Verify field configuration
            var ifields = await GetSearchableFields(dataSetName, client);
            Console.WriteLine($"Searchable fields: {string.Join(", ", ifields)}");

            var sres = await GetSortableFields(dataSetName, client);
            Console.WriteLine($"Sortable fields: {string.Join(", ", sres)}");

            var sres2 = await GetFacetableFields(dataSetName, client);
            Console.WriteLine($"Facetable fields: {string.Join(", ", sres2)}");

            var sres3 = await GetFilterableFields(dataSetName, client);
            Console.WriteLine($"Filterable fields: {string.Join(", ", sres3)}");

            // Create filters (optional - dataset specific examples)
            if (dataset == "pokedex")
            {
                RangeFilterProxy filter = new RangeFilterProxy
                {
                    FieldName = "speed",
                    LowerLimit = 10.5,
                    UpperLimit = 50.0
                };

                var filt1 = await CreateRangeFilter(dataSetName, filter, client);

                ValueFilterProxy vf = new ValueFilterProxy
                {
                    FieldName = "speed",
                    Value = 50
                };

                var filt2 = await CreateValueFilter(dataSetName, vf, client);

                CombinedFilterProxy cf = new CombinedFilterProxy(filt1, filt2, true);
                var combFilt = await CombineFilters(dataSetName, cf, client);

                var bp = new BoostProxy { FilterProxy = combFilt, BoostStrength = BoostStrength.High };

                var boostProxy = await CreateBoost(dataSetName, bp, client);
            }

            // here check if data is present in db for this dataset
            var loadFromDb = false;
            bool result2;
            if (!loadFromDb)
                result2 = await LoadStreamAsync(dataSetName, file, client);
            // result2 = await LoadString(dataSetName, file, client);
            proceed = true;
            var status = await GetStatus(dataSetName, client);

            do
            {
                Console.Write("*");
                status = await GetStatus(dataSetName, client);
                if (status != null)
                    proceed = status.SystemState == SystemState.Loading;
                else
                    proceed = false;
                await Task.Delay(100);
            } while (proceed);

            var userDataSets = await GetUserDataSets(client);

            var state = await GetStatus(dataSetName, client);

            var numberOfRecords = await GetNumberOfJsonRecordsInDb(dataSetName, client);

            if (numberOfRecords > 0 && loadFromDb)
                success = await LoadFromDatabaseAsync(dataSetName, client);


            success = await IndexDataSet(dataSetName, client);
            proceed = true;
            do
            {
                Console.Write(".");
                status = await GetStatus(dataSetName, client);
                if (status != null)
                    proceed = status.SystemState != SystemState.Ready;
                else
                    proceed = false;
                await Task.Delay(100);
            } while (proceed);

            CloudQuery query = new CloudQuery
            {
                Text = dataset == "tmdb" ? "titanic" : "raic",
                MaxNumberOfRecordsToReturn = 5,
                SortBy = dataset == "tmdb" ? "popularity" : "name"
            };
            var res = await Search(query, dataSetName, client);
            if (res == null)
            {
                Console.WriteLine("Search returned null");
                return;
            }
            foreach (var item in res.Records)
            {
                var rec = await GetJson(dataSetName, new long[] { item.DocumentKey }, client);
                Console.WriteLine($"Key:{item.DocumentKey} Score:{item.Score}");
                Console.WriteLine(rec[0]);
            }
            Console.ReadLine();
        }

        #endregion Private Methods
    }
}