using System;
using System.Drawing;
using System.Windows.Forms;
using MORT.Service.Plamo;

namespace MORT;
public partial class Form1
{
    private Panel pnPlamo = null!;
    private Label lbPlamoNotice = null!;
    private Label lbPlamoStatus = null!;
    private readonly PlamoService _plamoService;
    private void InitializePlamoPanel()
    {
        // Copy the already-scaled DeepL panel geometry rather than introducing another DPI baseline.
        pnPlamo = new Panel { Name = "pnPlamo", Bounds = pnDeepl.Bounds, Visible = false };
        var lbPlamoInformation = new Label { Name = "PlamoInfo",
            Text = MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO INFO"), Bounds = lbDeepLInfo.Bounds,
            Font = lbDeepLInfo.Font, ForeColor = lbDeepLInfo.ForeColor,
            TextAlign = lbDeepLInfo.TextAlign, AutoEllipsis = true
        };
        lbPlamoNotice = new Label {
            Text = PlamoService.InitializationNotice, Visible = false,
            Bounds = Rectangle.FromLTRB(lbDeepLStatus.Left, lbDeepLInfo.Bottom, btnCheckDeeplState.Left, lbDeepLStatus.Top),
            Font = lbDeepLStatus.Font, ForeColor = lbDeepLStatus.ForeColor
        };
        lbPlamoStatus = new Label {
            Text = string.Format(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO STATUS"), MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO NEEDS INIT")), Bounds = lbDeepLStatus.Bounds,
            Font = lbDeepLStatus.Font, ForeColor = lbDeepLStatus.ForeColor,
            TextAlign = lbDeepLStatus.TextAlign, AutoEllipsis = true
        };
        lbPlamoStatus.Width = btnCheckDeeplState.Left - lbDeepLStatus.Left;
        lbPlamoStatus.TextAlign = ContentAlignment.TopLeft;
        var button = new Button { Name = "PlamoSettings",
            Text = MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO SETTINGS"), Bounds = btnCheckDeeplState.Bounds, Font = btnCheckDeeplState.Font,
            BackColor = btnCheckDeeplState.BackColor, ForeColor = btnCheckDeeplState.ForeColor,
            FlatStyle = btnCheckDeeplState.FlatStyle, UseVisualStyleBackColor = false
        };
        button.FlatAppearance.BorderColor = btnCheckDeeplState.FlatAppearance.BorderColor;
        button.Click += (_,_) => {
            using var dialog = new PlamoSettingsForm(_plamoService);
            dialog.ShowDialog(this);
            RefreshPlamoStatus();
        };
        pnPlamo.Controls.AddRange(new Control[] { lbPlamoInformation, lbPlamoNotice, lbPlamoStatus, button });
        pnTranslate.Controls.Add(pnPlamo);
        _plamoService.StatusChanged += OnPlamoStatusChanged;
    }
    private void RefreshPlamoStatus()
    {
        lbPlamoStatus.Text = string.Format(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO STATUS"), _plamoService.DisplayStatus);
        lbPlamoNotice.Visible = _plamoService.IsInitializing;
    }
    private void OnPlamoStatusChanged(string status)
    {
        if (IsDisposed || !IsHandleCreated) return;
        try { BeginInvoke((Action)(() => { if (!IsDisposed) RefreshPlamoStatus(); })); }
        catch (InvalidOperationException) { }
    }
    private void StopPlamo()
    {
        _plamoService.StatusChanged -= OnPlamoStatusChanged;
        _plamoService.Dispose();
    }
}
