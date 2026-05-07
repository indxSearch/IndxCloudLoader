// TODO: remove this file once IndxSearchLib v5 NuGet package is published
using Indx.Api;
using Indx.CloudApi;

namespace IndxCloudLoader
{
    internal class BM25FFieldProxy
    {
        public string FieldName { get; set; }
        public float? BM25Fb { get; set; }
        public float? BM25Fk1 { get; set; }
    }

    internal class VectorQueryProxy
    {
        public string FieldName { get; set; }
        public float[] Vector { get; set; }
        public int MaxResults { get; set; } = 10;
        public FilterProxy Filter { get; set; }
    }

    internal class HybridQueryProxy
    {
        public string Text { get; set; }
        public int MaxNumberOfRecordsToReturn { get; set; } = 10;
        public FilterProxy Filter { get; set; }
        public int TimeOutLimitMilliseconds { get; set; } = 1000;
        public string EmbeddingField { get; set; }
        public float[] Vector { get; set; }
        public float Alpha { get; set; } = 0.5f;
    }

    internal class EmbeddingResultEntry
    {
        public long DocumentKey { get; set; }
        public float Score { get; set; }
    }

    internal class UpdateFieldProxy
    {
        public string FieldName { get; set; }
        public object Value { get; set; }
    }

    internal class FilterFieldUpdateProxy
    {
        public FilterProxy Filter { get; set; }
        public string FieldName { get; set; }
        public object Value { get; set; }
    }
}
