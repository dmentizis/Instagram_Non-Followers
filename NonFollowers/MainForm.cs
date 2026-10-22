using NonFollowers.Classes;
using NonFollowers.Methods;

namespace NonFollowers;

public partial class MainForm : Form
{
    private readonly Color accent = Color.FromArgb(92, 65, 205);
    private string[] followerPaths = [];
    private string followingPath = "";
    private string pendingPath = "";
    private readonly TextBox followersFile = FileBox("Select followers_1.json (all parts if split)");
    private readonly TextBox followingFile = FileBox("Select following.json");
    private readonly TextBox pendingFile = FileBox("Select pending_follow_requests.json");
    private readonly Button compare = new() { Text = "Compare profiles", Enabled = false };
    private readonly Button loadPending = new() { Text = "Load requests", Enabled = false };
    private readonly DataGridView comparisonGrid = CreateGrid();
    private readonly DataGridView pendingGrid = CreateGrid();
    private readonly Label comparisonStatus = Status("Choose your exports to find who doesn't follow you back.");
    private readonly Label pendingStatus = Status("Choose an export to see your pending follow requests.");
    private TabControl tabs = null!;

    public MainForm() { InitializeComponent(); }

    private void BuildInterface()
    {
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(24), RowCount = 3, ColumnCount = 1 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 84));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
        var header = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        header.Controls.Add(new Label { Text = "Connection Explorer", AutoSize = true, Font = new Font("Segoe UI", 23, FontStyle.Bold) });
        header.Controls.Add(new Label { Text = "A clearer view of your Instagram connections.", AutoSize = true, ForeColor = Color.DimGray });
        layout.Controls.Add(header, 0, 0);
        tabs = new TabControl { Dock = DockStyle.Fill, Padding = new Point(20, 10) };
        tabs.TabPages.Add(BuildTab(false));
        tabs.TabPages.Add(BuildTab(true));
        layout.Controls.Add(tabs, 0, 1);
        layout.Controls.Add(new Label { Text = "Private by design · Files are processed on your computer. Results reflect the export date.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 9), ForeColor = Color.DimGray }, 0, 2);
        Controls.Add(layout);
        compare.Click += async (_, _) => await LoadResults(false);
        loadPending.Click += async (_, _) => await LoadResults(true);
    }

    private TabPage BuildTab(bool pending)
    {
        var page = new TabPage(pending ? "Pending requests" : "Not following back") { BackColor = Color.White, Padding = new Padding(18) };
        var panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5 };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, pending ? 60 : 112));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        panel.Controls.Add(new Label { Text = pending ? "Follow requests still awaiting a response" : "Discover accounts that don't follow you back", AutoSize = true, Font = new Font("Segoe UI", 13, FontStyle.Bold) }, 0, 0);
        var files = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = pending ? 1 : 2 };
        files.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
        files.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        files.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        if (pending) AddFileRow(files, 0, "Requests", pendingFile, () => SelectFile(ExportKind.Pending));
        else
        {
            AddFileRow(files, 0, "Followers", followersFile, () => SelectFile(ExportKind.Followers));
            AddFileRow(files, 1, "Following", followingFile, () => SelectFile(ExportKind.Following));
        }
        panel.Controls.Add(files, 0, 1);
        var action = pending ? loadPending : compare;
        StyleButton(action, true);
        action.Width = 190;
        panel.Controls.Add(action, 0, 2);
        panel.Controls.Add(pending ? pendingStatus : comparisonStatus, 0, 3);
        panel.Controls.Add(pending ? pendingGrid : comparisonGrid, 0, 4);
        page.Controls.Add(panel);
        return page;
    }

    private void AddFileRow(TableLayoutPanel panel, int row, string title, TextBox box, Action select)
    {
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        panel.Controls.Add(new Label { Text = title, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
        panel.Controls.Add(box, 1, row);
        var browse = new Button { Text = "Browse…", Dock = DockStyle.Top };
        StyleButton(browse, false);
        browse.Click += (_, _) => select();
        panel.Controls.Add(browse, 2, row);
    }

    private void SelectFile(ExportKind kind)
    {
        using var dialog = new OpenFileDialog { Filter = "Instagram JSON exports (*.json)|*.json", Multiselect = kind == ExportKind.Followers, Title = $"Select {kind.ToString().ToLowerInvariant()} export", CheckFileExists = true };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        if (kind == ExportKind.Followers)
        {
            followerPaths = dialog.FileNames;
            followersFile.Text = string.Join("; ", followerPaths);
        }
        else if (kind == ExportKind.Following) followingFile.Text = followingPath = dialog.FileName;
        else pendingFile.Text = pendingPath = dialog.FileName;
        var pending = kind == ExportKind.Pending;
        (pending ? pendingGrid : comparisonGrid).DataSource = null;
        (pending ? pendingStatus : comparisonStatus).Text = "Files selected. " + (pending ? "Load requests to view results." : "Compare profiles to view results.");
        compare.Enabled = followerPaths.Length > 0 && followingPath.Length > 0;
        loadPending.Enabled = pendingPath.Length > 0;
    }

    private async Task LoadResults(bool pending)
    {
        var grid = pending ? pendingGrid : comparisonGrid;
        var status = pending ? pendingStatus : comparisonStatus;
        grid.DataSource = null;
        status.Text = "Reading your exports…";
        tabs.Enabled = false;
        UseWaitCursor = true;
        try
        {
            var users = await Task.Run(() => pending
                ? LogicMethods.LoadUsers(pendingPath, ExportKind.Pending)
                : LogicMethods.CompareUsers(LogicMethods.LoadUsers(followingPath, ExportKind.Following),
                    followerPaths.SelectMany(path => LogicMethods.LoadUsers(path, ExportKind.Followers))));
            grid.DataSource = users;
            status.Text = users.Count == 0
                ? (pending ? "No pending follow requests in this export." : "All accounts you follow follow you back in these exports.")
                : $"{users.Count:N0} " + (pending ? "pending requests · Open a profile to view it on Instagram." : "accounts don't follow you back · Sorted by username.");
        }
        catch (Exception ex)
        {
            status.Text = "Unable to load results. Check your files and try again.";
            MessageBox.Show(this, ex.Message, "Could not read export", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { tabs.Enabled = true; UseWaitCursor = false; }
    }

    private void StyleButton(Button button, bool primary)
    {
        button.Height = 38;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = primary ? 0 : 1;
        button.FlatAppearance.BorderColor = Color.FromArgb(218, 220, 232);
        button.BackColor = primary ? accent : Color.FromArgb(246, 245, 252);
        button.ForeColor = primary ? Color.White : accent;
        button.Cursor = Cursors.Hand;
    }

    private static TextBox FileBox(string placeholder) => new()
    {
        ReadOnly = true, PlaceholderText = placeholder, Dock = DockStyle.Top,
        Margin = new Padding(3, 7, 12, 3), BackColor = Color.White, AccessibleName = placeholder
    };
    private static Label Status(string text) => new() { Text = text, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.FromArgb(90, 82, 120) };

    private static DataGridView CreateGrid()
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill, AutoGenerateColumns = false, ReadOnly = true,
            AllowUserToAddRows = false, AllowUserToDeleteRows = false, AllowUserToResizeRows = false,
            RowHeadersVisible = false, BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal, GridColor = Color.FromArgb(236, 237, 244),
            SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
            EnableHeadersVisualStyles = false, ColumnHeadersHeight = 42,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(237, 234, 248), ForeColor = Color.FromArgb(59, 44, 100), Font = new Font("Segoe UI", 10, FontStyle.Bold), Padding = new Padding(8) };
        grid.DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(8, 4, 8, 4), SelectionBackColor = Color.FromArgb(225, 218, 250), SelectionForeColor = Color.FromArgb(35, 28, 60) };
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 249, 253);
        grid.RowTemplate.Height = 42;
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(User.Username), HeaderText = "Username", FillWeight = 35, MinimumWidth = 130, SortMode = DataGridViewColumnSortMode.NotSortable });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(User.Url), HeaderText = "Profile link", FillWeight = 65, MinimumWidth = 180, SortMode = DataGridViewColumnSortMode.NotSortable });
        grid.Columns.Add(new DataGridViewButtonColumn { Name = "OpenProfile", HeaderText = "", Text = "Open in browser", UseColumnTextForButtonValue = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.None, Width = 160 });
        grid.CellContentClick += (_, e) =>
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || grid.Columns[e.ColumnIndex].Name != "OpenProfile") return;
            if (grid.Rows[e.RowIndex].DataBoundItem is not User user) return;
            try { LogicMethods.OpenInBrowser(user.Url); }
            catch (Exception ex) { MessageBox.Show(grid.FindForm(), ex.Message, "Could not open profile", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        };
        return grid;
    }
}
