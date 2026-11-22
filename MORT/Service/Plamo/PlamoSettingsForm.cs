using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT.Service.Plamo;

/// <summary>Chrome bridge's dark, section-based presentation with native DPI-aware layout.</summary>
public sealed class PlamoSettingsForm : Form
{
    private static readonly Color Background = Color.FromArgb(22,24,29), PanelColor = Color.FromArgb(31,35,43);
    private static readonly Color TextColor = Color.FromArgb(230,233,239), Dim = Color.FromArgb(154,163,178);
    private static readonly Color Accent = Color.FromArgb(110,168,254), InputColor = Color.FromArgb(18,21,26);
    private readonly PlamoService _service;
    private readonly Button _models = MakeButton("", false);
    private readonly ContextMenuStrip _modelMenu = new();
    private PlamoModel _selectedModel;
    private bool _showBusy;
    private readonly CheckBox _cuda = new() { Text = MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO CUDA"), AutoSize = true, Margin = new Padding(0,12,0,0) };
    private readonly Label _status = new() { AutoSize = true, Dock = DockStyle.Fill, Margin = new Padding(0,10,0,0) };
    private readonly Label _installed = new() { AutoSize = true, Dock = DockStyle.Fill, ForeColor = Dim, Margin = new Padding(0,8,0,0) };
    private readonly Label _summary = new() { AutoSize = true, Dock = DockStyle.Fill, Margin = new Padding(0,0,0,8) };
    private readonly ProgressBar _progress = new() { Dock = DockStyle.Fill, Height = 8, Style = ProgressBarStyle.Marquee, Visible = false, Margin = new Padding(0,8,0,0) };
    private readonly Button _download = MakeButton(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO DOWNLOAD"), true);
    private readonly Button _cancel = MakeButton(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO CANCEL"), false);
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 1000 };
    private CancellationTokenSource? _operation;
    private bool _closeAfterCancel;
    private bool _syncingSelection;
    private string? _operationError;

    public PlamoSettingsForm(PlamoService service)
    {
        SuspendLayout();
        _service = service;
        _selectedModel = service.SelectedModel;
        Text = "MORT · " + MORT.LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE PLAMO");
        Font = new Font("맑은 고딕", 10F);
        AutoScaleDimensions = new SizeF(96,96); AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Background; ForeColor = TextColor;
        ClientSize = new Size(760,540); MinimumSize = new Size(560,540);
        StartPosition = FormStartPosition.CenterParent; MinimizeBox = false; ShowInTaskbar = false;
        var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(24), Margin = Padding.Empty };
        var layout = CreateColumn();
        layout.Margin = Padding.Empty;
        void Add(Control control) => AddRow(layout, control);
        Add(new Label { Text = MORT.LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE PLAMO"), AutoSize = true, Font = new Font(Font.FontFamily,17F,FontStyle.Bold), Margin = new Padding(0,0,0,6) });

        var state = Section(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO ENGINE STATUS"));
        _summary.Font = new Font(Font, FontStyle.Bold);
        _summary.Padding = new Padding(12,8,12,8);
        _summary.BackColor = Color.FromArgb(40,45,55);
        var progressSpace = new Panel { Height = 16, Margin = new Padding(0,8,0,0) };
        _progress.Dock = DockStyle.Fill;
        progressSpace.Controls.Add(_progress);
        AddRow(state, _summary); AddRow(state, _status); AddRow(state, progressSpace);
        Add(state);
        var model = Section(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO MODEL MODE"));
        _models.BackColor = InputColor;
        _models.FlatAppearance.BorderSize = 1;
        _models.FlatAppearance.BorderColor = Color.FromArgb(48,55,67);
        _models.TextAlign = ContentAlignment.MiddleLeft;
        _models.Padding = new Padding(12,6,36,6);
        _models.Margin = new Padding(0,0,0,4);
        _models.AccessibleName = MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO SELECT");
        _models.Paint += (_,e) => {
            float unit = _models.DeviceDpi / 96F;
            float x = _models.ClientSize.Width - 18 * unit, y = _models.ClientSize.Height / 2F;
            using var pen = new Pen(_models.Enabled ? TextColor : Dim, 1.5F * unit);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawLines(pen,new[] { new PointF(x - 4 * unit,y - 2 * unit), new PointF(x,y + 2 * unit), new PointF(x + 4 * unit,y - 2 * unit) });
        };
        _modelMenu.ShowImageMargin = false;
        _modelMenu.BackColor = InputColor;
        _modelMenu.ForeColor = TextColor;
        _modelMenu.Renderer = new ModelMenuRenderer();
        foreach (var entry in PlamoService.Models) {
            var item = new ToolStripMenuItem(entry.Title) { Tag = entry, Padding = new Padding(10,8,10,8) };
            item.Click += async (_,_) => {
                SelectModel(entry);
                await SaveSelectionAsync();
            };
            _modelMenu.Items.Add(item);
        }
        _models.Click += (_,_) => {
            if (_operation != null) return;
            _modelMenu.Font = _models.Font;
            foreach (ToolStripMenuItem item in _modelMenu.Items) {
                item.AutoSize = false;
                item.Size = new Size(_models.Width - 2, _models.Font.Height + 20 * DeviceDpi / 96);
                item.Text = ((PlamoModel)item.Tag!).Title + (((PlamoModel)item.Tag!).Id == _selectedModel.Id ? "  ✓" : "");
            }
            _modelMenu.Show(_models,new Point(0,_models.Height));
        };
        SelectModel(service.SelectedModel);
        _cuda.Checked = service.Settings.UseCuda;
        AddRow(model, _models); AddRow(model, _installed); AddRow(model, _cuda);
        AddRow(model, new Label { Text = MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO CPU"), AutoSize = true, ForeColor = Dim, Margin = new Padding(0,4,0,0) });
        var actions = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Dock = DockStyle.Top, WrapContents = true, Margin = new Padding(0,18,0,0) };
        actions.Controls.AddRange(new Control[] { _download,_cancel }); AddRow(model, actions); Add(model);

        Add(CreateModelComparison());
        scroll.Controls.Add(layout);
        var footer = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(24,12,24,16), Margin = Padding.Empty, WrapContents = false };
        var modelLink = new LinkLabel { Text = MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO HUB"), AutoSize = true, LinkColor = Accent, ActiveLinkColor = TextColor, VisitedLinkColor = Accent, Margin = new Padding(0,8,20,0) };
        modelLink.LinkClicked += (_,_) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"https://huggingface.co/pfnet/plamo-3-translate-{_selectedModel.Id}-GGUF") { UseShellExecute = true });
        footer.Controls.Add(modelLink);
        var frame = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Margin = Padding.Empty };
        frame.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
        frame.RowStyles.Add(new RowStyle(SizeType.Percent,100));
        frame.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        frame.Controls.Add(scroll,0,0); frame.Controls.Add(footer,0,1);
        Controls.Add(frame);
        _download.Click += async (_,_) => await Run(service.InstallAsync);
        _cancel.Click += (_,_) => { _operation?.Cancel(); _cancel.Enabled = false; };

        _cuda.CheckedChanged += async (_,_) => await SaveSelectionAsync();
        service.StatusChanged += StatusChanged;
        _timer.Tick += (_,_) => { if (_operation == null) RefreshControls(); };
        Shown += (_,_) => _timer.Start();
        FormClosing += (_,e) => { if (_operation != null) { _closeAfterCancel = true; _operation.Cancel(); e.Cancel = true; } };
        FormClosed += (_,_) => { _timer.Stop(); _timer.Dispose(); service.StatusChanged -= StatusChanged; _modelMenu.Dispose(); };
        RefreshControls();
        ResumeLayout(true);
    }
    private TableLayoutPanel CreateModelComparison()
    {
        string TextFor(string key) => MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO COMP " + key);
        var section = Section(TextFor("COMPARE"));
        var table = new TableLayoutPanel { ColumnCount = 3, AutoSize = true, Margin = Padding.Empty };
        foreach (float width in new[] { 40f, 25f, 35f })
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, width));

        void Row(string model, string speed, string quality, bool header = false)
        {
            int row = table.RowCount++;
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            int col = 0;
            foreach (string value in new[] { model, speed, quality })
                table.Controls.Add(new Label { Text = value, AutoSize = true, Dock = DockStyle.Fill,
                    ForeColor = header ? Dim : TextColor, BackColor = header ? InputColor : PanelColor,
                    Padding = new Padding(8,5,8,5), Margin = Padding.Empty }, col++, row);
        }
        Row(TextFor("MODEL"), TextFor("SPEED"), TextFor("QUALITY"), true);
        foreach (var model in PlamoService.Models) {
            var (speed, quality) = model.Id switch {
                "0.8b" => ("FAST", "BASIC"),
                "2b" => ("MEDIUM", "BALANCED"),
                "8b" => ("SLOW", "HIGH"),
                _ => (null, null)
            };
            string name = model.Id.ToUpperInvariant();
            if (model.Id == PlamoSettings.DefaultModel)
                name = string.Format(TextFor("DEFAULT MODEL"), name);
            Row(name, speed == null ? "—" : TextFor(speed), quality == null ? "—" : TextFor(quality));
        }
        AddRow(section, table);
        return section;
    }

    private void SelectModel(PlamoModel model)
    {
        _selectedModel = model;
        _models.Text = model.Title;
    }
    private sealed class ModelMenuRenderer : ToolStripProfessionalRenderer
    {
        public ModelMenuRenderer() { RoundedEdges = false; }
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.Clear(InputColor);
        }
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            using var brush = new SolidBrush(e.Item.Selected ? Color.FromArgb(48,62,82) : InputColor);
            e.Graphics.FillRectangle(brush,new Rectangle(Point.Empty,e.Item.Size));
        }
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = TextColor;
            base.OnRenderItemText(e);
        }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(65,78,96));
            e.Graphics.DrawRectangle(pen,0,0,e.ToolStrip.Width - 1,e.ToolStrip.Height - 1);
        }
    }
    private static Button MakeButton(string title, bool primary)
    {
        var button = new Button { Text = title, AutoSize = true, MinimumSize = new Size(88,36), Padding = new Padding(10,4,10,4), FlatStyle = FlatStyle.Flat, BackColor = primary ? Accent : Color.FromArgb(42,47,58), ForeColor = primary ? Color.FromArgb(16,19,26) : TextColor, Margin = new Padding(0,0,8,0), UseVisualStyleBackColor = false };
        button.FlatAppearance.BorderSize = 0; return button;
    }
    private static TableLayoutPanel CreateColumn(bool card = false)
    {
        TableLayoutPanel column = card ? new SectionPanel() : new TableLayoutPanel();
        column.AutoSize = true; column.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        column.ColumnCount = 1; column.RowCount = 0;
        column.Dock = DockStyle.Top; column.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
        column.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
        column.Layout += (_,_) => {
            // Bound AutoSize text before measuring row height, including long error messages.
            foreach (Control control in column.Controls) {
                if (control is not Label label) continue;
                int width = Math.Max(1, column.ClientSize.Width - column.Padding.Horizontal - label.Margin.Horizontal);
                if (label.MaximumSize.Width != width) label.MaximumSize = new Size(width,0);
            }
        };
        return column;
    }
    private static void AddRow(TableLayoutPanel column, Control control)
    {
        control.Dock = DockStyle.Top;
        column.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        column.Controls.Add(control,0,column.RowCount++);
    }
    private TableLayoutPanel Section(string title)
    {
        var section = CreateColumn(true);
        section.BackColor = PanelColor;
        section.Padding = new Padding(20);
        section.Margin = new Padding(0,0,0,16);
        AddRow(section, new Label { Text = title, AutoSize = true, Font = new Font(Font,FontStyle.Bold), ForeColor = Accent, Margin = new Padding(0,0,0,14) });
        return section;
    }
    private sealed class SectionPanel : TableLayoutPanel
    {
        public SectionPanel() { DoubleBuffered = true; ResizeRedraw = true; }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent?.BackColor ?? Background);
            if (Width < 2 || Height < 2) return;
            float diameter = Math.Min(16F * DeviceDpi / 96F, Math.Min(Width - 1,Height - 1));
            float right = Width - 1, bottom = Height - 1;
            using var path = new GraphicsPath();
            path.AddArc(0,0,diameter,diameter,180,90);
            path.AddArc(right - diameter,0,diameter,diameter,270,90);
            path.AddArc(right - diameter,bottom - diameter,diameter,diameter,0,90);
            path.AddArc(0,bottom - diameter,diameter,diameter,90,90);
            path.CloseFigure();
            var smoothing = e.Graphics.SmoothingMode;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var fill = new SolidBrush(BackColor);
            using var border = new Pen(Color.FromArgb(48,55,67));
            e.Graphics.FillPath(fill,path); e.Graphics.DrawPath(border,path);
            e.Graphics.SmoothingMode = smoothing;
        }
    }
    private void StatusChanged(string status)
    {
        if (IsDisposed || !IsHandleCreated) return;
        try { BeginInvoke((Action)(() => { if (!IsDisposed && (_operation == null || _showBusy)) { RefreshSummary(); } })); }
        catch (InvalidOperationException) { }
    }
    private bool InstalledSelection()
    {
        var model = _selectedModel;
        string runtime = Path.Combine(_service.Root, "b12405" + (_cuda.Checked ? "-cuda" : "-cpu"), "llama-server.exe");
        return File.Exists(Path.Combine(_service.Root, model.FileName)) && File.Exists(runtime);
    }
    private async Task SaveSelectionAsync()
    {
        if (_operation != null || _syncingSelection) return;
        string model = _selectedModel.Id;
        bool cuda = _cuda.Checked;
        await Run(token => _service.ReconfigureAsync(model, cuda, token), false);
    }
    private void RefreshSummary()
    {
        _summary.Text = _service.DisplayStatus;
        _status.Text = _service.IsInitializing ? PlamoService.InitializationNotice : _operationError ?? "";
        _status.Visible = _status.Text.Length > 0;
        _summary.ForeColor = _showBusy ? Color.FromArgb(232,196,106) : _service.IsReady ? Color.FromArgb(95,208,138) : Dim;
    }
    private void RefreshControls()
    {
        bool busy = _operation != null, ready = _service.IsReady, installed = InstalledSelection();
        _models.Enabled = _cuda.Enabled = !busy;
        _download.Enabled = !busy && !ready;
        _download.Text = installed ? MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO REPAIR") : MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO DOWNLOAD");
        _cancel.Visible = _showBusy; _cancel.Enabled = busy;
        _progress.Visible = _showBusy;
        _installed.Text = installed ? MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO INSTALLED") : MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO NEED FILES");
        RefreshSummary();
    }
    private async Task Run(Func<CancellationToken, Task> action, bool configure = true)
    {
        if (_operation != null || _syncingSelection) return;
        try {
            _operationError = null;
            if (configure) _service.Configure(_selectedModel.Id, _cuda.Checked);
            _operation = new();
            var operation = Task.Run(() => action(_operation.Token));
            // Short settings writes should not flash progress or reflow the cards.
            if (await Task.WhenAny(operation, Task.Delay(200)) != operation) {
                _showBusy = true;
                RefreshControls();
            }
            await operation;
        } catch (OperationCanceledException) { }
        catch (Exception ex) { _operationError = ex.Message; }
        finally {
            _syncingSelection = true;
            try { SelectModel(_service.SelectedModel); _cuda.Checked = _service.Settings.UseCuda; }
            finally { _syncingSelection = false; }
            _operation?.Dispose(); _operation = null; _showBusy = false; RefreshControls();
            if (_closeAfterCancel) Close();
        }
    }
}
