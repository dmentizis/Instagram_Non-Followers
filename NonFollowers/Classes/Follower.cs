using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonFollowers.Classes
{
    public class Follower
    {
        public string? title { get; set; }
        public List<object>? media_list_data { get; set; }
        public List<FollowerStringListDatum>? string_list_data { get; set; }
    }

    public class FollowerStringListDatum
    {
        public string? href { get; set; }
        public string? value { get; set; }
        public int? timestamp { get; set; }
    }
}