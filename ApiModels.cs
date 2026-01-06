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

    /// <summary>
    /// Class to represent status of the search engine.
    /// </summary>
    public class SystemStatus
    {
        /// <summary>
        /// Number of documents loaded and indexed.
        /// </summary>
        public int DocumentCount { get; set; }

        /// <summary>
        /// ErrorMessage is typically used to give fast and comprehensible error information feedback.
        /// </summary>
        public string ErrorMessage { get; set; } = "";

        /// <summary>
        /// Set if Query == null.
        /// </summary>
        public bool InvalidArgument { get; set; }

        /// <summary>
        /// Only relevant for certain configurations.
        /// </summary>
        public bool InvalidDataSetName { get; set; }

        /// <summary>
        /// Will return true if invalid state operations are attempted.
        /// </summary>
        public bool InvalidState { get; set; }

        /// <summary>
        /// ReIndexRequired is returned true when a fraction over a limit of the documents has been deleted or inserted.
        /// </summary>
        public bool ReIndexRequired { get; set; }

        /// <summary>
        /// Returns the number of calls to the search function after occurring.
        /// </summary>
        public int SearchCounter { get; set; }

        /// <summary>
        /// Duration time of last indexing operation.
        /// </summary>
        public int SecondsToIndex { get; set; }

        /// <summary>
        /// Reflects the state of the system.
        /// </summary>
        public SystemState SystemState { get; set; }

        /// <summary>
        /// Returns timestamp for call to constructor.
        /// </summary>
        public DateTime TimeOfInstanceCreation { get; set; }

        /// <summary>
        /// Returns timestamp of last call to Index.
        /// </summary>
        public DateTime TimeOfLastIndexBuild { get; set; }

        /// <summary>
        /// Returned true if the maximum length of client text is exceeded.
        /// </summary>
        public bool TooLongClientText { get; set; }

        /// <summary>
        /// Returned true if the maximum length of the search text is exceeded.
        /// </summary>
        public bool TooLongSearchText { get; set; }

        /// <summary>
        /// Returned true if the instance is created with an invalid configurationNumber.
        /// </summary>
        public bool UnknownConfigurationError { get; set; }

        /// <summary>
        /// Returns version number of Indx.
        /// </summary>
        public string Version { get; set; }
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

    /// <summary>
    /// Search query class for Cloud based Indx
    /// </summary>
    public class CloudQuery
    {
        /// <summary>
        /// Use CreateBoost endpoint to obtain the BoostProxies
        /// </summary>
        public BoostProxy[] Boosts { get; set; }

        /// <summary>
        /// The number of results from RR to be considered by Coverage.
        /// </summary>
        public int CoverageDepth { get; set; } = 500;

        /// <summary>
        /// Enables boost
        /// </summary>
        public bool EnableBoost { get; set; }

        /// <summary>
        /// Coverage is a function that detects exact matching on tokens in the query, and lifts them up to the top of the result list.
        /// If set to false, the search will run with pattern recognition (Relevancy Ranking) only.
        /// </summary>
        public bool EnableCoverage { get; set; } = true;

        /// <summary>
        /// Will make SearchResult return facet
        /// </summary>
        public bool EnableFacets { get; set; }

        /// <summary>
        /// Use the various CreateFilter endpoints to obtain the FilterProxies
        /// </summary>
        public FilterProxy Filter { get; set; }

        /// <summary>
        /// If the instance of the search engine has had a logger injected into its constructor,
        /// every search will be logged. The log prefix is then first added to the log text.
        /// </summary>
        public string LogPrefix { get; set; } = string.Empty;

        /// <summary>
        /// Defines the maximum number of documents to be returned.
        /// </summary>
        public int MaxNumberOfRecordsToReturn { get; set; }

        /// <summary>
        /// Removes all duplicates of documents with the same foreign key.
        /// </summary>
        public bool RemoveDuplicates { get; set; }

        /// <summary>
        /// Only applicable if SortBy !=null
        /// </summary>
        public bool SortAscending { get; set; }

        /// <summary>
        /// Use this in order to setup sorting of the results.
        /// </summary>
        public string SortBy { get; set; } = string.Empty;

        /// <summary>
        /// Refers to the text (input) to be searched for
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Sets maximum waiting time in case of overloaded CPU. We recommend 1000 ms.
        /// </summary>
        public int TimeOutLimitMilliseconds { get; set; } = 1000;
    }

    /// <summary>
    /// Search result class
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Returned true if there are not enough CPU resources to complete the search within the specified time.
        /// </summary>
        public bool DidTimeOut { get; set; }

        /// <summary>
        /// Returns a KeyValuePair list with the Facets for the results.
        /// </summary>
        public Dictionary<string, KeyValuePair<string, int>[]> Facets { get; set; }

        /// <summary>
        /// Returns the search records.
        /// </summary>
        public ScoreEntry[] Records { get; set; }

        /// <summary>
        /// The index number the Coverage system sets for truncation point. If this is -1 the system did not find any matches.
        /// </summary>
        public int TruncationIndex { get; set; }

        /// <summary>
        /// Returns the score value at truncation point in the list.
        /// </summary>
        public byte TruncationScore { get; set; }
    }

    /// <summary>
    /// Structure used to represent search results in Indx. It contains a score field
    /// of byte and a long field called DocumentKey.
    /// </summary>
    public struct ScoreEntry
    {
        /// <summary>
        /// Id, Key or internal index of a document.
        /// </summary>
        public long DocumentKey { get; set; }

        /// <summary>
        /// The score value of the document.
        /// </summary>
        public byte Score { get; set; }

        public ScoreEntry(byte score, long documentKey)
        {
            Score = score;
            DocumentKey = documentKey;
        }
    }

    #endregion

    #region Filters

    /// <summary>
    /// Placeholder for Filter objects on cloud client side
    /// </summary>
    public class FilterProxy
    {
        /// <summary>
        /// For internal use only
        /// </summary>
        public string HashString { get; set; }

        public FilterProxy()
        {
        }

        public FilterProxy(string hashString)
        {
            HashString = hashString;
        }
    }

    /// <summary>
    /// API class used by client side to create a RangeFilter
    /// </summary>
    public class RangeFilterProxy
    {
        public string FieldName { get; set; }
        public double LowerLimit { get; set; }
        public double UpperLimit { get; set; }
    }

    /// <summary>
    /// ValueFilterProxy used to make a value filter on cloud client side
    /// </summary>
    public class ValueFilterProxy
    {
        public string FieldName { get; set; }
        public object Value { get; set; }
    }

    /// <summary>
    /// Use this class to combine two filters with and / or operator.
    /// </summary>
    public class CombinedFilterProxy
    {
        /// <summary>
        /// A of result=A logical op B
        /// </summary>
        public FilterProxy A { get; set; }

        /// <summary>
        /// B as explained for A above
        /// </summary>
        public FilterProxy B { get; set; }

        /// <summary>
        /// UseAndOperation if not set to true, logical or between filters will result
        /// </summary>
        public bool UseAndOperation { get; set; }

        public CombinedFilterProxy()
        {
        }

        public CombinedFilterProxy(FilterProxy a, FilterProxy b, bool useAndOperation)
        {
            A = a;
            B = b;
            UseAndOperation = useAndOperation;
        }
    }

    #endregion

    #region Boost

    /// <summary>
    /// Use this proxy object from cloud api to create a boost setting
    /// </summary>
    public class BoostProxy
    {
        public BoostStrength BoostStrength { get; set; }
        public FilterProxy FilterProxy { get; set; }
    }

    public enum BoostStrength
    {
        Low = 1,
        Med = 2,
        High = 3
    }

    #endregion
}
