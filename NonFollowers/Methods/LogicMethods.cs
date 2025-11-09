using Newtonsoft.Json;
using NonFollowers.Classes;
using System.Diagnostics;

namespace NonFollowers.Methods
{
    public static class LogicMethods
    {
        public static void OpenInBrowser(string? url)
        {
            try
            {
                ProcessStartInfo psi = new()
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not open URL in browser.", ex);
            }
        }

        public static bool IsNullOrEmpty<T>(List<T>? list)
        {
            return list == null || list.Count == 0;
        }

        public static bool IsNullObject(object? obj)
        {
            return obj == null;
        }

        public static List<T>? LoadJsonToObjectList<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception("Invalid File Path");


            using StreamReader r = new(filePath);
            string json = r.ReadToEnd();
            List<T>? items = JsonConvert.DeserializeObject<List<T>>(json);

            return items;
        }

        public static T? LoadJsonToObject<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception("Invalid File Path");

            using StreamReader r = new(filePath);
            string json = r.ReadToEnd();
            T? item = JsonConvert.DeserializeObject<T>(json);

            return item;
        }

        public static void CompareLists(List<Following.RelationshipsFollowing> Following, List<Follower> Followers, List<User> Users)
        {
            foreach (var followingUsr in Following)
            {
                bool foundInFollowers = false;
                var followingUsername = followingUsr.title;

                foreach (var followerUsr in Followers)
                {

                    string? followerUsername = followerUsr.String_list_data?.FirstOrDefault()?.Value;

                    if (followingUsername == followerUsername)
                    {
                        foundInFollowers = true;
                        continue;
                    }
                }

                if (!foundInFollowers)
                {
                    Users.Add(new User { Username = followingUsr.title ?? string.Empty, Url = followingUsr?.string_list_data?.FirstOrDefault()?.href ?? string.Empty });
                }
            }
        }
    }

}
