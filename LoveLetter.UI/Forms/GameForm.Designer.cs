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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.GameSplitContainer = new System.Windows.Forms.SplitContainer();
            this.CardsLeftLabel = new System.Windows.Forms.Label();
            this.CardsCount = new System.Windows.Forms.Label();
            this.RefreshIcon = new System.Windows.Forms.PictureBox();
            this.AdditionalCardPicture = new System.Windows.Forms.PictureBox();
            this.EndTurnBtn = new System.Windows.Forms.Button();
            this.PlayerValueLabel = new System.Windows.Forms.Label();
            this.PlayerValueValue = new System.Windows.Forms.NumericUpDown();
            this.PlayerNumberLabel = new System.Windows.Forms.Label();
            this.PlayerNumberValue = new System.Windows.Forms.NumericUpDown();
            this.DeckPicture = new System.Windows.Forms.PictureBox();
            this.InitialCardPicture = new System.Windows.Forms.PictureBox();
            this.AuditTabControl = new System.Windows.Forms.TabControl();
            this.AuditPage = new System.Windows.Forms.TabPage();
            this.AuditGrid = new System.Windows.Forms.DataGridView();
            this.CardHistoryPage = new System.Windows.Forms.TabPage();
            this.CardHistoryListBox = new System.Windows.Forms.ListBox();
            this.Info = new System.Windows.Forms.TabPage();
            this.TurnPlayerNumberValue = new System.Windows.Forms.Label();
            this.TurnPlayerNumberLabel = new System.Windows.Forms.Label();
            this.YourNicknameValue = new System.Windows.Forms.Label();
            this.YourNicknameLabel = new System.Windows.Forms.Label();
            this.YourPlayerNumberValue = new System.Windows.Forms.Label();
            this.YourNumberLabel = new System.Windows.Forms.Label();
            this.PollingTimer = new System.Windows.Forms.Timer(this.components);
            this.AfkTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GameSplitContainer)).BeginInit();
            this.GameSplitContainer.Panel1.SuspendLayout();
            this.GameSplitContainer.Panel2.SuspendLayout();
            this.GameSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RefreshIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdditionalCardPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerValueValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerNumberValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeckPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InitialCardPicture)).BeginInit();
            this.AuditTabControl.SuspendLayout();
            this.AuditPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AuditGrid)).BeginInit();
            this.CardHistoryPage.SuspendLayout();
            this.Info.SuspendLayout();
            this.SuspendLayout();
            // 
            // GameSplitContainer
            // 
            this.GameSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GameSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.GameSplitContainer.Margin = new System.Windows.Forms.Padding(2);
            this.GameSplitContainer.Name = "GameSplitContainer";
            // 
            // GameSplitContainer.Panel1
            // 
            this.GameSplitContainer.Panel1.Controls.Add(this.CardsLeftLabel);
            this.GameSplitContainer.Panel1.Controls.Add(this.CardsCount);
            this.GameSplitContainer.Panel1.Controls.Add(this.RefreshIcon);
            this.GameSplitContainer.Panel1.Controls.Add(this.AdditionalCardPicture);
            this.GameSplitContainer.Panel1.Controls.Add(this.EndTurnBtn);
            this.GameSplitContainer.Panel1.Controls.Add(this.PlayerValueLabel);
            this.GameSplitContainer.Panel1.Controls.Add(this.PlayerValueValue);
            this.GameSplitContainer.Panel1.Controls.Add(this.PlayerNumberLabel);
            this.GameSplitContainer.Panel1.Controls.Add(this.PlayerNumberValue);
            this.GameSplitContainer.Panel1.Controls.Add(this.DeckPicture);
            this.GameSplitContainer.Panel1.Controls.Add(this.InitialCardPicture);
            // 
            // GameSplitContainer.Panel2
            // 
            this.GameSplitContainer.Panel2.Controls.Add(this.AuditTabControl);
            this.GameSplitContainer.Size = new System.Drawing.Size(1105, 447);
            this.GameSplitContainer.SplitterDistance = 709;
            this.GameSplitContainer.SplitterWidth = 3;
            this.GameSplitContainer.TabIndex = 0;
            // 
            // CardsLeftLabel
            // 
            this.CardsLeftLabel.AutoSize = true;
            this.CardsLeftLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CardsLeftLabel.Location = new System.Drawing.Point(458, 9);
            this.CardsLeftLabel.Name = "CardsLeftLabel";
            this.CardsLeftLabel.Size = new System.Drawing.Size(98, 28);
            this.CardsLeftLabel.TabIndex = 10;
            this.CardsLeftLabel.Text = "Cards left:";
            // 
            // CardsCount
            // 
            this.CardsCount.AutoSize = true;
            this.CardsCount.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CardsCount.Location = new System.Drawing.Point(562, 9);
            this.CardsCount.Name = "CardsCount";
            this.CardsCount.Size = new System.Drawing.Size(23, 28);
            this.CardsCount.TabIndex = 9;
            this.CardsCount.Text = "0";
            // 
            // RefreshIcon
            // 
            this.RefreshIcon.Image = global::LoveLetter.UI.Properties.Resources.reload;
            this.RefreshIcon.Location = new System.Drawing.Point(669, 3);
            this.RefreshIcon.Name = "RefreshIcon";
            this.RefreshIcon.Size = new System.Drawing.Size(38, 34);
            this.RefreshIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.RefreshIcon.TabIndex = 8;
            this.RefreshIcon.TabStop = false;
            this.RefreshIcon.Click += new System.EventHandler(this.RefreshIcon_Click);
            // 
            // AdditionalCardPicture
            // 
            this.AdditionalCardPicture.Location = new System.Drawing.Point(385, 185);
            this.AdditionalCardPicture.Margin = new System.Windows.Forms.Padding(2);
            this.AdditionalCardPicture.Name = "AdditionalCardPicture";
            this.AdditionalCardPicture.Size = new System.Drawing.Size(150, 218);
            this.AdditionalCardPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.AdditionalCardPicture.TabIndex = 7;
            this.AdditionalCardPicture.TabStop = false;
            this.AdditionalCardPicture.Click += new System.EventHandler(this.AdditionalCardPicture_Click);
            // 
            // EndTurnBtn
            // 
            this.EndTurnBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.EndTurnBtn.Location = new System.Drawing.Point(20, 286);
            this.EndTurnBtn.Margin = new System.Windows.Forms.Padding(2);
            this.EndTurnBtn.Name = "EndTurnBtn";
            this.EndTurnBtn.Size = new System.Drawing.Size(162, 64);
            this.EndTurnBtn.TabIndex = 6;
            this.EndTurnBtn.Text = "End Turn";
            this.EndTurnBtn.UseVisualStyleBackColor = true;
            this.EndTurnBtn.Click += new System.EventHandler(this.EndTurnBtn_Click);
            // 
            // PlayerValueLabel
            // 
            this.PlayerValueLabel.AutoSize = true;
            this.PlayerValueLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayerValueLabel.Location = new System.Drawing.Point(38, 101);
            this.PlayerValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PlayerValueLabel.Name = "PlayerValueLabel";
            this.PlayerValueLabel.Size = new System.Drawing.Size(267, 28);
            this.PlayerValueLabel.TabIndex = 5;
            this.PlayerValueLabel.Text = "Enter player value(for Guard):";
            // 
            // PlayerValueValue
            // 
            this.PlayerValueValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayerValueValue.Location = new System.Drawing.Point(38, 136);
            this.PlayerValueValue.Margin = new System.Windows.Forms.Padding(2);
            this.PlayerValueValue.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.PlayerValueValue.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.PlayerValueValue.Name = "PlayerValueValue";
            this.PlayerValueValue.Size = new System.Drawing.Size(96, 34);
            this.PlayerValueValue.TabIndex = 4;
            this.PlayerValueValue.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.PlayerValueValue.ValueChanged += new System.EventHandler(this.PlayerValueValue_ValueChanged);
            // 
            // PlayerNumberLabel
            // 
            this.PlayerNumberLabel.AutoSize = true;
            this.PlayerNumberLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayerNumberLabel.Location = new System.Drawing.Point(38, 18);
            this.PlayerNumberLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PlayerNumberLabel.Name = "PlayerNumberLabel";
            this.PlayerNumberLabel.Size = new System.Drawing.Size(193, 28);
            this.PlayerNumberLabel.TabIndex = 3;
            this.PlayerNumberLabel.Text = "Enter player number:";
            // 
            // PlayerNumberValue
            // 
            this.PlayerNumberValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayerNumberValue.Location = new System.Drawing.Point(38, 54);
            this.PlayerNumberValue.Margin = new System.Windows.Forms.Padding(2);
            this.PlayerNumberValue.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.PlayerNumberValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PlayerNumberValue.Name = "PlayerNumberValue";
            this.PlayerNumberValue.Size = new System.Drawing.Size(96, 34);
            this.PlayerNumberValue.TabIndex = 2;
            this.PlayerNumberValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PlayerNumberValue.ValueChanged += new System.EventHandler(this.PlayerNumberValue_ValueChanged);
            // 
            // DeckPicture
            // 
            this.DeckPicture.Image = global::LoveLetter.UI.Properties.Resources.Backside;
            this.DeckPicture.Location = new System.Drawing.Point(439, 42);
            this.DeckPicture.Margin = new System.Windows.Forms.Padding(2);
            this.DeckPicture.Name = "DeckPicture";
            this.DeckPicture.Size = new System.Drawing.Size(266, 128);
            this.DeckPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.DeckPicture.TabIndex = 1;
            this.DeckPicture.TabStop = false;
            // 
            // InitialCardPicture
            // 
            this.InitialCardPicture.Image = global::LoveLetter.UI.Properties.Resources.Guard;
            this.InitialCardPicture.Location = new System.Drawing.Point(208, 185);
            this.InitialCardPicture.Margin = new System.Windows.Forms.Padding(2);
            this.InitialCardPicture.Name = "InitialCardPicture";
            this.InitialCardPicture.Size = new System.Drawing.Size(150, 218);
            this.InitialCardPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.InitialCardPicture.TabIndex = 0;
            this.InitialCardPicture.TabStop = false;
            this.InitialCardPicture.Click += new System.EventHandler(this.InitialCardPicture_Click);
            // 
            // AuditTabControl
            // 
            this.AuditTabControl.Controls.Add(this.AuditPage);
            this.AuditTabControl.Controls.Add(this.CardHistoryPage);
            this.AuditTabControl.Controls.Add(this.Info);
            this.AuditTabControl.Location = new System.Drawing.Point(2, 2);
            this.AuditTabControl.Margin = new System.Windows.Forms.Padding(2);
            this.AuditTabControl.Name = "AuditTabControl";
            this.AuditTabControl.SelectedIndex = 0;
            this.AuditTabControl.Size = new System.Drawing.Size(391, 445);
            this.AuditTabControl.TabIndex = 0;
            // 
            // AuditPage
            // 
            this.AuditPage.Controls.Add(this.AuditGrid);
            this.AuditPage.Location = new System.Drawing.Point(4, 29);
            this.AuditPage.Margin = new System.Windows.Forms.Padding(2);
            this.AuditPage.Name = "AuditPage";
            this.AuditPage.Padding = new System.Windows.Forms.Padding(2);
            this.AuditPage.Size = new System.Drawing.Size(383, 412);
            this.AuditPage.TabIndex = 0;
            this.AuditPage.Text = "Audit";
            this.AuditPage.UseVisualStyleBackColor = true;
            // 
            // AuditGrid
            // 
            this.AuditGrid.AllowUserToAddRows = false;
            this.AuditGrid.AllowUserToDeleteRows = false;
            this.AuditGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.AuditGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.AuditGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.AuditGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.AuditGrid.Location = new System.Drawing.Point(4, 2);
            this.AuditGrid.Name = "AuditGrid";
            this.AuditGrid.ReadOnly = true;
            this.AuditGrid.RowHeadersWidth = 51;
            this.AuditGrid.RowTemplate.Height = 29;
            this.AuditGrid.Size = new System.Drawing.Size(383, 407);
            this.AuditGrid.TabIndex = 0;
            // 
            // CardHistoryPage
            // 
            this.CardHistoryPage.Controls.Add(this.CardHistoryListBox);
            this.CardHistoryPage.Location = new System.Drawing.Point(4, 29);
            this.CardHistoryPage.Margin = new System.Windows.Forms.Padding(2);
            this.CardHistoryPage.Name = "CardHistoryPage";
            this.CardHistoryPage.Padding = new System.Windows.Forms.Padding(2);
            this.CardHistoryPage.Size = new System.Drawing.Size(383, 412);
            this.CardHistoryPage.TabIndex = 1;
            this.CardHistoryPage.Text = "Card History";
            this.CardHistoryPage.UseVisualStyleBackColor = true;
            // 
            // CardHistoryListBox
            // 
            this.CardHistoryListBox.FormattingEnabled = true;
            this.CardHistoryListBox.ItemHeight = 20;
            this.CardHistoryListBox.Location = new System.Drawing.Point(3, 3);
            this.CardHistoryListBox.Name = "CardHistoryListBox";
            this.CardHistoryListBox.Size = new System.Drawing.Size(375, 404);
            this.CardHistoryListBox.TabIndex = 0;
            // 
            // Info
            // 
            this.Info.Controls.Add(this.TurnPlayerNumberValue);
            this.Info.Controls.Add(this.TurnPlayerNumberLabel);
            this.Info.Controls.Add(this.YourNicknameValue);
            this.Info.Controls.Add(this.YourNicknameLabel);
            this.Info.Controls.Add(this.YourPlayerNumberValue);
            this.Info.Controls.Add(this.YourNumberLabel);
            this.Info.Location = new System.Drawing.Point(4, 29);
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(383, 412);
            this.Info.TabIndex = 2;
            this.Info.Text = "Your Info";
            this.Info.UseVisualStyleBackColor = true;
            // 
            // TurnPlayerNumberValue
            // 
            this.TurnPlayerNumberValue.AutoSize = true;
            this.TurnPlayerNumberValue.Location = new System.Drawing.Point(147, 54);
            this.TurnPlayerNumberValue.Name = "TurnPlayerNumberValue";
            this.TurnPlayerNumberValue.Size = new System.Drawing.Size(0, 20);
            this.TurnPlayerNumberValue.TabIndex = 5;
            // 
            // TurnPlayerNumberLabel
            // 
            this.TurnPlayerNumberLabel.AutoSize = true;
            this.TurnPlayerNumberLabel.Location = new System.Drawing.Point(5, 54);
            this.TurnPlayerNumberLabel.Name = "TurnPlayerNumberLabel";
            this.TurnPlayerNumberLabel.Size = new System.Drawing.Size(143, 20);
            this.TurnPlayerNumberLabel.TabIndex = 4;
            this.TurnPlayerNumberLabel.Text = "Turn Player Number:";
            // 
            // YourNicknameValue
            // 
            this.YourNicknameValue.AutoSize = true;
            this.YourNicknameValue.Location = new System.Drawing.Point(5, 119);
            this.YourNicknameValue.Name = "YourNicknameValue";
            this.YourNicknameValue.Size = new System.Drawing.Size(49, 20);
            this.YourNicknameValue.TabIndex = 3;
            this.YourNicknameValue.Text = "Player";
            // 
            // YourNicknameLabel
            // 
            this.YourNicknameLabel.AutoSize = true;
            this.YourNicknameLabel.Location = new System.Drawing.Point(5, 94);
            this.YourNicknameLabel.Name = "YourNicknameLabel";
            this.YourNicknameLabel.Size = new System.Drawing.Size(111, 20);
            this.YourNicknameLabel.TabIndex = 2;
            this.YourNicknameLabel.Text = "Your Nickname:";
            // 
            // YourPlayerNumberValue
            // 
            this.YourPlayerNumberValue.AutoSize = true;
            this.YourPlayerNumberValue.Location = new System.Drawing.Point(147, 13);
            this.YourPlayerNumberValue.Name = "YourPlayerNumberValue";
            this.YourPlayerNumberValue.Size = new System.Drawing.Size(0, 20);
            this.YourPlayerNumberValue.TabIndex = 1;
            // 
            // YourNumberLabel
            // 
            this.YourNumberLabel.AutoSize = true;
            this.YourNumberLabel.Location = new System.Drawing.Point(5, 13);
            this.YourNumberLabel.Name = "YourNumberLabel";
            this.YourNumberLabel.Size = new System.Drawing.Size(99, 20);
            this.YourNumberLabel.TabIndex = 0;
            this.YourNumberLabel.Text = "Your Number:";
            // 
            // PollingTimer
            // 
            this.PollingTimer.Interval = 5000;
            this.PollingTimer.Tick += new System.EventHandler(this.PollingTimer_Tick);
            // 
            // AfkTimer
            // 
            this.AfkTimer.Interval = 300000;
            this.AfkTimer.Tick += new System.EventHandler(this.AfkTimer_Tick);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 447);
            this.Controls.Add(this.GameSplitContainer);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GameForm";
            this.Text = "GameForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameForm_FormClosed);
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.GameSplitContainer.Panel1.ResumeLayout(false);
            this.GameSplitContainer.Panel1.PerformLayout();
            this.GameSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GameSplitContainer)).EndInit();
            this.GameSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RefreshIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdditionalCardPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerValueValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerNumberValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeckPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InitialCardPicture)).EndInit();
            this.AuditTabControl.ResumeLayout(false);
            this.AuditPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AuditGrid)).EndInit();
            this.CardHistoryPage.ResumeLayout(false);
            this.Info.ResumeLayout(false);
            this.Info.PerformLayout();
            this.ResumeLayout(false);

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
        private DataGridView AuditGrid;
        private TabPage Info;
        private Label TurnPlayerNumberValue;
        private Label TurnPlayerNumberLabel;
        private Label YourNicknameValue;
        private Label YourNicknameLabel;
        private Label YourPlayerNumberValue;
        private Label YourNumberLabel;
        private System.Windows.Forms.Timer AfkTimer;
        private PictureBox RefreshIcon;
        private Label CardsCount;
        private ListBox CardHistoryListBox;
        private Label CardsLeftLabel;
    }
}