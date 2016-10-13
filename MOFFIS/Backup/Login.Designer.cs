namespace CSSDK
{
    partial class Login
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
            this.NameEditBox = new System.Windows.Forms.TextBox();
            this.PasswordEditBox = new System.Windows.Forms.TextBox();
            this.NameLable = new System.Windows.Forms.Label();
            this.PasswordLable = new System.Windows.Forms.Label();
            this.OK_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NameEditBox
            // 
            this.NameEditBox.AccessibleDescription = "Provide name for Login";
            this.NameEditBox.AccessibleName = "NameEditBox";
            this.NameEditBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.NameEditBox.Location = new System.Drawing.Point(108, 40);
            this.NameEditBox.Name = "NameEditBox";
            this.NameEditBox.Size = new System.Drawing.Size(172, 20);
            this.NameEditBox.TabIndex = 0;
            this.NameEditBox.Text = "Peachtree Software";
            // 
            // PasswordEditBox
            // 
            this.PasswordEditBox.Location = new System.Drawing.Point(108, 95);
            this.PasswordEditBox.Name = "PasswordEditBox";
            this.PasswordEditBox.Size = new System.Drawing.Size(172, 20);
            this.PasswordEditBox.TabIndex = 1;
            this.PasswordEditBox.Text = "9E5643PCU118X6C";
            // 
            // NameLable
            // 
            this.NameLable.AccessibleName = "Name";
            this.NameLable.AutoSize = true;
            this.NameLable.Location = new System.Drawing.Point(51, 44);
            this.NameLable.Name = "NameLable";
            this.NameLable.Size = new System.Drawing.Size(44, 13);
            this.NameLable.TabIndex = 2;
            this.NameLable.Text = "Nombre";
            // 
            // PasswordLable
            // 
            this.PasswordLable.AutoSize = true;
            this.PasswordLable.Location = new System.Drawing.Point(54, 101);
            this.PasswordLable.Name = "PasswordLable";
            this.PasswordLable.Size = new System.Drawing.Size(53, 13);
            this.PasswordLable.TabIndex = 3;
            this.PasswordLable.Text = "Password";
            // 
            // OK_Button
            // 
            this.OK_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_Button.Location = new System.Drawing.Point(108, 176);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(75, 23);
            this.OK_Button.TabIndex = 4;
            this.OK_Button.Text = "Ok";
            this.OK_Button.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(111, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Nombre de Cuenta 3rd Party";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Password de Cuenta 3rd Party";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OK_Button);
            this.Controls.Add(this.PasswordLable);
            this.Controls.Add(this.NameLable);
            this.Controls.Add(this.PasswordEditBox);
            this.Controls.Add(this.NameEditBox);
            this.Name = "Login";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox NameEditBox;
        public System.Windows.Forms.TextBox PasswordEditBox;
        private System.Windows.Forms.Label NameLable;
        private System.Windows.Forms.Label PasswordLable;
        public System.Windows.Forms.Button OK_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}