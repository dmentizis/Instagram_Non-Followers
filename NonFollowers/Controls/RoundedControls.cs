using System.Drawing.Drawing2D;

namespace NonFollowers.Controls;

internal static class RoundedShape
{
    public static GraphicsPath Create(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        int diameter = Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height));
        path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
        path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
        path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }
}

internal sealed class RoundedButton : Button
{
    private bool hovered;
    private bool pressed;
    public RoundedButton()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }
    protected override void OnMouseEnter(EventArgs e) { hovered = true; base.OnMouseEnter(e); Invalidate(); }
    protected override void OnMouseLeave(EventArgs e) { hovered = false; pressed = false; base.OnMouseLeave(e); Invalidate(); }
    protected override void OnMouseDown(MouseEventArgs e) { pressed = e.Button == MouseButtons.Left; base.OnMouseDown(e); Invalidate(); }
    protected override void OnMouseUp(MouseEventArgs e) { pressed = false; base.OnMouseUp(e); Invalidate(); }
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent?.BackColor ?? SystemColors.Control);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var shape = RoundedShape.Create(new Rectangle(0, 0, Width - 1, Height - 1), LogicalToDeviceUnits(6));
        var fill = !Enabled ? Color.FromArgb(235, 232, 244) : pressed ? ControlPaint.Dark(BackColor, .08f) : hovered ? ControlPaint.Light(BackColor, .08f) : BackColor;
        using var brush = new SolidBrush(fill);
        e.Graphics.FillPath(brush, shape);
        if (FlatAppearance.BorderSize > 0)
        {
            using var pen = new Pen(FlatAppearance.BorderColor);
            e.Graphics.DrawPath(pen, shape);
        }
        TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, Enabled ? ForeColor : Color.FromArgb(125, 119, 143), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        if (Focused && ShowFocusCues)
            ControlPaint.DrawFocusRectangle(e.Graphics, Rectangle.Inflate(ClientRectangle, -5, -5), ForeColor, fill);
    }
}

internal sealed class RoundedFileField : Panel
{
    public RoundedFileField(TextBox box)
    {
        DoubleBuffered = true;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        BackColor = Color.White;
        Dock = DockStyle.Fill;
        Margin = new Padding(3, 3, 8, 3);
        Padding = new Padding(9, 5, 9, 4);
        box.BorderStyle = BorderStyle.None;
        box.Margin = Padding.Empty;
        box.Dock = DockStyle.Fill;
        Controls.Add(box);
    }
    public override Size GetPreferredSize(Size proposedSize)
    {
        // A single-line TextBox has a font-dependent height even with Dock.Fill.
        var box = (TextBox)Controls[0];
        return new Size(Padding.Horizontal + 1, box.PreferredHeight + Padding.Vertical);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var path = RoundedShape.Create(new Rectangle(0, 0, Width - 1, Height - 1), LogicalToDeviceUnits(6));
        using var pen = new Pen(Color.FromArgb(218, 220, 232));
        e.Graphics.DrawPath(pen, path);
    }
}
