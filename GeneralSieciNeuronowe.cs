class RandomNumber
{
    public static Random rnd = new Random();
}

class Neuron
{
    public List<double> wagi = new List<double>();
    public double value = 0;
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
    
}
class Warstwa
{
    List<Neuron>Neurons = new List<Neuron>();
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
}
class Program
{
    static void Main()
    {
        Network test = new Network(new List<int> { 2, 1});
        test.calcValue(new List<double> { 0,1 });
        Console.WriteLine("Test");
    }
}
