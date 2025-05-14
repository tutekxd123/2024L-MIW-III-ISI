using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
class RandomNumber
{
    public static readonly Random rnd = new Random();
}
class Chrom : ICloneable
{
    public byte chrom;
    public Chrom()
    {
        this.chrom = Convert.ToByte(RandomNumber.rnd.Next(0, 2));
    }
    public Chrom(byte bytee)
    {
        this.chrom = bytee;
    }
    public object Clone()
    {
        return new Chrom(this.chrom);
    }

}
class Param : ICloneable
{
    public List<Chrom> Chroms = new List<Chrom>();

    public Param(int sizeparam = 2)
    {
        for (int i = 0; i < sizeparam; i++)
        {
            Chroms.Add(new Chrom());
        }
    }
    public Param(int sizeparam, double codednumber, double MinNumber, double MaxNumber)
    {
        //Okej mamy wartosc modelu np 1
        //Oblicz wartosc dziesietna np dla 1
        codednumber = Math.Round(((codednumber - MinNumber) / (MaxNumber - MinNumber)) * (Math.Pow(2, sizeparam) - 1));
        //Algorytm z Dziesietnej na Binarne
        byte[] bity = new byte[sizeparam];
        for (int i = 0; codednumber > 1; i++)
        {
            bity[i] = (byte)(codednumber % 2);
            codednumber = codednumber / 2;
        }
        bity = bity.Reverse().ToArray();

        for (int i = 0; i < sizeparam; i++)
        {
            Chroms.Add(new Chrom(bity[i]));

        }
    }
    public Param(List<Chrom> chroms)
    {
        foreach (Chrom chrom in chroms)
        {
            Chroms.Add((Chrom)chrom.Clone());
        }

    }
    public string GetBinaryParm()
    {
        string valuebinary = "";
        foreach (Chrom chrom in this.Chroms)
        {
            valuebinary += chrom.chrom.ToString();
        }
        return valuebinary;
    }
    public Param MixParam(Param other, int randomnumber)//1 z drugim?
    {
        if (Chroms.Count() != other.Chroms.Count())
        {
            throw new ArgumentException("Nie można mieszać innych gatunków");
        }

        //Slice 1 i slice 2 i polaczenie list?
        Param newparam = new Param(Chroms.Count());
        newparam.Chroms = Chroms.Slice(0, randomnumber).Concat(other.Chroms.Slice(randomnumber, Chroms.Count() - randomnumber)).ToList();
        return newparam;

    }

    public void Mutation()
    {
        int randomnumber = RandomNumber.rnd.Next(0, this.Chroms.Count);
        if (this.Chroms[randomnumber].chrom == 0)
        {
            this.Chroms[randomnumber].chrom = 1;
        }
        else
        {
            this.Chroms[randomnumber].chrom = 0;
        }
    }

    public double Decode(double MinNumber, double MaxNumber)
    {
        string valuebinary = "";
        int value;
        foreach (Chrom chrom in this.Chroms)
        {
            valuebinary += chrom.chrom.ToString();
        }
        value = Convert.ToInt32(valuebinary, 2);

        double step = (MaxNumber - MinNumber) / (Math.Pow(2, Chroms.Count()) - 1);
        return ((double)value * step) + MinNumber;
    }


    public object Clone()
    {
        List<Chrom> Chromsslist = new List<Chrom>();
        foreach (Chrom chrom in Chroms)
        {
            Chromsslist.Add(new Chrom(chrom.chrom));
        }
        return new Param(Chromsslist);

    }

}
class Body : ICloneable
{
    List<Param> Params = new List<Param>();
    double MaxNumber;
    double MinNumber;
    public Body(int paramsize = 3, int paramcount = 1, double maxNumber = 2, double MinNumber = -1)
    {
        this.MaxNumber = maxNumber;
        this.MinNumber = MinNumber;
        for (int i = 0; i < paramcount; i++)
        {
            Params.Add(new Param(paramsize));
        }
    }
    public Body(List<double> listofpm, int paramsize = 3, double maxNumber = 2, double MinNumber = -1)
    {
        this.MaxNumber = maxNumber;
        this.MinNumber = MinNumber;
        for (int i = 0; i < listofpm.Count(); i++)
        {
            Params.Add(new Param(paramsize, listofpm[i], MinNumber, maxNumber));
        }
    }
    public Body(List<Param> parms, double maxNumber, double MinNumber)
    {
        this.MaxNumber = maxNumber;
        this.MinNumber = MinNumber;
        Params = parms;
    }

    public object Clone()
    {
        List<Param> Paramtest = new List<Param>();
        foreach (Param parms in Params)
        {
            Paramtest.Add(new Param(parms.Chroms));
        }
        return new Body(Paramtest, this.MaxNumber, this.MinNumber);
    }
    public string getValueParms()
    {
        string result = "";
        for (int i = 0; i < this.Params.Count; i++)
        {
            result += String.Format("Param {0}:{1} Decode: {2}", i, this.Params[i].GetBinaryParm(), Math.Round(this.Params[i].Decode(this.MinNumber, this.MaxNumber), 3));
        }
        return result;
    }
    public void Mutation()
    {
        foreach (Param parm in this.Params)
        {
            parm.Mutation();
        }
    }
    public Body[] GetMix(Body other) //zmiana 2 dzieci
    {

        Body[] result = new Body[2];

        if (this.Params.Count != other.Params.Count)
        {
            throw new ArgumentException("Różne gatunki się nie mieszaja");
        }
        /*
        else if (this.getResult() == other.getResult())
        {
            throw new ArgumentException("Nie można sam ze sobą sie mieszać bo to nie ma sensu");
        }
        */
        //Disabled, zadanie nr.2 punkt.2 
        result[0] = new Body(new List<Param>(), this.MaxNumber, this.MinNumber);
        result[1] = new Body(new List<Param>(), this.MaxNumber, this.MinNumber);
        int randomnumber = RandomNumber.rnd.Next(1, this.Params[0].Chroms.Count());
        for (int i = 0; i < this.Params.Count; i++)
        {
            result[0].Params.Add((Param)this.Params[i].MixParam(other.Params[i], randomnumber).Clone());
            result[1].Params.Add((Param)other.Params[i].MixParam(this.Params[i], randomnumber).Clone());
        }

        //Mutacje? BRAK! Metody narazie
        return result;
    }
    public double getResult() //Model dopasowanie
    {
        if (Params.Count == 2) //Pierwsze Zadanie
        {
            return Math.Sin(Params[0].Decode(this.MinNumber, this.MaxNumber) * 0.05) +
                Math.Sin(Params[1].Decode(this.MinNumber, this.MaxNumber) * 0.05) +
                (0.4 * Math.Sin(Params[0].Decode(this.MinNumber, this.MaxNumber) * 0.15) * Math.Sin(Params[1].Decode(this.MinNumber, this.MaxNumber) * 0.15));

        }
        else if(Params.Count == 3)
        {
            double sumaerror = 0;
            List<double> sinusikexample = new List<double> { 0.64181, 0.68587, 0.44783, 0.40836, 0.38241, -0.05933, -0.12478, -0.36847, -0.39935, -0.50881, -0.63435, -0.59979, -0.64107, -0.51808, -0.38127, -0.12349, -0.09624, 0.27893, 0.48965, 0.33089, 0.70615, 0.53342, 0.43321, 0.64790, 0.48834, 0.18440, -0.02389, -0.10261, -0.33594, -0.35101, -0.62027, -0.55719, -0.66377, -0.62740 };
            for(double i = -0.6,index = 0; i <= 6; i += 0.2,index++)
            {
                double result = Params[0].Decode(this.MinNumber, this.MaxNumber) * (Math.Sin((Params[1].Decode(this.MinNumber, this.MaxNumber) * i) + Params[2].Decode(this.MinNumber, this.MaxNumber)));
                //Mapping 0.6->6 6/0.2
                sumaerror += Math.Pow(result-sinusikexample[(int)index],2);
            }
            return sumaerror;
        }
        else //Prosty Model im wieksza liczba binarna tym lepszy model (srednia!)
        {
            double result = 0;
            foreach (Param param in this.Params)
            {
                result += param.Decode(this.MinNumber, this.MaxNumber);
            }
            result = result / Params.Count();
            return result;
        }
    }
}
class BodyManager : ICloneable
{
    private List<Body> ListBodies = new List<Body>();
    private double MaxNumber;
    private double MinNumber;

    public List<Body> getList()
    {
        return ListBodies;
    }
    public void changelist(List<Body> list)
    {
        this.ListBodies = list;
    }
    public int CountBodies()
    {
        return ListBodies.Count;
    }
    public BodyManager(List<Body> List, double MaxNumber, double MinNumber)
    {
        this.ListBodies = List;
        this.MaxNumber = MaxNumber;
        this.MinNumber = MinNumber;
    }

    //int paramsize = 3,int paramcount=1,double maxNumber=2,double MinNumber=-1
    public BodyManager(int bodysize = 3, int paramsize = 3, int paramcount = 3, double MaxNumber = 2, double MinNumber = -1)
    {
        for (int i = 0; i < bodysize; i++)
        {
            AddBody(new Body(paramsize, paramcount, MaxNumber, MinNumber));
        }
        this.MaxNumber = MaxNumber;
        this.MinNumber = MinNumber;
    }
    public void AddBody(Body other)
    {
        ListBodies.Add((Body)other.Clone());
    }
    public BodyManager(List<List<double>> Listofbodys, int paramsize, double MaxNumber = 2, double MinNumber = -1)
    {
        //(List<double> listofpm,int paramsize=3,double maxNumber=2,double MinNumber = -1)
        for (int i = 0; i < Listofbodys.Count; i++)
        {
            AddBody(new Body(Listofbodys[i], paramsize, MaxNumber, MinNumber));
        }
        this.MaxNumber = MaxNumber;
        this.MinNumber = MinNumber;
    }
    public void showBodies()
    {
        for (int i = 0; i < this.ListBodies.Count; i++)
        {
            Console.WriteLine("Body{0}: Value:{1} Info:{2}", i, Math.Round(this.ListBodies[i].getResult(), 2), this.ListBodies[i].getValueParms());
        }
    }

    public List<Body> SelectBybest(int numberofbest = 3, int option = 1)
    {
        if (this.ListBodies.Count < numberofbest)
        {
            throw new Exception("Error za duzo wybierasz najlepszych mniej niz jest bodies");
        }
        List<Body> bests = new List<Body>();
        bests = this.ListBodies.OrderByDescending(obj => obj.getResult()).TakeLast(numberofbest).ToList(); //Sortuj + bierz 3 pierwsze
        if (option == 0)
        {
            int found = 0;
            foreach (Body best in bests)
            {
                foreach (Body winner in this.ListBodies)
                {
                    if (winner.getResult() == best.getResult())
                    {
                        found = 1;
                        break;
                    }

                }
                if (found == 0)
                {
                    this.AddBody((Body)best.Clone());
                    return bests;
                }
            }
            if (found == 1)
            {
                Console.WriteLine("NOT ADDED?");
            }
        }
        return bests;
    }

    public void Mixing()
    {
        List<Body> toAdd = new List<Body>();
        for (int i = 0; i < ListBodies.Count(); i++)
        {
            for (int j = i; j < ListBodies.Count(); j++)
            {
                if (ListBodies[i].getResult() != ListBodies[j].getResult())
                {
                    var potomkowie = ListBodies[i].GetMix(ListBodies[j]);
                    toAdd.Add((Body)potomkowie[0].Clone());
                    toAdd.Add((Body)potomkowie[1].Clone());
                }
            }
        }
        foreach (Body add in toAdd)
        {
            //ADD UNIKALNI? czy nie?
            this.AddBody((Body)add.Clone());
        }
    }
    public void MutationAll(double chance = 100)
    {
        if (chance < 0)
        {
            chance = 0;
        }
        else if (chance > 100)
        {
            chance = 100;
        }
        foreach (Body body in ListBodies)
        {
            if (RandomNumber.rnd.Next(0, 100) < chance)
            {
                body.Mutation();
            }
        }


    }
    public List<Body> Tournament(int sizeoftour, int countoftour, int option = 0)
    {
        if (this.ListBodies.Count < sizeoftour)
        {
            throw new ArgumentException("Za duży turniej za mało członków na 1 turniej");
        }
        else if (sizeoftour == 1)
        {
            throw new ArgumentException("Turniej jednoosobowy bądz nieparzysta liczba członków");
        }
        else if (sizeoftour <= 0 || countoftour <= 0)
        {
            throw new ArgumentException("Nieprawidłowa rozmiaru turnieju");
        }
        else if (this.ListBodies.Count * sizeoftour < countoftour)
        {
            throw new ArgumentException("Wiecej turniejów niż mozliwych członków");
        }
        List<Body> Winners = new List<Body>();
        for (int i = 0; i < countoftour; i++)
        {
            List<Body> TourMembers = new List<Body>();
            int firstrandom = RandomNumber.rnd.Next(0, sizeoftour);
            TourMembers.Add(this.ListBodies[firstrandom]);
            for (int j = 0; j < sizeoftour; j++)
            {
                //Losuj unikalnych przeciwnikow, jezeli sie uda wylosowac co sie juz jest to losuj jeszcze raz
                int random = RandomNumber.rnd.Next(0, this.ListBodies.Count());
                if (TourMembers.Contains(this.ListBodies[random]))
                {
                    j--;
                    continue;
                }
                TourMembers.Add(this.ListBodies[random]);


            }
            if (TourMembers.Any())
            {
                Winners.Add((Body)TourMembers.MinBy(element => element.getResult()).Clone());
            }
            else
            {
                throw new Exception("Turniej bez zwyciezcow??");
            }

        }
        //Tutaj Dodamy najlepszych z poprzedniej Puli którzy nie sa zwyciezcami;
        if (option == 0)
        {
            this.ListBodies = Winners;
        }
        return Winners;
    }
    public object Clone()
    {
        List<Body> templist = new List<Body>();
        foreach (Body body in this.ListBodies)
        {
            templist.Add((Body)body.Clone());
        }
        return new BodyManager(templist, this.MaxNumber, this.MinNumber);
    }
    public void getBestAndAvg()
    {
        var best = this.SelectBybest(1)[0];
        var avg = this.ListBodies.Average(element => element.getResult());
        Console.WriteLine("Najlepszy Osobnik: {0} Średnie dopasowanie: {1}", best.getResult(), avg);
    }
}


class Program
{
    //test ShowBodies OK!
    //Test SelectbyBest OK! "Hot Deck"
    //Test Mixingeach(PARAM) OK!
    //Test Decode OK!
    //To Do: //Coding(troche nie ma sensu ale mozna zrobic) dobra to w BodyManager damy liste a w body Zrobienie na podstawie Param a w Param dekodowanie na podstawie liczby 
    //Operator tourniejowy do testu
    //Operator mutacji OK!
    //Operator Kodowania OK!

    //Do poprawy po turnieju dodaj do puli Najlepsze te które nie są w zwyciezczach zeby dałoby rade 
    public static void Main()
    {
        BodyManager test = new BodyManager(13, 8, 3, 3, 0);
        for (int i = 0; i < 300; i++)
        {

            Console.WriteLine("Stan Body:");
            test.showBodies();

            Console.WriteLine("Operator Turniej");
            var bests = test.SelectBybest(test.CountBodies(), 1);
            test.Tournament(3, test.CountBodies() - 1);

            var list = test.getList();
            list[0].GetMix(list[1]);
            list[2].GetMix(list[3]);
            list[8].GetMix(list[9]);
            list[list.Count - 2].GetMix(list[list.Count - 1]);
            for(int j = 4; i < list.Count; i++)
            {
                list[j].Mutation();
            }
            //Dodawanie najlepszego hotdeck ze starej listy
            foreach (Body body in bests)
            {
                int found = 0;
                foreach (Body body2 in list)
                {
                    if (body.getResult() == body2.getResult())
                    {
                        found = 1;
                        break;
                    }
                }
                if (found == 0)
                {
                    test.AddBody(body);
                    break;
                }
            }

            test.showBodies();
            Console.WriteLine("Mutation Test");
            test.MutationAll(100);
            test.showBodies();
            Console.WriteLine("Add Best :)");
            
           

            test.showBodies();

            test.getBestAndAvg();
            //Dodać wypisywanie średnie i najlepsze UwU
        }

    }
}
