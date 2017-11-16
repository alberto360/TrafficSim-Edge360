namespace SparrowDiagram
{
    partial class SparrowDiagram
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
            this.components = new System.ComponentModel.Container();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.infoPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.sparrowPlane1 = new SparrowPlane();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // infoPropertyGrid
            // 
            this.infoPropertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.infoPropertyGrid.Location = new System.Drawing.Point(0, 12);
            this.infoPropertyGrid.Name = "infoPropertyGrid";
            this.infoPropertyGrid.Size = new System.Drawing.Size(233, 430);
            this.infoPropertyGrid.TabIndex = 1;
            // 
            // sparrowPlane1
            // 
            this.sparrowPlane1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sparrowPlane1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.sparrowPlane1.Location = new System.Drawing.Point(239, 12);
            this.sparrowPlane1.Name = "sparrowPlane1";
            this.sparrowPlane1.Size = new System.Drawing.Size(1072, 718);
            this.sparrowPlane1.TabIndex = 0;
            // 
            // SparrowDiagram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1323, 742);
            this.Controls.Add(this.infoPropertyGrid);
            this.Controls.Add(this.sparrowPlane1);
            this.Name = "SparrowDiagram";
            this.Text = "Sparrow Diagram";
            this.Load += new System.EventHandler(this.SparrowDiagram_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource1;
        private SparrowPlane sparrowPlane1;
        private System.Windows.Forms.PropertyGrid infoPropertyGrid;
    }
}

