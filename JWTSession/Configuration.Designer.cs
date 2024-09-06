namespace pGina.Plugin.JWTSession
{
    partial class Configuration
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
            this.Loginserver_label = new System.Windows.Forms.Label();
            this.Loginserver_textBox = new System.Windows.Forms.TextBox();
            this.save_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.help_button = new System.Windows.Forms.Button();
            this.LoginserverPwd_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LoginserverSession_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Loginserver_label
            // 
            this.Loginserver_label.AutoSize = true;
            this.Loginserver_label.Location = new System.Drawing.Point(12, 9);
            this.Loginserver_label.Name = "Loginserver_label";
            this.Loginserver_label.Size = new System.Drawing.Size(107, 13);
            this.Loginserver_label.TabIndex = 0;
            this.Loginserver_label.Text = "JWT HTTP Endpoint";
            this.Loginserver_label.Click += new System.EventHandler(this.Loginserver_label_Click);
            // 
            // Loginserver_textBox
            // 
            this.Loginserver_textBox.Location = new System.Drawing.Point(12, 25);
            this.Loginserver_textBox.Name = "Loginserver_textBox";
            this.Loginserver_textBox.Size = new System.Drawing.Size(237, 20);
            this.Loginserver_textBox.TabIndex = 1;
            // 
            // save_button
            // 
            this.save_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.save_button.Location = new System.Drawing.Point(12, 204);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(75, 23);
            this.save_button.TabIndex = 5;
            this.save_button.Text = "Save";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.Btn_Save);
            // 
            // cancel_button
            // 
            this.cancel_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_button.Location = new System.Drawing.Point(93, 204);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(75, 23);
            this.cancel_button.TabIndex = 4;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.Btn_Cancel);
            // 
            // help_button
            // 
            this.help_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.help_button.Location = new System.Drawing.Point(174, 204);
            this.help_button.Name = "help_button";
            this.help_button.Size = new System.Drawing.Size(75, 23);
            this.help_button.TabIndex = 3;
            this.help_button.Text = "Help";
            this.help_button.UseVisualStyleBackColor = true;
            this.help_button.Click += new System.EventHandler(this.Btn_help);
            // 
            // LoginserverPwd_textBox
            // 
            this.LoginserverPwd_textBox.Location = new System.Drawing.Point(9, 99);
            this.LoginserverPwd_textBox.Name = "LoginserverPwd_textBox";
            this.LoginserverPwd_textBox.Size = new System.Drawing.Size(237, 20);
            this.LoginserverPwd_textBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "JWT PasswordChange HTTP Endpoint";
            // 
            // LoginserverSession_textBox
            // 
            this.LoginserverSession_textBox.Location = new System.Drawing.Point(12, 146);
            this.LoginserverSession_textBox.Name = "LoginserverSession_textBox";
            this.LoginserverSession_textBox.Size = new System.Drawing.Size(237, 20);
            this.LoginserverSession_textBox.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "JWT Session Background Endpoint";
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 239);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LoginserverSession_textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LoginserverPwd_textBox);
            this.Controls.Add(this.help_button);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.save_button);
            this.Controls.Add(this.Loginserver_textBox);
            this.Controls.Add(this.Loginserver_label);
            this.Name = "Configuration";
            this.Text = "JWTSession plugin Configuration";
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Loginserver_label;
        private System.Windows.Forms.TextBox Loginserver_textBox;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Button help_button;
        private System.Windows.Forms.TextBox LoginserverPwd_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox LoginserverSession_textBox;
        private System.Windows.Forms.Label label2;
    }
}