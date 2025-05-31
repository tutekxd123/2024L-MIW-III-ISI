using System.Reflection;
using System.Windows.Forms;
using KNN;

namespace Klasyfikator_k_nn
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var methods = typeof(Metrics).GetMethods().Where(m => m.ReturnType == typeof(double) && m.GetParameters().Length == 3 && m.GetParameters()[0].ParameterType == typeof(Obj)).ToList();
            comboBox1.DataSource = methods;
            comboBox1.DisplayMember = "Name";

        }

        private Objectknn? LoadKnn()
        {

            Objectknn knn = new((int)numericUpDown1.Value);
            knn.parametrmetric = (int)numericUpDown2.Value;
            knn.LoadDataFromString(textBox1.Text);
            if (knn.BaseData.Count <= 0)
            {
                MessageBox.Show("Brak próbek");
                return null;
            }
            knn.NormalizeData();

            return knn;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var stringobj = textBox2.Text;


            var knn = this.LoadKnn();
            if (stringobj == "")
            {
                MessageBox.Show("Brak Podanej Próbki do klasyfikacji");
                return;
            }
            var obj = new Obj(textBox2.Text, 0);
            var SelectedMetric = comboBox1.SelectedItem as MethodInfo;
            if (SelectedMetric != null)
            {
                var SelectedMethod = Delegate.CreateDelegate(typeof(Metrics.DistanceMetrics), SelectedMetric) as Metrics.DistanceMetrics;

                var result = knn.Klasyfikuj(obj, SelectedMethod);
                MessageBox.Show($"Próbka nale¿y do: {result}");
            }
            else
            {
                MessageBox.Show("Z³a metryka");
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var SelectedMetric = comboBox1.SelectedValue as MethodInfo;
            var knn = this.LoadKnn();
            if (SelectedMetric != null && knn != null)
            {
                var SelectedMethod = Delegate.CreateDelegate(typeof(Metrics.DistanceMetrics), SelectedMetric) as Metrics.DistanceMetrics;
                if (SelectedMethod != null)
                {
                    var test = knn.TestOwnKNN(SelectedMethod);
                    textBox3.Text = $"OK: {test[0]} {test[0] * 100 / test[2]}% BAD: {test[1]} {test[1] * 100 / test[2]}%";
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            textBox2.Text = "";
            numericUpDown1.Value = 3;
            numericUpDown2.Value = 1;
            textBox1.Text = "";
        }
    }
}
