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
            WaitingRoomGrid = new DataGridView();
            GameStartBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)WaitingRoomGrid).BeginInit();
            SuspendLayout();
            // 
            // WaitingRoomGrid
            // 
            WaitingRoomGrid.AllowUserToAddRows = false;
            WaitingRoomGrid.AllowUserToDeleteRows = false;
            WaitingRoomGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            WaitingRoomGrid.Location = new Point(1, 0);
            WaitingRoomGrid.Name = "WaitingRoomGrid";
            WaitingRoomGrid.ReadOnly = true;
            WaitingRoomGrid.RowHeadersWidth = 62;
            WaitingRoomGrid.RowTemplate.Height = 33;
            WaitingRoomGrid.Size = new Size(1039, 397);
            WaitingRoomGrid.TabIndex = 0;
            // 
            // GameStartBtn
            // 
            GameStartBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            GameStartBtn.Location = new Point(361, 434);
            GameStartBtn.Name = "GameStartBtn";
            GameStartBtn.Size = new Size(317, 91);
            GameStartBtn.TabIndex = 1;
            GameStartBtn.Text = "Start Game";
            GameStartBtn.UseVisualStyleBackColor = true;
            GameStartBtn.Click += GameStartBtn_Click;
            // 
            // WaitingRoomForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1043, 559);
            Controls.Add(GameStartBtn);
            Controls.Add(WaitingRoomGrid);
            Name = "WaitingRoomForm";
            Text = "WaitingRoomForm";
            ((System.ComponentModel.ISupportInitialize)WaitingRoomGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView WaitingRoomGrid;
        private Button GameStartBtn;
    }
}