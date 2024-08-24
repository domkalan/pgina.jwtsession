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
            this.Description = new System.Windows.Forms.Label();
            this.globalSessionTime = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Loginserver_label
            // 
            this.Loginserver_label.AutoSize = true;
            this.Loginserver_label.Location = new System.Drawing.Point(12, 9);
            this.Loginserver_label.Name = "Loginserver_label";
            this.Loginserver_label.Size = new System.Drawing.Size(185, 13);
            this.Loginserver_label.TabIndex = 0;
            this.Loginserver_label.Text = "The Webserver address (Loginserver)";
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
            this.save_button.Location = new System.Drawing.Point(174, 204);
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
            this.help_button.Location = new System.Drawing.Point(12, 204);
            this.help_button.Name = "help_button";
            this.help_button.Size = new System.Drawing.Size(75, 23);
            this.help_button.TabIndex = 3;
            this.help_button.Text = "Help";
            this.help_button.UseVisualStyleBackColor = true;
            this.help_button.Click += new System.EventHandler(this.Btn_help);
            // 
            // Description
            // 
            this.Description.AutoSize = true;
            this.Description.Location = new System.Drawing.Point(12, 113);
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(89, 26);
            this.Description.TabIndex = 2;
            this.Description.Text = "Macros:\r\n  %u = UserName\r\n";
            // 
            // globalSessionTime
            // 
            this.globalSessionTime.Location = new System.Drawing.Point(12, 64);
            this.globalSessionTime.Name = "globalSessionTime";
            this.globalSessionTime.Size = new System.Drawing.Size(237, 20);
            this.globalSessionTime.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Global sesion time";
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 239);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.globalSessionTime);
            this.Controls.Add(this.Description);
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
        private System.Windows.Forms.Label Description;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox globalSessionTime;
    }
}