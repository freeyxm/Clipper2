using Clipper2Lib;
using Clipper2Lib.Native;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Test
{
    internal class TestClipper
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        public void Test()
        {
            Graphics g = Graphics.FromHwnd(GetConsoleWindow());

            Paths64 subjects = new Paths64();
            Paths64 clips = new Paths64();
            Paths64 result = new Paths64();


            { // subjects
                Path64 path = new Path64();
                path.Add(new Point64(0, 25));
                path.Add(new Point64(0, 125));
                path.Add(new Point64(100, 125));
                path.Add(new Point64(100, 25));
                subjects.Add(path);
            }
            { // clips
                Path64 path = new Path64();
                path.Add(new Point64(50, 50));
                path.Add(new Point64(50, 150));
                path.Add(new Point64(150, 150));
                path.Add(new Point64(150, 50));
                clips.Add(path);
            }
            { // clips
                Path64 path = new Path64();
                path.Add(new Point64(75, 0));
                path.Add(new Point64(75, 100));
                path.Add(new Point64(175, 100));
                path.Add(new Point64(175, 0));
                clips.Add(path);
            }

            Point offset = new Point(50, 50);
            Point size = new Point(200, 200);
            List<Point> points = new List<Point>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    points.Add(new Point(offset.X + j * size.X, offset.Y + i * size.Y));
                }
            }

            int index = 0;
            Draw(g, subjects, points[index], Color.Red);
            Draw(g, clips, points[index], Color.Green, Color.Blue);

            result.Clear();
            ClipperNative.Intersect(subjects, clips, FillRule.NonZero, ref result);
            Draw(g, result, points[++index], Color.Yellow);
            //Print("Intersect", result);

            result.Clear();
            ClipperNative.Union(subjects, clips, FillRule.NonZero, ref result);
            Draw(g, result, points[++index], Color.Yellow);
            //Print("Union", result);

            result.Clear();
            ClipperNative.Difference(subjects, clips, FillRule.NonZero, ref result);
            Draw(g, result, points[++index], Color.Yellow);
            //Print("Difference", result);

            result.Clear();
            ClipperNative.Xor(subjects, clips, FillRule.NonZero, ref result);
            Draw(g, result, points[++index], Color.Yellow, Color.Violet);
            //Print("Xor", result);
        }

        void Print(string name, Paths64 paths)
        {
            Console.WriteLine(name);
            for (int i = 0; i < paths.Count; i++)
            {
                Console.Write($"path {i}: ");
                var path = paths[i];
                for (int j = 0; j < path.Count; j++)
                {
                    Console.Write(path[j].ToString());
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        void Draw(Graphics g, Paths64 paths, Point offset, params Color[] colors)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                Draw(g, paths[i], offset, colors[i % colors.Length]);
            }
        }

        void Draw(Graphics g, Path64 path, Point offset, Color color)
        {
            Point[] points = new Point[path.Count];
            for (int i = 0; i < path.Count; i++)
            {
                var p = path[i];
                points[i] = new Point((int)p.X + offset.X, (int)p.Y + offset.Y);
            }
            g.DrawPolygon(new Pen(color), points);
        }
    }
}
