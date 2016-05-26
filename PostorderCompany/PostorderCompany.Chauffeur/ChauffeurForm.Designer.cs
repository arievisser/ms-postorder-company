namespace PostorderCompany.Chauffeur
{
    partial class ChauffeurForm
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
            this.handetekening_txt = new System.Windows.Forms.TextBox();
            this.chauffeur_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Send_btn
            // 
            this.Send_btn.Location = new System.Drawing.Point(296, 194);
            this.Send_btn.Name = "Send_btn";
            this.Send_btn.Size = new System.Drawing.Size(123, 23);
            this.Send_btn.TabIndex = 1;
            this.Send_btn.Text = "Markeer \"Verstuurd\"";
            this.Send_btn.UseVisualStyleBackColor = true;
            this.Send_btn.Click += new System.EventHandler(this.Send_btn_Click);
            // 
            // Delivered_btn
            // 
            this.Delivered_btn.Location = new System.Drawing.Point(296, 236);
            this.Delivered_btn.Name = "Delivered_btn";
            this.Delivered_btn.Size = new System.Drawing.Size(123, 23);
            this.Delivered_btn.TabIndex = 2;
            this.Delivered_btn.Text = "Markeer \"Afgeleverd\"";
            this.Delivered_btn.UseVisualStyleBackColor = true;
            this.Delivered_btn.Click += new System.EventHandler(this.Delivered_btn_Click);
            // 
            // OverView
            // 
            this.OverView.AccessibleName = "";
            this.OverView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OverView.DisplayMember = "pakketId";
            this.OverView.FormattingEnabled = true;
            this.OverView.Location = new System.Drawing.Point(10, 10);
            this.OverView.Name = "OverView";
            this.OverView.Size = new System.Drawing.Size(409, 160);
            this.OverView.TabIndex = 3;
            this.OverView.ValueMember = "pakketId";
            this.OverView.SelectedIndexChanged += new System.EventHandler(this.OverView_SelectedIndexChanged);
            // 
            // handetekening_txt
            // 
            this.handetekening_txt.Location = new System.Drawing.Point(10, 238);
            this.handetekening_txt.Name = "handetekening_txt";
            this.handetekening_txt.Size = new System.Drawing.Size(280, 20);
            this.handetekening_txt.TabIndex = 5;
            // 
            // chauffeur_txt
            // 
            this.chauffeur_txt.Location = new System.Drawing.Point(10, 196);
            this.chauffeur_txt.Name = "chauffeur_txt";
            this.chauffeur_txt.Size = new System.Drawing.Size(280, 20);
            this.chauffeur_txt.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Naam chauffeur";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 219);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Handtekening";
            // 
            // ChauffeurOverView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 269);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chauffeur_txt);
            this.Controls.Add(this.handetekening_txt);
            this.Controls.Add(this.OverView);
            this.Controls.Add(this.Delivered_btn);
            this.Controls.Add(this.Send_btn);
            this.MinimumSize = new System.Drawing.Size(450, 308);
            this.Name = "ChauffeurOverView";
            this.Text = "ChauffeurOverView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChauffeurOverView_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Send_btn;
        private System.Windows.Forms.Button Delivered_btn;
        private System.Windows.Forms.ListBox OverView;
        private System.Windows.Forms.TextBox handetekening_txt;
        private System.Windows.Forms.TextBox chauffeur_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}