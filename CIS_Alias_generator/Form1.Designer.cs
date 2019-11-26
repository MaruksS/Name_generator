namespace CIS_Alias_generator
{
    partial class Form1
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
            this.lbl_name = new System.Windows.Forms.Label();
            this.txt_input = new System.Windows.Forms.TextBox();
            this.txt_output = new System.Windows.Forms.TextBox();
            this.lbl_alias = new System.Windows.Forms.Label();
            this.btn_generate = new System.Windows.Forms.Button();
            this.chb_Person = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.Location = new System.Drawing.Point(24, 85);
            this.lbl_name.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(64, 25);
            this.lbl_name.TabIndex = 0;
            this.lbl_name.Text = "Name";
            // 
            // txt_input
            // 
            this.txt_input.Location = new System.Drawing.Point(152, 82);
            this.txt_input.Margin = new System.Windows.Forms.Padding(6);
            this.txt_input.Name = "txt_input";
            this.txt_input.Size = new System.Drawing.Size(516, 29);
            this.txt_input.TabIndex = 1;
            // 
            // txt_output
            // 
            this.txt_output.Location = new System.Drawing.Point(152, 177);
            this.txt_output.Margin = new System.Windows.Forms.Padding(6);
            this.txt_output.Multiline = true;
            this.txt_output.Name = "txt_output";
            this.txt_output.Size = new System.Drawing.Size(803, 366);
            this.txt_output.TabIndex = 2;
            // 
            // lbl_alias
            // 
            this.lbl_alias.AutoSize = true;
            this.lbl_alias.Location = new System.Drawing.Point(29, 177);
            this.lbl_alias.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_alias.Name = "lbl_alias";
            this.lbl_alias.Size = new System.Drawing.Size(55, 25);
            this.lbl_alias.TabIndex = 3;
            this.lbl_alias.Text = "Alias";
            // 
            // btn_generate
            // 
            this.btn_generate.Location = new System.Drawing.Point(839, 76);
            this.btn_generate.Margin = new System.Windows.Forms.Padding(6);
            this.btn_generate.Name = "btn_generate";
            this.btn_generate.Size = new System.Drawing.Size(138, 42);
            this.btn_generate.TabIndex = 4;
            this.btn_generate.Text = "Generate";
            this.btn_generate.UseVisualStyleBackColor = true;
            this.btn_generate.Click += new System.EventHandler(this.onBtnClick);
            // 
            // chb_Person
            // 
            this.chb_Person.AutoSize = true;
            this.chb_Person.Location = new System.Drawing.Point(677, 87);
            this.chb_Person.Name = "chb_Person";
            this.chb_Person.Size = new System.Drawing.Size(86, 29);
            this.chb_Person.TabIndex = 6;
            this.chb_Person.Text = "Entity";
            this.chb_Person.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1032, 38);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectConfigToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(56, 34);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // selectConfigToolStripMenuItem
            // 
            this.selectConfigToolStripMenuItem.Name = "selectConfigToolStripMenuItem";
            this.selectConfigToolStripMenuItem.Size = new System.Drawing.Size(223, 34);
            this.selectConfigToolStripMenuItem.Text = "Select config";
            this.selectConfigToolStripMenuItem.Click += new System.EventHandler(this.selectConfigToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AcceptButton = this.btn_generate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 823);
            this.Controls.Add(this.chb_Person);
            this.Controls.Add(this.btn_generate);
            this.Controls.Add(this.lbl_alias);
            this.Controls.Add(this.txt_output);
            this.Controls.Add(this.txt_input);
            this.Controls.Add(this.lbl_name);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.Text = "Alias Generator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.TextBox txt_input;
        private System.Windows.Forms.TextBox txt_output;
        private System.Windows.Forms.Label lbl_alias;
        private System.Windows.Forms.Button btn_generate;
        private System.Windows.Forms.CheckBox chb_Person;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectConfigToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

