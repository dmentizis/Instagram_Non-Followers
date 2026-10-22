#nullable enable
namespace NonFollowers;

partial class MainForm
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1080, 740);
        MinimumSize = new Size(820, 620);
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.FromArgb(244, 246, 251);
        ForeColor = Color.FromArgb(30, 40, 60);
        Text = "Instagram · Connection Explorer";
        StartPosition = FormStartPosition.CenterScreen;
        BuildInterface();
    }
}
