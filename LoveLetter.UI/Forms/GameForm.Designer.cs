namespace LoveLetter.UI.Forms
{
    partial class GameForm
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
            components = new System.ComponentModel.Container();
            GameSplitContainer = new SplitContainer();
            AdditionalCardPicture = new PictureBox();
            EndTurnBtn = new Button();
            PlayerValueLabel = new Label();
            PlayerValueValue = new NumericUpDown();
            PlayerNumberLabel = new Label();
            PlayerNumberValue = new NumericUpDown();
            DeckPicture = new PictureBox();
            InitialCardPicture = new PictureBox();
            AuditTabControl = new TabControl();
            AuditPage = new TabPage();
            CardHistoryPage = new TabPage();
            PollingTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)GameSplitContainer).BeginInit();
            GameSplitContainer.Panel1.SuspendLayout();
            GameSplitContainer.Panel2.SuspendLayout();
            GameSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AdditionalCardPicture).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PlayerValueValue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PlayerNumberValue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DeckPicture).BeginInit();
            ((System.ComponentModel.ISupportInitialize)InitialCardPicture).BeginInit();
            AuditTabControl.SuspendLayout();
            SuspendLayout();
            // 
            // GameSplitContainer
            // 
            GameSplitContainer.Dock = DockStyle.Fill;
            GameSplitContainer.Location = new Point(0, 0);
            GameSplitContainer.Name = "GameSplitContainer";
            // 
            // GameSplitContainer.Panel1
            // 
            GameSplitContainer.Panel1.Controls.Add(AdditionalCardPicture);
            GameSplitContainer.Panel1.Controls.Add(EndTurnBtn);
            GameSplitContainer.Panel1.Controls.Add(PlayerValueLabel);
            GameSplitContainer.Panel1.Controls.Add(PlayerValueValue);
            GameSplitContainer.Panel1.Controls.Add(PlayerNumberLabel);
            GameSplitContainer.Panel1.Controls.Add(PlayerNumberValue);
            GameSplitContainer.Panel1.Controls.Add(DeckPicture);
            GameSplitContainer.Panel1.Controls.Add(InitialCardPicture);
            // 
            // GameSplitContainer.Panel2
            // 
            GameSplitContainer.Panel2.Controls.Add(AuditTabControl);
            GameSplitContainer.Size = new Size(1266, 559);
            GameSplitContainer.SplitterDistance = 884;
            GameSplitContainer.TabIndex = 0;
            // 
            // AdditionalCardPicture
            // 
            AdditionalCardPicture.Location = new Point(481, 231);
            AdditionalCardPicture.Name = "AdditionalCardPicture";
            AdditionalCardPicture.Size = new Size(188, 316);
            AdditionalCardPicture.TabIndex = 7;
            AdditionalCardPicture.TabStop = false;
            AdditionalCardPicture.Click += AdditionalCardPicture_Click;
            // 
            // EndTurnBtn
            // 
            EndTurnBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            EndTurnBtn.Location = new Point(25, 358);
            EndTurnBtn.Name = "EndTurnBtn";
            EndTurnBtn.Size = new Size(203, 80);
            EndTurnBtn.TabIndex = 6;
            EndTurnBtn.Text = "End Turn";
            EndTurnBtn.UseVisualStyleBackColor = true;
            EndTurnBtn.Click += EndTurnBtn_Click;
            // 
            // PlayerValueLabel
            // 
            PlayerValueLabel.AutoSize = true;
            PlayerValueLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            PlayerValueLabel.Location = new Point(47, 126);
            PlayerValueLabel.Name = "PlayerValueLabel";
            PlayerValueLabel.Size = new Size(325, 32);
            PlayerValueLabel.TabIndex = 5;
            PlayerValueLabel.Text = "Enter player value(for Guard):";
            // 
            // PlayerValueValue
            // 
            PlayerValueValue.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            PlayerValueValue.Location = new Point(47, 170);
            PlayerValueValue.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            PlayerValueValue.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            PlayerValueValue.Name = "PlayerValueValue";
            PlayerValueValue.Size = new Size(120, 39);
            PlayerValueValue.TabIndex = 4;
            PlayerValueValue.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // PlayerNumberLabel
            // 
            PlayerNumberLabel.AutoSize = true;
            PlayerNumberLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            PlayerNumberLabel.Location = new Point(47, 23);
            PlayerNumberLabel.Name = "PlayerNumberLabel";
            PlayerNumberLabel.Size = new Size(237, 32);
            PlayerNumberLabel.TabIndex = 3;
            PlayerNumberLabel.Text = "Enter player number:";
            // 
            // PlayerNumberValue
            // 
            PlayerNumberValue.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            PlayerNumberValue.Location = new Point(47, 67);
            PlayerNumberValue.Maximum = new decimal(new int[] { 4, 0, 0, 0 });
            PlayerNumberValue.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            PlayerNumberValue.Name = "PlayerNumberValue";
            PlayerNumberValue.Size = new Size(120, 39);
            PlayerNumberValue.TabIndex = 2;
            PlayerNumberValue.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // DeckPicture
            // 
            DeckPicture.Location = new Point(530, 21);
            DeckPicture.Name = "DeckPicture";
            DeckPicture.Size = new Size(332, 137);
            DeckPicture.TabIndex = 1;
            DeckPicture.TabStop = false;
            // 
            // InitialCardPicture
            // 
            InitialCardPicture.Location = new Point(260, 231);
            InitialCardPicture.Name = "InitialCardPicture";
            InitialCardPicture.Size = new Size(188, 316);
            InitialCardPicture.TabIndex = 0;
            InitialCardPicture.TabStop = false;
            InitialCardPicture.Click += InitialCardPicture_Click;
            // 
            // AuditTabControl
            // 
            AuditTabControl.Controls.Add(AuditPage);
            AuditTabControl.Controls.Add(CardHistoryPage);
            AuditTabControl.Location = new Point(3, 3);
            AuditTabControl.Name = "AuditTabControl";
            AuditTabControl.SelectedIndex = 0;
            AuditTabControl.Size = new Size(372, 556);
            AuditTabControl.TabIndex = 0;
            // 
            // AuditPage
            // 
            AuditPage.Location = new Point(4, 34);
            AuditPage.Name = "AuditPage";
            AuditPage.Padding = new Padding(3);
            AuditPage.Size = new Size(364, 518);
            AuditPage.TabIndex = 0;
            AuditPage.Text = "Audit";
            AuditPage.UseVisualStyleBackColor = true;
            // 
            // CardHistoryPage
            // 
            CardHistoryPage.Location = new Point(4, 34);
            CardHistoryPage.Name = "CardHistoryPage";
            CardHistoryPage.Padding = new Padding(3);
            CardHistoryPage.Size = new Size(364, 518);
            CardHistoryPage.TabIndex = 1;
            CardHistoryPage.Text = "Card History";
            CardHistoryPage.UseVisualStyleBackColor = true;
            // 
            // PollingTimer
            // 
            PollingTimer.Enabled = true;
            PollingTimer.Interval = 10000;
            PollingTimer.Tick += PollingTimer_Tick;
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1266, 559);
            Controls.Add(GameSplitContainer);
            Name = "GameForm";
            Text = "GameForm";
            FormClosing += GameForm_FormClosing;
            FormClosed += GameForm_FormClosed;
            Load += GameForm_Load;
            GameSplitContainer.Panel1.ResumeLayout(false);
            GameSplitContainer.Panel1.PerformLayout();
            GameSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)GameSplitContainer).EndInit();
            GameSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)AdditionalCardPicture).EndInit();
            ((System.ComponentModel.ISupportInitialize)PlayerValueValue).EndInit();
            ((System.ComponentModel.ISupportInitialize)PlayerNumberValue).EndInit();
            ((System.ComponentModel.ISupportInitialize)DeckPicture).EndInit();
            ((System.ComponentModel.ISupportInitialize)InitialCardPicture).EndInit();
            AuditTabControl.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer GameSplitContainer;
        private PictureBox InitialCardPicture;
        private PictureBox DeckPicture;
        private NumericUpDown PlayerNumberValue;
        private Label PlayerValueLabel;
        private NumericUpDown PlayerValueValue;
        private Label PlayerNumberLabel;
        private Button EndTurnBtn;
        private PictureBox AdditionalCardPicture;
        private TabControl AuditTabControl;
        private TabPage AuditPage;
        private TabPage CardHistoryPage;
        private System.Windows.Forms.Timer PollingTimer;
    }
}