namespace AppManagementTool
{
    partial class MainWindow
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
            this.appListBox = new System.Windows.Forms.ListBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.loadConfigFileButton = new System.Windows.Forms.Button();
            this.loadAppsButton = new System.Windows.Forms.Button();
            this.createButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // appListBox
            // 
            this.appListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.appListBox.DisplayMember = "Title";
            this.appListBox.FormattingEnabled = true;
            this.appListBox.ItemHeight = 12;
            this.appListBox.Location = new System.Drawing.Point(11, 12);
            this.appListBox.Name = "appListBox";
            this.appListBox.Size = new System.Drawing.Size(621, 388);
            this.appListBox.TabIndex = 0;
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(329, 406);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(144, 31);
            this.generateButton.TabIndex = 1;
            this.generateButton.Text = "生成配置文件";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // loadConfigFileButton
            // 
            this.loadConfigFileButton.Location = new System.Drawing.Point(11, 406);
            this.loadConfigFileButton.Name = "loadConfigFileButton";
            this.loadConfigFileButton.Size = new System.Drawing.Size(144, 31);
            this.loadConfigFileButton.TabIndex = 2;
            this.loadConfigFileButton.Text = "添加应用";
            this.loadConfigFileButton.UseVisualStyleBackColor = true;
            this.loadConfigFileButton.Visible = false;
            this.loadConfigFileButton.Click += new System.EventHandler(this.loadConfigFileButton_Click);
            // 
            // loadAppsButton
            // 
            this.loadAppsButton.Location = new System.Drawing.Point(170, 406);
            this.loadAppsButton.Name = "loadAppsButton";
            this.loadAppsButton.Size = new System.Drawing.Size(144, 31);
            this.loadAppsButton.TabIndex = 3;
            this.loadAppsButton.Text = "加载应用";
            this.loadAppsButton.UseVisualStyleBackColor = true;
            this.loadAppsButton.Click += new System.EventHandler(this.loadAppsButton_Click);
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(488, 406);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(144, 31);
            this.createButton.TabIndex = 4;
            this.createButton.Text = "生成安装包";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 447);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.loadAppsButton);
            this.Controls.Add(this.loadConfigFileButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.appListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "App Management Tool";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox appListBox;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button loadConfigFileButton;
        private System.Windows.Forms.Button loadAppsButton;
        private System.Windows.Forms.Button createButton;
    }
}

