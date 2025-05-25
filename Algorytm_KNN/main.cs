namespace KNN
{
    public class Obj
    {
        public List<double> Param = new();
        public string Idklasa = "";
        public Obj(List<double>listparm,string idclass)
        {
            this.Param = listparm;
            this.Idklasa = idclass;
        }
    }

    public class Metrics
    {
        public delegate double DistanceMetrics(Obj A, Obj B, object param = null);

        public static double GetDistanceEukl(Obj A, Obj B, object param = null)
        {
            double result = 0;
            for (int i = 0; i <A.Param.Count; i++)
            {
                result += Math.Pow(A.Param[i] - B.Param[i], 2);
            }
            result = Math.Sqrt(result);            
            return result;
        }

        public static double GetDistanceCzebyszewa(Obj A, Obj B, object param = null)
        {
            if (A.Param.Count != B.Param.Count) throw new Exception("Parametry musza miec tyle samo atrybutów");
            double result = 0;

           return result;
        }
        //public double GetDistanceByLog(Params otherParam)
        //{
        //    if (A.Param.Count != B.Param.Count) throw new Exception("Parametry musza miec tyle samo atrybutów");
        //    double result = 0;
        //    for (int i = 0; i < this.Param.Count(); i++)
        //    {
        //        result += Math.Log(Math.Abs(this.Param[i] - otherParam.Param[i]));
        //    }
        //    this.distance = result;
        //    return result;
        //}
        //public double GetDistanceMinkowskiego(Params otherParam, int parametr)
        //{
        //    if (A.Param.Count != B.Param.Count) throw new Exception("Parametry musza miec tyle samo atrybutów");
        //    double result = 0;
        //    for (int i = 0; i < this.Param.Count(); i++)
        //    {
        //        result += Math.Pow(Math.Abs(this.Param[i] - otherParam.Param[i]), parametr);
        //    }
        //    result = Math.Pow(result, 1 / parametr);
        //    this.distance = result;
        //    return result;
        //}
        //public double GetDistanceManhattan(Params otherParam)
        //{
        //   if (A.Param.Count != B.Param.Count) throw new Exception("Parametry musza miec tyle samo atrybutów");
        //    double result = 0;
        //    for (int i = 0; i < this.Param.Count(); i++)
        //    {
        //        result += Math.Abs(this.Param[i] - otherParam.Param[i]);
        //    }
        //    this.distance = result;
        //    return result;
        //}
    }




    class Objectknn{
        public List<Obj> BaseData = new List<Obj>();
        public int kparametr = 0;
        public Objectknn(int kparametr)
        {
            this.Params = new();
            this.kparametr = kparametr;
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

                    if (maxValues[j] > Params[i].Param[j])
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
                List<double> dataline = new List<double>();
                string idclass = "";
                try
                {
                    var arraylines = line.Split(' ').ToList();
                    arraylines = arraylines.Select(x => x.Replace('.', ',')).ToList();
                    dataline = arraylines.Slice(0, arraylines.Count() - 1).Select(x => double.Parse(x)).ToList();
                    idclass = arraylines.Last().Replace("\r", "");
                }
                catch
                {
                    throw new Exception("Nieprawidłowe Dane wejściowe!");
                }
                BaseData.Add(new Obj());
            }
        }
        public string GetClass(Obj other)
        {
            var methods = typeof(Metrics).GetMethods().Where(m => m.ReturnType == typeof(double) && m.GetParameters().Length == 3 && m.GetParameters()[0].ParameterType == typeof(Obj));
            //var decyzja = Klasyfikuj(asdfa, asdfad, 5, Metrics.GetDistanceEukl);
        }


        public double Klasyfikuj(Obj[] bazaWiedzy, Obj obiekt, int k, Metrics.DistanceMetrics m)
        {
            // Napisac algorytm Knn, gdzie m używamy jak metryki np. Euklidesowej


            var wynik = m(bazaWiedzy[0], obiekt);



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
