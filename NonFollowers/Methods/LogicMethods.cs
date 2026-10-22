using NonFollowers.Classes;
using System.Diagnostics;
using System.Text.Json;

namespace NonFollowers.Methods;

public enum ExportKind { Followers, Following, Pending }

public static class LogicMethods
{
    public static List<User> LoadUsers(string path, ExportKind kind)
    {
        using var document = JsonDocument.Parse(File.ReadAllText(path));
        var root = document.RootElement;
        string key = kind switch
        {
            ExportKind.Followers => "relationships_followers",
            ExportKind.Following => "relationships_following",
            _ => "relationships_follow_requests_sent"
        };
        bool singlePending = kind == ExportKind.Pending && root.ValueKind == JsonValueKind.Object && !root.TryGetProperty(key, out _) &&
            root.TryGetProperty("label_values", out var fields) && fields.ValueKind == JsonValueKind.Array;
        if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty(key, out var wrapped))
            root = wrapped;
        else if (!singlePending && (root.ValueKind != JsonValueKind.Array || kind == ExportKind.Following))
            throw new InvalidDataException($"Select a valid {kind.ToString().ToLowerInvariant()} export ({Path.GetFileName(path)}).");
        if (!singlePending && root.ValueKind != JsonValueKind.Array)
            throw new InvalidDataException($"The {kind} list must be a JSON array.");

        var users = new List<User>();
        IEnumerable<JsonElement> entries = singlePending ? new[] { root } : root.EnumerateArray();
        foreach (var item in entries)
        {
            string? username = null;
            if (kind == ExportKind.Pending && item.ValueKind == JsonValueKind.Object &&
                item.TryGetProperty("label_values", out var labels) && labels.ValueKind == JsonValueKind.Array)
            {
                foreach (var label in labels.EnumerateArray())
                    if (ReadString(label, "label") == "Username") username = ReadString(label, "value");
            }
            else if (item.ValueKind == JsonValueKind.Object && item.TryGetProperty("string_list_data", out var data) &&
                     data.ValueKind == JsonValueKind.Array && data.GetArrayLength() > 0)
            {
                username = ReadString(data[0], "value");
                if (string.IsNullOrWhiteSpace(username)) username = ReadString(item, "title");
            }
            username = username?.Trim();
            if (string.IsNullOrEmpty(username) || username.Length > 30 ||
                username.Any(c => !(char.IsAsciiLetterOrDigit(c) || c == '_' || c == '.')))
                throw new InvalidDataException($"An entry in {Path.GetFileName(path)} has no valid Instagram username. Check that you selected the correct export.");
            users.Add(new User { Username = username, Url = $"https://www.instagram.com/{username}/" });
        }
        return users.DistinctBy(u => u.Username, StringComparer.OrdinalIgnoreCase)
            .OrderBy(u => u.Username, StringComparer.OrdinalIgnoreCase).ToList();
    }

    private static string? ReadString(JsonElement element, string property) =>
        element.ValueKind == JsonValueKind.Object && element.TryGetProperty(property, out var value) &&
        value.ValueKind == JsonValueKind.String ? value.GetString() : null;

    public static List<User> CompareUsers(IEnumerable<User> following, IEnumerable<User> followers)
    {
        var names = followers.Select(u => u.Username).ToHashSet(StringComparer.OrdinalIgnoreCase);
        return following.Where(u => !names.Contains(u.Username))
            .DistinctBy(u => u.Username, StringComparer.OrdinalIgnoreCase)
            .OrderBy(u => u.Username, StringComparer.OrdinalIgnoreCase).ToList();
    }

    public static void OpenInBrowser(string? url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || uri.Scheme != "https" ||
            uri.Host != "www.instagram.com" || !string.IsNullOrEmpty(uri.UserInfo))
            throw new InvalidDataException("This is not a valid Instagram profile link.");
        Process.Start(new ProcessStartInfo(uri.AbsoluteUri) { UseShellExecute = true });
    }
}
