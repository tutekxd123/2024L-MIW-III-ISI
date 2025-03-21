
using System.Collections.Generic;

class RandomNumber
{
    public static readonly Random rnd = new Random();
}
class Chrom : ICloneable
{
    private string value = "";
    public string getValue()
    {
        return this.value;
    }
    public int getLength()
    {
        return this.value.Length;
    }
    public Chrom(int size = 4)
    {
        string tempvalue = "";
        for (int i = 0; i < size; i++)
        {
            tempvalue += RandomNumber.rnd.Next(0, 2);
        }
        this.value = tempvalue;

    }
    public Chrom(string value)
    {
        if (value.Length < 1)
        {
            throw new ArgumentException("ERROR Null Value Chrome Construct");
        }
        else
        {
            this.value = value;
        }
    }
    public Chrom GetMix(Chrom chrom2)
    {
        int pos = RandomNumber.rnd.Next(0, this.value.Length);
        string value1 = this.value;
        string value2 = chrom2.getValue();
        string valueresult = value1.Substring(0, pos) + value2.Substring(pos, value2.Length - pos);
        Chrom result = new Chrom(valueresult);
        result.Mutation();
        return result;
    }
    public void Mutation(int option=0)
    {
        int randomnumhappen = RandomNumber.rnd.Next(0, 15);
        if (randomnumhappen != 4 && option==0)
        {
            return;
        }
        int randomnum = RandomNumber.rnd.Next(0, this.value.Length);
        char[] array = this.value.ToCharArray(); //change one char string is immuable
        if (this.value[randomnum] == '0')
        {
            array[randomnum] = '1';
        }
        else
        {
            array[randomnum] = '0';
        }
        //Console.WriteLine("Mutation!");
        this.value = new string(array);
    }

    public object Clone()
    {
        return new Chrom(this.value);
    }
    public double getResult()
    {
        double number = (3 / (Math.Pow(2, this.value.Length) - 1)); //cast to double int always = 0

        double result = (double)Convert.ToInt16(this.value, 2) * number;
        result = result - 2;
        return result;
    }
}

//Chrom jest OK!
class Param : ICloneable
{
    public List<Chrom> Chroms;
    public Param(string[] value)
    {
        Chroms = new List<Chrom>();
        value = value ?? new string[] { "0000" };
        for (int i = 0; i < value.Length; i++)
        {
            Chroms.Add(new Chrom(value[i])); //OK
        }
    }
    public Param Mix(Param other)
    {
        Param result = new Param(this.Chroms[0].getLength(),other.Chroms.Count);
        if (this.Chroms.Count != other.Chroms.Count)
        {
            throw new ArgumentException("Liczba Chromosomow nie zgadza się");
        }
        for (int i = 0; i < Chroms.Count; i++)
        {
            result.Chroms[i] = this.Chroms[i].GetMix(other.Chroms[i]);
        }
        return result;
    }
    public Param(int sizechrom = 4, int countchrom = 1)
    {
        Chroms = new List<Chrom>();
        for (int i = 0; i < countchrom; i++)
        {
            Chroms.Add(new Chrom(sizechrom)); // OK
        }
    }
    public double getResult()
    {
        double result = 0;
        for (int i = 0; i < Chroms.Count; i++)
        {
            result += Chroms[i].getResult();
        }
        return (result / Chroms.Count);
    }
    public object Clone()
    {
        List<Chrom> Chromss= new List<Chrom>(); ;
        Param newparam = new Param();
        foreach (Chrom chrom in this.Chroms)
        {
            Chromss.Add((Chrom)chrom.Clone());
        }
        newparam.Chroms = Chromss;
        return newparam;

    }
}
class Body : ICloneable
{
    private List<Param> Params;
    public Body(string[,] value)//# moment trzeba przemyslec to 2D array
    {
        Params = new List<Param>();
        for (int i = 0; i < value.Length; i++)
        {
            string[] row = new string[value.GetLength(1)]; //2D size
            for (int j = 0; j < value.GetLength(0); j++)
            {
                row[j] = value[i, j];
            }
            Params.Add(new Param(row)); //OK
        }
    }
    public Body(int sizeparams = 4, int sizechrom = 4 , int countchrom=1)
    {
        Params = new List<Param>();
        for (int i = 0; i < sizeparams; i++)
        {
            Params.Add(new Param(sizechrom,countchrom));
        }

    }
    public void deleteparmdebug()
    {
        Params = new List<Param>();
    }
    public object Clone()
    {
        Body clone = new Body();
        clone.Params = new List<Param>();

        foreach (Param param in this.Params)
        {
            clone.Params.Add((Param)param.Clone());
        }

        return clone;
    }
    public double getResult()
    {
        double result = 0;
        for (int i = 0; i < Params.Count; i++)
        {
            result += Params[i].getResult();
        }
        return Math.Round((result / Params.Count), 3);
    }
    public Body mixBody(Body otherbody)
    {
        Body result = new Body(this.getLengthParams(), this.getLengthChrom());
        List<Param> NewParms = new List<Param>();
        if (this.Params.Count != otherbody.getLengthParams())
        {
            throw new ArgumentException("Organizmy innych gatunkow nie mieszają się");
        }

        for (int i = 0; i < this.Params.Count; i++)
        {
            NewParms.Add(this.Params[i].Mix(otherbody.Params[i]));
        }
        result.changeParams(NewParms);
        return result;
    }

    public void Mutation()
    {
        foreach(Param param in Params)
        {
            foreach(Chrom chrom in param.Chroms)
            {
                chrom.Mutation(1);
            }
        }
    }
    private void changeParams(List<Param> ListParam)
    {
        this.Params = ListParam;
    }
    private int getLengthParams()
    {
        return this.Params.Count;
    }
    private int getLengthChrom()
    {
        return this.Params[0].Chroms.Count;
    }
}

class BodyManager
{
    public List<Body> Bodies = new List<Body>();
    public BodyManager(int sizefamily = 5,int chromsize=4,int parmsize=4,int countchrom=4)
    {
        for (int i = 0; i < sizefamily; i++)
        {
            Bodies.Add(new Body(parmsize,chromsize,countchrom));
        }
    }
    public BodyManager(List<Body> bodies)
    {
        Bodies = bodies;
    }

    public void addBodies(Body somebody)
    {
        this.Bodies.Add(somebody);
    }
    public void showBodies()
    {
        for (int i = 0; i < Bodies.Count; i++)
        {
            Console.WriteLine("Body: {0} Result: {1}", i, Bodies[i].getResult());
        }
    }
    private List<Body> Tournament(int sizeintour = 2, int numberoftour = 10)
    {
        if (this.Bodies.Count == 1)
        {
            Console.WriteLine("Warning! Turniej nie ma sensu 1 członek Warning!");
            return new List<Body>() { Bodies[0] };
        }
        else if (this.Bodies.Count == 0)
        {
            throw new ArgumentException("Turniej bez członków");
        }
        else if (this.Bodies.Count * sizeintour < numberoftour)
        {
            throw new ArgumentException("Wiecej turniejów niż mozliwych członków");
        }
        List<Body> Winners = new List<Body>();
        for (int i = 0; i < numberoftour; i++)
        {
            List<Body> tour = new List<Body>();
            while (tour.Count < sizeintour && tour.Count < this.Bodies.Count)
            {

                var numberrandind = RandomNumber.rnd.Next(0, this.Bodies.Count);
                Body Potencjal = (this.Bodies[numberrandind]);
                if (!tour.Contains(Potencjal)) // sam ze soba zeby nie grał
                {
                    tour.Add(Potencjal);
                }
                

            }
            if (tour.Any())
            {
                Winners.Add((Body)tour.MaxBy(element => element.getResult()).Clone() ?? (Body)tour[0].Clone()); //Warning jezeli null Maxby to daj pierwszy element
            }

        }
        return Winners;
    }
    public void getmixeach()
    {
        List<Body> toAdd = new List<Body>();
        for (int i = 0; i < this.Bodies.Count; i++)
        {
            for (int j = 0; j < this.Bodies.Count; j++)
            {
                if (this.Bodies[i] != this.Bodies[j])
                {
                    toAdd.Add(this.Bodies[i].mixBody(this.Bodies[j]));
                }
            }
        }
        foreach (Body body in toAdd)
        {
            addBodies(body);
        }
    }
    public void starttour(int sizeoftour = 2, int numberoftours = 10)
    {
        this.Bodies = this.Tournament(sizeoftour,numberoftours);
    }
    public void selectbybest(int numberofbest = 3)
    {
        if (this.Bodies.Count < numberofbest)
        {
            throw new Exception("Error za duzo wybierasz najlepszych mniej niz jest bodies");
        }
        List<Body> bests = new List<Body>();
        bests = this.Bodies.OrderByDescending(obj => obj.getResult()).Take(numberofbest).ToList(); //Sortuj + bierz 3 pierwsze
        this.Bodies = bests;
    }
    public void Mutation()
    {
        foreach (Body body in this.Bodies)
        {
            body.Mutation();
        }
    }

}

class Program
{
    static void Main()
    {
        var test = new BodyManager(5,5,2,1); //Family Size, Chrom Size, Parm Size, Count Chrom
        Console.WriteLine("Wygenerowane Organizmy");
        test.showBodies();
        for (int i = 0; i < 40; i++)
        {

            Console.WriteLine("Mixing");
            test.getmixeach();
            test.showBodies();
            Console.WriteLine("Po turnieju");
            test.starttour(2, 3); //specialnie 
            //test.selectbybest(10);
            //test.Mutation();
            test.showBodies();
        }
        Console.WriteLine("Wynik Ostateczny");
        test.showBodies();

    }
}
