﻿namespace Otp.WindowsForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelApiAddress = new System.Windows.Forms.Label();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.labelSelectedDokumentum = new System.Windows.Forms.Label();
            this.textBoxSelectedDokumentum = new System.Windows.Forms.TextBox();
            this.buttonDokumentumSelect = new System.Windows.Forms.Button();
            this.buttonGetDokumentumok = new System.Windows.Forms.Button();
            this.buttonDokumentumDownload = new System.Windows.Forms.Button();
            this.buttonDokumentumUpload = new System.Windows.Forms.Button();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.buttonDokumentumGetSize = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelApiAddress
            // 
            this.labelApiAddress.AutoSize = true;
            this.labelApiAddress.Location = new System.Drawing.Point(12, 9);
            this.labelApiAddress.Name = "labelApiAddress";
            this.labelApiAddress.Size = new System.Drawing.Size(62, 20);
            this.labelApiAddress.TabIndex = 0;
            this.labelApiAddress.Text = "API cím:";
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Location = new System.Drawing.Point(80, 6);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(708, 27);
            this.textBoxAddress.TabIndex = 1;
            this.textBoxAddress.Text = "http://localhost/api/dokumentumok/";
            // 
            // labelSelectedDokumentum
            // 
            this.labelSelectedDokumentum.AutoSize = true;
            this.labelSelectedDokumentum.Location = new System.Drawing.Point(12, 45);
            this.labelSelectedDokumentum.Name = "labelSelectedDokumentum";
            this.labelSelectedDokumentum.Size = new System.Drawing.Size(180, 20);
            this.labelSelectedDokumentum.TabIndex = 2;
            this.labelSelectedDokumentum.Text = "Feltöltendő dokumentum:";
            // 
            // textBoxSelectedDokumentum
            // 
            this.textBoxSelectedDokumentum.Location = new System.Drawing.Point(198, 42);
            this.textBoxSelectedDokumentum.Name = "textBoxSelectedDokumentum";
            this.textBoxSelectedDokumentum.ReadOnly = true;
            this.textBoxSelectedDokumentum.Size = new System.Drawing.Size(490, 27);
            this.textBoxSelectedDokumentum.TabIndex = 3;
            // 
            // buttonDokumentumSelect
            // 
            this.buttonDokumentumSelect.Location = new System.Drawing.Point(694, 41);
            this.buttonDokumentumSelect.Name = "buttonDokumentumSelect";
            this.buttonDokumentumSelect.Size = new System.Drawing.Size(94, 29);
            this.buttonDokumentumSelect.TabIndex = 4;
            this.buttonDokumentumSelect.Text = "Kiválaszt";
            this.buttonDokumentumSelect.UseVisualStyleBackColor = true;
            this.buttonDokumentumSelect.Click += new System.EventHandler(this.buttonDokumentumSelect_Click);
            // 
            // buttonGetDokumentumok
            // 
            this.buttonGetDokumentumok.Location = new System.Drawing.Point(12, 82);
            this.buttonGetDokumentumok.Name = "buttonGetDokumentumok";
            this.buttonGetDokumentumok.Size = new System.Drawing.Size(180, 51);
            this.buttonGetDokumentumok.TabIndex = 5;
            this.buttonGetDokumentumok.Text = "Szerver fájlok listázása";
            this.buttonGetDokumentumok.UseVisualStyleBackColor = true;
            this.buttonGetDokumentumok.Click += new System.EventHandler(this.buttonGetDokumentumok_Click);
            // 
            // buttonDokumentumDownload
            // 
            this.buttonDokumentumDownload.Location = new System.Drawing.Point(198, 82);
            this.buttonDokumentumDownload.Name = "buttonDokumentumDownload";
            this.buttonDokumentumDownload.Size = new System.Drawing.Size(180, 51);
            this.buttonDokumentumDownload.TabIndex = 6;
            this.buttonDokumentumDownload.Text = "Dokumentum letöltése";
            this.buttonDokumentumDownload.UseVisualStyleBackColor = true;
            this.buttonDokumentumDownload.Click += new System.EventHandler(this.buttonDokumentumDownload_Click);
            // 
            // buttonDokumentumUpload
            // 
            this.buttonDokumentumUpload.Location = new System.Drawing.Point(570, 82);
            this.buttonDokumentumUpload.Name = "buttonDokumentumUpload";
            this.buttonDokumentumUpload.Size = new System.Drawing.Size(180, 51);
            this.buttonDokumentumUpload.TabIndex = 7;
            this.buttonDokumentumUpload.Text = "Dokumentum feltöltése";
            this.buttonDokumentumUpload.UseVisualStyleBackColor = true;
            this.buttonDokumentumUpload.Click += new System.EventHandler(this.buttonDokumentumUpload_Click);
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxInfo.Location = new System.Drawing.Point(12, 139);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ReadOnly = true;
            this.textBoxInfo.Size = new System.Drawing.Size(776, 299);
            this.textBoxInfo.TabIndex = 8;
            // 
            // buttonDokumentumGetSize
            // 
            this.buttonDokumentumGetSize.Location = new System.Drawing.Point(384, 82);
            this.buttonDokumentumGetSize.Name = "buttonDokumentumGetSize";
            this.buttonDokumentumGetSize.Size = new System.Drawing.Size(180, 51);
            this.buttonDokumentumGetSize.TabIndex = 9;
            this.buttonDokumentumGetSize.Text = "Dokumentum fájméret lekérdezése";
            this.buttonDokumentumGetSize.UseVisualStyleBackColor = true;
            this.buttonDokumentumGetSize.Click += new System.EventHandler(this.buttonDokumentumGetSize_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonDokumentumGetSize);
            this.Controls.Add(this.textBoxInfo);
            this.Controls.Add(this.buttonDokumentumUpload);
            this.Controls.Add(this.buttonDokumentumDownload);
            this.Controls.Add(this.buttonGetDokumentumok);
            this.Controls.Add(this.buttonDokumentumSelect);
            this.Controls.Add(this.textBoxSelectedDokumentum);
            this.Controls.Add(this.labelSelectedDokumentum);
            this.Controls.Add(this.textBoxAddress);
            this.Controls.Add(this.labelApiAddress);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelApiAddress;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.Label labelSelectedDokumentum;
        private System.Windows.Forms.TextBox textBoxSelectedDokumentum;
        private System.Windows.Forms.Button buttonDokumentumSelect;
        private System.Windows.Forms.Button buttonGetDokumentumok;
        private System.Windows.Forms.Button buttonDokumentumDownload;
        private System.Windows.Forms.Button buttonDokumentumUpload;
        private System.Windows.Forms.TextBox textBoxInfo;
        private System.Windows.Forms.Button buttonDokumentumGetSize;
    }
}

