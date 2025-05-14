using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

class RandomNumber
{
    public static Random rnd = new Random();
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
    public List<List<double>> probkidotest = new List<List<double>>()
        {
            new List<double>{0,0,0},
            new List<double>{1,0,1},
            new List<double>{0,1,1},
            new List<double>{1,1,0}
        };

    public Network network = new Network(new List<int> { 2,2, 1 });
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
        //Params[0].Decode(this.MinNumber, this.MaxNumber), Params[1].Decode(this.MinNumber, this.MaxNumber)
        //extra numbs=>3 parms
        var wagi = new List<List<double>>();
        wagi.Add(new List<double> { Params[4].Decode(this.MinNumber, this.MaxNumber), Params[5].Decode(this.MinNumber, this.MaxNumber) });
        network.Changewags(2, wagi);

        var wagi2 = new List<List<double>>();
        wagi2.Add(new List<double> { Params[0].Decode(this.MinNumber, this.MaxNumber), Params[1].Decode(this.MinNumber, this.MaxNumber) });
        wagi2.Add(new List<double> { Params[2].Decode(this.MinNumber, this.MaxNumber), Params[3].Decode(this.MinNumber, this.MaxNumber) });

        var extranumbers = new List<List<double>>();
        extranumbers.Add(new List<double> { Params[6].Decode(this.MinNumber, this.MaxNumber) });
        extranumbers.Add(new List<double> { Params[7].Decode(this.MinNumber, this.MaxNumber), Params[8].Decode(this.MinNumber, this.MaxNumber) });

        network.Changewags(1, wagi2);
        network.ChangeextraNumber(1, extranumbers[1]);
        network.ChangeextraNumber(2, extranumbers[0]);
        return network.CheckError(this.probkidotest).SelectMany(x => x).Sum();
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
                if (i == j) continue;
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
            for (int j = 0; j < sizeoftour - 1; j++)
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
        Console.WriteLine("Najlepszy Osobnik: {0} Średnie dopasowanie: {1} {2}", best.getResult(), avg, best.getValueParms());
        var testtest = best.network.CheckError(best.probkidotest);
        Console.WriteLine(":D");
    }
}


class Neuron
{
    public List<double> wagi = new List<double>();
    public double value = 0;
    public double extranumber = 0;
    public double error = 0;
    public Neuron(List<double> wagi)
    {
        this.wagi = wagi;
    }
    public Neuron(int size)
    {
        for (int i = 0; i < size; i++)
        {
            //wagi.Add(2);
            wagi.Add(RandomNumber.rnd.NextDouble());
        }
    }
    public double Sigmoid()
    {
        return 1.0 / (1 + Math.Exp(-(this.value + this.extranumber)));
    }


}
class Warstwa
{
    public List<Neuron> Neurons = new List<Neuron>();
    public Warstwa(List<Neuron> neurons)
    {
        Neurons = neurons;
    }
    public Warstwa(int size, int sizeneuron)
    {
        Neurons = new List<Neuron>();
        for (int i = 0; i < size; i++)
        {
            Neurons.Add(new Neuron(sizeneuron));
        }
    }
    public void ChangeExtraNumber(List<double> extranumbers)
    {
        if (extranumbers.Count != this.Neurons.Count)
        {
            throw new Exception("ChangeExtraNumber Class: Warstwa diffrent length lists");
        }
        for (int i = 0; i < extranumbers.Count; i++)
        {
            this.Neurons[i].extranumber = extranumbers[i];
        }
    }
    public void CalcValue(Warstwa preview)
    {
        foreach (var neuron in Neurons)
        {
            neuron.value = 0; //delete value
        }

        for (int i = 0; i < this.Neurons.Count; i++)
        {
            for (int j = 0; j < this.Neurons[i].wagi.Count; j++)
            {
                this.Neurons[i].value += preview.Neurons[j].value * this.Neurons[i].wagi[j];
            }
            this.Neurons[i].value = this.Neurons[i].Sigmoid(); //Sigmoid aktywacja?
        }
    }
    public void changeValue(List<double> Neuronvalue)
    {
        if (Neuronvalue.Count != this.Neurons.Count)
        {
            throw new Exception("Niewłasciwe dane wejściowe");
        }
        for (int i = 0; i < Neuronvalue.Count; i++)
        {
            Neurons[i].value = Neuronvalue[i];
        }

    }
    public List<List<double>> getWags()
    {
        List<List<double>> result = new List<List<double>>();
        for (int i = 0; i < this.Neurons.Count; i++)
        {
            result.Add(new List<double>());
            for (int j = 0; j < this.Neurons[i].wagi.Count; j++)
            {
                result[i].Add(this.Neurons[i].wagi[j]);
            }
        }
        return result;
    }

}
class Network
{
    public List<Warstwa> Warstwa = new List<Warstwa>();
    public Network(List<int> Params) //2 2 3 ->2 neurony->2 neurony -> 3 neurony?
    {
        Warstwa.Add(new Warstwa(Params[0], 0)); //Entry!
        for (int i = 1; i < Params.Count; i++)
        {
            Warstwa.Add(new Warstwa(Params[i], Params[i - 1]));
        }
    }
    public void calcValue(List<double> entry)
    {
        Warstwa[0].changeValue(entry);
        for (int i = 1; i < this.Warstwa.Count; i++)
        {
            Warstwa[i].CalcValue(Warstwa[i - 1]);
        }
    }
    public List<double> getResultLast()
    {
        List<double> result = new List<double>();
        foreach (Neuron neuron in Warstwa[Warstwa.Count - 1].Neurons)
        {
            result.Add(neuron.value);
        }
        return result;
    }
    public List<List<double>> CheckError(List<List<double>> probki)
    {
        List<List<double>> result = new List<List<double>>();
        var lastWarstwa = this.Warstwa[this.Warstwa.Count - 1];
        if (probki[0].Count != (this.Warstwa[0].Neurons.Count + lastWarstwa.Neurons.Count))
        {
            throw new Exception("Próbka ma nieprawidlowy rozmiar probek");
        }
        for (int i = 0; i < probki.Count; i++)

        {
            //mamy listy
            List<double> input = probki[i].Slice(0, this.Warstwa[0].Neurons.Count);
            List<double> output = probki[i].Slice(this.Warstwa[0].Neurons.Count, lastWarstwa.Neurons.Count);
            this.calcValue(input);
            List<double> resultlast = this.getResultLast();
            //Co dalej?
            result.Add(new List<double>());
            //Console.WriteLine("Dla próbki {0}", string.Join(",", probki[i]));
            for (int j = 0; j < resultlast.Count; j++)
            {
                double tempresult = Math.Pow(resultlast[j] - output[j], 2);
                //Console.WriteLine("Neuron nr.{0} Bład:{1}", j, tempresult);
                lastWarstwa.Neurons[j].error = tempresult;
                result[i].Add(tempresult);
            }
        }
        //dajmy usredniony blad dla zadania
        return result;

    }

    public void ChangeextraNumber(int indexwarstwa, List<double> extranumbers)
    {
        this.Warstwa[indexwarstwa].ChangeExtraNumber(extranumbers);
        //XD
    }
    public List<List<List<Double>>> GetWags()
    {
        List<List<List<Double>>> result = new List<List<List<double>>>();
        for (int i = 0; i < this.Warstwa.Count; i++)
        {
            result.Add(this.Warstwa[i].getWags());
        }
        return result;
        //Wszystkie Warstwy wszystkie wyniki?
    }
    public void Changewags(int indexwarstwa, List<List<double>> Wags)
    {
        for (var i = 0; i < Wags.Count; i++)
        {
            this.Warstwa[indexwarstwa].Neurons[i].wagi = Wags[i];
        }
    }
    public void CalcPoprawka(double paramLearning, List<double> expectresult)
    {
        //1. Wez ostatnia warstwe
        //Dla ostatniej warstwy wylicz koretky wyjscia
        //Zwiększ wage o korekte? (tego nie wiem?)
        List<double> corrections = new List<double>();
        var lastWarstwa = this.Warstwa[this.Warstwa.Count - 1];
        for (int i = 0; i < lastWarstwa.Neurons.Count; i++)
        {
            corrections.Add(paramLearning * (expectresult[i] - lastWarstwa.Neurons[i].value));
        }

    }
}
class Program
{
    static void Main()
    {
        BodyManager test = new BodyManager(16, 16, 9, -10, 10);
        for (int i = 0; i < 15000; i++)
        {

            //Console.WriteLine("Stan Body:");
            //test.showBodies();
            var bests = test.SelectBybest(test.CountBodies(), 1);
            //Console.WriteLine("Operator Turniej");
            test.Tournament(3, test.CountBodies() - 1);



            test.MutationAll(15);

            var list = test.getList();
            list[0].GetMix(list[1]);
            list[2].GetMix(list[3]);
            list[9].GetMix(list[10]);
            list[list.Count - 2].GetMix(list[list.Count - 1]);
            for (int j = 4; i < list.Count; i++)
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

            //test.showBodies();
            // Console.WriteLine("Mutation Test");
            //test.MutationAll(100);
            // test.showBodies();
            // Console.WriteLine("Add Best :)");



            // test.showBodies();
            //test.getBestAndAvg();
            //Dodać wypisywanie średnie i najlepsze UwU
        }
        test.getBestAndAvg();
        //test.calcValue(new List<double> { 0,1 });

        var bestbody = test.SelectBybest(1, 1);
        bestbody[0].network.calcValue(new List<double> { 0, 1 });
        bestbody[0].network.calcValue(new List<double> { 1, 0 });
        bestbody[0].network.calcValue(new List<double> { 1, 1 });
        bestbody[0].network.calcValue(new List<double> { 0, 0 });
    }

}
