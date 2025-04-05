class RandomNumber
{
    public static Random rnd = new Random();
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
        return 1.0 / (1 + Math.Exp(-this.value));
    }

    
}
class Warstwa
{
    public List<Neuron>Neurons = new List<Neuron>();
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
            for(int j = 0; j < this.Neurons[i].wagi.Count; j++)
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

}
class Network
{
    List<Warstwa>Warstwa = new List<Warstwa>();
    public Network(List<int>Params) //2 2 3 ->2 neurony->2 neurony -> 3 neurony?
    {
        Warstwa.Add(new Warstwa(Params[0], 0)); //Entry!
        for (int i = 1; i < Params.Count; i++)
        {
            Warstwa.Add(new Warstwa(Params[i], Params[i-1]));
        }
    }
    public void calcValue(List<double>entry)
    {
        Warstwa[0].changeValue(entry);
        for(int i = 1; i < this.Warstwa.Count; i++)
        {
            Warstwa[i].CalcValue(Warstwa[i - 1]);
        }
    }
    public List<double> getResultLast()
    {
        List<double> result = new List<double>();
        foreach(Neuron neuron in Warstwa[Warstwa.Count - 1].Neurons)
        {
            result.Add(neuron.value);
        }
        return result;
    }
    public List<List<double>> CheckError(List<List<double>> probki)
    {
        List<List<double>> result = new List<List<double>>();
        var lastWarstwa = this.Warstwa[this.Warstwa.Count - 1];
        if (probki[0].Count!=(this.Warstwa[0].Neurons.Count + lastWarstwa.Neurons.Count) )
        {
            throw new Exception("Próbka ma nieprawidlowy rozmiar probek");
        }
        for(int i=0;i<probki.Count;i++)

        {
            //mamy listy
            List<double> input = probki[i].Slice(0, this.Warstwa[0].Neurons.Count);
            List<double> output = probki[i].Slice(this.Warstwa[0].Neurons.Count, lastWarstwa.Neurons.Count);
            this.calcValue(input);
            List<double> resultlast = this.getResultLast();
            //Co dalej?
            result.Add(new List<double>());
            Console.WriteLine("Dla próbki {0}", string.Join(",", probki[i]));
            for (int j=0;j<resultlast.Count;j++){
                double tempresult = Math.Pow(resultlast[j] - output[j], 2);
                Console.WriteLine("Neuron nr.{0} Bład:{1}",j,tempresult);
                lastWarstwa.Neurons[j].error = tempresult;
                result[i].Add(tempresult);
            }
            
            
        }
        
        return result;
    }
}
class Program
{
    static void Main()
    {
        List<List<double>> probki = new List<List<double>>()
        {
            new List<double>{0,0,0},
            new List<double>{1,0,1},
            new List<double>{0,1,1},
            new List<double>{1,1,0}
        };

        Network test = new Network(new List<int> { 2, 1});
        //test.calcValue(new List<double> { 0,1 });
        test.CheckError(probki);

    }
}
