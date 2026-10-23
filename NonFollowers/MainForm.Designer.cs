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
        SuspendLayout();
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1080, 740);
        MinimumSize = new Size(640, 480);
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.FromArgb(244, 246, 251);
        ForeColor = Color.FromArgb(30, 40, 60);
        Text = "Instagram · Connection Explorer";
        StartPosition = FormStartPosition.CenterScreen;
        BuildInterface();
        ResumeLayout(true);
    }
}
