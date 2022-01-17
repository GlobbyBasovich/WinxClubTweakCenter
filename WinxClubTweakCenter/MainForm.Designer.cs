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
            this.buttonApply = new System.Windows.Forms.Button();
            this.checkBoxResolution = new System.Windows.Forms.CheckBox();
            this.checkBoxFirstPersonMovement = new System.Windows.Forms.CheckBox();
            this.checkBoxFirstPersonAnywhere = new System.Windows.Forms.CheckBox();
            this.checkBoxFlight = new System.Windows.Forms.CheckBox();
            this.inputResolution = new System.Windows.Forms.ComboBox();
            this.checkBoxLookingUp = new System.Windows.Forms.CheckBox();
            this.checkBoxSkipLogos = new System.Windows.Forms.CheckBox();
            this.checkBoxStarChallenges = new System.Windows.Forms.CheckBox();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Location = new System.Drawing.Point(212, 173);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(100, 25);
            this.buttonApply.TabIndex = 0;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.ButtonApply_Click);
            // 
            // checkBoxResolution
            // 
            this.checkBoxResolution.AutoSize = true;
            this.checkBoxResolution.Location = new System.Drawing.Point(12, 12);
            this.checkBoxResolution.Name = "checkBoxResolution";
            this.checkBoxResolution.Size = new System.Drawing.Size(90, 17);
            this.checkBoxResolution.TabIndex = 2;
            this.checkBoxResolution.Text = "Set resolution";
            this.checkBoxResolution.UseVisualStyleBackColor = true;
            this.checkBoxResolution.CheckedChanged += new System.EventHandler(this.CheckBoxResolution_CheckedChanged);
            // 
            // checkBoxFirstPersonMovement
            // 
            this.checkBoxFirstPersonMovement.AutoSize = true;
            this.checkBoxFirstPersonMovement.Location = new System.Drawing.Point(12, 35);
            this.checkBoxFirstPersonMovement.Name = "checkBoxFirstPersonMovement";
            this.checkBoxFirstPersonMovement.Size = new System.Drawing.Size(195, 17);
            this.checkBoxFirstPersonMovement.TabIndex = 4;
            this.checkBoxFirstPersonMovement.Text = "Allow first person camera movement";
            this.checkBoxFirstPersonMovement.UseVisualStyleBackColor = true;
            // 
            // checkBoxFirstPersonAnywhere
            // 
            this.checkBoxFirstPersonAnywhere.AutoSize = true;
            this.checkBoxFirstPersonAnywhere.Location = new System.Drawing.Point(12, 58);
            this.checkBoxFirstPersonAnywhere.Name = "checkBoxFirstPersonAnywhere";
            this.checkBoxFirstPersonAnywhere.Size = new System.Drawing.Size(192, 17);
            this.checkBoxFirstPersonAnywhere.TabIndex = 5;
            this.checkBoxFirstPersonAnywhere.Text = "Allow first person camera anywhere";
            this.checkBoxFirstPersonAnywhere.UseVisualStyleBackColor = true;
            // 
            // checkBoxFlight
            // 
            this.checkBoxFlight.AutoSize = true;
            this.checkBoxFlight.Location = new System.Drawing.Point(12, 150);
            this.checkBoxFlight.Name = "checkBoxFlight";
            this.checkBoxFlight.Size = new System.Drawing.Size(113, 17);
            this.checkBoxFlight.TabIndex = 9;
            this.checkBoxFlight.Text = "Enable flight ability";
            this.checkBoxFlight.UseVisualStyleBackColor = true;
            // 
            // inputResolution
            // 
            this.inputResolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.inputResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputResolution.Enabled = false;
            this.inputResolution.FormattingEnabled = true;
            this.inputResolution.Location = new System.Drawing.Point(206, 10);
            this.inputResolution.Name = "inputResolution";
            this.inputResolution.Size = new System.Drawing.Size(106, 21);
            this.inputResolution.TabIndex = 3;
            // 
            // checkBoxLookingUp
            // 
            this.checkBoxLookingUp.AutoSize = true;
            this.checkBoxLookingUp.Location = new System.Drawing.Point(12, 81);
            this.checkBoxLookingUp.Name = "checkBoxLookingUp";
            this.checkBoxLookingUp.Size = new System.Drawing.Size(103, 17);
            this.checkBoxLookingUp.TabIndex = 6;
            this.checkBoxLookingUp.Text = "Allow looking up";
            this.checkBoxLookingUp.UseVisualStyleBackColor = true;
            // 
            // checkBoxSkipLogos
            // 
            this.checkBoxSkipLogos.AutoSize = true;
            this.checkBoxSkipLogos.Location = new System.Drawing.Point(12, 104);
            this.checkBoxSkipLogos.Name = "checkBoxSkipLogos";
            this.checkBoxSkipLogos.Size = new System.Drawing.Size(113, 17);
            this.checkBoxSkipLogos.TabIndex = 7;
            this.checkBoxSkipLogos.Text = "Skip logos on start";
            this.checkBoxSkipLogos.UseVisualStyleBackColor = true;
            // 
            // checkBoxStarChallenges
            // 
            this.checkBoxStarChallenges.AutoSize = true;
            this.checkBoxStarChallenges.Location = new System.Drawing.Point(12, 127);
            this.checkBoxStarChallenges.Name = "checkBoxStarChallenges";
            this.checkBoxStarChallenges.Size = new System.Drawing.Size(152, 17);
            this.checkBoxStarChallenges.TabIndex = 8;
            this.checkBoxStarChallenges.Text = "Add time in star challenges";
            this.checkBoxStarChallenges.UseVisualStyleBackColor = true;
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonHelp.Location = new System.Drawing.Point(12, 173);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(100, 25);
            this.buttonHelp.TabIndex = 1;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.ButtonHelp_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 210);
            this.Controls.Add(this.checkBoxFlight);
            this.Controls.Add(this.checkBoxStarChallenges);
            this.Controls.Add(this.checkBoxSkipLogos);
            this.Controls.Add(this.checkBoxLookingUp);
            this.Controls.Add(this.checkBoxFirstPersonAnywhere);
            this.Controls.Add(this.checkBoxFirstPersonMovement);
            this.Controls.Add(this.inputResolution);
            this.Controls.Add(this.checkBoxResolution);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonApply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Winx Club Tweak Center";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.CheckBox checkBoxResolution;
        private System.Windows.Forms.ComboBox inputResolution;
        private System.Windows.Forms.CheckBox checkBoxFirstPersonMovement;
        private System.Windows.Forms.CheckBox checkBoxFirstPersonAnywhere;
        private System.Windows.Forms.CheckBox checkBoxLookingUp;
        private System.Windows.Forms.CheckBox checkBoxSkipLogos;
        private System.Windows.Forms.CheckBox checkBoxStarChallenges;
        private System.Windows.Forms.CheckBox checkBoxFlight;
    }
}

