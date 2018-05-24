namespace C18210.user
{
    partial class ModifyPassword
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
            this.Button1 = new System.Windows.Forms.Button();
            this.TextBox4 = new System.Windows.Forms.TextBox();
            this.TextBox3 = new System.Windows.Forms.TextBox();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.ComboBox1 = new System.Windows.Forms.ComboBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(190, 177);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(79, 29);
            this.Button1.TabIndex = 36;
            this.Button1.Text = "确定";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // TextBox4
            // 
            this.TextBox4.Location = new System.Drawing.Point(91, 135);
            this.TextBox4.Name = "TextBox4";
            this.TextBox4.PasswordChar = '*';
            this.TextBox4.Size = new System.Drawing.Size(179, 21);
            this.TextBox4.TabIndex = 35;
            // 
            // TextBox3
            // 
            this.TextBox3.Location = new System.Drawing.Point(91, 103);
            this.TextBox3.Name = "TextBox3";
            this.TextBox3.PasswordChar = '*';
            this.TextBox3.Size = new System.Drawing.Size(179, 21);
            this.TextBox3.TabIndex = 34;
            // 
            // TextBox2
            // 
            this.TextBox2.Location = new System.Drawing.Point(91, 70);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.PasswordChar = '*';
            this.TextBox2.Size = new System.Drawing.Size(179, 21);
            this.TextBox2.TabIndex = 33;
            // 
            // ComboBox1
            // 
            this.ComboBox1.FormattingEnabled = true;
            this.ComboBox1.Location = new System.Drawing.Point(91, 39);
            this.ComboBox1.Name = "ComboBox1";
            this.ComboBox1.Size = new System.Drawing.Size(179, 20);
            this.ComboBox1.TabIndex = 32;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(12, 138);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(65, 12);
            this.Label4.TabIndex = 31;
            this.Label4.Text = "确认密码：";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(12, 107);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(53, 12);
            this.Label3.TabIndex = 30;
            this.Label3.Text = "新密码：";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(12, 74);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(53, 12);
            this.Label2.TabIndex = 29;
            this.Label2.Text = "旧密码：";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(12, 43);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(53, 12);
            this.Label1.TabIndex = 28;
            this.Label1.Text = "用户名：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 177);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 12);
            this.label5.TabIndex = 37;
            // 
            // ModifyPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 224);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.TextBox4);
            this.Controls.Add(this.TextBox3);
            this.Controls.Add(this.TextBox2);
            this.Controls.Add(this.ComboBox1);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Name = "ModifyPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "密码修改";
            this.Load += new System.EventHandler(this.ModifyPassword_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.TextBox TextBox4;
        internal System.Windows.Forms.TextBox TextBox3;
        internal System.Windows.Forms.TextBox TextBox2;
        internal System.Windows.Forms.ComboBox ComboBox1;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label label5;
    }
}