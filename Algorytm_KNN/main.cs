using Klasyfikator_k_nn;

namespace KNN
{
    class Params
    {
        public List<double> Param = new();
        public string Idklasa = "";
        public Params(List<double> param, string idklasa)
        {
            Param = param;
            Idklasa = idklasa;
        }
    }
    class Objectknn
    {
        public List<Params> Params = new List<Params>();
        public Objectknn()
        {
            Params = new();
        }
        public void TestOwnKNN()
        {
            Console.WriteLine("TEST OWN!");
        }
        public void NormalizeData()
        {
            //get List Param Min Value Max Value
            List<double> minValues = new();
            List<double> maxValues = new();
            //init Min Value,MaxValue
            for (int i = 0; i < Params[0].Param.Count; i++)
            {
                minValues.Add(Params[0].Param[i]);
                maxValues.Add(Params[0].Param[i]);
            }

            for (int i = 0; i < Params.Count; i++)
            {
                for (int j = 0; j < Params[i].Param.Count; j++)
                {
                    if (minValues[j] > Params[i].Param[j])
                    {
                        minValues[j] = Params[i].Param[j];
                    }

                    if (maxValues[j] < Params[i].Param[j])
                    {
                        maxValues[j] = Params[i].Param[j];
                    }
                }
            }
            //OKAY MIN i MAX VALUE MAMY
            //NORMALIZE
            for (int i = 0; i < Params.Count; i++)
            {
                for (int j = 0; j < Params[i].Param.Count; j++)
                {
                    Params[i].Param[j] = (Params[i].Param[j] - minValues[j]) / (maxValues[j] - minValues[j]);
                }
            }
        }
        public void LoadDataFromString(string data)
        {
            this.Params = new();
            var lines = data.Split('\n');
            foreach (var line in lines)
            {
                List<double>dataline = new List<double>();
                string idclass = "";
                try
                {
                    var arraylines = line.Split(' ').ToList();
                    arraylines = arraylines.Select(x => x.Replace('.', ',')).ToList();
                    dataline = arraylines.Slice(0,arraylines.Count()-1).Select(x=>double.Parse(x)).ToList();
                    idclass = arraylines.Last().Replace("\r","");
                }
                catch
                {
                    throw new Exception("Nieprawidłowe Dane wejściowe!");
                }
                Params.Add(new Params(dataline, idclass));
            }
        }
        public static string getClass(Params other,int optioncalc=0)
        {

            return "";
        }
        public static double GetDistanceEukl(List<double> Param1, List<double> otherparam)
        {
            double result = 0;
            for (int i = 0; i < otherparam.Count(); i++)
            {
                result += Math.Pow(otherparam[i] - Param1[i], 2);
            }
            result = Math.Sqrt(result);
            return result;
        }
        public static double GetDistanceManhattan(List<double> Param1, List<double> otherparam)
        {
            double result = 0;
            for (int i = 0; i < otherparam.Count(); i++)
            {
                result += Math.Abs(otherparam[i] - Param1[i]);
            }
            return result;
        }

    }
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
