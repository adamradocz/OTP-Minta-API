namespace Otp.WindowsForms
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
            this.labelAddress = new System.Windows.Forms.Label();
            this.textBoxAddress = new System.Windows.Forms.MaskedTextBox();
            this.buttonGetDokumentumok = new System.Windows.Forms.Button();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.buttonDokumentumDownload = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonDokumentumUpload = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.labelSelectedDokumentum = new System.Windows.Forms.Label();
            this.textBoxSelectedDokumentum = new System.Windows.Forms.TextBox();
            this.buttonDokumentumSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Location = new System.Drawing.Point(13, 13);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(60, 17);
            this.labelAddress.TabIndex = 0;
            this.labelAddress.Text = "API Cím:";
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Location = new System.Drawing.Point(79, 10);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(709, 22);
            this.textBoxAddress.TabIndex = 2;
            this.textBoxAddress.Text = "http://localhost/api/dokumentumok/";
            // 
            // buttonGetDokumentumok
            // 
            this.buttonGetDokumentumok.Location = new System.Drawing.Point(16, 81);
            this.buttonGetDokumentumok.Name = "buttonGetDokumentumok";
            this.buttonGetDokumentumok.Size = new System.Drawing.Size(174, 46);
            this.buttonGetDokumentumok.TabIndex = 3;
            this.buttonGetDokumentumok.Text = "Szerver fájlok listázása";
            this.buttonGetDokumentumok.UseVisualStyleBackColor = true;
            this.buttonGetDokumentumok.Click += new System.EventHandler(this.buttonGetDokumentumok_Click);
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxInfo.Location = new System.Drawing.Point(16, 133);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ReadOnly = true;
            this.textBoxInfo.Size = new System.Drawing.Size(772, 305);
            this.textBoxInfo.TabIndex = 4;
            // 
            // buttonDokumentumDownload
            // 
            this.buttonDokumentumDownload.Location = new System.Drawing.Point(196, 81);
            this.buttonDokumentumDownload.Name = "buttonDokumentumDownload";
            this.buttonDokumentumDownload.Size = new System.Drawing.Size(174, 46);
            this.buttonDokumentumDownload.TabIndex = 5;
            this.buttonDokumentumDownload.Text = "Dokumentum letöltése";
            this.buttonDokumentumDownload.UseVisualStyleBackColor = true;
            this.buttonDokumentumDownload.Click += new System.EventHandler(this.buttonDokumentumDownload_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "saveFileDialog";
            // 
            // buttonDokumentumUpload
            // 
            this.buttonDokumentumUpload.Location = new System.Drawing.Point(376, 81);
            this.buttonDokumentumUpload.Name = "buttonDokumentumUpload";
            this.buttonDokumentumUpload.Size = new System.Drawing.Size(174, 46);
            this.buttonDokumentumUpload.TabIndex = 6;
            this.buttonDokumentumUpload.Text = "Dokumentum feltöltése";
            this.buttonDokumentumUpload.UseVisualStyleBackColor = true;
            this.buttonDokumentumUpload.Click += new System.EventHandler(this.buttonDokumentumUpload_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // labelSelectedDokumentum
            // 
            this.labelSelectedDokumentum.AutoSize = true;
            this.labelSelectedDokumentum.Location = new System.Drawing.Point(13, 42);
            this.labelSelectedDokumentum.Name = "labelSelectedDokumentum";
            this.labelSelectedDokumentum.Size = new System.Drawing.Size(167, 17);
            this.labelSelectedDokumentum.TabIndex = 7;
            this.labelSelectedDokumentum.Text = "Feltöltendő dokumentum:";
            // 
            // textBoxSelectedDokumentum
            // 
            this.textBoxSelectedDokumentum.Location = new System.Drawing.Point(186, 38);
            this.textBoxSelectedDokumentum.Name = "textBoxSelectedDokumentum";
            this.textBoxSelectedDokumentum.ReadOnly = true;
            this.textBoxSelectedDokumentum.Size = new System.Drawing.Size(521, 22);
            this.textBoxSelectedDokumentum.TabIndex = 8;
            // 
            // buttonDokumentumSelect
            // 
            this.buttonDokumentumSelect.Location = new System.Drawing.Point(713, 36);
            this.buttonDokumentumSelect.Name = "buttonDokumentumSelect";
            this.buttonDokumentumSelect.Size = new System.Drawing.Size(75, 24);
            this.buttonDokumentumSelect.TabIndex = 10;
            this.buttonDokumentumSelect.Text = "Kiválaszt";
            this.buttonDokumentumSelect.UseVisualStyleBackColor = true;
            this.buttonDokumentumSelect.Click += new System.EventHandler(this.buttonDokumentumSelect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonDokumentumSelect);
            this.Controls.Add(this.textBoxSelectedDokumentum);
            this.Controls.Add(this.labelSelectedDokumentum);
            this.Controls.Add(this.buttonDokumentumUpload);
            this.Controls.Add(this.buttonDokumentumDownload);
            this.Controls.Add(this.textBoxInfo);
            this.Controls.Add(this.buttonGetDokumentumok);
            this.Controls.Add(this.textBoxAddress);
            this.Controls.Add(this.labelAddress);
            this.Name = "Form1";
            this.Text = "Otp Kliens";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAddress;
        private System.Windows.Forms.MaskedTextBox textBoxAddress;
        private System.Windows.Forms.Button buttonGetDokumentumok;
        private System.Windows.Forms.TextBox textBoxInfo;
        private System.Windows.Forms.Button buttonDokumentumDownload;
        private System.Windows.Forms.OpenFileDialog saveFileDialog;
        private System.Windows.Forms.Button buttonDokumentumUpload;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label labelSelectedDokumentum;
        private System.Windows.Forms.TextBox textBoxSelectedDokumentum;
        private System.Windows.Forms.Button buttonDokumentumSelect;
    }
}

