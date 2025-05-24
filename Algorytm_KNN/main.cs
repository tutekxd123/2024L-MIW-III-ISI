namespace KNN
{
    class Params{
        public List<double> Param = new();
        public string Idklasa = "";

    }
    class Objectknn{
        public List<List<double>> Param = new();
        public string Idklasa = "";
        public Objectknn()
        {
            Param = new();
            Idklasa = "";
        }
        public void NormalizeData()
        {
            //calc Wzor
            foreach(var param in Param)
            {
                
            }
        }
        public static void LoadDataFromString(string data)
        {
            
            //Wczytanie Pliku
        }
        public static string getClass()
        {
            return "";
        }
        public static double GetDistanceEukl(List<double>Param1,List<double>otherparam)
        {
            double result = 0;
            for(int i = 0; i < otherparam.Count(); i++)
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
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
