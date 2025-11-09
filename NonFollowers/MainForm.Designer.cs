namespace NonFollowers
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnFollowers = new Button();
            btnFollowing = new Button();
            txtFollowersFilePath = new TextBox();
            txtFollowingFilePath = new TextBox();
            btnCompare = new Button();
            dataGridView1 = new DataGridView();
            usernameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            urlDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            btnNaviagateTo = new DataGridViewButtonColumn();
            userBindingSource = new BindingSource(components);
            openFileDialog1 = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)userBindingSource).BeginInit();
            SuspendLayout();
            // 
            // btnFollowers
            // 
            btnFollowers.Location = new Point(12, 12);
            btnFollowers.Name = "btnFollowers";
            btnFollowers.Size = new Size(134, 23);
            btnFollowers.TabIndex = 0;
            btnFollowers.Text = "Followers File";
            btnFollowers.UseVisualStyleBackColor = true;
            btnFollowers.Click += BtnFollowers_Click;
            // 
            // btnFollowing
            // 
            btnFollowing.Location = new Point(12, 41);
            btnFollowing.Name = "btnFollowing";
            btnFollowing.Size = new Size(134, 23);
            btnFollowing.TabIndex = 1;
            btnFollowing.Text = "Following File";
            btnFollowing.UseVisualStyleBackColor = true;
            btnFollowing.Click += BtnFollowing_Click;
            // 
            // txtFollowersFilePath
            // 
            txtFollowersFilePath.Enabled = false;
            txtFollowersFilePath.Location = new Point(180, 12);
            txtFollowersFilePath.Name = "txtFollowersFilePath";
            txtFollowersFilePath.Size = new Size(542, 23);
            txtFollowersFilePath.TabIndex = 2;
            // 
            // txtFollowingFilePath
            // 
            txtFollowingFilePath.Enabled = false;
            txtFollowingFilePath.Location = new Point(180, 42);
            txtFollowingFilePath.Name = "txtFollowingFilePath";
            txtFollowingFilePath.Size = new Size(542, 23);
            txtFollowingFilePath.TabIndex = 3;
            // 
            // btnCompare
            // 
            btnCompare.Enabled = false;
            btnCompare.Location = new Point(12, 70);
            btnCompare.Name = "btnCompare";
            btnCompare.Size = new Size(710, 55);
            btnCompare.TabIndex = 4;
            btnCompare.Text = "Compare";
            btnCompare.UseVisualStyleBackColor = true;
            btnCompare.Click += BtnCompare_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { usernameDataGridViewTextBoxColumn, urlDataGridViewTextBoxColumn, btnNaviagateTo });
            dataGridView1.DataSource = userBindingSource;
            dataGridView1.Location = new Point(12, 131);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(710, 300);
            dataGridView1.TabIndex = 5;
            dataGridView1.CellContentClick += DataGridView1_CellContentClick;
            // 
            // usernameDataGridViewTextBoxColumn
            // 
            usernameDataGridViewTextBoxColumn.DataPropertyName = "Username";
            usernameDataGridViewTextBoxColumn.HeaderText = "Username";
            usernameDataGridViewTextBoxColumn.Name = "usernameDataGridViewTextBoxColumn";
            usernameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // urlDataGridViewTextBoxColumn
            // 
            urlDataGridViewTextBoxColumn.DataPropertyName = "Url";
            urlDataGridViewTextBoxColumn.HeaderText = "Url";
            urlDataGridViewTextBoxColumn.Name = "urlDataGridViewTextBoxColumn";
            urlDataGridViewTextBoxColumn.ReadOnly = true;
            urlDataGridViewTextBoxColumn.Width = 380;
            // 
            // btnNaviagateTo
            // 
            btnNaviagateTo.HeaderText = "Navigate";
            btnNaviagateTo.Name = "btnNaviagateTo";
            btnNaviagateTo.Text = "Open In Browser";
            btnNaviagateTo.UseColumnTextForButtonValue = true;
            btnNaviagateTo.Width = 170;
            // 
            // userBindingSource
            // 
            userBindingSource.DataSource = typeof(Classes.User);
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(734, 437);
            Controls.Add(dataGridView1);
            Controls.Add(btnCompare);
            Controls.Add(txtFollowingFilePath);
            Controls.Add(txtFollowersFilePath);
            Controls.Add(btnFollowing);
            Controls.Add(btnFollowers);
            MaximizeBox = false;
            MaximumSize = new Size(750, 476);
            MinimizeBox = false;
            MinimumSize = new Size(750, 476);
            Name = "MainForm";
            Text = "MainForm";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)userBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnFollowers;
        private Button btnFollowing;
        private TextBox txtFollowersFilePath;
        private TextBox txtFollowingFilePath;
        private Button btnCompare;
        private DataGridView dataGridView1;
        private OpenFileDialog openFileDialog1;
        private BindingSource userBindingSource;
        private DataGridViewTextBoxColumn usernameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn;
        private DataGridViewButtonColumn btnNaviagateTo;
    }
}
