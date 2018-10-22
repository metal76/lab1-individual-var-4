using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4
{



    public partial class Form1 : Form
    {
        Graphics g;
        Bitmap im;
        Point p;
        Brush MyBrush;
        int w, h;
        List<Point> contur= new List<Point>() ;
        ////////////////////////////////////список точек ребра и многоугольника///////////////////////////
        List<Point> list_polygon = new List<Point>();
        List<Point> edge = new List<Point>(2);
        Point pt = new Point(300,300);
        List<Point> edge2 = new List<Point>(2);

 


        bool cmp(Point a, Point b)
        {
            return a.X < b.X || a.X == b.X && a.Y < b.Y;
        }

        bool cw(Point a, Point b, Point c)
        {
            return a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y) < 0;
        }

        bool ccw(Point a, Point b, Point c)
        {
            return a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y) > 0;
        }

        void sort(List<Point>  a)
        {
            for (int i = 0; i < a.Count; i++)
            {
                for (int j = i+1; j < a.Count; j++)
                {
                    if (! cmp(a[i], a[j]))
                    {
                        Point t = a[i];
                        a[i] = a[j];
                        a[j] = t;
                    }
                }
            }
        }
        void convex_hull(List<Point> a)
        {
            if (a.Count == 1) return;
            
            sort(a);
            Point p1 = a[0], p2 = a.Last();
            List<Point> up = new List<Point>();
            List<Point> down = new List<Point>();
            up.Add(p1);
            down.Add(p1);
            for (int i = 1; i < a.Count(); ++i)
            {
                if (i == a.Count() - 1 || cw(p1, a[i], p2))
                {
                    while (up.Count() >= 2 && !cw(up[up.Count() - 2], up[up.Count() - 1], a[i]))
                        up.RemoveAt(up.Count() - 1);
                    up.Add(a[i]);
                }
                if (i == a.Count() - 1 || ccw(p1, a[i], p2))
                {
                    while (down.Count() >= 2 && !ccw(down[down.Count() - 2], down[down.Count() - 1], a[i]))
                        down.RemoveAt(down.Count() - 1);
                    down.Add(a[i]);
                }
            }
            a.RemoveRange(0, a.Count());
            for (int i = 0; i < up.Count(); ++i)
                a.Add(up[i]);
            for (int i = down.Count() - 2; i > 0; --i)
                a.Add(down[i]);
        }

        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;

            im = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(im);
            pictureBox1.Image = im;
            w = pictureBox1.Width;
            h = pictureBox1.Height;

            MyBrush = Brushes.LightGray;
            p.X = -1;
            p.Y = -1;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {           
                pt.X = e.X;
                pt.Y = e.Y;
                contur.Add(new Point(e.X, e.Y));
                g.DrawLine(new Pen(Color.Black, 5), pt.X - 3, pt.Y - 3, pt.X + 3, pt.Y + 3);
                g.DrawLine(new Pen(Color.Black, 5), pt.X - 3, pt.Y + 3, pt.X + 3, pt.Y - 3);               
                pictureBox1.Refresh();            
        }



       
        private void Erase_graphics()
        {
            pictureBox1.Image = null;
            im = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(im);
            pictureBox1.Image = im;
        }




        private void button2_Click(object sender, EventArgs e)
        {
            if (contur.Count() > 1)
            {
                convex_hull(contur);
                for (int i = 0; i < contur.Count() - 1; ++i)
                {
                    g.DrawLine(new Pen(Color.Green, 4), contur[i].X, contur[i].Y, contur[i + 1].X, contur[i + 1].Y);
                }
                g.DrawLine(new Pen(Color.Green, 4), contur[contur.Count() - 1].X, contur[contur.Count() - 1].Y, contur[0].X, contur[0].Y);
                pictureBox1.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Erase_graphics();
            contur.RemoveRange(0, contur.Count()); ;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                pictureBox1.Image = null;
                im = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                g = Graphics.FromImage(im);
                pictureBox1.Image = im;
            }
        }
    }
}
