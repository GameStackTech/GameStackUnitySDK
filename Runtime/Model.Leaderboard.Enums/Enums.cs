namespace GameStack.Model.Leaderboard.Enums
{
    /// <summary>
    /// Status defines the state of and entity like an application or
    /// leaderboard.
    /// </summary>
    public static class Status
    {
        /// <summary>
        /// Active is a live state.
        /// </summary>
        public const string Active = "ACTIVE";
        /// <summary>
        /// Status is a diabled state.
        /// </summary>
        public const string Deactivated = "DEACTIVATED";
    }

    /// <summary>
    /// UserState defines valid user access states.
    /// </summary>
    public static class UserState
    {
        /// <summary>
        /// Allowed defines a user that is currently active and allowed to
        /// access resources.
        /// </summary>
        public const string Allowed = "ALLOWED";
        /// <summary>
        /// Banned defines a user who has been banned from participation and
        /// access to resources.
        /// </summary>
        public const string Banned = "BANNED";
        /// <summary>
        /// Inactive defines a user who is not currently active and is not
        /// allowed to participate..
        /// </summary>
        public const string Inactive = "DEACTIVATED";
    }

    /// <summary>
    /// DataType defines data types for values.
    /// </summary>
    public static class DataType
    {
        /// <summary>
        /// String is a String data type.
        /// </summary>
        public const string String = "STRING";
        /// <summary>
        /// Int is an Integer data type.
        /// </summary>
        public const string Int = "INT";
        /// <summary>
        /// Float is a Floating Point data type.
        /// </summary>
        public const string Float = "FLOAT";
    }

    /// <summary>
    /// SortOperation defines sortng that will be applied.
    /// </summary>
    public static class SortOperation
    {
        /// <summary>
        /// Asc sorts values in asscending order.
        /// </summary>
        public const string Asc = "ASC";
        /// <summary>
        /// Desc sorts values in descending order.
        /// </summary>
        public const string Desc = "DESC";
        /// <summary>
        /// Rank sorts values by overall ranking in the leaderboard.
        /// </summary>
        public const string Rank = "RANK";
    }

    /// <summary>
    /// StatOperation defines the operation that will be applied to a statistic.
    /// </summary>
    public static class StatOperation
    {
        /// <summary>
        /// Avg averages a statistic.
        /// </summary>
        public const string Avg = "AVG";
        /// <summary>
        /// Max provides the maximum occurrence of a statistic.
        /// </summary>
        public const string Max = "MAX";
        /// <summary>
        /// Min provides the minimum occurrence of a statistic.
        /// </summary>
        public const string Min = "MIN";
        /// <summary>
        /// Sum totals a statistic.
        /// </summary>
        public const string Sum = "SUM";
    }

    /// <summary>
    /// FilterOperation defines the comparison operation a filter will perform.
    /// </summary>
    public static class FilterOperation
    {
        /// <summary>
        /// Eq matches values equal to a value.
        /// </summary>
        public const string Eq = "EQ";
        /// <summary>
        /// GT matches values greater than a value.
        /// </summary>
        public const string GT = "GT";
        /// <summary>
        /// LT matches values less than a value.
        /// </summary>
        public const string LT = "LT";
        /// <summary>
        /// GTE matches values greater than or equal to a value.
        /// </summary>
        public const string GTE = "GTE";
        /// <summary>
        /// LTE matches values less than or equal to a value.
        /// </summary>
        public const string LTE = "LTE";
    }
}
