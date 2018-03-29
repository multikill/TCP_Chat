namespace TCP_Chat
{
    partial class MyClient
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
            System.Windows.Forms.Button btn_send;
            this.btn_connect = new System.Windows.Forms.Button();
            this.lbl_host = new System.Windows.Forms.Label();
            this.lbl_port = new System.Windows.Forms.Label();
            this.txt_host = new System.Windows.Forms.TextBox();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.txt_receive = new System.Windows.Forms.TextBox();
            this.txt_send = new System.Windows.Forms.TextBox();
            btn_send = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(445, 11);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(75, 23);
            this.btn_connect.TabIndex = 0;
            this.btn_connect.Text = "Connect";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // lbl_host
            // 
            this.lbl_host.AutoSize = true;
            this.lbl_host.Location = new System.Drawing.Point(26, 16);
            this.lbl_host.Name = "lbl_host";
            this.lbl_host.Size = new System.Drawing.Size(110, 13);
            this.lbl_host.TabIndex = 1;
            this.lbl_host.Text = "Connect to Hostname";
            // 
            // lbl_port
            // 
            this.lbl_port.AutoSize = true;
            this.lbl_port.Location = new System.Drawing.Point(319, 16);
            this.lbl_port.Name = "lbl_port";
            this.lbl_port.Size = new System.Drawing.Size(26, 13);
            this.lbl_port.TabIndex = 2;
            this.lbl_port.Text = "Port";
            // 
            // txt_host
            // 
            this.txt_host.Location = new System.Drawing.Point(154, 12);
            this.txt_host.Name = "txt_host";
            this.txt_host.Size = new System.Drawing.Size(147, 20);
            this.txt_host.TabIndex = 3;
            // 
            // txt_port
            // 
            this.txt_port.Location = new System.Drawing.Point(363, 12);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(64, 20);
            this.txt_port.TabIndex = 4;
            this.txt_port.Text = "13000";
            // 
            // txt_receive
            // 
            this.txt_receive.Location = new System.Drawing.Point(29, 51);
            this.txt_receive.Multiline = true;
            this.txt_receive.Name = "txt_receive";
            this.txt_receive.ReadOnly = true;
            this.txt_receive.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txt_receive.Size = new System.Drawing.Size(491, 185);
            this.txt_receive.TabIndex = 5;
            // 
            // txt_send
            // 
            this.txt_send.Location = new System.Drawing.Point(29, 243);
            this.txt_send.Multiline = true;
            this.txt_send.Name = "txt_send";
            this.txt_send.Size = new System.Drawing.Size(491, 89);
            this.txt_send.TabIndex = 6;
            this.txt_send.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_send_KeyDown);
            this.txt_send.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_send_KeyUp);
            // 
            // btn_send
            // 
            btn_send.Location = new System.Drawing.Point(445, 338);
            btn_send.Name = "btn_send";
            btn_send.Size = new System.Drawing.Size(75, 23);
            btn_send.TabIndex = 7;
            btn_send.Text = "Senden";
            btn_send.UseVisualStyleBackColor = true;
            btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 380);
            this.Controls.Add(btn_send);
            this.Controls.Add(this.txt_send);
            this.Controls.Add(this.txt_receive);
            this.Controls.Add(this.txt_port);
            this.Controls.Add(this.txt_host);
            this.Controls.Add(this.lbl_port);
            this.Controls.Add(this.lbl_host);
            this.Controls.Add(this.btn_connect);
            this.Name = "MyClient";
            this.Text = "MyClient";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MyClient_FormClosed);
            this.Load += new System.EventHandler(this.MyClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
            //this.components = new System.ComponentModel.Container();
        }

        #endregion

        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.Label lbl_host;
        private System.Windows.Forms.Label lbl_port;
        private System.Windows.Forms.TextBox txt_host;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.TextBox txt_receive;
        private System.Windows.Forms.TextBox txt_send;
    }
}