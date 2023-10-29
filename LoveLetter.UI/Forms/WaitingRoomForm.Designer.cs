namespace LoveLetter.UI.Forms
{
    partial class WaitingRoomForm
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
            this.GameStartBtn = new System.Windows.Forms.Button();
            this.QuitLobbyBtn = new System.Windows.Forms.Button();
            this.PollingTimer = new System.Windows.Forms.Timer(this.components);
            this.WaitingRoomListBox = new System.Windows.Forms.ListBox();
            this.GameStartTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // GameStartBtn
            // 
            this.GameStartBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GameStartBtn.Location = new System.Drawing.Point(493, 347);
            this.GameStartBtn.Margin = new System.Windows.Forms.Padding(2);
            this.GameStartBtn.Name = "GameStartBtn";
            this.GameStartBtn.Size = new System.Drawing.Size(254, 73);
            this.GameStartBtn.TabIndex = 1;
            this.GameStartBtn.Text = "Start Game";
            this.GameStartBtn.UseVisualStyleBackColor = true;
            this.GameStartBtn.Click += new System.EventHandler(this.GameStartBtn_Click);
            // 
            // QuitLobbyBtn
            // 
            this.QuitLobbyBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.QuitLobbyBtn.Location = new System.Drawing.Point(94, 347);
            this.QuitLobbyBtn.Margin = new System.Windows.Forms.Padding(2);
            this.QuitLobbyBtn.Name = "QuitLobbyBtn";
            this.QuitLobbyBtn.Size = new System.Drawing.Size(254, 73);
            this.QuitLobbyBtn.TabIndex = 2;
            this.QuitLobbyBtn.Text = "Quit";
            this.QuitLobbyBtn.UseVisualStyleBackColor = true;
            this.QuitLobbyBtn.Click += new System.EventHandler(this.QuitLobbyBtn_Click);
            // 
            // PollingTimer
            // 
            this.PollingTimer.Enabled = true;
            this.PollingTimer.Interval = 5000;
            this.PollingTimer.Tick += new System.EventHandler(this.PollingTimer_Tick);
            // 
            // WaitingRoomListBox
            // 
            this.WaitingRoomListBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.WaitingRoomListBox.FormattingEnabled = true;
            this.WaitingRoomListBox.ItemHeight = 28;
            this.WaitingRoomListBox.Location = new System.Drawing.Point(94, 38);
            this.WaitingRoomListBox.Name = "WaitingRoomListBox";
            this.WaitingRoomListBox.Size = new System.Drawing.Size(653, 284);
            this.WaitingRoomListBox.TabIndex = 3;
            // 
            // GameStartTimer
            // 
            this.GameStartTimer.Enabled = true;
            this.GameStartTimer.Interval = 5000;
            this.GameStartTimer.Tick += new System.EventHandler(this.GameStartTimer_Tick);
            // 
            // WaitingRoomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 447);
            this.Controls.Add(this.WaitingRoomListBox);
            this.Controls.Add(this.QuitLobbyBtn);
            this.Controls.Add(this.GameStartBtn);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "WaitingRoomForm";
            this.Text = "WaitingRoomForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WaitingRoomForm_FormClosing);
            this.Load += new System.EventHandler(this.WaitingRoomForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Button GameStartBtn;
        private Button QuitLobbyBtn;
        private System.Windows.Forms.Timer PollingTimer;
        private ListBox WaitingRoomListBox;
        private System.Windows.Forms.Timer GameStartTimer;
    }
}