namespace MOFFIS
{
    public partial class frmPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.ComprobantesFiscMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mitCrearInvoice = new System.Windows.Forms.ToolStripMenuItem();
            this.mitNotasCredito = new System.Windows.Forms.ToolStripMenuItem();
            this.mitNotasDebito = new System.Windows.Forms.ToolStripMenuItem();
            this.ComprobantesNoFiscMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mitImprimirUltimaFactura = new System.Windows.Forms.ToolStripMenuItem();
            this.mitAnularDocumento = new System.Windows.Forms.ToolStripMenuItem();
            this.setearVariableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reimportacionUltimoDocumentoFiscalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facturaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notaCreditoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notaDebitoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reciboToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MantenimientoMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mantenimientoPeachtreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BSCMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mantenimientoEmpresaUsuariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Reportes = new System.Windows.Forms.ToolStripMenuItem();
            this.generarReportesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SoporteTecnicoMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mitSoporteTecnico = new System.Windows.Forms.ToolStripMenuItem();
            this.SalirMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mitCambiarDeUsuario = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printPreviewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ComprobantesFiscMenu,
            this.ComprobantesNoFiscMenu,
            this.MantenimientoMenu,
            this.BSCMenu,
            this.Reportes,
            this.SoporteTecnicoMenu,
            this.SalirMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1277, 27);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // ComprobantesFiscMenu
            // 
            this.ComprobantesFiscMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mitCrearInvoice,
            this.mitNotasCredito,
            this.mitNotasDebito});
            this.ComprobantesFiscMenu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprobantesFiscMenu.Name = "ComprobantesFiscMenu";
            this.ComprobantesFiscMenu.Size = new System.Drawing.Size(155, 23);
            this.ComprobantesFiscMenu.Text = "&Comprobantes Fiscales";
            this.ComprobantesFiscMenu.Visible = false;
            // 
            // mitCrearInvoice
            // 
            this.mitCrearInvoice.Image = ((System.Drawing.Image)(resources.GetObject("mitCrearInvoice.Image")));
            this.mitCrearInvoice.Name = "mitCrearInvoice";
            this.mitCrearInvoice.Size = new System.Drawing.Size(177, 22);
            this.mitCrearInvoice.Text = "Facturas";
            this.mitCrearInvoice.Click += new System.EventHandler(this.mitCrearInvoice_Click);
            // 
            // mitNotasCredito
            // 
            this.mitNotasCredito.Image = ((System.Drawing.Image)(resources.GetObject("mitNotasCredito.Image")));
            this.mitNotasCredito.Name = "mitNotasCredito";
            this.mitNotasCredito.Size = new System.Drawing.Size(177, 22);
            this.mitNotasCredito.Text = "Notas de Crédito";
            this.mitNotasCredito.Click += new System.EventHandler(this.mitNotasCredito_Click);
            // 
            // mitNotasDebito
            // 
            this.mitNotasDebito.Image = ((System.Drawing.Image)(resources.GetObject("mitNotasDebito.Image")));
            this.mitNotasDebito.Name = "mitNotasDebito";
            this.mitNotasDebito.Size = new System.Drawing.Size(177, 22);
            this.mitNotasDebito.Text = "Notas de Debito";
            this.mitNotasDebito.Click += new System.EventHandler(this.mitNotasDebito_Click);
            // 
            // ComprobantesNoFiscMenu
            // 
            this.ComprobantesNoFiscMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mitImprimirUltimaFactura,
            this.mitAnularDocumento,
            this.setearVariableToolStripMenuItem,
            this.reimportacionUltimoDocumentoFiscalToolStripMenuItem});
            this.ComprobantesNoFiscMenu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprobantesNoFiscMenu.Name = "ComprobantesNoFiscMenu";
            this.ComprobantesNoFiscMenu.Size = new System.Drawing.Size(177, 23);
            this.ComprobantesNoFiscMenu.Text = "&Comprobantes No Fiscales";
            // 
            // mitImprimirUltimaFactura
            // 
            this.mitImprimirUltimaFactura.Image = ((System.Drawing.Image)(resources.GetObject("mitImprimirUltimaFactura.Image")));
            this.mitImprimirUltimaFactura.Name = "mitImprimirUltimaFactura";
            this.mitImprimirUltimaFactura.Size = new System.Drawing.Size(309, 22);
            this.mitImprimirUltimaFactura.Text = "Re Imprimir Documento";
            this.mitImprimirUltimaFactura.Click += new System.EventHandler(this.mitImprimirUltimaFactura_Click);
            // 
            // mitAnularDocumento
            // 
            this.mitAnularDocumento.Name = "mitAnularDocumento";
            this.mitAnularDocumento.Size = new System.Drawing.Size(309, 22);
            this.mitAnularDocumento.Text = "Anular Documento Fiscal";
            this.mitAnularDocumento.Click += new System.EventHandler(this.mitAnularDocumento_Click);
            // 
            // setearVariableToolStripMenuItem
            // 
            this.setearVariableToolStripMenuItem.Name = "setearVariableToolStripMenuItem";
            this.setearVariableToolStripMenuItem.Size = new System.Drawing.Size(309, 22);
            this.setearVariableToolStripMenuItem.Text = "Setear Variable";
            this.setearVariableToolStripMenuItem.Click += new System.EventHandler(this.setearVariableToolStripMenuItem_Click);
            // 
            // reimportacionUltimoDocumentoFiscalToolStripMenuItem
            // 
            this.reimportacionUltimoDocumentoFiscalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.facturaToolStripMenuItem,
            this.notaCreditoToolStripMenuItem,
            this.notaDebitoToolStripMenuItem,
            this.reciboToolStripMenuItem});
            this.reimportacionUltimoDocumentoFiscalToolStripMenuItem.Name = "reimportacionUltimoDocumentoFiscalToolStripMenuItem";
            this.reimportacionUltimoDocumentoFiscalToolStripMenuItem.Size = new System.Drawing.Size(309, 22);
            this.reimportacionUltimoDocumentoFiscalToolStripMenuItem.Text = "Re-importacion ultimo documento fiscal";
            // 
            // facturaToolStripMenuItem
            // 
            this.facturaToolStripMenuItem.Name = "facturaToolStripMenuItem";
            this.facturaToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.facturaToolStripMenuItem.Text = "Factura";
            this.facturaToolStripMenuItem.Click += new System.EventHandler(this.facturaToolStripMenuItem_Click);
            // 
            // notaCreditoToolStripMenuItem
            // 
            this.notaCreditoToolStripMenuItem.Name = "notaCreditoToolStripMenuItem";
            this.notaCreditoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.notaCreditoToolStripMenuItem.Text = "Nota Credito";
            this.notaCreditoToolStripMenuItem.Click += new System.EventHandler(this.notaCreditoToolStripMenuItem_Click);
            // 
            // notaDebitoToolStripMenuItem
            // 
            this.notaDebitoToolStripMenuItem.Name = "notaDebitoToolStripMenuItem";
            this.notaDebitoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.notaDebitoToolStripMenuItem.Text = "Nota Debito";
            this.notaDebitoToolStripMenuItem.Click += new System.EventHandler(this.notaDebitoToolStripMenuItem_Click);
            // 
            // reciboToolStripMenuItem
            // 
            this.reciboToolStripMenuItem.Name = "reciboToolStripMenuItem";
            this.reciboToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.reciboToolStripMenuItem.Text = "Recibo";
            this.reciboToolStripMenuItem.Click += new System.EventHandler(this.reciboToolStripMenuItem_Click);
            // 
            // MantenimientoMenu
            // 
            this.MantenimientoMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mantenimientoPeachtreeToolStripMenuItem});
            this.MantenimientoMenu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MantenimientoMenu.Name = "MantenimientoMenu";
            this.MantenimientoMenu.Size = new System.Drawing.Size(107, 23);
            this.MantenimientoMenu.Text = "&Mantenimiento";
            this.MantenimientoMenu.Visible = false;
            // 
            // mantenimientoPeachtreeToolStripMenuItem
            // 
            this.mantenimientoPeachtreeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mantenimientoPeachtreeToolStripMenuItem.Image")));
            this.mantenimientoPeachtreeToolStripMenuItem.Name = "mantenimientoPeachtreeToolStripMenuItem";
            this.mantenimientoPeachtreeToolStripMenuItem.Size = new System.Drawing.Size(279, 22);
            this.mantenimientoPeachtreeToolStripMenuItem.Text = "Mantenimiento Empresa Peachtree";
            this.mantenimientoPeachtreeToolStripMenuItem.Click += new System.EventHandler(this.mantenimientoPeachtreeToolStripMenuItem_Click);
            // 
            // BSCMenu
            // 
            this.BSCMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mantenimientoEmpresaUsuariosToolStripMenuItem});
            this.BSCMenu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BSCMenu.Name = "BSCMenu";
            this.BSCMenu.Size = new System.Drawing.Size(42, 23);
            this.BSCMenu.Text = "BSC";
            this.BSCMenu.Visible = false;
            // 
            // mantenimientoEmpresaUsuariosToolStripMenuItem
            // 
            this.mantenimientoEmpresaUsuariosToolStripMenuItem.Image = global::MOFFIS.Properties.Resources.process_accept;
            this.mantenimientoEmpresaUsuariosToolStripMenuItem.Name = "mantenimientoEmpresaUsuariosToolStripMenuItem";
            this.mantenimientoEmpresaUsuariosToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.mantenimientoEmpresaUsuariosToolStripMenuItem.Text = "Mantenimiento Empresa Usuarios";
            this.mantenimientoEmpresaUsuariosToolStripMenuItem.Click += new System.EventHandler(this.mantenimientoEmpresaUsuariosToolStripMenuItem_Click);
            // 
            // Reportes
            // 
            this.Reportes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generarReportesToolStripMenuItem});
            this.Reportes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Reportes.Name = "Reportes";
            this.Reportes.Size = new System.Drawing.Size(75, 23);
            this.Reportes.Text = "Reportes";
            this.Reportes.Visible = false;
            // 
            // generarReportesToolStripMenuItem
            // 
            this.generarReportesToolStripMenuItem.Name = "generarReportesToolStripMenuItem";
            this.generarReportesToolStripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.generarReportesToolStripMenuItem.Text = "Generar Reportes";
            this.generarReportesToolStripMenuItem.Click += new System.EventHandler(this.generarReportesToolStripMenuItem_Click);
            // 
            // SoporteTecnicoMenu
            // 
            this.SoporteTecnicoMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mitSoporteTecnico});
            this.SoporteTecnicoMenu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SoporteTecnicoMenu.Name = "SoporteTecnicoMenu";
            this.SoporteTecnicoMenu.Size = new System.Drawing.Size(115, 23);
            this.SoporteTecnicoMenu.Text = "Soporte Técnico";
            // 
            // mitSoporteTecnico
            // 
            this.mitSoporteTecnico.Name = "mitSoporteTecnico";
            this.mitSoporteTecnico.Size = new System.Drawing.Size(171, 22);
            this.mitSoporteTecnico.Text = "Soporte Técnico";
            this.mitSoporteTecnico.Click += new System.EventHandler(this.mitSoporteTecnico_Click);
            // 
            // SalirMenu
            // 
            this.SalirMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salirToolStripMenuItem1,
            this.mitCambiarDeUsuario});
            this.SalirMenu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SalirMenu.Name = "SalirMenu";
            this.SalirMenu.Size = new System.Drawing.Size(45, 23);
            this.SalirMenu.Text = "Salir";
            // 
            // salirToolStripMenuItem1
            // 
            this.salirToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("salirToolStripMenuItem1.Image")));
            this.salirToolStripMenuItem1.Name = "salirToolStripMenuItem1";
            this.salirToolStripMenuItem1.Size = new System.Drawing.Size(193, 22);
            this.salirToolStripMenuItem1.Text = "Salir";
            this.salirToolStripMenuItem1.Click += new System.EventHandler(this.salirToolStripMenuItem1_Click);
            // 
            // mitCambiarDeUsuario
            // 
            this.mitCambiarDeUsuario.Name = "mitCambiarDeUsuario";
            this.mitCambiarDeUsuario.Size = new System.Drawing.Size(193, 22);
            this.mitCambiarDeUsuario.Text = "Cambiar de Usuario";
            this.mitCambiarDeUsuario.Click += new System.EventHandler(this.mitCambiarDeUsuario_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.toolStripSeparator1,
            this.printToolStripButton,
            this.printPreviewToolStripButton,
            this.toolStripSeparator2,
            this.helpToolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1134, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "ToolStrip";
            this.toolStrip.Visible = false;
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newToolStripButton.Text = "New";
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "Open";
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "Save";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // printToolStripButton
            // 
            this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
            this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.printToolStripButton.Name = "printToolStripButton";
            this.printToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.printToolStripButton.Text = "Print";
            // 
            // printPreviewToolStripButton
            // 
            this.printPreviewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printPreviewToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripButton.Image")));
            this.printPreviewToolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.printPreviewToolStripButton.Name = "printPreviewToolStripButton";
            this.printPreviewToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.printPreviewToolStripButton.Text = "Print Preview";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // helpToolStripButton
            // 
            this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
            this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.helpToolStripButton.Name = "helpToolStripButton";
            this.helpToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.helpToolStripButton.Text = "Help";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 674);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1277, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::MOFFIS.Properties.Resources.final2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1277, 696);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MOFFIS";
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion



        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem MantenimientoMenu;
        private System.Windows.Forms.ToolStripMenuItem ComprobantesFiscMenu;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripButton printToolStripButton;
        private System.Windows.Forms.ToolStripButton printPreviewToolStripButton;
        private System.Windows.Forms.ToolStripButton helpToolStripButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem mitCrearInvoice;
        private System.Windows.Forms.ToolStripMenuItem mitNotasCredito;
        private System.Windows.Forms.ToolStripMenuItem SalirMenu;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ComprobantesNoFiscMenu;
        private System.Windows.Forms.ToolStripMenuItem mitImprimirUltimaFactura;
        private System.Windows.Forms.ToolStripMenuItem mitNotasDebito;
        private System.Windows.Forms.ToolStripMenuItem mantenimientoPeachtreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BSCMenu;
        private System.Windows.Forms.ToolStripMenuItem mitCambiarDeUsuario;
        private System.Windows.Forms.ToolStripMenuItem mitAnularDocumento;
        private System.Windows.Forms.ToolStripMenuItem mantenimientoEmpresaUsuariosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Reportes;
        private System.Windows.Forms.ToolStripMenuItem generarReportesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SoporteTecnicoMenu;
        private System.Windows.Forms.ToolStripMenuItem mitSoporteTecnico;
        private System.Windows.Forms.ToolStripMenuItem setearVariableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reimportacionUltimoDocumentoFiscalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facturaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notaCreditoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notaDebitoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reciboToolStripMenuItem;
    }
}



