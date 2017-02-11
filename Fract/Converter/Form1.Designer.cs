namespace Converter
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
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelList = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelRang = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Загрузить файл";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.labelList);
            this.panel1.Location = new System.Drawing.Point(22, 67);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(95, 230);
            this.panel1.TabIndex = 1;
            // 
            // labelList
            // 
            this.labelList.AutoSize = true;
            this.labelList.Location = new System.Drawing.Point(4, 4);
            this.labelList.Name = "labelList";
            this.labelList.Size = new System.Drawing.Size(35, 13);
            this.labelList.TabIndex = 0;
            this.labelList.Text = "label1";
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.labelRang);
            this.panel2.Location = new System.Drawing.Point(151, 67);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(427, 230);
            this.panel2.TabIndex = 2;
            // 
            // labelRang
            // 
            this.labelRang.AutoSize = true;
            this.labelRang.Location = new System.Drawing.Point(4, 4);
            this.labelRang.Name = "labelRang";
            this.labelRang.Size = new System.Drawing.Size(35, 13);
            this.labelRang.TabIndex = 0;
            this.labelRang.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = " Числа     x   y   af   k   x0   y0   br";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Шифр";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(151, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(127, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Расшифровать";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 309);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Converter";
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelRang;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
    }
}

