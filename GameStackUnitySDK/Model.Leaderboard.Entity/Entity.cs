using System;

namespace GameStackUnitySDK.Model.Leaderboard.Entity
{
    /// <summary>
    /// Value contains a type (string, int, or float) and value pair.
    /// </summary>
    [Serializable]
    public class Value
    {
        /// <summary>
        /// val is the data value.
        /// </summary>
        public dynamic val;
        /// <summary>
        /// type is the data type of the value.
        /// </summary>
        public string type;
    }

    /// <summary>
    /// Dimension contains a field name and the data that applies to that field.
    /// </summary>
    [Serializable]
    public class Dimension
    {
        /// <summary>
        /// field is the name of the Dimension to apply values to.
        /// </summary>
        public string field;
        /// <summary>
        /// data is the value that applies to this dimension.
        /// </summary>
        public Value data;
    }

    /// <summary>
    /// Sort defines what field to sort on and how to sort the results.
    /// </summary>
    [Serializable]
    public class Sort
    {
        /// <summary>
        /// op is the SortOperation that will be applied to data.
        /// </summary>
        public string op;
        /// <summary>
        /// target is the aspect that will be sorted on.
        /// </summary>
        public string target;
    }

    /// <summary>
    /// Filter defines how to filter a data set.
    /// </summary>
    [Serializable]
    public class Filter
    {
        /// <summary>
        /// op is the FilterOperation that will be applied to data.
        /// </summary>
        public string op;
        /// <summary>
        /// target is the aspect that this filter will be applied to.
        /// </summary>
        public string target;
        /// <summary>
        /// val is the value a filter will use when comparing to the target as
        /// specified by the operation.
        /// </summary>
        public Value val;
        /// <summary>
        /// not is an optional flag that dictates this filter applies to matches
        /// that do not meet the criteria.
        /// </summary>
        public bool not;
    }

    /// <summary>
    /// Pagination defines how a request should be paginated or contain
    /// information on how to paginate a request.
    /// </summary>
    [System.Serializable]
    public class Pagination
    {
        /// <summary>
        /// count is the FilterOperation that will be applied to data.
        /// </summary>
        public int count;
        /// <summary>
        /// token is the optional pagination token to use for a pagination
        /// request.
        /// </summary>
        public string token;
        /// <summary>
        /// prev is a token that will provide the previous page of data when
        /// used as the token for a paginated request.
        /// </summary>
        public Value prev;
        /// <summary>
        /// next is a token that will provide the next page of data when used
        /// as the token for a paginated request.
        /// </summary>
        public bool next;
    }
}

