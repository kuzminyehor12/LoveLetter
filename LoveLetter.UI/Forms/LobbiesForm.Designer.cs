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
            LobbiesGrid = new DataGridView();
            NicknameValue = new TextBox();
            NicknameLabel = new Label();
            CreateLobbyBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)LobbiesGrid).BeginInit();
            SuspendLayout();
            // 
            // LobbiesGrid
            // 
            LobbiesGrid.AllowUserToAddRows = false;
            LobbiesGrid.AllowUserToDeleteRows = false;
            LobbiesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            LobbiesGrid.Location = new Point(0, 92);
            LobbiesGrid.Name = "LobbiesGrid";
            LobbiesGrid.ReadOnly = true;
            LobbiesGrid.RowHeadersWidth = 62;
            LobbiesGrid.RowTemplate.Height = 33;
            LobbiesGrid.Size = new Size(1048, 472);
            LobbiesGrid.TabIndex = 0;
            LobbiesGrid.CellContentDoubleClick += LobbiesGrid_CellContentDoubleClick;
            LobbiesGrid.CellDoubleClick += LobbiesGrid_CellDoubleClick;
            // 
            // NicknameValue
            // 
            NicknameValue.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            NicknameValue.Location = new Point(12, 44);
            NicknameValue.Name = "NicknameValue";
            NicknameValue.Size = new Size(558, 39);
            NicknameValue.TabIndex = 1;
            // 
            // NicknameLabel
            // 
            NicknameLabel.AutoSize = true;
            NicknameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            NicknameLabel.Location = new Point(12, 9);
            NicknameLabel.Name = "NicknameLabel";
            NicknameLabel.Size = new Size(286, 32);
            NicknameLabel.TabIndex = 2;
            NicknameLabel.Text = "Enter nickname(optional):";
            // 
            // CreateLobbyBtn
            // 
            CreateLobbyBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            CreateLobbyBtn.Location = new Point(694, 9);
            CreateLobbyBtn.Name = "CreateLobbyBtn";
            CreateLobbyBtn.Size = new Size(327, 71);
            CreateLobbyBtn.TabIndex = 3;
            CreateLobbyBtn.Text = "Create Lobby";
            CreateLobbyBtn.UseVisualStyleBackColor = true;
            CreateLobbyBtn.Click += CreateLobbyBtn_Click;
            // 
            // LobbiesForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1043, 559);
            Controls.Add(CreateLobbyBtn);
            Controls.Add(NicknameLabel);
            Controls.Add(NicknameValue);
            Controls.Add(LobbiesGrid);
            Name = "LobbiesForm";
            Text = "LobbiesForm";
            Load += LobbiesForm_Load;
            ((System.ComponentModel.ISupportInitialize)LobbiesGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView LobbiesGrid;
        private TextBox NicknameValue;
        private Label NicknameLabel;
        private Button CreateLobbyBtn;
    }
}