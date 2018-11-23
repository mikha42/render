using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace render
{
    public partial class Form1 : Form
    {

        vector3 cam = new vector3(0, 40, 0);
        vector3 camdir = new vector3(0, -1, 0);
        vector3 camup = new vector3(-1, 0, 0);
        vector3 up = new vector3(-1, 0, 0);
        vector3 camleft = new vector3(0, 0, -1);

        int centerMouseX;
        int centerMouseY;

        vector3 rotAxis;

        double zoom = 1;
        double half = Math.Cos(0.5 * Math.PI);
        Graphics g;
        Bitmap b;
        int res = 1080;
        double aspect = 16d / 9d;

        List<Tuple<vector3, vector3>> draw = new List<Tuple<vector3, vector3>>();

        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 25;
            timer1.Enabled = true;

            centerMouseY = Screen.PrimaryScreen.Bounds.Height / 2;
            centerMouseX = Screen.PrimaryScreen.Bounds.Width / 2;

            Win32.POINT p = new Win32.POINT();
            p.x = centerMouseX;
            p.y = centerMouseY;
            Win32.ClientToScreen(this.Handle, ref p);
            Win32.SetCursorPos(p.x, p.y);

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            rotAxis = new vector3(0, 1, 0);

            bool makecube = false;
            bool makeSphere = false;
            bool makeSquigles = false;
            int squigleCount = 10;
            int squigleLength = 100;

            b = new Bitmap((int)(res * aspect), res);
            g = Graphics.FromImage(b);


            if (makeSquigles)
            {
                for (int count = 0; count < squigleCount; count++)
                {
                    vector3 squigle = randomDir() * 0.1;
                    vector3 squigle2;
                    draw.Add(new Tuple<vector3, vector3>(new vector3(0, 0, 0), squigle));
                    for (int i = 0; i < squigleLength; i++)
                        draw.Add(new Tuple<vector3, vector3>(squigle2 = squigle.copy, squigle += squigle2.rotateAbout(0.1, randomDir()).normalize() * 0.1));
                }
                
            }
            if (makeSphere)
            {
                /*addLine(-1, 0, 0, 1, 0, 0);
                addLine(0, -1, 0, 0, 1, 0);
                addLine(0, 0, -1, 0, 0, 1);
                makeCircle(new vector3(0, 0, 0), new vector3(1, 0, 0), 1, 80);
                makeCircle(new vector3(0, 0, 0), new vector3(0, 1, 0), 1, 80);
                makeCircle(new vector3(0, 0, 0), new vector3(0, 0, 1), 1, 80);*/
                for (double i = -1; i <= 1; i += 0.025)
                    //makeCircle(new vector3(0, 0, 0), randomDir(), 1, 100);
                    makeCircle(new vector3(i, 0, 0), new vector3(1, 0, 0), Math.Sqrt(1 - i * i), 10 + (int)Math.Round(20 * Math.Sqrt(1 - i * i)));
            }
            if (makecube)
            {
                List<vector3> cube = new List<vector3>();
                for (int i = 0; i < 8; i++)
                    cube.Add(new vector3(
                        (i % 2) - .5,
                        (i % 4 == 2 || i % 4 == 3) ? -.5 : .5,
                        (i > 3) ? -.5 : .5
                        ));

                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < i; j++)
                        if ((cube[i] - cube[j]).length == 1)
                            draw.Add(new Tuple<vector3, vector3>(cube[i], cube[j]));
            }
            if (false)
            {
                makeCircle(new vector3(0, 0, 0), new vector3(1, 0, 0), 1, 80);
                makeCircle(new vector3(0, 0, 0), new vector3(0, 1, 0), 1, 80);
                makeCircle(new vector3(0, 0, 0), new vector3(0, 0, 1), 1, 80);
                addLine(0, -1, 0, 0, 1, 0);
                addLine(0, 0, -1, 0, 0, 1);
                addLine(-1, 0, 0, 1, 0, 0);
            }
            if (true)
            {
                vector3 center = new vector3(0, 0, 0);
                vector3 normal = randomDir();
                rotAxis = normal;
                double radius = 14;
                int resoulution = 40;
                double dt = (2 * Math.PI) / (double)resoulution;
                vector3 p1;
                if (normal.length == 0)
                    return;
                if (normal.z == 0)
                    if (normal.x == 0)
                        p1 = new vector3(1, (normal.x + normal.z) / normal.y, 1).normalize();
                    else
                        p1 = new vector3((normal.z + normal.y) / normal.x, 1, 1).normalize();
                else
                    p1 = new vector3(1, 1, (normal.x + normal.y) / normal.z).normalize();
                vector3 p2 = vector3.cross(p1, normal).normalize();
                double test = vector3.dot(p1, p2);

                for (double t = 0; t < 3 * Math.PI; t += dt)
                    square((t - Math.PI * 2) * normal * 1.5 + center + p1 * Math.Sin(t) * (radius += r.NextDouble() * 4 - 2) + p2 * Math.Cos(t) * radius, randomDir(), r.NextDouble() * 2 + 0.3);
                return;
                for (int i = 0; i < 50; i++)
                    square(randomDir() * r.NextDouble() * 50 + randomDir() * 10,   randomDir(), r.NextDouble() * 3 + 1);
            }

        }

        private void square(vector3 center, vector3 direction, double size)
        {
            List<vector3> cube = new List<vector3>();
            for (int i = 0; i < 8; i++)
                cube.Add(new vector3(
                    (i % 2) - .5,
                    (i % 4 == 2 || i % 4 == 3) ? -.5 : .5,
                    (i > 3) ? -.5 : .5
                    ));

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < i; j++)
                    if ((cube[i] - cube[j]).length == 1)
                        draw.Add(new Tuple<vector3, vector3>(cube[i].copy.rotateLike(direction) * size + center, cube[j].copy.rotateLike(direction) * size + center));
        }

        private vector3 randomDir()
        {
            return new vector3(
                r.NextDouble() - 0.5,
                r.NextDouble() - 0.5,
                r.NextDouble() - 0.5
                ).normalize();
        }

        private void makeCircle(vector3 center, vector3 normal, double radius, int resoulution)
        {
            double dt = (2 * Math.PI) / (double)resoulution;
            vector3 p1;
            if (normal.length == 0)
                return;
            if (normal.z == 0)
                if (normal.x == 0)
                    p1 = new vector3(1, (normal.x + normal.z) / normal.y, 1).normalize();
                else
                    p1 = new vector3((normal.z + normal.y) / normal.x, 1, 1).normalize();
            else
                p1 = new vector3(1, 1, (normal.x + normal.y) / normal.z).normalize();
            vector3 p2 = vector3.cross(p1, normal).normalize();
            double test = vector3.dot(p1, p2);

            for (double t = 0; t < 2 * Math.PI; t += dt)
                draw.Add(new Tuple<vector3, vector3>(
                        center + p1 * Math.Sin(t) * radius + p2 * Math.Cos(t) * radius,
                        center + p1 * Math.Sin(t + dt) * radius + p2 * Math.Cos(t + dt) * radius
                    ));

        }

        Random r = new Random();

        private void addLine(double a1, double a2, double a3, double a4, double a5, double a6 )
        {
            draw.Add(new Tuple<vector3, vector3>(new vector3(a1, a2, a3), new vector3(a4, a5, a6)));
        }

        private void renderTest()
        {

            g.FillRectangle(new SolidBrush(Color.Magenta), 0, 0, (int)(res * aspect), res);
            foreach (Tuple<vector3, vector3> i in draw)
                drawLine( i.Item1, i.Item2);
            pictureBox1.Image = b;
        }


        private void drawLine(vector3 a, vector3 b)
        {
            Tuple<bool, int, int, double> p1 = vector2pixel.draw3D((int)(res * aspect), res, a, cam, camdir, camup, camleft, 0.3, zoom);
            Tuple<bool, int, int, double> p2 = vector2pixel.draw3D((int)(res * aspect), res, b, cam, camdir, camup, camleft, 0.3, zoom);
            if (p1.Item1 && p2.Item1)
            {
                /*float size = (float)((p1.Item4 + p2.Item4) / 2f) - 2f;
                size = 7.5f * (float)Math.Pow(1.4, -size);*/
                g.DrawLine(new Pen(Color.Blue, 1f), p1.Item2, p1.Item3, p2.Item2, p2.Item3);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        public Color randcolor()
        {
            return Color.FromArgb(
                r.Next(0, 255),
                r.Next(0, 255),
                r.Next(0, 255)
                );
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Looking
            double sensitivity = 10;
            double dx;
            double dy;
            dx = MousePosition.X - centerMouseX;
            dy = MousePosition.Y - centerMouseY;
            dx = -sensitivity * (dx / 10000);
            dy = sensitivity * (dy / 10000);
            camdir.rotateAbout(dx, up);
            camleft.rotateAbout(dx, up);
            camup.rotateAbout(dx, up);
            camdir.rotateAbout(dy, camleft);
            camup.rotateAbout(dy, camleft);

            Win32.POINT p = new Win32.POINT();
            p.x = centerMouseX;
            p.y = centerMouseY;
            Win32.ClientToScreen(this.Handle, ref p);
            Win32.SetCursorPos(p.x, p.y);

            //Movement
            double movementspeed = 0.03;

            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                movementspeed *= 20;
            }

            if (Keyboard.IsKeyDown(Key.W))
            {
                cam += camdir * movementspeed;
            }
            if (Keyboard.IsKeyDown(Key.S))
            {
                cam -= camdir * movementspeed;
            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                cam -= camleft * movementspeed;
            }
            if (Keyboard.IsKeyDown(Key.D))
            {
                cam += camleft * movementspeed;
            }
            if (Keyboard.IsKeyDown(Key.Space))
            {
                cam -= up * movementspeed;
            }
            if (Keyboard.IsKeyDown(Key.C))
            {
                cam += up * movementspeed;
            }
            if (Keyboard.IsKeyDown(Key.Q))
            {
                up.rotateAbout(0.05, camdir);
                camleft.rotateAbout(0.05, camdir);
                camup.rotateAbout(0.05, camdir);
            }
            if (Keyboard.IsKeyDown(Key.E))
            {
                up.rotateAbout(-0.05, camdir);
                camleft.rotateAbout(-0.05, camdir);
                camup.rotateAbout(-0.05, camdir);
            }

            //World actions
            if (Keyboard.IsKeyDown(Key.F))
            {
                
            }
            zoom = (0.5 + zoom + zoom +
                ( Keyboard.IsKeyDown(Key.LeftShift) ? 1 : zoom ) +
                ( Keyboard.IsKeyDown(Key.F) ? -0.4 : zoom )
                ) /5;
            /*for (int i = 0; i < draw.Count; i++)
            {
                vector3[] v = new vector3[] { draw[i].Item1, draw[i].Item2 };
                for (int j = 0; j < 2; j++)
                    v[j].rotateAbout(0.002, rotAxis);
                draw[i] = new Tuple<vector3, vector3>(v[0], v[1]);
            }*/
            /*for (int i = 0; i < draw.Count; i += 12)
            {
                vector3 move = 0 * randomDir() / 20;
                vector3 center = new vector3(0, 0, 0);
                for (int j = 0; j < 12; j++)
                    center += draw[j + i].Item1 + draw[j + i].Item2;
                for (int j = 0; j < 12; j++)
                    draw[j + i] = new Tuple<vector3, vector3>(
                        ((draw[j + i].Item1) - center).rotateAbout(0.01, center.copy.normalize()) + center + move,
                        ((draw[j + i].Item2) - center).rotateAbout(0.01, center.copy.normalize()) + center + move
                        );
            }*/

            renderTest();
        }
    }
}
