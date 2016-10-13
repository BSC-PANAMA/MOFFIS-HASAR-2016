namespace MOFFIS
{
    partial class frmReportes
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Reportes = new System.Windows.Forms.TabPage();
            this.btnGenerarReporteZ = new System.Windows.Forms.Button();
            this.btnGenerarReporteX = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.Reportes.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Reportes);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(103, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1061, 609);
            this.tabControl1.TabIndex = 3;
            // 
            // Reportes
            // 
            this.Reportes.Controls.Add(this.btnGenerarReporteZ);
            this.Reportes.Controls.Add(this.btnGenerarReporteX);
            this.Reportes.Location = new System.Drawing.Point(4, 25);
            this.Reportes.Name = "Reportes";
            this.Reportes.Padding = new System.Windows.Forms.Padding(3);
            this.Reportes.Size = new System.Drawing.Size(1053, 580);
            this.Reportes.TabIndex = 4;
            this.Reportes.Text = "Reportes";
            this.Reportes.UseVisualStyleBackColor = true;
            // 
            // btnGenerarReporteZ
            // 
            this.btnGenerarReporteZ.BackColor = System.Drawing.SystemColors.Info;
            this.btnGenerarReporteZ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerarReporteZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarReporteZ.Location = new System.Drawing.Point(379, 189);
            this.btnGenerarReporteZ.Name = "btnGenerarReporteZ";
            this.btnGenerarReporteZ.Size = new System.Drawing.Size(254, 75);
            this.btnGenerarReporteZ.TabIndex = 1;
            this.btnGenerarReporteZ.Text = "Generar Reporte Z";
            this.btnGenerarReporteZ.UseVisualStyleBackColor = false;
            this.btnGenerarReporteZ.Click += new System.EventHandler(this.btnGenerarReporteZ_Click);
            // 
            // btnGenerarReporteX
            // 
            this.btnGenerarReporteX.BackColor = System.Drawing.SystemColors.Info;
            this.btnGenerarReporteX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerarReporteX.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarReporteX.Location = new System.Drawing.Point(379, 73);
            this.btnGenerarReporteX.Name = "btnGenerarReporteX";
            this.btnGenerarReporteX.Size = new System.Drawing.Size(254, 75);
            this.btnGenerarReporteX.TabIndex = 0;
            this.btnGenerarReporteX.Text = "Generar Reporte X";
            this.btnGenerarReporteX.UseVisualStyleBackColor = false;
            this.btnGenerarReporteX.Click += new System.EventHandler(this.btnGenerarReporteX_Click);
            // 
            // frmReportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1267, 643);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmReportes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmReportes";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabControl1.ResumeLayout(false);
            this.Reportes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Reportes;
        private System.Windows.Forms.Button btnGenerarReporteZ;
        private System.Windows.Forms.Button btnGenerarReporteX;
    }
}