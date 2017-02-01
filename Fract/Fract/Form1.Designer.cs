namespace Fract
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCompress = new System.Windows.Forms.Button();
            this.buttonOpenImage = new System.Windows.Forms.Button();
            this.numberCoefCompress = new System.Windows.Forms.NumericUpDown();
            this.numberRangSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonDecompress = new System.Windows.Forms.Button();
            this.comboBoxBaseImage = new System.Windows.Forms.ComboBox();
            this.numberIteracDecompr = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelStartImage = new System.Windows.Forms.Panel();
            this.pictureBoxStartImage = new System.Windows.Forms.PictureBox();
            this.panelEndImage = new System.Windows.Forms.Panel();
            this.pictureBoxEndImage = new System.Windows.Forms.PictureBox();
            this.textBoxTest = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.labelCompressCharacteristic = new System.Windows.Forms.Label();
            this.labelDecompressCharacteristic = new System.Windows.Forms.Label();
            this.buttonTest = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberCoefCompress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberRangSize)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberIteracDecompr)).BeginInit();
            this.panelStartImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStartImage)).BeginInit();
            this.panelEndImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEndImage)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonCompress);
            this.groupBox1.Controls.Add(this.buttonOpenImage);
            this.groupBox1.Controls.Add(this.numberCoefCompress);
            this.groupBox1.Controls.Add(this.numberRangSize);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(423, 116);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Настройки компрессии";
            // 
            // buttonCompress
            // 
            this.buttonCompress.Location = new System.Drawing.Point(207, 85);
            this.buttonCompress.Name = "buttonCompress";
            this.buttonCompress.Size = new System.Drawing.Size(165, 23);
            this.buttonCompress.TabIndex = 5;
            this.buttonCompress.Text = "Компрессия";
            this.buttonCompress.UseVisualStyleBackColor = true;
            this.buttonCompress.Click += new System.EventHandler(this.buttonCompress_Click);
            // 
            // buttonOpenImage
            // 
            this.buttonOpenImage.Location = new System.Drawing.Point(19, 85);
            this.buttonOpenImage.Name = "buttonOpenImage";
            this.buttonOpenImage.Size = new System.Drawing.Size(142, 23);
            this.buttonOpenImage.TabIndex = 4;
            this.buttonOpenImage.Text = "Загрузить изображение";
            this.buttonOpenImage.UseVisualStyleBackColor = true;
            this.buttonOpenImage.Click += new System.EventHandler(this.buttonOpenImage_Click);
            // 
            // numberCoefCompress
            // 
            this.numberCoefCompress.Location = new System.Drawing.Point(160, 56);
            this.numberCoefCompress.Maximum = new decimal(new int[] {
            4000000,
            0,
            0,
            0});
            this.numberCoefCompress.Minimum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numberCoefCompress.Name = "numberCoefCompress";
            this.numberCoefCompress.Size = new System.Drawing.Size(120, 20);
            this.numberCoefCompress.TabIndex = 3;
            this.numberCoefCompress.Value = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            // 
            // numberRangSize
            // 
            this.numberRangSize.Location = new System.Drawing.Point(160, 27);
            this.numberRangSize.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numberRangSize.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numberRangSize.Name = "numberRangSize";
            this.numberRangSize.Size = new System.Drawing.Size(120, 20);
            this.numberRangSize.TabIndex = 2;
            this.numberRangSize.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Коэффициент компрессии:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Размер рангового блока:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonDecompress);
            this.groupBox2.Controls.Add(this.comboBoxBaseImage);
            this.groupBox2.Controls.Add(this.numberIteracDecompr);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(454, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(447, 116);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Настройки декомпрессии";
            // 
            // buttonDecompress
            // 
            this.buttonDecompress.Location = new System.Drawing.Point(82, 87);
            this.buttonDecompress.Name = "buttonDecompress";
            this.buttonDecompress.Size = new System.Drawing.Size(165, 23);
            this.buttonDecompress.TabIndex = 6;
            this.buttonDecompress.Text = "Декомпрессия";
            this.buttonDecompress.UseVisualStyleBackColor = true;
            this.buttonDecompress.Click += new System.EventHandler(this.buttonDecompress_Click);
            // 
            // comboBoxBaseImage
            // 
            this.comboBoxBaseImage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBaseImage.FormattingEnabled = true;
            this.comboBoxBaseImage.Items.AddRange(new object[] {
            "Белое",
            "Черное",
            "Клеточка большая",
            "Клеточка маленькая"});
            this.comboBoxBaseImage.Location = new System.Drawing.Point(167, 27);
            this.comboBoxBaseImage.Name = "comboBoxBaseImage";
            this.comboBoxBaseImage.Size = new System.Drawing.Size(121, 21);
            this.comboBoxBaseImage.TabIndex = 3;
            // 
            // numberIteracDecompr
            // 
            this.numberIteracDecompr.Location = new System.Drawing.Point(231, 56);
            this.numberIteracDecompr.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numberIteracDecompr.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberIteracDecompr.Name = "numberIteracDecompr";
            this.numberIteracDecompr.Size = new System.Drawing.Size(120, 20);
            this.numberIteracDecompr.TabIndex = 2;
            this.numberIteracDecompr.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(188, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Количество итераций декопрессии:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Базовое изображение:";
            // 
            // panelStartImage
            // 
            this.panelStartImage.AutoScroll = true;
            this.panelStartImage.Controls.Add(this.pictureBoxStartImage);
            this.panelStartImage.Location = new System.Drawing.Point(46, 161);
            this.panelStartImage.Name = "panelStartImage";
            this.panelStartImage.Size = new System.Drawing.Size(338, 140);
            this.panelStartImage.TabIndex = 2;
            // 
            // pictureBoxStartImage
            // 
            this.pictureBoxStartImage.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxStartImage.Name = "pictureBoxStartImage";
            this.pictureBoxStartImage.Size = new System.Drawing.Size(311, 109);
            this.pictureBoxStartImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxStartImage.TabIndex = 0;
            this.pictureBoxStartImage.TabStop = false;
            // 
            // panelEndImage
            // 
            this.panelEndImage.AutoScroll = true;
            this.panelEndImage.Controls.Add(this.pictureBoxEndImage);
            this.panelEndImage.Location = new System.Drawing.Point(467, 161);
            this.panelEndImage.Name = "panelEndImage";
            this.panelEndImage.Size = new System.Drawing.Size(338, 140);
            this.panelEndImage.TabIndex = 3;
            // 
            // pictureBoxEndImage
            // 
            this.pictureBoxEndImage.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxEndImage.Name = "pictureBoxEndImage";
            this.pictureBoxEndImage.Size = new System.Drawing.Size(311, 109);
            this.pictureBoxEndImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxEndImage.TabIndex = 1;
            this.pictureBoxEndImage.TabStop = false;
            // 
            // textBoxTest
            // 
            this.textBoxTest.Location = new System.Drawing.Point(261, 325);
            this.textBoxTest.Multiline = true;
            this.textBoxTest.Name = "textBoxTest";
            this.textBoxTest.Size = new System.Drawing.Size(353, 41);
            this.textBoxTest.TabIndex = 0;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // labelCompressCharacteristic
            // 
            this.labelCompressCharacteristic.AutoSize = true;
            this.labelCompressCharacteristic.Location = new System.Drawing.Point(46, 315);
            this.labelCompressCharacteristic.Name = "labelCompressCharacteristic";
            this.labelCompressCharacteristic.Size = new System.Drawing.Size(35, 13);
            this.labelCompressCharacteristic.TabIndex = 4;
            this.labelCompressCharacteristic.Text = "label5";
            // 
            // labelDecompressCharacteristic
            // 
            this.labelDecompressCharacteristic.AutoSize = true;
            this.labelDecompressCharacteristic.Location = new System.Drawing.Point(655, 315);
            this.labelDecompressCharacteristic.Name = "labelDecompressCharacteristic";
            this.labelDecompressCharacteristic.Size = new System.Drawing.Size(35, 13);
            this.labelDecompressCharacteristic.TabIndex = 5;
            this.labelDecompressCharacteristic.Text = "label5";
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(134, 342);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 6;
            this.buttonTest.Text = "button1";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 378);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.labelDecompressCharacteristic);
            this.Controls.Add(this.labelCompressCharacteristic);
            this.Controls.Add(this.textBoxTest);
            this.Controls.Add(this.panelEndImage);
            this.Controls.Add(this.panelStartImage);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Фрактальное сжатие";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberCoefCompress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberRangSize)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberIteracDecompr)).EndInit();
            this.panelStartImage.ResumeLayout(false);
            this.panelStartImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStartImage)).EndInit();
            this.panelEndImage.ResumeLayout(false);
            this.panelEndImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEndImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numberRangSize;
        private System.Windows.Forms.NumericUpDown numberCoefCompress;
        private System.Windows.Forms.NumericUpDown numberIteracDecompr;
        private System.Windows.Forms.ComboBox comboBoxBaseImage;
        private System.Windows.Forms.Button buttonOpenImage;
        private System.Windows.Forms.Button buttonCompress;
        private System.Windows.Forms.Button buttonDecompress;
        private System.Windows.Forms.Panel panelStartImage;
        private System.Windows.Forms.Panel panelEndImage;
        private System.Windows.Forms.TextBox textBoxTest;
        private System.Windows.Forms.PictureBox pictureBoxStartImage;
        private System.Windows.Forms.PictureBox pictureBoxEndImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label labelCompressCharacteristic;
        private System.Windows.Forms.Label labelDecompressCharacteristic;
        private System.Windows.Forms.Button buttonTest;
    }
}

