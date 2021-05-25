using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DBinPacking_Graphics
{
    public partial class Form1 : Form
    {
        private static Random rnd = new Random();
        //輸入矩形長寬與想要切割的次數
        public static int W = 500;
        public static int H = 500;
        public static int CuttingTimes = 3;

        public Graphics g;
        Font drawFont = new Font("Arial", 12);
        List<Rect> list = Cutting(new Rect(0, 0, 0, W, H), CuttingTimes);

        public Form1()
        {
            InitializeComponent();
            this.Width = W + 500;
            this.Height = H + 400;
            RandomPacking.Location = new Point(W + 200, 200);
            label1.Location = new Point(W + 200, 400);
        }
        private void Start_Click(object sender, EventArgs e)
        {
            Start.Visible = false;
            RandomPacking.Visible = true;
            
            g = this.CreateGraphics();
            foreach (Rect r in list)
            {
                Pen pen = new Pen(Color.FromArgb(128, rnd.Next(2, 255), rnd.Next(2, 255), rnd.Next(2, 255)), 2);
                SolidBrush brush = new SolidBrush(Color.FromArgb(128, rnd.Next(2, 255), rnd.Next(2, 255), rnd.Next(2, 255)));
                //Rectangle rect = TransformForCutting(r);
                //g.DrawRectangle(pen, rect);
                //g.DrawString("" + r.ID, drawFont, new SolidBrush(Color.Black), rect.X + 2, rect.Y + 2);
                g.DrawRectangle(pen, new Rectangle(r.X, r.Y, r.Width, r.Height));
                g.DrawString("" + r.ID, drawFont, new SolidBrush(Color.Black), r.X + 2, r.Y + 2);
            }
            g.Dispose();
        }

        private static Rectangle TransformForCutting(Rect r)
        {
            return new Rectangle(r.X, H - r.Y - r.Height, r.Width, r.Height);
        }
        private static List<Rect> Cutting(Rect r, int N)
        {
            List<Rect> list = new List<Rect>();
            list.Add(r);
            int flag = rnd.Next(0, 2);
            for (int i = 1; i < N; i++)
            {
                for (int j = 1; j <= (int)Math.Pow(2, i); j += 2)
                {
                    Rect p = list[j - 1];
                    if (flag == 0 && p.Width > 1)
                    {
                        int w;
                        do { w = rnd.Next(p.X + p.Width / 3, p.Width + p.X - p.Width / 3); }
                        while (w - p.X == 0);
                        list[j - 1] = new Rect(j, p.X, p.Y, w - p.X, p.Height);
                        list.Insert(j, new Rect(j + 1, w, p.Y, p.Width - list[j - 1].Width, p.Height));
                        flag = 1;
                    }
                    else if (flag == 1 && p.Height > 1)
                    {
                        int h;
                        do { h = rnd.Next(p.Y + p.Height / 3, p.Height + p.Y - p.Height / 3); }
                        while (h - p.Y == 0);
                        list[j - 1] = new Rect(j, p.X, p.Y, p.Width, h - p.Y);
                        list.Insert(j, new Rect(j + 1, p.X, h, p.Width, p.Height - list[j - 1].Height));
                        flag = 0;
                    }
                    else { break; }
                }
            }
            return list;
        }
        private static List<Rect> Shuffle(List<Rect> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                Rect value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
        private static List<Rect> Placement(List<Rect> rects, List<Rect> place)
        {
            rects[0].X = 0;
            rects[0].Y = 0;
            place.Add(rects[0]);
            for (int i = 1; i < rects.Count; i++)
            {
                rects[i].X = W - rects[i].Width;
                rects[i].Y = 2 * H;
                Rect prev = new Rect(rects[i].ID, rects[i].X, rects[i].Y, rects[i].Width, rects[i].Height);
                while (true)
                {
                    rects[i] = top(rects[i], place);
                    rects[i] = left(rects[i], place);
                    if (rects[i].X == prev.X && rects[i].Y == prev.Y)
                    {
                        break;
                    }
                    prev = new Rect(rects[i].ID, rects[i].X, rects[i].Y, rects[i].Width, rects[i].Height);
                }
                place.Add(rects[i]);
            }

            return place;
        }
        private static int findUp(Rect r, List<Rect> list)
        {
            IEnumerable<Rect> no = list.Where(val => val.Y > r.Y || (val.X < r.X && val.X + val.Width <= r.X) || (val.X >= r.X + r.Width && val.X > r.X));
            IEnumerable<Rect> results = list.Except(no);
            if (!results.Any()) { return 0; }
            int max = results.Max(val => val.Y + val.Height);
            return max;
        }
        private static Rect top(Rect rect, List<Rect> list)
        {
            int y = findUp(rect, list);
            while (rect.Y != y)
            {
                if (rect.Y == 0) { break; }
                rect.Y -= 1;
            }
            return rect;
        }
        private static int findLeft(Rect r, List<Rect> list)
        {
            IEnumerable<Rect> no = list.Where(val => val.X > r.X || (val.Y < r.Y && val.Y + val.Height <= r.Y) || (val.Y >= r.Y + r.Height && val.Y > r.Y));
            IEnumerable<Rect> results = list.Except(no);
            if (!results.Any()) { return 0; }
            int max = results.Max(val => val.X + val.Width);
            return max;
        }

        private static Rect left(Rect rect, List<Rect> list)
        {
            int x = findLeft(rect, list);
            while (rect.X != x)
            {
                if (rect.X == 0) { break; }
                rect.X -= 1;
            }
            return rect;
        }
        private static Rectangle TransformForPlacement(Rect r, int h)
        {
            return new Rectangle(r.X, h - r.Y - r.Height, r.Width, r.Height);
        }

        private void RandomPacking_Click(object sender, EventArgs e)
        {
            g = this.CreateGraphics();

            g.Clear(Color.WhiteSmoke);
            List<Rect> rects = Shuffle(list);
            List<Rect> place = new List<Rect>();
            place.Add(new Rect(18, 0, 0, 175, 47));
            place.Add(new Rect(17, 175, 0, 175, 62));
            place.Add(new Rect(22, 350, 0, 77, 195));
            place.Add(new Rect(11, 427, 0, 50, 92));
            place.Add(new Rect(21, 0, 47, 77, 127));
            place.Add(new Rect(26, 427, 92, 50, 149));
            place.Add(new Rect(1, 0, 174, 201, 38));
            place.Add(new Rect(9, 0, 212, 124, 86));
            place.Add(new Rect(2, 124, 212, 201, 48));
            place.Add(new Rect(23, 325, 195, 57, 322));
            place.Add(new Rect(12, 382, 241, 74, 92));
            place.Add(new Rect(32, 477, 0, 16, 500));
            place.Add(new Rect(30, 382, 333, 43, 258));
            place.Add(new Rect(28, 425, 333, 26, 172));
            place.Add(new Rect(6, 0, 298, 131, 60));
            place.Add(new Rect(10, 0, 358, 124, 46));
            place.Add(new Rect(29, 451, 500, 43, 242));
            place.Add(new Rect(20, 0, 404, 117, 69));
            place.Add(new Rect(13, 0, 473, 31, 77));
            place.Add(new Rect(5, 0, 550, 131, 63));
            place.Add(new Rect(31, 0, 613, 15, 500));
            place.Add(new Rect(16, 15, 613, 23, 224));
            place.Add(new Rect(15, 38, 613, 23, 224));
            place.Add(new Rect(24, 61, 613, 41, 322));
            place.Add(new Rect(4, 102, 613, 107, 67));
            place.Add(new Rect(14, 102, 680, 31, 147));
            place.Add(new Rect(7, 133, 680, 39, 123));
            place.Add(new Rect(27, 172, 680, 24, 172));
            place.Add(new Rect(19, 196, 680, 58, 69));
            place.Add(new Rect(8, 131, 260, 31, 123));
            place.Add(new Rect(25, 162, 260, 50, 179));
            place.Add(new Rect(3, 117, 439, 94, 67));

            //place = Placement(rects, place);
            int h = place.Max(val => val.Y + val.Height);
            label1.Visible = true;
            label1.Text = "放置順序: \n";
            int count = 1;
            foreach (Rect r in place)
            {
                SolidBrush brush = new SolidBrush(Color.FromArgb(128, rnd.Next(2, 255), rnd.Next(2, 255), rnd.Next(2, 255)));
                //Rectangle rect = TransformForPlacement(r, h);
                //g.FillRectangle(brush, rect);
                //g.DrawString("" + r.ID, drawFont, new SolidBrush(Color.Black), rect.X + 2, rect.Y + 2);
                g.FillRectangle(brush, new Rectangle(r.X, r.Y, r.Width, r.Height));
                g.DrawString("" + r.ID, drawFont, new SolidBrush(Color.Black), r.X + 2, r.Y + 2);
                if (count % 10 == 0)
                {
                    label1.Text += r.ID + "\n";
                }
                else
                {
                    label1.Text += r.ID + "  ";
                }

                count++;
            }
            RandomPacking.Location = new Point(W + 200, 200);
            label1.Location = new Point(W + 100, 400);
            this.Height = h + 200;
            g.DrawRectangle(new Pen(Color.Black, 3), new Rectangle(0, 0, W, h));

            g.Dispose();
        }
    }
}

