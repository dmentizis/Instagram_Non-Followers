namespace NonFollowers.Classes
{

    public class Following
    {
        public List<RelationshipsFollowing>? relationships_following { get; set; }

        public class RelationshipsFollowing
        {
            public string? title { get; set; }
            public List<FolowingStringListDatum>? string_list_data { get; set; }
        }

        public class FolowingStringListDatum
        {
            public string? href { get; set; }
            public int? timestamp { get; set; }
        }

    }


}
