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

        /// <summary>
        /// Loads and configures a dataset in the IndxCloudApi search server.
        ///
        /// Process Overview:
        /// 1. Validation - Check file existence and API connectivity
        /// 2. Create/Open Dataset - Initialize or open existing dataset
        /// 3. Analyze Data - Parse JSON structure and identify fields
        /// 4. Configure Fields - Set searchable, filterable, facetable, and sortable properties
        /// 5. Load Data - Stream JSON data to the search server
        /// 6. Index Dataset - Build search indexes for fast querying
        /// 7. Test Search - Verify dataset with a sample query
        ///
        /// Field Types Explained:
        /// - Searchable: Fields that can be queried with full-text search (e.g., title, description)
        /// - Filterable: Fields that can be used to filter results (e.g., genre, year)
        /// - Facetable: Fields that can be aggregated for faceted navigation (e.g., category counts)
        /// - Sortable: Fields that can be used to sort results (e.g., popularity, date)
        /// </summary>
        private static async Task LoadDataset(string datasetName)
        {
            // ━━━ Step 0: Load and Validate Configuration ━━━
            var config = DatasetConfig.GetConfig(datasetName);
            if (config == null)
            {
                ConsoleHelper.WriteError($"Unknown dataset: {datasetName}");
                ConsoleHelper.WriteInfo($"Available datasets: {string.Join(", ", DatasetConfig.GetAvailableDatasets())}");
                return;
            }

            // Validate data file exists
            if (!File.Exists(config.FilePath))
            {
                ConsoleHelper.WriteError($"Data file not found: {config.FilePath}");
                ConsoleHelper.WriteInfo("Please ensure the data file exists in the correct location.");
                ConsoleHelper.WriteInfo($"Expected path: {Path.GetFullPath(config.FilePath)}");
                return;
            }

            ConsoleHelper.WriteHeader($"Loading Dataset: {config.Name}");
            ConsoleHelper.WriteInfo($"Data file: {config.FilePath}");
            ConsoleHelper.WriteInfo($"File size: {new FileInfo(config.FilePath).Length / 1024 / 1024} MB");
            Console.WriteLine();

            // ━━━ Step 1: Initialize HTTP Client and Authentication ━━━
            HttpClient client = new();
            if (Debugger.IsAttached)
                client.Timeout = TimeSpan.FromMinutes(5);
            SetBearerToken(client, bearerToken);

            // ━━━ Step 2: Create or Open Dataset ━━━
            ConsoleHelper.WriteInfo("Creating or opening dataset...");
            try
            {
                var createSuccess = await CreateOrOpenDataSet(SearchControllerRoute, config.Name, 400, client);
                if (!createSuccess)
                {
                    ConsoleHelper.WriteError("Failed to create or open dataset.");
                    ConsoleHelper.WriteInfo("Troubleshooting:");
                    ConsoleHelper.WriteInfo("  1. Verify API_URI in .env.local is correct");
                    ConsoleHelper.WriteInfo("  2. Ensure IndxCloudApi is running");
                    ConsoleHelper.WriteInfo("  3. Check that BEARER_TOKEN is valid (not expired)");
                    return;
                }
                ConsoleHelper.WriteSuccess("Dataset opened successfully");
            }
            catch (HttpRequestException ex)
            {
                ConsoleHelper.WriteError($"Network error: {ex.Message}");
                ConsoleHelper.WriteInfo("Troubleshooting:");
                ConsoleHelper.WriteInfo($"  - Cannot connect to: {uri}");
                ConsoleHelper.WriteInfo("  - Is IndxCloudApi running?");
                ConsoleHelper.WriteInfo("  - Check your firewall settings");
                return;
            }

            // ━━━ Step 3: Analyze Data Structure ━━━
            ConsoleHelper.WriteInfo("Analyzing data structure...");
            var status0 = await Analyze(config.Name, config.FilePath, client);
            if (status0 == null)
            {
                ConsoleHelper.WriteError("Failed to analyze data file");
                return;
            }
            ConsoleHelper.WriteSuccess("Data structure analyzed");

            var status1 = await GetStatus(config.Name, client);

            // ━━━ Step 4: Discover and Display All Fields ━━━
            ConsoleHelper.WriteInfo("Discovering fields in dataset...");
            var list = await GetAllFields(config.Name, client);
            ConsoleHelper.WriteSuccess($"Found {list.Length} fields");
            ConsoleHelper.WriteInfo($"Fields: {string.Join(", ", list)}");
            Console.WriteLine();

            // ━━━ Step 5: Configure Searchable Fields ━━━
            // Searchable fields are used for full-text search queries.
            // Weight determines relevance (High > Med > Low) in search results.
            ConsoleHelper.WriteInfo("Configuring searchable fields...");
            var myres = await SetSearchableFields(config.Name, config.SearchableFields, client);
            if (!myres)
            {
                ConsoleHelper.WriteError("Failed to set searchable fields");
                return;
            }
            ConsoleHelper.WriteSuccess($"Configured {config.SearchableFields.Length} searchable fields");
            foreach (var field in config.SearchableFields)
            {
                ConsoleHelper.WriteInfo($"  - {field.Name} (weight: {field.Weight})");
            }

            // ━━━ Step 6: Configure Filterable Fields ━━━
            // Filterable fields can be used in filter expressions (e.g., year > 2020).
            ConsoleHelper.WriteInfo("Configuring filterable fields...");
            var myres2 = await SetFilterableFields(config.Name, config.FilterableFields, client);
            if (!myres2)
            {
                ConsoleHelper.WriteError("Failed to set filterable fields");
                return;
            }
            ConsoleHelper.WriteSuccess($"Configured {config.FilterableFields.Length} filterable fields");

            // ━━━ Step 7: Configure Facetable Fields ━━━
            // Facetable fields enable aggregated counts for filtering UI (e.g., Genre: Action (42)).
            ConsoleHelper.WriteInfo("Configuring facetable fields...");
            var facetsResult = await SetFacetableFields(config.Name, config.FacetableFields, client);
            if (!facetsResult)
            {
                ConsoleHelper.WriteError("Failed to set facetable fields");
                return;
            }
            ConsoleHelper.WriteSuccess($"Configured {config.FacetableFields.Length} facetable fields");

            // ━━━ Step 8: Configure Sortable Fields ━━━
            // Sortable fields allow results to be ordered (e.g., sort by popularity desc).
            ConsoleHelper.WriteInfo("Configuring sortable fields...");
            var sortres = await SetSortableFields(config.Name, config.SortableFields, client);
            if (!sortres)
            {
                ConsoleHelper.WriteError("Failed to set sortable fields");
                return;
            }
            ConsoleHelper.WriteSuccess($"Configured {config.SortableFields.Length} sortable fields");
            Console.WriteLine();

            // ━━━ Step 9: Verify Field Configuration ━━━
            ConsoleHelper.WriteInfo("Verifying field configuration...");
            var ifields = await GetSearchableFields(config.Name, client);
            var sres = await GetSortableFields(config.Name, client);
            var sres2 = await GetFacetableFields(config.Name, client);
            var sres3 = await GetFilterableFields(config.Name, client);
            ConsoleHelper.WriteSuccess("Field configuration verified");
            Console.WriteLine();

            // Create filters (optional - dataset specific examples)
            if (config.Name == "pokedex")
            {
                RangeFilterProxy filter = new RangeFilterProxy
                {
                    FieldName = "speed",
                    LowerLimit = 10.5,
                    UpperLimit = 50.0
                };

                var filt1 = await CreateRangeFilter(config.Name, filter, client);

                ValueFilterProxy vf = new ValueFilterProxy
                {
                    FieldName = "speed",
                    Value = 50
                };

                var filt2 = await CreateValueFilter(config.Name, vf, client);

                CombinedFilterProxy cf = new CombinedFilterProxy(filt1, filt2, true);
                var combFilt = await CombineFilters(config.Name, cf, client);

                var bp = new BoostProxy { FilterProxy = combFilt, BoostStrength = BoostStrength.High };

                var boostProxy = await CreateBoost(config.Name, bp, client);
            }

            // ━━━ Step 10: Load Data from File ━━━
            ConsoleHelper.WriteHeader("Loading Data");
            ConsoleHelper.WriteInfo($"Streaming data from {config.FilePath}...");

            var loadFromDb = false;
            bool result2;
            if (!loadFromDb)
            {
                result2 = await LoadStreamAsync(config.Name, config.FilePath, client);
                if (!result2)
                {
                    ConsoleHelper.WriteError("Failed to load data");
                    return;
                }
            }

            // Monitor loading progress
            var proceed = true;
            var loadingStartTime = DateTime.Now;
            var dotCount = 0;
            var status = await GetStatus(config.Name, client);

            do
            {
                dotCount++;
                ConsoleHelper.WriteProgress($"Loading data{new string('.', dotCount % 4)}   ");

                status = await GetStatus(config.Name, client);
                if (status != null)
                    proceed = status.SystemState == SystemState.Loading;
                else
                    proceed = false;
                await Task.Delay(100);
            } while (proceed);

            Console.WriteLine(); // New line after progress
            var loadingDuration = DateTime.Now - loadingStartTime;
            ConsoleHelper.WriteSuccess($"Data loaded in {loadingDuration.TotalSeconds:F1} seconds");

            // Get record count
            var numberOfRecords = await GetNumberOfJsonRecordsInDb(config.Name, client);
            ConsoleHelper.WriteInfo($"Total records: {numberOfRecords:N0}");
            Console.WriteLine();

            // ━━━ Step 11: Build Search Index ━━━
            ConsoleHelper.WriteHeader("Building Search Index");
            ConsoleHelper.WriteInfo("Indexing dataset (this may take a moment)...");

            var success = await IndexDataSet(config.Name, client);
            if (!success)
            {
                ConsoleHelper.WriteError("Failed to start indexing");
                return;
            }

            // Monitor indexing progress
            proceed = true;
            var indexingStartTime = DateTime.Now;
            dotCount = 0;
            do
            {
                dotCount++;
                ConsoleHelper.WriteProgress($"Indexing{new string('.', dotCount % 4)}   ");

                status = await GetStatus(config.Name, client);
                if (status != null)
                    proceed = status.SystemState != SystemState.Ready;
                else
                    proceed = false;
                await Task.Delay(100);
            } while (proceed);

            Console.WriteLine(); // New line after progress
            var indexingDuration = DateTime.Now - indexingStartTime;
            ConsoleHelper.WriteSuccess($"Index built in {indexingDuration.TotalSeconds:F1} seconds");
            Console.WriteLine();

            // ━━━ Step 12: Run Test Search ━━━
            ConsoleHelper.WriteHeader("Running Test Search");
            ConsoleHelper.WriteInfo($"Search query: \"{config.TestQuery}\"");

            CloudQuery query = new CloudQuery
            {
                Text = config.TestQuery,
                MaxNumberOfRecordsToReturn = 5,
                SortBy = config.SortableFields.FirstOrDefault() ?? ""
            };

            var res = await Search(query, config.Name, client);
            if (res == null)
            {
                ConsoleHelper.WriteError("Search returned null");
                return;
            }

            var resultsCount = res.Records.Count();
            ConsoleHelper.WriteSuccess($"Found {resultsCount} results");
            Console.WriteLine();

            // Display top results
            int resultNum = 1;
            foreach (var item in res.Records)
            {
                var rec = await GetJson(config.Name, new long[] { item.DocumentKey }, client);
                Console.WriteLine($"Result {resultNum}:");
                ConsoleHelper.WriteInfo($"  Score: {item.Score:F2}");
                ConsoleHelper.WriteInfo($"  Document Key: {item.DocumentKey}");
                ConsoleHelper.WriteInfo($"  Data: {rec[0]}");
                Console.WriteLine();
                resultNum++;
            }

            // ━━━ Final Summary ━━━
            ConsoleHelper.WriteSummary("Dataset Load Complete", new Dictionary<string, object>
            {
                { "Dataset", config.Name },
                { "Total Records", $"{numberOfRecords:N0}" },
                { "Searchable Fields", config.SearchableFields.Length },
                { "Filterable Fields", config.FilterableFields.Length },
                { "Facetable Fields", config.FacetableFields.Length },
                { "Sortable Fields", config.SortableFields.Length },
                { "Loading Time", $"{loadingDuration.TotalSeconds:F1}s" },
                { "Indexing Time", $"{indexingDuration.TotalSeconds:F1}s" },
                { "Test Query Results", resultsCount }
            });

            ConsoleHelper.WriteSuccess("Dataset is ready for use!");
            Console.WriteLine();
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        #endregion Private Methods
    }
}