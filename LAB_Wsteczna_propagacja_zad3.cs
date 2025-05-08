using System.Runtime.CompilerServices;

class RandomNumber
{
    public static Random rnd = new Random();
    public static void Shuffle<T>(List<T> array)
    {
        for (int i = array.Count - 1; i >= 0; i--)
        {
            var random = rnd.Next(array.Count);
            (array[i], array[random]) = (array[random], array[i]);
        }
    }
}

class Neuron
{
    public List<double> wagi = new List<double>();
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
    public void CheckError(List<List<double>> probki)
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
            List<double> resultlast = this.getResultLast(Warstwa.Count - 1);
            //Co dalej?
            Console.WriteLine("Dla próbki {0}", string.Join(",", probki[i]));
            for (int j = 0; j < resultlast.Count; j++)
            {
                double tempresult = output[j] - resultlast[j];
                Console.WriteLine("Neuron nr.{0} Bład:{1} Wyjscie: {2}", j, tempresult, resultlast[j]);
            }


        }

    }

    public void TrainNetwork(List<List<double>> probki)
    {
        RandomNumber.Shuffle(probki);
        Console.WriteLine("PRZED UCZENIEM!");
        this.CheckError(probki);

        //Calc ostatnia warstwa
        double paramteruczenia = 0.3;
        //
        for (int i = 0; i < probki.Count; i++)
        {
            this.calcValue(probki[i].Slice(0, this.Warstwa[0].Neurons.Count));
            var goodvalues = probki[i].Slice(this.Warstwa[0].Neurons.Count, probki[i].Count - this.Warstwa[0].Neurons.Count);
            List<double> valueslast = this.getResultLast(Warstwa.Count - 1);
            for (int j = 0; j < valueslast.Count; j++)
            {
                var pochodnabledu = goodvalues[j] - valueslast[j];
                // pochodnabledu * pochodna sigmoid
                double delta = pochodnabledu * this.dsigmoidValue(valueslast[j]); //this.dsigmoidValue(valueslast[j]) //Razy pochodna sigmoida wartosci neurona
                this.Warstwa[Warstwa.Count - 1].Neurons[j].delta = delta;
            }

        }


        //Delta obliczna dla Ostatniej Warstwy? Liczenie Delt Dla Wszystkich Warstw poza wejsciowa
        for (int i = Warstwa.Count - 2; i >= 0; i--) //dla pierwszej warstwy nic nie robimy
        {
            var warstwa = Warstwa[i];
            var warstwanext = Warstwa[i + 1]; //poprzednia warstwa
            for (int j = 0; j < warstwanext.Neurons.Count; j++)
            { //iterujemy po Warstwie ze chcemy obliczyc dla kazdego neuronu delta?
                double sumapoprawek = 0;
                //var poprawkabias = warstwanext.Neurons[j].delta;
                // warstwanext.Neurons[j].extranumber += paramteruczenia* warstwanext.Neurons[j].delta; // Aktualizacja BIAS
                for (int k = 0; k < warstwa.Neurons.Count; k++)
                {
                    sumapoprawek += warstwanext.Neurons[j].wagi[k] * warstwanext.Neurons[j].delta;
                    // var poprawka = warstwanext.Neurons[j].delta * warstwa.Neurons[k].value;
                    //warstwanext.Neurons[j].wagi[k] += poprawka * paramteruczenia; //AKTUALZIACJA WAG!

                }
                warstwa.Neurons[j].delta = sumapoprawek * this.dsigmoidValue(warstwa.Neurons[j].value); // DELTA DLA POPRZEDNIEJ WARSTWY
            }
        }

        for (int i = Warstwa.Count - 2; i >= 0; i--) //Aktualizujemy Wagi bo nie mozna wczesniej tego zrobic bo delta bazuje jednak na wagach :D
        {
            var warstwa = Warstwa[i];
            var warstwanext = Warstwa[i + 1]; //poprzednia warstwa
            for (int j = 0; j < warstwanext.Neurons.Count; j++)
            {
                var poprawkabias = warstwanext.Neurons[j].delta;
                warstwanext.Neurons[j].extranumber += paramteruczenia * warstwanext.Neurons[j].delta;
                for (int k = 0; k < warstwa.Neurons.Count; k++)
                {
                    var poprawka = warstwanext.Neurons[j].delta * warstwa.Neurons[k].value;
                    warstwanext.Neurons[j].wagi[k] += poprawka * paramteruczenia;
                }
            }
        }

        Console.WriteLine("PO UCZENIU!");
        this.CheckError(probki);

    }

}



class Program
{
    static void Main()
    {

        Network test = new Network(new List<int> { 3, 3, 2, 2 });
        List<List<double>> probki = new List<List<double>>()
        {
            new List<double>{0,0,0,0,0},
            new List<double>{0,1,0,1,0},
            new List<double>{1,0,0,1,0},
            new List<double>{1,1,0,0,1},
            new List<double>{0,0,1,1,0},
            new List<double>{0,1,1,0,1},
            new List<double>{1,0,1,0,1},
            new List<double>{1,1,1,1,1},
        };




        for (int i = 0; i < 100000; i++)
        {
            Console.WriteLine("ITERACJA nr.{0}", i);
            test.TrainNetwork(probki);
        }

        test.TrainNetwork(probki);

    }
}
