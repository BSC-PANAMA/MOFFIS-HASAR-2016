namespace MOFFIS
{
    partial class frmNoFiscal_Re_Imprimir
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
            this.btnReimprimir = new System.Windows.Forms.Button();
            this.panel14 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp2 = new System.Windows.Forms.DateTimePicker();
            this.dtp1 = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTipoDocumento = new System.Windows.Forms.Label();
            this.lblNombreCliente = new System.Windows.Forms.Label();
            this.lblNumeroFactura = new System.Windows.Forms.Label();
            this.lblIdentificadorCliente = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.btnRecargarListados = new System.Windows.Forms.Button();
            this.lvFacturas = new System.Windows.Forms.ListView();
            this.lblFechaFactura = new System.Windows.Forms.Label();
            this.panel14.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReimprimir
            // 
            this.btnReimprimir.BackColor = System.Drawing.SystemColors.Info;
            this.btnReimprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReimprimir.Location = new System.Drawing.Point(1034, 232);
            this.btnReimprimir.Name = "btnReimprimir";
            this.btnReimprimir.Size = new System.Drawing.Size(192, 36);
            this.btnReimprimir.TabIndex = 2;
            this.btnReimprimir.Text = "Re Imprimir Documento";
            this.btnReimprimir.UseVisualStyleBackColor = false;
            this.btnReimprimir.Click += new System.EventHandler(this.btnReimprimir_Click);
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel14.Controls.Add(this.label3);
            this.panel14.Controls.Add(this.label2);
            this.panel14.Controls.Add(this.dtp2);
            this.panel14.Controls.Add(this.dtp1);
            this.panel14.Controls.Add(this.label11);
            this.panel14.Controls.Add(this.label10);
            this.panel14.Controls.Add(this.label9);
            this.panel14.Controls.Add(this.label8);
            this.panel14.Controls.Add(this.label7);
            this.panel14.Controls.Add(this.btnReimprimir);
            this.panel14.Controls.Add(this.lblTipoDocumento);
            this.panel14.Controls.Add(this.lblNombreCliente);
            this.panel14.Controls.Add(this.lblNumeroFactura);
            this.panel14.Controls.Add(this.lblIdentificadorCliente);
            this.panel14.Controls.Add(this.label33);
            this.panel14.Controls.Add(this.btnRecargarListados);
            this.panel14.Controls.Add(this.lvFacturas);
            this.panel14.Controls.Add(this.lblFechaFactura);
            this.panel14.Location = new System.Drawing.Point(12, 42);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(1243, 556);
            this.panel14.TabIndex = 139;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 456);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 166;
            this.label3.Text = "Fecha Final";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 421);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 165;
            this.label2.Text = "Fecha Inicial";
            // 
            // dtp2
            // 
            this.dtp2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp2.Location = new System.Drawing.Point(90, 449);
            this.dtp2.Name = "dtp2";
            this.dtp2.Size = new System.Drawing.Size(116, 20);
            this.dtp2.TabIndex = 164;
            // 
            // dtp1
            // 
            this.dtp1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp1.Location = new System.Drawing.Point(90, 415);
            this.dtp1.Name = "dtp1";
            this.dtp1.Size = new System.Drawing.Size(114, 20);
            this.dtp1.TabIndex = 163;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(888, 163);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(104, 23);
            this.label11.TabIndex = 134;
            this.label11.Text = "Tipo De Documento";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(888, 127);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(139, 23);
            this.label10.TabIndex = 133;
            this.label10.Text = "Fecha De Documento";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(888, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 23);
            this.label9.TabIndex = 132;
            this.label9.Text = "Nombre Del Cliente";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(888, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(139, 23);
            this.label8.TabIndex = 131;
            this.label8.Text = "Identificador Del Cliente";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(888, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(140, 23);
            this.label7.TabIndex = 130;
            this.label7.Text = "Numero de Documento";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTipoDocumento
            // 
            this.lblTipoDocumento.BackColor = System.Drawing.Color.White;
            this.lblTipoDocumento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTipoDocumento.Location = new System.Drawing.Point(1034, 163);
            this.lblTipoDocumento.Name = "lblTipoDocumento";
            this.lblTipoDocumento.Size = new System.Drawing.Size(192, 20);
            this.lblTipoDocumento.TabIndex = 128;
            this.lblTipoDocumento.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNombreCliente
            // 
            this.lblNombreCliente.BackColor = System.Drawing.Color.White;
            this.lblNombreCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNombreCliente.Location = new System.Drawing.Point(1034, 94);
            this.lblNombreCliente.Name = "lblNombreCliente";
            this.lblNombreCliente.Size = new System.Drawing.Size(192, 20);
            this.lblNombreCliente.TabIndex = 127;
            this.lblNombreCliente.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNumeroFactura
            // 
            this.lblNumeroFactura.BackColor = System.Drawing.Color.White;
            this.lblNumeroFactura.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNumeroFactura.Location = new System.Drawing.Point(1034, 23);
            this.lblNumeroFactura.Name = "lblNumeroFactura";
            this.lblNumeroFactura.Size = new System.Drawing.Size(192, 20);
            this.lblNumeroFactura.TabIndex = 126;
            this.lblNumeroFactura.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblIdentificadorCliente
            // 
            this.lblIdentificadorCliente.BackColor = System.Drawing.Color.White;
            this.lblIdentificadorCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblIdentificadorCliente.Location = new System.Drawing.Point(1034, 58);
            this.lblIdentificadorCliente.Name = "lblIdentificadorCliente";
            this.lblIdentificadorCliente.Size = new System.Drawing.Size(192, 20);
            this.lblIdentificadorCliente.TabIndex = 125;
            this.lblIdentificadorCliente.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label33
            // 
            this.label33.Location = new System.Drawing.Point(5, 2);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(145, 20);
            this.label33.TabIndex = 124;
            this.label33.Text = "Listado de Documentos";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRecargarListados
            // 
            this.btnRecargarListados.BackColor = System.Drawing.SystemColors.Info;
            this.btnRecargarListados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecargarListados.Location = new System.Drawing.Point(8, 488);
            this.btnRecargarListados.Name = "btnRecargarListados";
            this.btnRecargarListados.Size = new System.Drawing.Size(104, 36);
            this.btnRecargarListados.TabIndex = 123;
            this.btnRecargarListados.Text = "Cargar Listado";
            this.btnRecargarListados.UseVisualStyleBackColor = false;
            this.btnRecargarListados.Click += new System.EventHandler(this.btnRecargarListados_Click);
            // 
            // lvFacturas
            // 
            this.lvFacturas.Location = new System.Drawing.Point(8, 22);
            this.lvFacturas.Name = "lvFacturas";
            this.lvFacturas.Size = new System.Drawing.Size(859, 377);
            this.lvFacturas.TabIndex = 111;
            this.lvFacturas.UseCompatibleStateImageBehavior = false;
            this.lvFacturas.DoubleClick += new System.EventHandler(this.lvFacturas_DoubleClick);
            // 
            // lblFechaFactura
            // 
            this.lblFechaFactura.BackColor = System.Drawing.Color.White;
            this.lblFechaFactura.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFechaFactura.Location = new System.Drawing.Point(1034, 128);
            this.lblFechaFactura.Name = "lblFechaFactura";
            this.lblFechaFactura.Size = new System.Drawing.Size(192, 20);
            this.lblFechaFactura.TabIndex = 116;
            this.lblFechaFactura.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmNoFiscal_Re_Imprimir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1267, 643);
            this.Controls.Add(this.panel14);
            this.Name = "frmNoFiscal_Re_Imprimir";
            this.Text = "frmNoFiscal_Re_Imprimir";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmNoFiscal_Re_Imprimir_Load);
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReimprimir;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Button btnRecargarListados;
        private System.Windows.Forms.ListView lvFacturas;
        internal System.Windows.Forms.Label lblFechaFactura;
        internal System.Windows.Forms.Label label11;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label label9;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Label lblTipoDocumento;
        internal System.Windows.Forms.Label lblNombreCliente;
        internal System.Windows.Forms.Label lblNumeroFactura;
        internal System.Windows.Forms.Label lblIdentificadorCliente;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtp2;
        private System.Windows.Forms.DateTimePicker dtp1;
    }
}