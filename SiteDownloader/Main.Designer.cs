namespace SiteDownloader
{
    partial class Main
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
            this.BtnBuscar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMáximoDeThreads = new System.Windows.Forms.TextBox();
            this.txtListaDeLinksIgnorados = new System.Windows.Forms.RichTextBox();
            this.txtListaDeLinksParaExplorar = new System.Windows.Forms.RichTextBox();
            this.txtListaDeLinksQuebrados = new System.Windows.Forms.RichTextBox();
            this.txtFiltrarLinksIniciadosCom = new System.Windows.Forms.RichTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnPausar = new System.Windows.Forms.Button();
            this.btnParar = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txtDir = new System.Windows.Forms.TextBox();
            this.btnDir = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnLog = new System.Windows.Forms.Button();
            this.txtListaDominiosEncontrados = new System.Windows.Forms.RichTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnObterDominiosEncontrados = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.txtExtensõesDeArquivos = new System.Windows.Forms.TextBox();
            this.btnResetar = new System.Windows.Forms.Button();
            this.txtListaProblemasEncontrados = new System.Windows.Forms.RichTextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.chkNãoDuplicarArquivos = new System.Windows.Forms.CheckBox();
            this.txtMonitor = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkReiniciarComArquivos = new System.Windows.Forms.CheckBox();
            this.chkIgnoreUrlFramentPart = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFiltrarLinksTerminadosCom = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // BtnBuscar
            // 
            this.BtnBuscar.Location = new System.Drawing.Point(291, 352);
            this.BtnBuscar.Name = "BtnBuscar";
            this.BtnBuscar.Size = new System.Drawing.Size(83, 23);
            this.BtnBuscar.TabIndex = 0;
            this.BtnBuscar.Text = "Buscar";
            this.BtnBuscar.UseVisualStyleBackColor = true;
            this.BtnBuscar.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Links Ignorados:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 255);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Links Quebrados:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Explorar Links:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(291, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Máximo de Threads:";
            // 
            // txtMáximoDeThreads
            // 
            this.txtMáximoDeThreads.Location = new System.Drawing.Point(294, 25);
            this.txtMáximoDeThreads.Name = "txtMáximoDeThreads";
            this.txtMáximoDeThreads.Size = new System.Drawing.Size(100, 20);
            this.txtMáximoDeThreads.TabIndex = 12;
            this.txtMáximoDeThreads.Text = "10";
            this.txtMáximoDeThreads.Validated += new System.EventHandler(this.txtMáximoDeThreads_Validated);
            // 
            // txtListaDeLinksIgnorados
            // 
            this.txtListaDeLinksIgnorados.Location = new System.Drawing.Point(10, 148);
            this.txtListaDeLinksIgnorados.Name = "txtListaDeLinksIgnorados";
            this.txtListaDeLinksIgnorados.Size = new System.Drawing.Size(275, 104);
            this.txtListaDeLinksIgnorados.TabIndex = 14;
            this.txtListaDeLinksIgnorados.Text = "";
            this.txtListaDeLinksIgnorados.WordWrap = false;
            // 
            // txtListaDeLinksParaExplorar
            // 
            this.txtListaDeLinksParaExplorar.Location = new System.Drawing.Point(10, 25);
            this.txtListaDeLinksParaExplorar.Name = "txtListaDeLinksParaExplorar";
            this.txtListaDeLinksParaExplorar.Size = new System.Drawing.Size(275, 104);
            this.txtListaDeLinksParaExplorar.TabIndex = 15;
            this.txtListaDeLinksParaExplorar.Text = "https://lelivros.pro";
            this.txtListaDeLinksParaExplorar.WordWrap = false;
            // 
            // txtListaDeLinksQuebrados
            // 
            this.txtListaDeLinksQuebrados.Location = new System.Drawing.Point(10, 271);
            this.txtListaDeLinksQuebrados.Name = "txtListaDeLinksQuebrados";
            this.txtListaDeLinksQuebrados.Size = new System.Drawing.Size(275, 104);
            this.txtListaDeLinksQuebrados.TabIndex = 16;
            this.txtListaDeLinksQuebrados.Text = "";
            this.txtListaDeLinksQuebrados.WordWrap = false;
            // 
            // txtFiltrarLinksIniciadosCom
            // 
            this.txtFiltrarLinksIniciadosCom.Location = new System.Drawing.Point(291, 271);
            this.txtFiltrarLinksIniciadosCom.Name = "txtFiltrarLinksIniciadosCom";
            this.txtFiltrarLinksIniciadosCom.Size = new System.Drawing.Size(281, 75);
            this.txtFiltrarLinksIniciadosCom.TabIndex = 19;
            this.txtFiltrarLinksIniciadosCom.Text = "https://lelivros.\nhttp://ler-agora.jegueajato.com\nhttp://lelivros.\nhttps://ler-ag" +
    "ora.jegueajato.com";
            this.txtFiltrarLinksIniciadosCom.WordWrap = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(288, 255);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(146, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Aceitar links que iniciem com:";
            // 
            // btnPausar
            // 
            this.btnPausar.Location = new System.Drawing.Point(390, 352);
            this.btnPausar.Name = "btnPausar";
            this.btnPausar.Size = new System.Drawing.Size(83, 23);
            this.btnPausar.TabIndex = 21;
            this.btnPausar.Text = "Pausar";
            this.btnPausar.UseVisualStyleBackColor = true;
            this.btnPausar.Click += new System.EventHandler(this.btnPausar_Click);
            // 
            // btnParar
            // 
            this.btnParar.Location = new System.Drawing.Point(489, 352);
            this.btnParar.Name = "btnParar";
            this.btnParar.Size = new System.Drawing.Size(83, 23);
            this.btnParar.TabIndex = 22;
            this.btnParar.Text = "Parar";
            this.btnParar.UseVisualStyleBackColor = true;
            this.btnParar.Click += new System.EventHandler(this.btnParar_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(288, 216);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(115, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "Pasta para downloads:";
            // 
            // txtDir
            // 
            this.txtDir.Enabled = false;
            this.txtDir.Location = new System.Drawing.Point(291, 232);
            this.txtDir.Name = "txtDir";
            this.txtDir.Size = new System.Drawing.Size(242, 20);
            this.txtDir.TabIndex = 27;
            this.txtDir.Text = "c:\\LeLivros";
            // 
            // btnDir
            // 
            this.btnDir.Location = new System.Drawing.Point(539, 232);
            this.btnDir.Name = "btnDir";
            this.btnDir.Size = new System.Drawing.Size(33, 20);
            this.btnDir.TabIndex = 29;
            this.btnDir.Text = "...";
            this.btnDir.UseVisualStyleBackColor = true;
            this.btnDir.Click += new System.EventHandler(this.btnDir_Click);
            // 
            // btnLog
            // 
            this.btnLog.Location = new System.Drawing.Point(667, 351);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(83, 23);
            this.btnLog.TabIndex = 30;
            this.btnLog.Text = "Gravar Log";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // txtListaDominiosEncontrados
            // 
            this.txtListaDominiosEncontrados.Location = new System.Drawing.Point(870, 25);
            this.txtListaDominiosEncontrados.Name = "txtListaDominiosEncontrados";
            this.txtListaDominiosEncontrados.Size = new System.Drawing.Size(275, 104);
            this.txtListaDominiosEncontrados.TabIndex = 32;
            this.txtListaDominiosEncontrados.Text = "";
            this.txtListaDominiosEncontrados.WordWrap = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(867, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(115, 13);
            this.label12.TabIndex = 31;
            this.label12.Text = "Dominios encontrados:";
            // 
            // btnObterDominiosEncontrados
            // 
            this.btnObterDominiosEncontrados.Location = new System.Drawing.Point(870, 135);
            this.btnObterDominiosEncontrados.Name = "btnObterDominiosEncontrados";
            this.btnObterDominiosEncontrados.Size = new System.Drawing.Size(275, 23);
            this.btnObterDominiosEncontrados.TabIndex = 33;
            this.btnObterDominiosEncontrados.Text = "Obter Domínios Encontrados";
            this.btnObterDominiosEncontrados.UseVisualStyleBackColor = true;
            this.btnObterDominiosEncontrados.Click += new System.EventHandler(this.btnObterDominiosEncontrados_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(291, 51);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(144, 13);
            this.label13.TabIndex = 35;
            this.label13.Text = "Filtrar extensão dos arquivos:";
            // 
            // txtExtensõesDeArquivos
            // 
            this.txtExtensõesDeArquivos.Location = new System.Drawing.Point(294, 67);
            this.txtExtensõesDeArquivos.Name = "txtExtensõesDeArquivos";
            this.txtExtensõesDeArquivos.Size = new System.Drawing.Size(141, 20);
            this.txtExtensõesDeArquivos.TabIndex = 36;
            this.txtExtensõesDeArquivos.Text = ".epub;.mobi;.pdf;.zip";
            // 
            // btnResetar
            // 
            this.btnResetar.Location = new System.Drawing.Point(578, 351);
            this.btnResetar.Name = "btnResetar";
            this.btnResetar.Size = new System.Drawing.Size(83, 23);
            this.btnResetar.TabIndex = 37;
            this.btnResetar.Text = "Resetar";
            this.btnResetar.UseVisualStyleBackColor = true;
            this.btnResetar.Click += new System.EventHandler(this.btnResetar_Click);
            // 
            // txtListaProblemasEncontrados
            // 
            this.txtListaProblemasEncontrados.Location = new System.Drawing.Point(870, 187);
            this.txtListaProblemasEncontrados.Name = "txtListaProblemasEncontrados";
            this.txtListaProblemasEncontrados.Size = new System.Drawing.Size(275, 104);
            this.txtListaProblemasEncontrados.TabIndex = 39;
            this.txtListaProblemasEncontrados.Text = "";
            this.txtListaProblemasEncontrados.WordWrap = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(867, 171);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(121, 13);
            this.label14.TabIndex = 38;
            this.label14.Text = "Problemas encontrados:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(870, 297);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(275, 23);
            this.button1.TabIndex = 40;
            this.button1.Text = "Obter Erros Encontrados";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // chkNãoDuplicarArquivos
            // 
            this.chkNãoDuplicarArquivos.Checked = true;
            this.chkNãoDuplicarArquivos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNãoDuplicarArquivos.Location = new System.Drawing.Point(294, 93);
            this.chkNãoDuplicarArquivos.Name = "chkNãoDuplicarArquivos";
            this.chkNãoDuplicarArquivos.Size = new System.Drawing.Size(141, 48);
            this.chkNãoDuplicarArquivos.TabIndex = 41;
            this.chkNãoDuplicarArquivos.Text = "Não fazer download (nem duplicar) de arquivos já existentes.";
            this.chkNãoDuplicarArquivos.UseVisualStyleBackColor = true;
            this.chkNãoDuplicarArquivos.CheckedChanged += new System.EventHandler(this.chkNãoDuplicarArquivos_CheckedChanged);
            // 
            // txtMonitor
            // 
            this.txtMonitor.Location = new System.Drawing.Point(444, 25);
            this.txtMonitor.Name = "txtMonitor";
            this.txtMonitor.Size = new System.Drawing.Size(180, 104);
            this.txtMonitor.TabIndex = 43;
            this.txtMonitor.Text = "";
            this.txtMonitor.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(441, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 44;
            this.label1.Text = "Monitor:";
            // 
            // chkReiniciarComArquivos
            // 
            this.chkReiniciarComArquivos.Checked = true;
            this.chkReiniciarComArquivos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReiniciarComArquivos.Location = new System.Drawing.Point(291, 184);
            this.chkReiniciarComArquivos.Name = "chkReiniciarComArquivos";
            this.chkReiniciarComArquivos.Size = new System.Drawing.Size(219, 29);
            this.chkReiniciarComArquivos.TabIndex = 45;
            this.chkReiniciarComArquivos.Text = "Reiniciar com informações dos arquivos";
            this.chkReiniciarComArquivos.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreUrlFramentPart
            // 
            this.chkIgnoreUrlFramentPart.Checked = true;
            this.chkIgnoreUrlFramentPart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreUrlFramentPart.Location = new System.Drawing.Point(294, 135);
            this.chkIgnoreUrlFramentPart.Name = "chkIgnoreUrlFramentPart";
            this.chkIgnoreUrlFramentPart.Size = new System.Drawing.Size(150, 23);
            this.chkIgnoreUrlFramentPart.TabIndex = 46;
            this.chkIgnoreUrlFramentPart.Text = "Ignore \'URL Frament\' part";
            this.chkIgnoreUrlFramentPart.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(578, 255);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 13);
            this.label2.TabIndex = 48;
            this.label2.Text = "Ignorar links que terminem com:";
            // 
            // txtFiltrarLinksTerminadosCom
            // 
            this.txtFiltrarLinksTerminadosCom.Location = new System.Drawing.Point(581, 271);
            this.txtFiltrarLinksTerminadosCom.Name = "txtFiltrarLinksTerminadosCom";
            this.txtFiltrarLinksTerminadosCom.Size = new System.Drawing.Size(281, 75);
            this.txtFiltrarLinksTerminadosCom.TabIndex = 47;
            this.txtFiltrarLinksTerminadosCom.Text = "/feed/\n/feed";
            this.txtFiltrarLinksTerminadosCom.WordWrap = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1258, 386);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFiltrarLinksTerminadosCom);
            this.Controls.Add(this.chkIgnoreUrlFramentPart);
            this.Controls.Add(this.chkReiniciarComArquivos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMonitor);
            this.Controls.Add(this.chkNãoDuplicarArquivos);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtListaProblemasEncontrados);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.btnResetar);
            this.Controls.Add(this.txtExtensõesDeArquivos);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnObterDominiosEncontrados);
            this.Controls.Add(this.txtListaDominiosEncontrados);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnLog);
            this.Controls.Add(this.btnDir);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtDir);
            this.Controls.Add(this.btnParar);
            this.Controls.Add(this.btnPausar);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtFiltrarLinksIniciadosCom);
            this.Controls.Add(this.txtListaDeLinksQuebrados);
            this.Controls.Add(this.txtListaDeLinksParaExplorar);
            this.Controls.Add(this.txtListaDeLinksIgnorados);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtMáximoDeThreads);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.BtnBuscar);
            this.Name = "Main";
            this.Text = "WebCrowler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnBuscar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMáximoDeThreads;
        private System.Windows.Forms.RichTextBox txtListaDeLinksIgnorados;
        private System.Windows.Forms.RichTextBox txtListaDeLinksParaExplorar;
        private System.Windows.Forms.RichTextBox txtListaDeLinksQuebrados;
        private System.Windows.Forms.RichTextBox txtFiltrarLinksIniciadosCom;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnPausar;
        private System.Windows.Forms.Button btnParar;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtDir;
        private System.Windows.Forms.Button btnDir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.RichTextBox txtListaDominiosEncontrados;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnObterDominiosEncontrados;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtExtensõesDeArquivos;
        private System.Windows.Forms.Button btnResetar;
        private System.Windows.Forms.RichTextBox txtListaProblemasEncontrados;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkNãoDuplicarArquivos;
        private System.Windows.Forms.RichTextBox txtMonitor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkReiniciarComArquivos;
        private System.Windows.Forms.CheckBox chkIgnoreUrlFramentPart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txtFiltrarLinksTerminadosCom;
    }
}

