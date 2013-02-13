using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Model.Arenas;
using Model.Entities;

namespace TugOfWarSpike2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Arena Arena = new Arena();

        private void Form1_Load(object sender, EventArgs e)
        {
            //Arena.AllItems.Add(new Unit() { Arena = Arena, Location = new PointF(0, 0), Radius = 0.1, Velocity = new PointF() { X = 0.004F, Y = 0.00F } });
            Player p1 = new Player(1);
            Player p2 = new Player(2);


            //Unit u = new Unit() { MyArena = Arena, GridLocation = new Point(-13, 0), Radius = DefaultRadius, Owner = p1, SightRange = 2 };
            //Arena.AllItems.Add(u);
            //u = new Unit() { MyArena = Arena, GridLocation = new Point(-13, 3), Radius = DefaultRadius, Owner = p1, SightRange = 2 };
            //Arena.AllItems.Add(u);
            //u = new Unit() { MyArena = Arena, GridLocation = new Point(13, 0), Radius = DefaultRadius, Owner = p2, SightRange = 2 };
            //Arena.AllItems.Add(u);
            //u = new Unit() { MyArena = Arena, GridLocation = new Point(14, 4), Radius = DefaultRadius, Owner = p2, SightRange = 2 };
            //Arena.AllItems.Add(u);
            //u = new Unit() { MyArena = Arena, GridLocation = new Point(13, 4), Radius = DefaultRadius, Owner = p2, SightRange = 2 };
            //Arena.AllItems.Add(u);

            int RowCount = 9;
            int ColumnCount = 2;
            double DefaultRadius = 0.08;
            int SightRange = 4;

            for (int x = 0; x < ColumnCount; x++)
            {
                for (int y = 0; y < RowCount; y++)
                {
                    Unit u = new Unit() { MyArena = Arena, GridLocation = new Point(x - 14, y - RowCount/2), Radius = DefaultRadius, Owner = p1, SightRange = SightRange };
                    Arena.AllItems.Add(u);
                }
            }

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Unit u = new Unit() { MyArena = Arena, GridLocation = new Point(x + 11, y - RowCount / 2), Radius = DefaultRadius, Owner = p2, SightRange = SightRange };
                    Arena.AllItems.Add(u);
                }
            }

         
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Arena.Paint(e.Graphics);
            dataGridView1.DataSource = Arena.AllItems;

        }

        private void tmrPhysics_Tick(object sender, EventArgs e)
        {
            Arena.Update();
        }

        private void tmrPaint_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void tmrAI_Tick(object sender, EventArgs e)
        {
            Arena.DoAILoop();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            tmrAI.Enabled = checkBox1.Checked;
            tmrPhysics.Enabled = checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Arena.DoAILoop();
            Arena.Update();
        }
    }
}
