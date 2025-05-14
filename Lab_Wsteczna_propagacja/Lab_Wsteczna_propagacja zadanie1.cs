using System.Runtime.CompilerServices;

class RandomNumber
{
    public static Random rnd = new Random();
    public static void Shuffle<T>(List<T> array)
    {
        for (int i = array.Count - 1; i >= 0; i--)
        {
            var random = rnd.Next(i+1);
            (array[i], array[random]) = (array[random], array[i]);
        }
    }
}

class Neuron
{
    public List<double> wagi = new List<double>();
    public List<double> wagidelta = new List<double>();
    public List<double> deltavalue = new List<double>();

    public double value = 0;
    public double extranumber = RandomNumber.rnd.NextDouble()*2 - 1;
    public double error = 0;
    public double delta = 0;
    public Neuron(List<double> wagi)
    {
        this.wagi = wagi;
    }
    public Neuron(int size)
    {
        for (int i = 0; i < size; i++)
        {
            //wagi.Add(2);
            wagi.Add(RandomNumber.rnd.NextDouble() * 2 - 1);
        }
    }
    public double Sigmoid()
    {
        return 1.0 / (1 + Math.Exp(-(this.value)));
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
            this.Neurons[i].value += this.Neurons[i].extranumber;
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

}
class Network
{
    public List<Warstwa> Warstwa = new List<Warstwa>();
    public double paramteruczenia = 0.1;
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
    public double dsigmoidValue(double numb) //pochodna sigmoida
    {
        return numb * (1 - numb);
    }
    public List<double> getResultLast(int index)
    {
        List<double> result = new List<double>();
        foreach (Neuron neuron in Warstwa[index].Neurons)
        {
            result.Add(neuron.value);
        }
        return result;
    }
    public List<double> CheckError(List<List<double>> probki, int option=0)
    {
        List<double> result = new List<double>();
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
            List<double> resultlast = this.getResultLast(Warstwa.Count - 1);
            //Co dalej?
            if(option==1) Console.WriteLine("Dla próbki {0}", string.Join(",", probki[i]));
            for (int j = 0; j < resultlast.Count; j++)
            {
                double tempresult = Math.Abs(output[j] - resultlast[j]);
                result.Add(tempresult);
                if (option == 1) Console.WriteLine("Neuron nr.{0} Bład:{1} Wyjscie: {2}", j, tempresult, resultlast[j]);
            }


        }
        return result;
    }

    public void TrainNetwork(List<List<double>> probki)
    {
        RandomNumber.Shuffle(probki);
        foreach (var probka in probki)
        {
            this.calcValue(probka.Slice(0, this.Warstwa[0].Neurons.Count));
            var goodvalues = probka.Slice(this.Warstwa[0].Neurons.Count, probka.Count - this.Warstwa[0].Neurons.Count);
            var lastawarstwa = this.Warstwa[this.Warstwa.Count - 1];
            int liczbaWag = lastawarstwa.Neurons[0].wagi.Count;
            var previewwarstaw = this.Warstwa[this.Warstwa.Count - 2];
            List<double> valueslast = this.getResultLast(Warstwa.Count - 1);
            for (int j = 0; j < valueslast.Count; j++)
            {
                double blad = 0;
                blad =(goodvalues[j] - valueslast[j]);
                lastawarstwa.Neurons[j].delta = paramteruczenia * blad * this.dsigmoidValue(this.Warstwa[this.Warstwa.Count - 1].Neurons[j].value);
            }

            for (int j = 0; j < valueslast.Count; j++)
            {

                lastawarstwa.Neurons[j].wagidelta = new List<double>(new double[liczbaWag]);
                lastawarstwa.Neurons[j].deltavalue = new List<double>(new double[liczbaWag]);
                for (int k = 0; k < lastawarstwa.Neurons[j].wagi.Count; k++)
                {
                    lastawarstwa.Neurons[j].wagidelta[k] = lastawarstwa.Neurons[j].delta * lastawarstwa.Neurons[j].wagi[k];
                    lastawarstwa.Neurons[j].deltavalue[k] = lastawarstwa.Neurons[j].delta * previewwarstaw.Neurons[k].value;;
                }
                //delta niech bedzie ze do bias zawsze i juz
            }


            
            //Delta obliczna dla Ostatniej Warstwy? Liczenie Delt Dla Wszystkich Warstw poza wejsciowa
            for (int i = Warstwa.Count - 2; i >= 1; i--) //dla pierwszej warstwy nic nie robimy
            {
                var warstwapreview = Warstwa[i - 1];
                var warstwa = Warstwa[i];
                var warstwanext = Warstwa[i + 1]; //poprzednia warstwa(w sensie blizej konca)
                foreach (var neuron in warstwa.Neurons) neuron.delta = 0; //zerujemy delty na wszelki wypadek przed liczeniem sumy
                for (int j = 0; j < warstwanext.Neurons.Count; j++)
                {
                    for (int x = 0; x < warstwanext.Neurons[j].wagidelta.Count; x++)
                    {
                        warstwa.Neurons[x].delta += warstwanext.Neurons[j].wagidelta[x]; //sumy delt
                    }
                } //z next bierzesz tylko delty sobie sumy
                for(int j = 0; j < warstwa.Neurons.Count; j++)
                {
                    var neuron = warstwa.Neurons[j];
                    neuron.delta *= this.dsigmoidValue(neuron.value);
                    var liczbawag = neuron.wagi.Count;
                    neuron.wagidelta = new List<double>();
                    neuron.deltavalue = new List<double>();
                    for (int x=0;x<neuron.wagi.Count;x++)
                    {
                        neuron.wagidelta.Add(neuron.wagi[x]* neuron.delta);
                        neuron.deltavalue.Add(neuron.delta * warstwapreview.Neurons[x].value);
                    }

                }


            }
            

            //Aktualizacja wag
            foreach(var WarstwaSingle in Warstwa)
            {
                foreach(var neuron in WarstwaSingle.Neurons)
                {
                    for(int i = 0; i < neuron.wagi.Count;i++)
                    {
                        neuron.wagi[i] += neuron.deltavalue[i];
                    }
                    //Akutalizacja bias
                    neuron.extranumber += neuron.delta;
                }
            }

        }
    }

    public bool TrainMore(List<List<double>> probki)
    {
        var errors = CheckError(probki);
        //Metoda odpowiada za sprawdzenie 
        foreach (var error in errors)
        {
            if (error >= 0.3)
            {
                return true;
            }
        }
        return false;
    }
}



class Program
{
    static void Main()
    {

        Network test = new Network(new List<int> { 2, 2, 1 });
        List<List<double>> probki = new List<List<double>>()
        {
            new List<double>{0,0,0},
            new List<double>{1,0,1},
            new List<double>{0,1,1},
            new List<double>{1,1,0}
        };

        //DEBUG WARTOSCI BNOWAK WYKLAD
        /*
        test.Warstwa[1].Neurons[0].wagi = new List<double> { 0.1, 0.2 };
        test.Warstwa[1].Neurons[1].wagi = new List<double> { 0.4, 0.5 };
        test.Warstwa[2].Neurons[0].wagi = new List<double> { 0.7,-0.8};
        test.Warstwa[1].Neurons[0].extranumber = 0.3;
        test.Warstwa[1].Neurons[1].extranumber = 0.6;
        test.Warstwa[2].Neurons[0].extranumber = 0.9;
        */
        Console.WriteLine("###PRZED UCZENIEM###");
        test.CheckError(probki,1);


        
        for (int i = 0; i < 15000; i++)
        {
            test.TrainNetwork(probki);
            var trainmore = test.TrainMore(probki);
            if (trainmore == false)
            {
                Console.WriteLine("Siec nauczyła sie wczesniej iteracja: {0}", i + 1);
                break;
            }

        }
        Console.WriteLine("###PO UCZENIU###");
        test.CheckError(probki,1);


    }
}
