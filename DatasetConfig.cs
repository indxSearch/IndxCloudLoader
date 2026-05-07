namespace IndxCloudLoader
{
    internal class DatasetConfig
    {
        private const float WeightHigh = 1.5f;
        private const float WeightMed = 1.25f;
        private const float WeightLow = 1.0f;

        public string Name { get; set; }
        public string FilePath { get; set; }
        public int Configuration { get; set; }
        public (string Name, float Weight)[] SearchableFields { get; set; }
        public string[] WordIndexingFields { get; set; }
        public string[] FilterableFields { get; set; }
        public string[] FacetableFields { get; set; }
        public string[] SortableFields { get; set; }
        public string TestQuery { get; set; }

        public static DatasetConfig GetConfig(string datasetName)
        {
            return datasetName.ToLower() switch
            {
                "tmdb" => new DatasetConfig
                {
                    Name = "tmdb",
                    FilePath = "data/tmdb_top10k.json",
                    Configuration = 400,
                    SearchableFields = new[]
                    {
                        ("title", WeightHigh),
                        ("original_title", WeightMed),
                        ("description", WeightMed),
                        ("actors", WeightLow)
                    },
                    WordIndexingFields = new[] { "title" },
                    FilterableFields = new[] { "release_year", "vote_average", "vote_count_tier", "genres", "decade", "actors", "language" },
                    FacetableFields = new[] { "release_year", "vote_average", "vote_count_tier", "genres", "decade", "actors", "language" },
                    SortableFields = new[] { "popularity", "vote_average" },
                    TestQuery = "titanic"
                },
                "pokedex" => new DatasetConfig
                {
                    Name = "pokedex",
                    FilePath = "data/pokedex.json",
                    Configuration = 400,
                    SearchableFields = new[]
                    {
                        ("name", WeightHigh),
                        ("type1", WeightMed),
                        ("type2", WeightLow)
                    },
                    WordIndexingFields = new[] { "name" },
                    FilterableFields = new[] { "speed", "attack", "hp", "type1", "type2", "is_legendary" },
                    FacetableFields = new[] { "speed", "attack", "hp", "type1", "type2", "is_legendary" },
                    SortableFields = new[] { "name", "speed" },
                    TestQuery = "raic"
                },
                _ => null
            };
        }

        public static string[] GetAvailableDatasets()
        {
            return new[] { "tmdb", "pokedex" };
        }
    }
}
