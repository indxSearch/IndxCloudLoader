namespace IndxCloudLoader.Models
{
    /// <summary>
    /// Data Transfer Objects (DTOs) for communicating with IndxCloudApi.
    /// These types match the API contract and are used for JSON serialization/deserialization.
    /// </summary>

    #region Field Weights

    /// <summary>
    /// Weight values for searchable fields, determining their importance in search relevance scoring.
    /// Higher priority (High = 0) means the field has more influence on search result ranking.
    /// </summary>
    public enum Weight
    {
        High = 0,
        Med = 1,
        Low = 2
    }

    #endregion

    #region System Status

    public class SystemStatus
    {
        public SystemState SystemState { get; set; }
        public string Message { get; set; }
        public int RecordCount { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public enum SystemState
    {
        /// <summary>
        /// System locked in hibernation state.
        /// </summary>
        Hibernated = -1,
        /// <summary>
        /// Created is the initial state.
        /// </summary>
        Created = 0,
        /// <summary>
        /// Loading is while data is Loaded/Inserted.
        /// </summary>
        Loading = 1,
        /// <summary>
        /// Loading finished, ready for indexing.
        /// </summary>
        Loaded = 2,
        /// <summary>
        /// Indexing is set during indexing.
        /// </summary>
        Indexing = 3,
        /// <summary>
        /// Ready is set once search engine is ready to search.
        /// </summary>
        Ready = 4,
        /// <summary>
        /// Error indicates some kind of serious error.
        /// </summary>
        Error = 255
    }

    #endregion

    #region Search

    public class CloudQuery
    {
        public string Text { get; set; }
        public int MaxNumberOfRecordsToReturn { get; set; }
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
        public FilterProxy Filter { get; set; }
        public string[] Facets { get; set; }
        public int Skip { get; set; }
    }

    public class Result
    {
        public List<SearchRecord> Records { get; set; }
        public Dictionary<string, FacetResult> Facets { get; set; }
        public int TotalCount { get; set; }
        public double SearchTime { get; set; }
    }

    public class SearchRecord
    {
        public long DocumentKey { get; set; }
        public double Score { get; set; }
        public Dictionary<string, object> Highlights { get; set; }
    }

    public class FacetResult
    {
        public Dictionary<string, int> Values { get; set; }
    }

    #endregion

    #region Filters

    public class FilterProxy
    {
        public string FilterId { get; set; }
        public string FilterType { get; set; }
        public DateTime Created { get; set; }
    }

    public class RangeFilterProxy
    {
        public string FieldName { get; set; }
        public double LowerLimit { get; set; }
        public double UpperLimit { get; set; }
        public bool IncludeLower { get; set; }
        public bool IncludeUpper { get; set; }
    }

    public class ValueFilterProxy
    {
        public string FieldName { get; set; }
        public object Value { get; set; }
    }

    public class CombinedFilterProxy
    {
        public FilterProxy Filter1 { get; set; }
        public FilterProxy Filter2 { get; set; }
        public bool IsAnd { get; set; }

        public CombinedFilterProxy()
        {
        }

        public CombinedFilterProxy(FilterProxy filter1, FilterProxy filter2, bool isAnd)
        {
            Filter1 = filter1;
            Filter2 = filter2;
            IsAnd = isAnd;
        }
    }

    #endregion

    #region Boost

    public class BoostProxy
    {
        public FilterProxy FilterProxy { get; set; }
        public BoostStrength BoostStrength { get; set; }
        public string BoostId { get; set; }
    }

    public enum BoostStrength
    {
        None = 0,
        Low = 25,
        Medium = 50,
        High = 100,
        VeryHigh = 200
    }

    #endregion
}
