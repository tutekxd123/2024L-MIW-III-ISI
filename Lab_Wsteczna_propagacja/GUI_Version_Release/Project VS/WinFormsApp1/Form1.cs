using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public void ConsoleBox(string stringtolog, params object[] args)
        {
            textBox2.Text += string.Format(stringtolog + "\r\n", args);
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Parsujemy Dane z programu i fajne byloby obsluzyc wyjatki
            double maxError;
            int maxEpocs;
            double parametruczenia;
            List<List<double>> probki = new List<List<double>>() { };
            List<int> WarstwySieci = new List<int>();
            //Parse MaxError
            string input = textBox4.Text.Replace('.', ',');
            if (!double.TryParse(input, out maxError))
            {
                MessageBox.Show("Zła liczba MaxError");
                return;
            }
            //Warstwy sieci Parse
            input = textBox1.Text;
            foreach (var number in input.Split(' '))
            {
                if (int.TryParse(number, out int liczba))
                {
                    WarstwySieci.Add(liczba);
                }
                else
                {
                    MessageBox.Show($"Error: {number}");
                    return;
                }
            }
            //Parse MaxEpocs
            input = textBox5.Text;
            if (!int.TryParse(input, out maxEpocs))
            {
                MessageBox.Show("Error MxEpocs");
                return;
            }

            //Parse Próbki
            input = textBox3.Text;
            foreach (var probka in input.Split("\r\n"))
            {
                //mamy wiersz tera trzeba split
                var array = probka.Split(' ');
                List<double> probkalist = new List<double>();
                foreach (var number in array)
                {
                    if (double.TryParse(number, out double liczba))
                    {
                        probkalist.Add(liczba);
                    }
                }
                probki.Add(probkalist);
            }
            //Parse Parametr Uczenia
            input = textBox7.Text.Replace('.', ',');
            if (!double.TryParse(input, out parametruczenia))
            {
                MessageBox.Show("Zła liczba parametruczenia");
                return;
            }

            Network test = new Network(WarstwySieci, this, maxError, parametruczenia);
            if (textBox6.Text.Length >= 10)
            {
                test.LoadWagsFromTxt(textBox6.Text);

            }
            /*
            List<List<double>> probki = new List<List<double>>()
            {
                new List<double>{0,0,0},
                new List<double>{1,0,1},
                new List<double>{0,1,1},
                new List<double>{1,1,0}
            };
            */
            this.ConsoleBox("###PRZED UCZENIEM###");
            test.CheckError(probki, 1);

            for (int i = 0; i < maxEpocs; i++)
            {
                var trainmore = test.TrainMore(probki);
                if (trainmore == false)
                {
                    this.ConsoleBox("Siec nauczyła sie wczesniej iteracja: {0}", i + 1);
                    break;
                }
                test.TrainNetwork(probki);

            }
            this.ConsoleBox("###PO UCZENIU###");
            test.CheckError(probki, 1);
            this.ConsoleBox("Dane o Sieci Neuronowej:");
            test.ShowWages();
            var wages = test.getArrayWags();
            string wagesstring = string.Join("\r\n\r\n", wages.Select(item => string.Join("\r\n", item.Select(wagi => string.Join(' ', wagi)))));
            textBox6.Text = wagesstring;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "2 2 1";
            textBox3.Text = "0 0 0\r\n0 1 1\r\n1 0 1\r\n1 1 0";
            textBox4.Text = "0.3";
            textBox5.Text = "150000";
            textBox7.Text = "0.25";
        }



        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "2 2 2 2";
            textBox3.Text = "0 0 0 1\r\n0 1 1 0\r\n1 0 1 0\r\n1 1 0 0";
            textBox4.Text = "0.3";
            textBox7.Text = "0.25";
            textBox5.Text = "150000";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "3 3 2 2";
            textBox3.Text = "0 0 0 0 0\r\n0 1 0 1 0\r\n1 0 0 1 0\r\n1 1 0 0 1\r\n0 0 1 1 0\r\n0 1 1 0 1\r\n1 0 1 0 1\r\n1 1 1 1 1";
            textBox4.Text = "0.4";
            textBox7.Text = "0.25";
            textBox5.Text = "150000";
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
