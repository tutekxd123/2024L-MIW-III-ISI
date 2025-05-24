namespace KNN
{
    class Objectknn{
        public List<double> Param = new();
        public string Idklasa = "";
        public Objectknn()
        {
            Param = new List<double>();
            Idklasa = "";
        }
        public void NormalizeData()
        {
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
        public double GetDistanceEukl()
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
