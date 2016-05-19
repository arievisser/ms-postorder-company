namespace PostorderCompany.Chauffeur
{
    partial class ChauffeurOverView
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
            this.Send_btn = new System.Windows.Forms.Button();
            this.Delivered_btn = new System.Windows.Forms.Button();
            this.OverView = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Send_btn
            // 
            this.Send_btn.Location = new System.Drawing.Point(183, 234);
            this.Send_btn.Name = "Send_btn";
            this.Send_btn.Size = new System.Drawing.Size(75, 23);
            this.Send_btn.TabIndex = 1;
            this.Send_btn.Text = "Versturen";
            this.Send_btn.UseVisualStyleBackColor = true;
            this.Send_btn.Click += new System.EventHandler(this.Send_btn_Click);
            // 
            // Delivered_btn
            // 
            this.Delivered_btn.Location = new System.Drawing.Point(264, 234);
            this.Delivered_btn.Name = "Delivered_btn";
            this.Delivered_btn.Size = new System.Drawing.Size(75, 23);
            this.Delivered_btn.TabIndex = 2;
            this.Delivered_btn.Text = "Afgeleverd";
            this.Delivered_btn.UseVisualStyleBackColor = true;
            this.Delivered_btn.Click += new System.EventHandler(this.Delivered_btn_Click);
            // 
            // OverView
            // 
            this.OverView.FormattingEnabled = true;
            this.OverView.Location = new System.Drawing.Point(13, 13);
            this.OverView.Name = "OverView";
            this.OverView.Size = new System.Drawing.Size(588, 199);
            this.OverView.TabIndex = 3;
            // 
            // ChauffeurOverView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 273);
            this.Controls.Add(this.OverView);
            this.Controls.Add(this.Delivered_btn);
            this.Controls.Add(this.Send_btn);
            this.Name = "ChauffeurOverView";
            this.Text = "ChauffeurOverView";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Send_btn;
        private System.Windows.Forms.Button Delivered_btn;
        private System.Windows.Forms.ListBox OverView;
    }
}