using MORT.Service.Plamo;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        MORT.LocalizeManager.LocalizeManager.Init("", MORT.LocalizeManager.AppLanguage.Korea);
        Application.SetHighDpiMode(HighDpiMode.DpiUnaware);
        string root = Path.Combine(Path.GetTempPath(), "mort-plamo-layout-" + Guid.NewGuid().ToString("N"));
        try {
            using var service = new PlamoService(root);
            foreach (float scale in new[] { 1F, 1.5F, 2F }) {
                using var form = new PlamoSettingsForm(service);
                form.AutoScaleMode = AutoScaleMode.None;
                // Exercise larger text as well as larger bounds without changing desktop DPI.
                var fonts = Descendants(form).Prepend(form).Select(c => (Control: c, Font: c.Font)).ToArray();
                foreach (var item in fonts)
                    item.Control.Font = new Font(item.Font.FontFamily,item.Font.Size * scale,item.Font.Style);
                form.Scale(new SizeF(scale,scale));
                foreach (var size in new[] { new Size(760,720), new Size(544,501), new Size(1000,800) }) {
                    form.ClientSize = new Size((int)(size.Width*scale),(int)(size.Height*scale));
                    Arrange(form);
                    var frame = (TableLayoutPanel)form.Controls[0];
                    var scroll = (Panel)frame.GetControlFromPosition(0,0)!;
                    var footer = (FlowLayoutPanel)frame.GetControlFromPosition(0,1)!;
                    Check(scroll.Bottom <= footer.Top, "content overlaps footer");
                    Check(footer.Bottom <= form.ClientSize.Height, "footer clipped");
                    Check(footer.Controls.Count == 1 && footer.Controls[0] is LinkLabel, "footer should contain only the model link");
                    Check(footer.Controls[0].Bounds.Bottom <= footer.ClientSize.Height, "model link clipped");
                    var content = (TableLayoutPanel)scroll.Controls[0];
                    Check(content.Width <= scroll.ClientSize.Width, "horizontal overflow");
                    VerifyColumns(content);
                    var status = Descendants(content).OfType<Label>().First(l => l.Text == service.DisplayStatus);
                    string original = status.Text;
                    status.Text = string.Join(" ", Enumerable.Repeat("다운로드 연결을 확인하세요. 긴 오류 메시지입니다.", 12));
                    Arrange(form);
                    VerifyColumns(content);
                    Check(status.Height >= status.GetPreferredSize(new Size(status.Width,0)).Height, "long status clipped");
                    status.Text = original;
                    Arrange(form);
                    Console.WriteLine($"PASS layout {size.Width}x{size.Height} at {scale*100}% (normal/long status, footer, rows)");
                }
            }
        } finally { Directory.Delete(root,true); }
    }
    private static void Arrange(Control control)
    {
        control.PerformLayout();
        foreach (Control child in control.Controls) Arrange(child);
        control.PerformLayout();
    }
    private static IEnumerable<Control> Descendants(Control parent)
    {
        foreach (Control child in parent.Controls) {
            yield return child;
            foreach (var nested in Descendants(child)) yield return nested;
        }
    }
    private static void VerifyColumns(TableLayoutPanel column)
    {
        int bottom = column.Padding.Top;
        foreach (Control child in column.Controls) {
            // These controls are intentionally collapsed while no operation is active.
            if (child is ProgressBar || child.Text == "작업 취소" || (child is Label && child.Text.Length == 0)) continue;
            Check(child.Top >= bottom, "rows overlap: " + child.Text);
            Check(child.Right <= column.ClientSize.Width - column.Padding.Right, "row clipped: " + child.Text);
            if (child is Label label)
                Check(label.Height >= label.GetPreferredSize(new Size(label.Width,0)).Height, "text clipped: " + label.Text);
            bottom = child.Bottom;
            if (child is TableLayoutPanel nested) VerifyColumns(nested);
        }
        Check(bottom <= column.Height - column.Padding.Bottom, "last row clipped");
    }
    private static void Check(bool valid, string message)
    {
        if (!valid) throw new InvalidOperationException(message);
    }
}
