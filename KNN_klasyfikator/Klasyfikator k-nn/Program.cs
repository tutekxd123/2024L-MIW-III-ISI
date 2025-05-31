using System.Xml.XPath;
using Klasyfikator_k_nn;

namespace KNN
{
    public class Obj
    {
        public List<double> Param = new();
        public string Idklasa = "";
        public Obj(List<double> listparm, string idclass)
        {
            this.Param = listparm;
            this.Idklasa = idclass;
        }
        public Obj(string line,int option=1)
        {
            try
            {
                var arraylines = line.Replace('\t', ' ').Split(' ').ToList();
                arraylines = arraylines.Select(x => x.Replace('.', ',')).ToList();
                var dataline = arraylines.Slice(0, arraylines.Count() - 1).Select(x => double.Parse(x)).ToList();
                var idclass = arraylines.Last().Replace("\r", "");
                if (option == 0)
                {
                    this.Param = arraylines.Select(x=>double.Parse(x)).ToList();
                }
                else
                {
                    this.Idklasa = idclass;
                    this.Param = dataline;
                }

            }
            catch
            {
                throw new Exception("Wystapi³ blad z parsowaniem obj");
            }
        }
    }

    public class Metrics
    {
        public delegate double DistanceMetrics(Obj A, Obj B, double param = 1);

        public static double GetDistanceEukl(Obj A, Obj B, double param = 1)
        {
             if (A.Param.Count != B.Param.Count) throw new Exception("Parametry musza miec tyle samo atrybutów");
            double result = 0;
            for (int i = 0; i < A.Param.Count; i++)
            {
                result += Math.Pow(A.Param[i] - B.Param[i], 2);
            }
            result = Math.Sqrt(result);
            return result;
        }

        public static double GetDistanceCzebyszewa(Obj A, Obj B, double param = 1)
        {
            if (A.Param.Count != B.Param.Count) throw new Exception("Parametry musza miec tyle samo atrybutów");
            double result = 0;
            for (int i = 0; i < A.Param.Count; i++)
            {
                double tempresult = Math.Abs(A.Param[i] - B.Param[i]);
                if (tempresult > result)
                {
                    result = tempresult;
                }
            }
            return result;
        }
        public static double GetDistanceByLog(Obj A, Obj B, double param = 1)
        {
            if (A.Param.Count != B.Param.Count) throw new Exception("Parametry musza miec tyle samo atrybutów");
            double result = 0;
            for (int i = 0; i < A.Param.Count(); i++)
            {
                result += Math.Abs(Math.Log(A.Param[i] - B.Param[i]));
            }
            return result;
        }
        public static double GetDistanceMinkowskiego(Obj A, Obj B, double param = 1)
        {
            if (A.Param.Count != B.Param.Count) throw new Exception("Parametry musza miec tyle samo atrybutów");
            double result = 0;
            for (int i = 0; i < A.Param.Count(); i++)
            {
                result += Math.Pow(Math.Abs(A.Param[i] - B.Param[i]), param);
            }
            result = Math.Pow(result, 1 / param);
            return result;
        }
        public static double GetDistanceManhattan(Obj A, Obj B, double param = 1)
        {
           if (A.Param.Count != B.Param.Count) throw new Exception("Parametry musza miec tyle samo atrybutów");
            double result = 0;
            for (int i = 0; i < A.Param.Count; i++)
            {
                result += Math.Abs(A.Param[i] - B.Param[i]);
            }
            return result;
        }
    }

    public class Distances
    {
        public double distance = 0;
        public string idclass = "";
        public Distances(double distance, string idclass)
        {
            this.distance = distance;
            this.idclass = idclass;
        }
    }
    class Objectknn
    {
        public List<Obj> BaseData = new List<Obj>();
        public List<Distances> distances = new List<Distances>();
        public int kparametr = 0;
        public int parametrmetric = 1;
        public Objectknn(int kparametr=3)
        {
            this.BaseData = new();
            this.kparametr = kparametr;
        }
        public int[] TestOwnKNN(Metrics.DistanceMetrics m)
        {
            int[] status = [0, 0,BaseData.Count];
            for (int i = 0; i < BaseData.Count; i++)
            {
                var objtotest = BaseData[i];
                var Datawithout = BaseData.Where((_, index) => index != i).ToList(); // Usuwanie elementu po indexie
                var objectnewknn = new Objectknn(this.kparametr);
                objectnewknn.BaseData = Datawithout;
                objectnewknn.NormalizeData();
                var probka = objectnewknn.Klasyfikuj(objtotest,m, parametrmetric);
                if (objtotest.Idklasa == probka)
                {
                    status[0]++;
                }
                else
                {
                    status[1]++;
                }
            }
            return status;
        }
        public void NormalizeData()
        {
            //get List Param Min Value Max Value
            List<double> minValues = new();
            List<double> maxValues = new();
            //init Min Value,MaxValue
            for (int i = 0; i < BaseData[0].Param.Count; i++)
            {
                minValues.Add(BaseData[0].Param[i]);
                maxValues.Add(BaseData[0].Param[i]);
            }

            for (int i = 0; i < BaseData.Count; i++)
            {
                for (int j = 0; j < BaseData[i].Param.Count; j++)
                {
                    if (minValues[j] > BaseData[i].Param[j])
                    {
                        minValues[j] = BaseData[i].Param[j];
                    }

                    if (maxValues[j] > BaseData[i].Param[j])
                    {
                        maxValues[j] = BaseData[i].Param[j];
                    }
                }
            }
            //OKAY MIN i MAX VALUE MAMY
            //NORMALIZE
            for (int i = 0; i < BaseData.Count; i++)
            {
                for (int j = 0; j < BaseData[i].Param.Count; j++)
                {
                    if(maxValues[j] - minValues[j] == 0)
                    {
                        continue;
                        //Jezeli max==Min to nie ma sensu normalizowac
                    }
                    BaseData[i].Param[j] = (BaseData[i].Param[j] - minValues[j]) / (maxValues[j] - minValues[j]);
                }
            }
        }
        public void LoadDataFromString(string data)
        {
            this.BaseData = new();
            var lines = data.Split('\n').Where(line=> line!="");//Usutawnie pustych linii
            foreach (var line in lines)
            {
                try
                {
                    BaseData.Add(new Obj(line));
                }
                catch
                {
                    throw new Exception("Nieprawid³owe Dane wejœciowe!");
                }
                
            }
        }

        public string Klasyfikuj(Obj obiekt, Metrics.DistanceMetrics m,double parametr=1)
        {

            // Napisac algorytm Knn, gdzie m u¿ywamy jak metryki np. Euklidesowej
            distances.Clear();
            for(int i = 0; i < this.BaseData.Count; i++)
            {
                distances.Add(new Distances(m(this.BaseData[i], obiekt, parametr), this.BaseData[i].Idklasa));
            }
            
            var result = distances.GroupBy(item => item.idclass).Select(group => { if (group.Count() < this.kparametr) { throw new Exception("K parametr jest wiekszy niz grupa!"); } return new { IdClass = group.Key, Sum = group.OrderBy(item => item.distance).Take(this.kparametr).Sum(item => item.distance) }; }).OrderBy(group=>group.Sum).ToList();
            if (result.Count < 2)
            {
                return result[0].IdClass; //jezeli jest 1 klasa to wywal to :D aczkolwiek to i tak powinno sie sprawdzic wczenisiej
            }
            if (result[0].Sum != result[1].Sum)
            {
                return result[0].IdClass; //Jezeli nie s¹ takie same przypadkiem
            }
            else
            {
                return "Unknow";
            }
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
