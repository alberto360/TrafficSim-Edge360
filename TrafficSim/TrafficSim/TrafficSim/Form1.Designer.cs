namespace TrafficSim
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
            this.infoPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.simMap1 = new TrafficSim.SimMap();
            this.SuspendLayout();
            // 
            // infoPropertyGrid
            // 
            this.infoPropertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.infoPropertyGrid.Location = new System.Drawing.Point(13, 13);
            this.infoPropertyGrid.Margin = new System.Windows.Forms.Padding(4);
            this.infoPropertyGrid.Name = "infoPropertyGrid";
            this.infoPropertyGrid.Size = new System.Drawing.Size(239, 304);
            this.infoPropertyGrid.TabIndex = 2;
            // 
            // simMap1
            // 
            this.simMap1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.simMap1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.simMap1.Location = new System.Drawing.Point(270, 13);
            this.simMap1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.simMap1.Name = "simMap1";
            this.simMap1.Size = new System.Drawing.Size(930, 689);
            this.simMap1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 715);
            this.Controls.Add(this.simMap1);
            this.Controls.Add(this.infoPropertyGrid);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid infoPropertyGrid;
        private SimMap simMap1;
    }
}

