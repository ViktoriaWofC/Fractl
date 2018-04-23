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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numberCoefCompress = new System.Windows.Forms.NumericUpDown();
            this.numberCoefCompressBar = new System.Windows.Forms.TrackBar();
            this.buttonCompress = new System.Windows.Forms.Button();
            this.buttonOpenImage = new System.Windows.Forms.Button();
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
            this.label5 = new System.Windows.Forms.Label();
            this.radioButtonGray = new System.Windows.Forms.RadioButton();
            this.radioButtonColors = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxClassif = new System.Windows.Forms.ComboBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxRangNumber = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxSearchDomen = new System.Windows.Forms.ComboBox();
            this.comboBoxColor = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberCoefCompress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberCoefCompressBar)).BeginInit();
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
            this.groupBox1.Controls.Add(this.numberCoefCompress);
            this.groupBox1.Controls.Add(this.numberCoefCompressBar);
            this.groupBox1.Controls.Add(this.buttonCompress);
            this.groupBox1.Controls.Add(this.buttonOpenImage);
            this.groupBox1.Controls.Add(this.numberRangSize);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(423, 140);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Настройки компрессии";
            // 
            // numberCoefCompress
            // 
            this.numberCoefCompress.Location = new System.Drawing.Point(156, 46);
            this.numberCoefCompress.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numberCoefCompress.Name = "numberCoefCompress";
            this.numberCoefCompress.Size = new System.Drawing.Size(120, 20);
            this.numberCoefCompress.TabIndex = 7;
            this.numberCoefCompress.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numberCoefCompress.ValueChanged += new System.EventHandler(this.numberCoefCompress_ValueChanged);
            // 
            // numberCoefCompressBar
            // 
            this.numberCoefCompressBar.Location = new System.Drawing.Point(19, 64);
            this.numberCoefCompressBar.Maximum = 4000;
            this.numberCoefCompressBar.Name = "numberCoefCompressBar";
            this.numberCoefCompressBar.Size = new System.Drawing.Size(389, 45);
            this.numberCoefCompressBar.TabIndex = 6;
            this.numberCoefCompressBar.Value = 2000;
            this.numberCoefCompressBar.ValueChanged += new System.EventHandler(this.numberCoefCompressBar_ValueChanged);
            // 
            // buttonCompress
            // 
            this.buttonCompress.Location = new System.Drawing.Point(207, 109);
            this.buttonCompress.Name = "buttonCompress";
            this.buttonCompress.Size = new System.Drawing.Size(165, 23);
            this.buttonCompress.TabIndex = 5;
            this.buttonCompress.Text = "Компрессия";
            this.buttonCompress.UseVisualStyleBackColor = true;
            this.buttonCompress.Click += new System.EventHandler(this.buttonCompress_Click);
            // 
            // buttonOpenImage
            // 
            this.buttonOpenImage.Location = new System.Drawing.Point(19, 109);
            this.buttonOpenImage.Name = "buttonOpenImage";
            this.buttonOpenImage.Size = new System.Drawing.Size(142, 23);
            this.buttonOpenImage.TabIndex = 4;
            this.buttonOpenImage.Text = "Загрузить изображение";
            this.buttonOpenImage.UseVisualStyleBackColor = true;
            this.buttonOpenImage.Click += new System.EventHandler(this.buttonOpenImage_Click);
            // 
            // numberRangSize
            // 
            this.numberRangSize.Location = new System.Drawing.Point(156, 19);
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
            8,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Коэффициент компрессии:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
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
            this.groupBox2.Location = new System.Drawing.Point(454, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(447, 140);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Настройки декомпрессии";
            // 
            // buttonDecompress
            // 
            this.buttonDecompress.Location = new System.Drawing.Point(78, 109);
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
            this.panelStartImage.Location = new System.Drawing.Point(46, 204);
            this.panelStartImage.Name = "panelStartImage";
            this.panelStartImage.Size = new System.Drawing.Size(338, 400);
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
            this.panelEndImage.Location = new System.Drawing.Point(467, 204);
            this.panelEndImage.Name = "panelEndImage";
            this.panelEndImage.Size = new System.Drawing.Size(338, 400);
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
            this.textBoxTest.Location = new System.Drawing.Point(251, 625);
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
            this.labelCompressCharacteristic.Location = new System.Drawing.Point(36, 615);
            this.labelCompressCharacteristic.Name = "labelCompressCharacteristic";
            this.labelCompressCharacteristic.Size = new System.Drawing.Size(35, 13);
            this.labelCompressCharacteristic.TabIndex = 4;
            this.labelCompressCharacteristic.Text = "label5";
            // 
            // labelDecompressCharacteristic
            // 
            this.labelDecompressCharacteristic.AutoSize = true;
            this.labelDecompressCharacteristic.Location = new System.Drawing.Point(489, 608);
            this.labelDecompressCharacteristic.Name = "labelDecompressCharacteristic";
            this.labelDecompressCharacteristic.Size = new System.Drawing.Size(35, 13);
            this.labelDecompressCharacteristic.TabIndex = 5;
            this.labelDecompressCharacteristic.Text = "label5";
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(124, 642);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 6;
            this.buttonTest.Text = "button1";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Изображение:";
            // 
            // radioButtonGray
            // 
            this.radioButtonGray.AutoSize = true;
            this.radioButtonGray.Checked = true;
            this.radioButtonGray.Location = new System.Drawing.Point(136, 3);
            this.radioButtonGray.Name = "radioButtonGray";
            this.radioButtonGray.Size = new System.Drawing.Size(118, 17);
            this.radioButtonGray.TabIndex = 8;
            this.radioButtonGray.TabStop = true;
            this.radioButtonGray.Text = "В оттенках серого";
            this.radioButtonGray.UseVisualStyleBackColor = true;
            this.radioButtonGray.CheckedChanged += new System.EventHandler(this.radioButtonGray_CheckedChanged);
            // 
            // radioButtonColors
            // 
            this.radioButtonColors.AutoSize = true;
            this.radioButtonColors.Location = new System.Drawing.Point(299, 3);
            this.radioButtonColors.Name = "radioButtonColors";
            this.radioButtonColors.Size = new System.Drawing.Size(68, 17);
            this.radioButtonColors.TabIndex = 8;
            this.radioButtonColors.Text = "Цветное";
            this.radioButtonColors.UseVisualStyleBackColor = true;
            this.radioButtonColors.CheckedChanged += new System.EventHandler(this.radioButtonColors_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(588, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Классификация:";
            // 
            // comboBoxClassif
            // 
            this.comboBoxClassif.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxClassif.FormattingEnabled = true;
            this.comboBoxClassif.Items.AddRange(new object[] {
            "Без классификации",
            "на основе нахождения центра масс",
            "на основе разницы граничных значений"});
            this.comboBoxClassif.Location = new System.Drawing.Point(685, 25);
            this.comboBoxClassif.Name = "comboBoxClassif";
            this.comboBoxClassif.Size = new System.Drawing.Size(201, 21);
            this.comboBoxClassif.TabIndex = 10;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(826, 617);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 49);
            this.button1.TabIndex = 11;
            this.button1.Text = "Показать\r\nдетали";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxRangNumber
            // 
            this.textBoxRangNumber.Location = new System.Drawing.Point(705, 644);
            this.textBoxRangNumber.Name = "textBoxRangNumber";
            this.textBoxRangNumber.Size = new System.Drawing.Size(100, 20);
            this.textBoxRangNumber.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(620, 647);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Номер блока:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(136, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Поиск доменного блока: ";
            // 
            // comboBoxSearchDomen
            // 
            this.comboBoxSearchDomen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSearchDomen.FormattingEnabled = true;
            this.comboBoxSearchDomen.Items.AddRange(new object[] {
            "Первый подходящий (меньше коэффициента компрессии)",
            "Минимальный",
            "Первый подходящий (с разбиаением)",
            "Тестовый (Минимальный)",
            "Несколько эталонов"});
            this.comboBoxSearchDomen.Location = new System.Drawing.Point(155, 25);
            this.comboBoxSearchDomen.Name = "comboBoxSearchDomen";
            this.comboBoxSearchDomen.Size = new System.Drawing.Size(323, 21);
            this.comboBoxSearchDomen.TabIndex = 18;
            this.comboBoxSearchDomen.SelectedIndexChanged += new System.EventHandler(this.comboBoxSearchDomen_SelectedIndexChanged);
            // 
            // comboBoxColor
            // 
            this.comboBoxColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxColor.Enabled = false;
            this.comboBoxColor.FormattingEnabled = true;
            this.comboBoxColor.Items.AddRange(new object[] {
            "RGB",
            "YIQ"});
            this.comboBoxColor.Location = new System.Drawing.Point(366, 2);
            this.comboBoxColor.Name = "comboBoxColor";
            this.comboBoxColor.Size = new System.Drawing.Size(112, 21);
            this.comboBoxColor.TabIndex = 19;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 678);
            this.Controls.Add(this.comboBoxColor);
            this.Controls.Add(this.comboBoxSearchDomen);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxRangNumber);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxClassif);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.radioButtonColors);
            this.Controls.Add(this.radioButtonGray);
            this.Controls.Add(this.label5);
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
            ((System.ComponentModel.ISupportInitialize)(this.numberCoefCompressBar)).EndInit();
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioButtonGray;
        private System.Windows.Forms.RadioButton radioButtonColors;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxClassif;
        private System.Windows.Forms.NumericUpDown numberCoefCompress;
        private System.Windows.Forms.TrackBar numberCoefCompressBar;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxRangNumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxSearchDomen;
        private System.Windows.Forms.ComboBox comboBoxColor;
    }
}

