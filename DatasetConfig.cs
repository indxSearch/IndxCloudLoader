using Indx.Api;

namespace IndxCloudLoader
{
    internal class DatasetConfig
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public (string Name, int Weight)[] SearchableFields { get; set; }
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
                    SearchableFields = new[]
                    {
                        ("title", (int)Weight.High),
                        ("original_title", (int)Weight.Med),
                        ("description", (int)Weight.Med),
                        ("actors", (int)Weight.Low)
                    },
                    FilterableFields = new[] { "release_year", "vote_average", "vote_count_tier", "genres", "decade", "actors", "language" },
                    FacetableFields = new[] { "release_year", "vote_average", "vote_count_tier", "genres", "decade", "actors", "language" },
                    SortableFields = new[] { "popularity", "vote_average" },
                    TestQuery = "titanic"
                },
                "pokedex" => new DatasetConfig
                {
                    Name = "pokedex",
                    FilePath = "data/pokedex.json",
                    SearchableFields = new[]
                    {
                        ("name", (int)Weight.High),
                        ("type1", (int)Weight.Med),
                        ("type2", (int)Weight.Low)
                    },
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
