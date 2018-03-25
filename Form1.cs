using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CScanner
{
    public partial class Form1 : Form
    {
        private NeironNet net;
        private Point start;
        private int[,] arr;

        private bool enableTrain;

        public Form1()
        {
            InitializeComponent();
            this.enableTrain = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NeironUtill.clearImage(pictureBox1);
            this.net = new NeironNet();

            String[] items = net.getLitaras();
            if (items.Length > 0)
                comboBox1.Items.AddRange(items);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            net.saveState();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point end = new Point(e.X, e.Y);
                Bitmap image = (Bitmap)pictureBox1.Image;

                using (Graphics g = Graphics.FromImage(image))
                {
                    g.DrawLine(new Pen(Color.BlueViolet), this.start, end);
                }

                pictureBox1.Image = image;
                this.start = end;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.start = new Point(e.X, e.Y);
        }

        private void button1_Click(object sender, EventArgs e)//CLEAR
        {
            NeironUtill.clearImage(pictureBox1);
            NeironUtill.clearImage(pictureBox2);
            NeironUtill.clearImage(pictureBox3);
        }

        private void button2_Click(object sender, EventArgs e)//LEARN
        {
            int[,] clip = NeironUtill.cutImage(
                (Bitmap)pictureBox1.Image, new Point(pictureBox1.Width, pictureBox1.Height));

            if (clip == null)
                return;

            this.arr = NeironUtill.laodArray(clip, new int[NeironNet.neironWidth, NeironNet.neironHeight]);
            string litera = net.checkLitera(arr);

            if (litera != null)
            {
                pictureBox2.Image = NeironUtill.getBitmap(clip);
                pictureBox3.Image = NeironUtill.getMemory(net.getByName(litera));
            }
            else
                litera = "null";

            DialogResult askResult =
                MessageBox.Show("Result = " + litera + "?", "", MessageBoxButtons.YesNo);

            if (askResult != DialogResult.Yes || !enableTrain)
                return;

            net.setTrain(litera, arr);
        }

        private void button3_Click(object sender, EventArgs e)//MEMORY
        {
            String litera = comboBox1.SelectedIndex >= 0 ?
                (String)comboBox1.Items[comboBox1.SelectedIndex] :
                comboBox1.Text;

            if (litera.Length == 0)
            {
                MessageBox.Show("LITERA = NULL");
                return;
            }

            net.setTrain(litera, arr);
        }

        private void button4_Click(object sender, EventArgs e)//DRAW
        {
            NeironUtill.clearImage(pictureBox1);
            NeironUtill.clearImage(pictureBox2);
            NeironUtill.clearImage(pictureBox3);

            pictureBox1.Image = NeironUtill.drawLitera(pictureBox1.Image, (String)comboBox1.SelectedItem);
        }

        private void button5_Click(object sender, EventArgs e)//TRAIN
        {
            enableTrain = !enableTrain;
            MessageBox.Show("TRAIN = " + enableTrain.ToString());
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String t = ((TextBox)sender).Text;
                if (t == null || t.Length == 0) return;
                comboBox1.Items.Add(t[0].ToString());
            }
        }
    }
}
