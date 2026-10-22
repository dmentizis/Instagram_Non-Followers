using NonFollowers;
using NonFollowers.Methods;
using System.Text.Json;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        var temp = Path.Combine(Path.GetTempPath(), "nonfollowers-check-" + Guid.NewGuid());
        Directory.CreateDirectory(temp);
        int checks = 0;
        void Check(bool condition, string name)
        {
            if (!condition) throw new Exception("FAILED: " + name);
            checks++;
            Console.WriteLine("PASS: " + name);
        }
        List<NonFollowers.Classes.User> Parse(string json, ExportKind kind)
        {
            string path = Path.Combine(temp, "fixture.json");
            File.WriteAllText(path, json);
            return LogicMethods.LoadUsers(path, kind);
        }
        void Reject(string json, ExportKind kind)
        {
            try { Parse(json, kind); }
            catch (Exception ex) when (ex is InvalidDataException or JsonException) { checks++; return; }
            throw new Exception("Invalid export was accepted");
        }
        try
        {
            var followers = Parse("""[{"string_list_data":[{"value":" Alice "}]}]""", ExportKind.Followers);
            var following = Parse("""{"relationships_following":[{"title":"alice","string_list_data":[{"href":"ignored"}]},{"title":"bob","string_list_data":[{}]},{"title":"BOB","string_list_data":[{}]}]}""", ExportKind.Following);
            Check(LogicMethods.CompareUsers(following, followers).Single().Username == "bob", "Case-insensitive comparison and deduplication");
            Check(Parse("[]", ExportKind.Followers).Count == 0, "Empty followers are valid");
            Check(LogicMethods.CompareUsers(following, []).Count == 2, "Empty follower list returns all following");
            Check(Parse("""{"relationships_following":[]}""", ExportKind.Following).Count == 0, "Empty following is valid");
            var pending = Parse("""[{"label_values":[{"label":"URL","value":""},{"label":"Username","value":"test_user"}]}]""", ExportKind.Pending);
            Check(pending.Single().Url == "https://www.instagram.com/test_user/", "Pending label format and missing URL fallback");
            Check(Parse("""{"relationships_follow_requests_sent":[{"string_list_data":[{"value":"legacy_user"}]}]}""", ExportKind.Pending).Count == 1, "Legacy pending format");
            Check(Parse("[]", ExportKind.Pending).Count == 0, "Empty pending export");
            Reject("{}", ExportKind.Followers);
            Reject("[]", ExportKind.Following);
            Reject("[{}]", ExportKind.Pending);
            Reject("null", ExportKind.Pending);
            Reject("not json", ExportKind.Followers);
            Reject("""[{"string_list_data":[{"value":"../../bad"}]}]""", ExportKind.Followers);
            try { LogicMethods.OpenInBrowser("file:///C:/Windows/notepad.exe"); throw new Exception("Unsafe URL accepted"); }
            catch (InvalidDataException) { checks++; }
            if (args.Length > 0)
            {
                var actualFollowers = LogicMethods.LoadUsers(Path.Combine(args[0], "followers_1.json"), ExportKind.Followers);
                var actualFollowing = LogicMethods.LoadUsers(Path.Combine(args[0], "following.json"), ExportKind.Following);
                var actualPending = LogicMethods.LoadUsers(Path.Combine(args[0], "pending_follow_requests.json"), ExportKind.Pending);
                var result = LogicMethods.CompareUsers(actualFollowing, actualFollowers);
                Check(result.All(u => !actualFollowers.Any(f => string.Equals(f.Username, u.Username, StringComparison.OrdinalIgnoreCase))), "Real export comparison");
                Console.WriteLine($"Sample totals: {actualFollowers.Count} followers, {actualFollowing.Count} following, {result.Count} not following back, {actualPending.Count} pending.");
            }
            ApplicationConfiguration.Initialize();
            using var form = new MainForm();
            form.CreateControl();
            void Prepare(Control control) { _ = control.Handle; control.PerformLayout(); foreach (Control child in control.Controls) Prepare(child); }
            Prepare(form);
            Check(form.Controls.Count > 0 && form.MinimumSize.Width >= 800, "Form initializes");
            if (args.Length > 1)
            {
                using var bitmap = new Bitmap(form.Width, form.Height);
                form.DrawToBitmap(bitmap, form.ClientRectangle);
                bitmap.Save(args[1]);
                var tabs = form.Controls[0].Controls.OfType<TabControl>().Single();
                tabs.SelectedIndex = 1;
                Prepare(form);
                var pendingGrid = tabs.TabPages[1].Controls[0].Controls.OfType<DataGridView>().Single();
                pendingGrid.DataSource = pending;
                Prepare(form);
                Check(pendingGrid.Rows.Count == 1 && pendingGrid.Columns["OpenProfile"] is DataGridViewButtonColumn, "Pending grid binds profile actions");
                using var pendingBitmap = new Bitmap(form.Width, form.Height);
                form.DrawToBitmap(pendingBitmap, form.ClientRectangle);
                pendingBitmap.Save(Path.Combine(Path.GetDirectoryName(args[1])!, "ui-pending-preview.png"));
            }
            Console.WriteLine($"All {checks} checks passed.");
        }
        finally { Directory.Delete(temp, true); }
    }
}


