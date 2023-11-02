namespace LoveLetter.UI.Forms
{
    partial class LobbiesForm
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
            this.LobbiesGrid = new System.Windows.Forms.DataGridView();
            this.NicknameValue = new System.Windows.Forms.TextBox();
            this.NicknameLabel = new System.Windows.Forms.Label();
            this.CreateLobbyBtn = new System.Windows.Forms.Button();
            this.RefreshIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LobbiesGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RefreshIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // LobbiesGrid
            // 
            this.LobbiesGrid.AllowUserToAddRows = false;
            this.LobbiesGrid.AllowUserToDeleteRows = false;
            this.LobbiesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.LobbiesGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.LobbiesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LobbiesGrid.Location = new System.Drawing.Point(0, 74);
            this.LobbiesGrid.Margin = new System.Windows.Forms.Padding(2);
            this.LobbiesGrid.Name = "LobbiesGrid";
            this.LobbiesGrid.ReadOnly = true;
            this.LobbiesGrid.RowHeadersWidth = 62;
            this.LobbiesGrid.RowTemplate.Height = 33;
            this.LobbiesGrid.Size = new System.Drawing.Size(838, 378);
            this.LobbiesGrid.TabIndex = 0;
            this.LobbiesGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.LobbiesGrid_CellDoubleClick);
            // 
            // NicknameValue
            // 
            this.NicknameValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NicknameValue.Location = new System.Drawing.Point(10, 35);
            this.NicknameValue.Margin = new System.Windows.Forms.Padding(2);
            this.NicknameValue.Name = "NicknameValue";
            this.NicknameValue.Size = new System.Drawing.Size(447, 34);
            this.NicknameValue.TabIndex = 1;
            // 
            // NicknameLabel
            // 
            this.NicknameLabel.AutoSize = true;
            this.NicknameLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NicknameLabel.Location = new System.Drawing.Point(10, 7);
            this.NicknameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.NicknameLabel.Name = "NicknameLabel";
            this.NicknameLabel.Size = new System.Drawing.Size(149, 28);
            this.NicknameLabel.TabIndex = 2;
            this.NicknameLabel.Text = "Enter nickname:";
            // 
            // CreateLobbyBtn
            // 
            this.CreateLobbyBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CreateLobbyBtn.Location = new System.Drawing.Point(555, 7);
            this.CreateLobbyBtn.Margin = new System.Windows.Forms.Padding(2);
            this.CreateLobbyBtn.Name = "CreateLobbyBtn";
            this.CreateLobbyBtn.Size = new System.Drawing.Size(262, 57);
            this.CreateLobbyBtn.TabIndex = 3;
            this.CreateLobbyBtn.Text = "Create Lobby";
            this.CreateLobbyBtn.UseVisualStyleBackColor = true;
            this.CreateLobbyBtn.Click += new System.EventHandler(this.CreateLobbyBtn_Click);
            // 
            // RefreshIcon
            // 
            this.RefreshIcon.Image = global::LoveLetter.UI.Properties.Resources.reload;
            this.RefreshIcon.Location = new System.Drawing.Point(462, 35);
            this.RefreshIcon.Name = "RefreshIcon";
            this.RefreshIcon.Size = new System.Drawing.Size(36, 34);
            this.RefreshIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.RefreshIcon.TabIndex = 4;
            this.RefreshIcon.TabStop = false;
            this.RefreshIcon.Click += new System.EventHandler(this.RefreshIcon_Click);
            // 
            // LobbiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 447);
            this.Controls.Add(this.RefreshIcon);
            this.Controls.Add(this.CreateLobbyBtn);
            this.Controls.Add(this.NicknameLabel);
            this.Controls.Add(this.NicknameValue);
            this.Controls.Add(this.LobbiesGrid);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LobbiesForm";
            this.Text = "LobbiesForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LobbiesForm_FormClosing);
            this.Load += new System.EventHandler(this.LobbiesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LobbiesGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RefreshIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView LobbiesGrid;
        private TextBox NicknameValue;
        private Label NicknameLabel;
        private Button CreateLobbyBtn;
        private PictureBox RefreshIcon;
    }
}