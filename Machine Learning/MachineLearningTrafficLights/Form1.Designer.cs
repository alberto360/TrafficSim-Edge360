namespace MachineLearningTrafficLights
{
    partial class FormLearnLights
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
            this.btnNewSimulation = new System.Windows.Forms.Button();
            this.textBoxShowResults = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnNewSimulation
            // 
            this.btnNewSimulation.Location = new System.Drawing.Point(492, 12);
            this.btnNewSimulation.Name = "btnNewSimulation";
            this.btnNewSimulation.Size = new System.Drawing.Size(156, 34);
            this.btnNewSimulation.TabIndex = 0;
            this.btnNewSimulation.Text = "NEW SIMULATION";
            this.btnNewSimulation.UseVisualStyleBackColor = true;
            this.btnNewSimulation.Click += new System.EventHandler(this.btnNewSimulation_Click);
            // 
            // textBoxShowResults
            // 
            this.textBoxShowResults.Location = new System.Drawing.Point(27, 52);
            this.textBoxShowResults.Multiline = true;
            this.textBoxShowResults.Name = "textBoxShowResults";
            this.textBoxShowResults.Size = new System.Drawing.Size(1092, 492);
            this.textBoxShowResults.TabIndex = 2;
            this.textBoxShowResults.Text = "Show Results";
            // 
            // FormLearnLights
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 556);
            this.Controls.Add(this.textBoxShowResults);
            this.Controls.Add(this.btnNewSimulation);
            this.Name = "FormLearnLights";
            this.Text = "Traffic Lights";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNewSimulation;
        private System.Windows.Forms.TextBox textBoxShowResults;
    }
}

