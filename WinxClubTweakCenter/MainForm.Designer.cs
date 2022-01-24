namespace WinxClubTweakCenter
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonOpen = new System.Windows.Forms.Button();
            this.decorGameFolder = new System.Windows.Forms.Label();
            this.containerTweaks = new System.Windows.Forms.GroupBox();
            this.checkBoxResolution = new System.Windows.Forms.CheckBox();
            this.inputResolution = new System.Windows.Forms.ComboBox();
            this.checkBoxFirstPersonMovement = new System.Windows.Forms.CheckBox();
            this.checkBoxFirstPersonAnywhere = new System.Windows.Forms.CheckBox();
            this.checkBoxLookingUp = new System.Windows.Forms.CheckBox();
            this.checkBoxSkipLogos = new System.Windows.Forms.CheckBox();
            this.checkBoxStarChallenges = new System.Windows.Forms.CheckBox();
            this.checkBoxFlight = new System.Windows.Forms.CheckBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.linkGitHub = new System.Windows.Forms.LinkLabel();
            this.containerTweaks.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(12, 12);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(100, 25);
            this.buttonOpen.TabIndex = 0;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.ButtonOpen_Click);
            // 
            // decorGameFolder
            // 
            this.decorGameFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.decorGameFolder.AutoEllipsis = true;
            this.decorGameFolder.Location = new System.Drawing.Point(118, 18);
            this.decorGameFolder.Name = "decorGameFolder";
            this.decorGameFolder.Size = new System.Drawing.Size(204, 13);
            this.decorGameFolder.TabIndex = 0;
            // 
            // containerTweaks
            // 
            this.containerTweaks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.containerTweaks.Controls.Add(this.checkBoxResolution);
            this.containerTweaks.Controls.Add(this.inputResolution);
            this.containerTweaks.Controls.Add(this.checkBoxFirstPersonMovement);
            this.containerTweaks.Controls.Add(this.checkBoxFirstPersonAnywhere);
            this.containerTweaks.Controls.Add(this.checkBoxLookingUp);
            this.containerTweaks.Controls.Add(this.checkBoxSkipLogos);
            this.containerTweaks.Controls.Add(this.checkBoxStarChallenges);
            this.containerTweaks.Controls.Add(this.checkBoxFlight);
            this.containerTweaks.Enabled = false;
            this.containerTweaks.Location = new System.Drawing.Point(12, 43);
            this.containerTweaks.Name = "containerTweaks";
            this.containerTweaks.Size = new System.Drawing.Size(310, 182);
            this.containerTweaks.TabIndex = 1;
            this.containerTweaks.TabStop = false;
            this.containerTweaks.Text = "Tweaks";
            // 
            // checkBoxResolution
            // 
            this.checkBoxResolution.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxResolution.AutoSize = true;
            this.checkBoxResolution.Location = new System.Drawing.Point(6, 21);
            this.checkBoxResolution.Name = "checkBoxResolution";
            this.checkBoxResolution.Size = new System.Drawing.Size(90, 17);
            this.checkBoxResolution.TabIndex = 0;
            this.checkBoxResolution.Text = "Set resolution";
            this.checkBoxResolution.UseVisualStyleBackColor = true;
            this.checkBoxResolution.CheckedChanged += new System.EventHandler(this.CheckBoxResolution_CheckedChanged);
            // 
            // inputResolution
            // 
            this.inputResolution.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.inputResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputResolution.Enabled = false;
            this.inputResolution.FormattingEnabled = true;
            this.inputResolution.Location = new System.Drawing.Point(198, 19);
            this.inputResolution.Name = "inputResolution";
            this.inputResolution.Size = new System.Drawing.Size(106, 21);
            this.inputResolution.TabIndex = 1;
            this.inputResolution.SelectedIndexChanged += new System.EventHandler(this.InputResolution_SelectedIndexChanged);
            // 
            // checkBoxFirstPersonMovement
            // 
            this.checkBoxFirstPersonMovement.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxFirstPersonMovement.AutoSize = true;
            this.checkBoxFirstPersonMovement.Location = new System.Drawing.Point(6, 44);
            this.checkBoxFirstPersonMovement.Name = "checkBoxFirstPersonMovement";
            this.checkBoxFirstPersonMovement.Size = new System.Drawing.Size(195, 17);
            this.checkBoxFirstPersonMovement.TabIndex = 2;
            this.checkBoxFirstPersonMovement.Text = "Allow first person camera movement";
            this.checkBoxFirstPersonMovement.UseVisualStyleBackColor = true;
            // 
            // checkBoxFirstPersonAnywhere
            // 
            this.checkBoxFirstPersonAnywhere.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxFirstPersonAnywhere.AutoSize = true;
            this.checkBoxFirstPersonAnywhere.Location = new System.Drawing.Point(6, 67);
            this.checkBoxFirstPersonAnywhere.Name = "checkBoxFirstPersonAnywhere";
            this.checkBoxFirstPersonAnywhere.Size = new System.Drawing.Size(192, 17);
            this.checkBoxFirstPersonAnywhere.TabIndex = 3;
            this.checkBoxFirstPersonAnywhere.Text = "Allow first person camera anywhere";
            this.checkBoxFirstPersonAnywhere.UseVisualStyleBackColor = true;
            // 
            // checkBoxLookingUp
            // 
            this.checkBoxLookingUp.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxLookingUp.AutoSize = true;
            this.checkBoxLookingUp.Location = new System.Drawing.Point(6, 90);
            this.checkBoxLookingUp.Name = "checkBoxLookingUp";
            this.checkBoxLookingUp.Size = new System.Drawing.Size(103, 17);
            this.checkBoxLookingUp.TabIndex = 4;
            this.checkBoxLookingUp.Text = "Allow looking up";
            this.checkBoxLookingUp.UseVisualStyleBackColor = true;
            // 
            // checkBoxSkipLogos
            // 
            this.checkBoxSkipLogos.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxSkipLogos.AutoSize = true;
            this.checkBoxSkipLogos.Location = new System.Drawing.Point(6, 113);
            this.checkBoxSkipLogos.Name = "checkBoxSkipLogos";
            this.checkBoxSkipLogos.Size = new System.Drawing.Size(113, 17);
            this.checkBoxSkipLogos.TabIndex = 5;
            this.checkBoxSkipLogos.Text = "Skip logos on start";
            this.checkBoxSkipLogos.UseVisualStyleBackColor = true;
            // 
            // checkBoxStarChallenges
            // 
            this.checkBoxStarChallenges.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxStarChallenges.AutoSize = true;
            this.checkBoxStarChallenges.Location = new System.Drawing.Point(6, 136);
            this.checkBoxStarChallenges.Name = "checkBoxStarChallenges";
            this.checkBoxStarChallenges.Size = new System.Drawing.Size(152, 17);
            this.checkBoxStarChallenges.TabIndex = 6;
            this.checkBoxStarChallenges.Text = "Add time in star challenges";
            this.checkBoxStarChallenges.UseVisualStyleBackColor = true;
            // 
            // checkBoxFlight
            // 
            this.checkBoxFlight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxFlight.AutoSize = true;
            this.checkBoxFlight.Location = new System.Drawing.Point(6, 159);
            this.checkBoxFlight.Name = "checkBoxFlight";
            this.checkBoxFlight.Size = new System.Drawing.Size(113, 17);
            this.checkBoxFlight.TabIndex = 99;
            this.checkBoxFlight.Text = "Enable flight ability";
            this.checkBoxFlight.UseVisualStyleBackColor = true;
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Enabled = false;
            this.buttonApply.Location = new System.Drawing.Point(222, 231);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(100, 25);
            this.buttonApply.TabIndex = 997;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.ButtonApply_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonHelp.Location = new System.Drawing.Point(12, 231);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(100, 25);
            this.buttonHelp.TabIndex = 998;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.ButtonHelp_Click);
            // 
            // linkGitHub
            // 
            this.linkGitHub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkGitHub.AutoSize = true;
            this.linkGitHub.Location = new System.Drawing.Point(12, 259);
            this.linkGitHub.Name = "linkGitHub";
            this.linkGitHub.Size = new System.Drawing.Size(67, 13);
            this.linkGitHub.TabIndex = 999;
            this.linkGitHub.TabStop = true;
            this.linkGitHub.Text = "GitHub page";
            this.linkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkGitHub_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 281);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.decorGameFolder);
            this.Controls.Add(this.containerTweaks);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.linkGitHub);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 320);
            this.MinimumSize = new System.Drawing.Size(350, 320);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Winx Club Tweak Center";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.containerTweaks.ResumeLayout(false);
            this.containerTweaks.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Label decorGameFolder;
        private System.Windows.Forms.GroupBox containerTweaks;
        private System.Windows.Forms.CheckBox checkBoxResolution;
        private System.Windows.Forms.ComboBox inputResolution;
        private System.Windows.Forms.CheckBox checkBoxFirstPersonMovement;
        private System.Windows.Forms.CheckBox checkBoxFirstPersonAnywhere;
        private System.Windows.Forms.CheckBox checkBoxLookingUp;
        private System.Windows.Forms.CheckBox checkBoxSkipLogos;
        private System.Windows.Forms.CheckBox checkBoxStarChallenges;
        private System.Windows.Forms.CheckBox checkBoxFlight;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.LinkLabel linkGitHub;
    }
}

