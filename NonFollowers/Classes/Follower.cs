namespace NonFollowers.Classes
{
    public class Follower
    {
        public string? Title { get; set; }
        public List<object>? Media_list_data { get; set; }
        public List<FollowerStringListDatum>? String_list_data { get; set; }
    }

    public class FollowerStringListDatum
    {
        public string? Href { get; set; }
        public string? Value { get; set; }
        public int? Timestamp { get; set; }
    }
}