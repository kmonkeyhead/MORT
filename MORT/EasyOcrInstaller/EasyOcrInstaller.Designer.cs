namespace MORT.EasyOcrInstaller
{
    partial class EasyOcrInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnInstall = new System.Windows.Forms.Button();
            btnEnableGpuInstallStep = new System.Windows.Forms.Button();
            pnMain = new System.Windows.Forms.Panel();
            btnGuide = new System.Windows.Forms.Button();
            cbForceInstall = new System.Windows.Forms.CheckBox();
            pnEnableGPU = new System.Windows.Forms.Panel();
            rbGpuCommandLine = new System.Windows.Forms.RadioButton();
            rbCuda = new System.Windows.Forms.RadioButton();
            btnEnableGpuInstall = new System.Windows.Forms.Button();
            tbGpuLine = new System.Windows.Forms.TextBox();
            lbGpuCommandLine = new System.Windows.Forms.Label();
            lbCudaVersion = new System.Windows.Forms.Label();
            cbCuda = new System.Windows.Forms.ComboBox();
            pnLog = new System.Windows.Forms.Panel();
            tbLog = new System.Windows.Forms.TextBox();
            pnMain.SuspendLayout();
            pnEnableGPU.SuspendLayout();
            pnLog.SuspendLayout();
            SuspendLayout();
            // 
            // btnInstall
            // 
            btnInstall.Location = new System.Drawing.Point(3, 48);
            btnInstall.Name = "btnInstall";
            btnInstall.Size = new System.Drawing.Size(858, 46);
            btnInstall.TabIndex = 0;
            btnInstall.Text = "기본 설치";
            btnInstall.UseVisualStyleBackColor = true;
            btnInstall.Click += btnInstall_Click;
            // 
            // btnEnableGpuInstallStep
            // 
            btnEnableGpuInstallStep.Location = new System.Drawing.Point(3, 100);
            btnEnableGpuInstallStep.Name = "btnEnableGpuInstallStep";
            btnEnableGpuInstallStep.Size = new System.Drawing.Size(858, 46);
            btnEnableGpuInstallStep.TabIndex = 1;
            btnEnableGpuInstallStep.Text = "GPU 가속 버전으로 설치 - cuda 필요";
            btnEnableGpuInstallStep.UseVisualStyleBackColor = true;
            btnEnableGpuInstallStep.Click += btnEnableGpuInstallStep_Click;
            // 
            // pnMain
            // 
            pnMain.Controls.Add(btnGuide);
            pnMain.Controls.Add(cbForceInstall);
            pnMain.Controls.Add(btnInstall);
            pnMain.Controls.Add(btnEnableGpuInstallStep);
            pnMain.Location = new System.Drawing.Point(9, 17);
            pnMain.Name = "pnMain";
            pnMain.Size = new System.Drawing.Size(861, 272);
            pnMain.TabIndex = 2;
            // 
            // btnGuide
            // 
            btnGuide.Location = new System.Drawing.Point(0, 176);
            btnGuide.Name = "btnGuide";
            btnGuide.Size = new System.Drawing.Size(858, 46);
            btnGuide.TabIndex = 3;
            btnGuide.Text = "사용법 확인";
            btnGuide.UseVisualStyleBackColor = true;
            btnGuide.Click += btnGuide_Click;
            // 
            // cbForceInstall
            // 
            cbForceInstall.AutoSize = true;
            cbForceInstall.Location = new System.Drawing.Point(3, 250);
            cbForceInstall.Name = "cbForceInstall";
            cbForceInstall.Size = new System.Drawing.Size(90, 19);
            cbForceInstall.TabIndex = 2;
            cbForceInstall.Text = "강제 재설치";
            cbForceInstall.UseVisualStyleBackColor = true;
            // 
            // pnEnableGPU
            // 
            pnEnableGPU.Controls.Add(rbGpuCommandLine);
            pnEnableGPU.Controls.Add(rbCuda);
            pnEnableGPU.Controls.Add(btnEnableGpuInstall);
            pnEnableGPU.Controls.Add(tbGpuLine);
            pnEnableGPU.Controls.Add(lbGpuCommandLine);
            pnEnableGPU.Controls.Add(lbCudaVersion);
            pnEnableGPU.Controls.Add(cbCuda);
            pnEnableGPU.Location = new System.Drawing.Point(9, 17);
            pnEnableGPU.Name = "pnEnableGPU";
            pnEnableGPU.Size = new System.Drawing.Size(861, 272);
            pnEnableGPU.TabIndex = 3;
            // 
            // rbGpuCommandLine
            // 
            rbGpuCommandLine.AutoSize = true;
            rbGpuCommandLine.Location = new System.Drawing.Point(18, 114);
            rbGpuCommandLine.Name = "rbGpuCommandLine";
            rbGpuCommandLine.Size = new System.Drawing.Size(117, 19);
            rbGpuCommandLine.TabIndex = 5;
            rbGpuCommandLine.Text = "명령어 직접 입력";
            rbGpuCommandLine.UseVisualStyleBackColor = true;
            rbGpuCommandLine.CheckedChanged += rbGpuCommandLine_CheckedChanged;
            // 
            // rbCuda
            // 
            rbCuda.AutoSize = true;
            rbCuda.Checked = true;
            rbCuda.Location = new System.Drawing.Point(18, 47);
            rbCuda.Name = "rbCuda";
            rbCuda.Size = new System.Drawing.Size(86, 19);
            rbCuda.TabIndex = 4;
            rbCuda.TabStop = true;
            rbCuda.Text = "CUDA 선택";
            rbCuda.UseVisualStyleBackColor = true;
            rbCuda.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // btnEnableGpuInstall
            // 
            btnEnableGpuInstall.Location = new System.Drawing.Point(3, 176);
            btnEnableGpuInstall.Name = "btnEnableGpuInstall";
            btnEnableGpuInstall.Size = new System.Drawing.Size(858, 46);
            btnEnableGpuInstall.TabIndex = 2;
            btnEnableGpuInstall.Text = "설치";
            btnEnableGpuInstall.UseVisualStyleBackColor = true;
            btnEnableGpuInstall.Click += btnEnableGpuInstall_Click;
            // 
            // tbGpuLine
            // 
            tbGpuLine.Location = new System.Drawing.Point(275, 114);
            tbGpuLine.Name = "tbGpuLine";
            tbGpuLine.Size = new System.Drawing.Size(433, 23);
            tbGpuLine.TabIndex = 3;
            // 
            // lbGpuCommandLine
            // 
            lbGpuCommandLine.AutoSize = true;
            lbGpuCommandLine.Location = new System.Drawing.Point(275, 88);
            lbGpuCommandLine.Name = "lbGpuCommandLine";
            lbGpuCommandLine.Size = new System.Drawing.Size(39, 15);
            lbGpuCommandLine.TabIndex = 2;
            lbGpuCommandLine.Text = "label2";
            // 
            // lbCudaVersion
            // 
            lbCudaVersion.AutoSize = true;
            lbCudaVersion.Location = new System.Drawing.Point(275, 18);
            lbCudaVersion.Name = "lbCudaVersion";
            lbCudaVersion.Size = new System.Drawing.Size(39, 15);
            lbCudaVersion.TabIndex = 1;
            lbCudaVersion.Text = "label1";
            // 
            // cbCuda
            // 
            cbCuda.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbCuda.FormattingEnabled = true;
            cbCuda.Items.AddRange(new object[] { "CUDA 11.8", "CUDA 12.1", "CUDA 12.4", "CUDA 12.6", "CUDA 12.8" });
            cbCuda.Location = new System.Drawing.Point(275, 47);
            cbCuda.Name = "cbCuda";
            cbCuda.Size = new System.Drawing.Size(433, 23);
            cbCuda.TabIndex = 0;
            // 
            // pnLog
            // 
            pnLog.Controls.Add(tbLog);
            pnLog.Location = new System.Drawing.Point(9, 17);
            pnLog.Name = "pnLog";
            pnLog.Size = new System.Drawing.Size(861, 272);
            pnLog.TabIndex = 3;
            // 
            // tbLog
            // 
            tbLog.Location = new System.Drawing.Point(10, 18);
            tbLog.Multiline = true;
            tbLog.Name = "tbLog";
            tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            tbLog.Size = new System.Drawing.Size(836, 239);
            tbLog.TabIndex = 0;
            // 
            // EasyOcrInstaller
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(888, 316);
            Controls.Add(pnMain);
            Controls.Add(pnLog);
            Controls.Add(pnEnableGPU);
            Name = "EasyOcrInstaller";
            Text = "EasyOcrInstaller";
            FormClosed += EasyOcrInstaller_FormClosed;
            pnMain.ResumeLayout(false);
            pnMain.PerformLayout();
            pnEnableGPU.ResumeLayout(false);
            pnEnableGPU.PerformLayout();
            pnLog.ResumeLayout(false);
            pnLog.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Button btnEnableGpuInstallStep;
        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.Panel pnEnableGPU;
        private System.Windows.Forms.Button btnEnableGpuInstall;
        private System.Windows.Forms.TextBox tbGpuLine;
        private System.Windows.Forms.Label lbGpuCommandLine;
        private System.Windows.Forms.Label lbCudaVersion;
        private System.Windows.Forms.ComboBox cbCuda;
        private System.Windows.Forms.RadioButton rbGpuCommandLine;
        private System.Windows.Forms.RadioButton rbCuda;
        private System.Windows.Forms.Panel pnLog;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.CheckBox cbForceInstall;
        private System.Windows.Forms.Button btnGuide;
    }
}