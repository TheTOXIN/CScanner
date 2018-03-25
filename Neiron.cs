using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CScanner
{
    class Neiron
    {
        public String name { get; set; }
        public double[,] weight { get; set; }
        public int countTrain;

        public Neiron() { }

        public void clear(String name, int x, int y)
        {
            this.name = name;
            this.weight = new double[x, y];

            for (int i = 0; i < this.weight.GetLength(0); i++)
                for (int j = 0; j < this.weight.GetLength(1); j++)
                    this.weight[i, j] = 0;

            this.countTrain = 0;
        }

        public double getResult(int[,] data)
        {
            if (this.weight.GetLength(0) != data.GetLength(0) ||
                this.weight.GetLength(1) != data.GetLength(1)) 
                return -1;

            double res = 0;

            for (int i = 0; i < this.weight.GetLength(0); i++)
                for (int j = 0; j < this.weight.GetLength(1); j++)
                    res += 1 - Math.Abs(this.weight[i, j] - data[i, j]);

            return res / (this.weight.GetLength(0) * weight.GetLength(1));
        }

        public int train(int[,] data)
        {
            if (this.weight.GetLength(0) != data.GetLength(0) ||
                this.weight.GetLength(1) != data.GetLength(1))
                return -1;

            this.countTrain++;

            for (int i = 0; i < this.weight.GetLength(0); i++)
            {
                for (int j = 0; j < this.weight.GetLength(1); j++)
                {
                    double val = data[i, j] == 0 ? 0 : 1;
                    this.weight[i, j] += 2 * (val - 0.5f) / countTrain;
                    if (this.weight[i, j] > 1) this.weight[i, j] = 1;
                    if (this.weight[i, j] < 0) this.weight[i, j] = 0;
                }
            }

            return countTrain;
        }

        public void printWeight()
        {
            for (int i = 0; i < this.weight.GetLength(0); i++)
            {
                for (int j = 0; j < this.weight.GetLength(1); j++)
                {
                    Console.Write(this.weight[i, j].ToString() + "   ");
                }
                Console.WriteLine();
            }
        }
    }
}
