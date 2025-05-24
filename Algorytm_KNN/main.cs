namespace KNN
{
    class Objectknn{
        public List<List<double>> Param = new();
        public string Idklasa = "";
        double distance = 0;
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
        public void LoadDataFromString()
        {
            //Wczytanie Pliku
        }
        public string getClass()
        {
            return "";
        }
        public double GetDistanceEukl(List<double>Param)
        {
            return 0;
        }
        public double GetDistanceManhattan()
        {
            return 0;
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
