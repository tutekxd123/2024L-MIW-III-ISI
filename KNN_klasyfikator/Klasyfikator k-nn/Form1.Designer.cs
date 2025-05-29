namespace Klasyfikator_k_nn
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
            components = new System.ComponentModel.Container();
            textBox1 = new TextBox();
            label1 = new Label();
            textBox2 = new TextBox();
            label2 = new Label();
            button1 = new Button();
            button2 = new Button();
            comboBox1 = new ComboBox();
            metricsBindingSource1 = new BindingSource(components);
            metricsBindingSource = new BindingSource(components);
            label3 = new Label();
            label4 = new Label();
            numericUpDown1 = new NumericUpDown();
            textBox3 = new TextBox();
            label5 = new Label();
            button3 = new Button();
            label6 = new Label();
            numericUpDown2 = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)metricsBindingSource1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)metricsBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(32, 184);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(330, 254);
            textBox1.TabIndex = 0;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(32, 166);
            label1.Name = "label1";
            label1.Size = new Size(41, 15);
            label1.TabIndex = 1;
            label1.Text = "Próbki";
            label1.Click += label1_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(32, 23);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(330, 23);
            textBox2.TabIndex = 2;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(32, 5);
            label2.Name = "label2";
            label2.Size = new Size(122, 15);
            label2.TabIndex = 3;
            label2.Text = "Próbka do Klasyfikacji";
            // 
            // button1
            // 
            button1.Location = new Point(287, 140);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 4;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(32, 140);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 5;
            button2.Text = "Reset";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "1", "2", "3" });
            comboBox1.Location = new Point(32, 69);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(330, 23);
            comboBox1.TabIndex = 6;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // metricsBindingSource1
            // 
            metricsBindingSource1.DataSource = typeof(KNN.Metrics);
            // 
            // metricsBindingSource
            // 
            metricsBindingSource.DataSource = typeof(KNN.Metrics);
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(32, 51);
            label3.Name = "label3";
            label3.Size = new Size(47, 15);
            label3.TabIndex = 7;
            label3.Text = "Metryki";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(32, 95);
            label4.Name = "label4";
            label4.Size = new Size(68, 15);
            label4.TabIndex = 8;
            label4.Text = "Parametr K:";
            label4.Click += label4_Click;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(106, 93);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(58, 23);
            numericUpDown1.TabIndex = 10;
            numericUpDown1.Value = new decimal(new int[] { 3, 0, 0, 0 });
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(379, 69);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(309, 23);
            textBox3.TabIndex = 11;
            textBox3.TextChanged += textBox3_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(495, 51);
            label5.Name = "label5";
            label5.Size = new Size(55, 15);
            label5.TabIndex = 12;
            label5.Text = "Test KNN";
            // 
            // button3
            // 
            button3.Location = new Point(484, 98);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 13;
            button3.Text = "TEST KNN!";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(32, 122);
            label6.Name = "label6";
            label6.Size = new Size(112, 15);
            label6.TabIndex = 14;
            label6.Text = "Paramtr do Metryki:";
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(150, 120);
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(45, 23);
            numericUpDown2.TabIndex = 15;
            numericUpDown2.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(712, 450);
            Controls.Add(numericUpDown2);
            Controls.Add(label6);
            Controls.Add(button3);
            Controls.Add(label5);
            Controls.Add(textBox3);
            Controls.Add(numericUpDown1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(comboBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)metricsBindingSource1).EndInit();
            ((System.ComponentModel.ISupportInitialize)metricsBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Label label1;
        private TextBox textBox2;
        private Label label2;
        private Button button1;
        private Button button2;
        private ComboBox comboBox1;
        private Label label3;
        private BindingSource metricsBindingSource;
        private BindingSource metricsBindingSource1;
        private Label label4;
        private NumericUpDown numericUpDown1;
        private TextBox textBox3;
        private Label label5;
        private Button button3;
        private Label label6;
        private NumericUpDown numericUpDown2;
    }
}
