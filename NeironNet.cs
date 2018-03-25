using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace CScanner
{
    class NeironNet
    {
        private const int countNeirons = 32;
        public const int neironWidth = 10;
        public const int neironHeight = 10;
        private const String memory = "memory.txt";

        private List<Neiron> neirons = null;

        public NeironNet()
        {
            this.neirons = init();
        }

        private static List<Neiron> init()
        {
            if (!File.Exists(memory)) 
                return new List<Neiron>();

            String[] lines = File.ReadAllLines(memory);
            if (lines.Length == 0)
                return new List<Neiron>();

            String jsonStr = lines[0];

            JavaScriptSerializer json = new JavaScriptSerializer();
            List<Object> objects = json.Deserialize<List<Object>>(jsonStr);
            List<Neiron> res = new List<Neiron>();

            foreach (var o in objects)
                res.Add(neironCreate((Dictionary<String, Object>)o));

            return res;
        }

        public static Neiron neironCreate(Dictionary<String, Object> o)
        {
            Neiron res = new Neiron();

            res.name = (string)o["name"];
            res.countTrain = (int)o["countTrain"];

            Object[] weightData = (Object[])o["weight"];
            int size = (int)Math.Sqrt(weightData.Length);

            res.weight = new double[size, size];
            
            int index = 0;

            for (int i = 0; i < res.weight.GetLength(0); i++)
            {
                for (int j = 0; j < res.weight.GetLength(1); j++)
                {
                    res.weight[i, j] = Double.Parse(weightData[index].ToString());
                    index++;
                }
            }

            return res;
        }

        public String checkLitera(int[,] arr)
        {
            String res = null;
            double max = 0;

            foreach (var n in this.neirons)
            {
                double d = n.getResult(arr);
                if (d > max)
                {
                    max = d;
                    res = n.name;
                }
            }

            return res;
        }

        public void saveState()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            String jsonStr = json.Serialize(this.neirons);

            System.IO.StreamWriter file = new System.IO.StreamWriter(memory);
            file.WriteLine(jsonStr);
            file.Close();
        }

        public String[] getLitaras()
        {
            var res = new List<String>();
            for (int i = 0; i < this.neirons.Count; i++)
                res.Add(this.neirons[i].name);
            res.Sort();

            return res.ToArray();
        }

        public void setTrain(String trainName, int[,] data)
        {
            Neiron neiron = getByName(trainName);

            if (neiron == null)
                neiron = create(trainName);

            int countTrain = neiron.train(data);

            MessageBox.Show("Litera - " + neiron.name + " count train = " + countTrain.ToString());
        }

        public Neiron getByName(String name)
        {
            return this.neirons.Find(v => v.name.Equals(name));
        }

        public Neiron create(String name)
        {
            Neiron n = new Neiron();
            n.clear(name, neironWidth, neironHeight);
            this.neirons.Add(n);

            return n;
        }
    }
}
