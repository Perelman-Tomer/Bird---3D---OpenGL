using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenGL;
using System.Runtime.InteropServices; 

namespace myOpenGL
{
    public partial class Form1 : Form
    {
        cOGL cGL;

        public Form1()
        {

            InitializeComponent();
            cGL = new cOGL(panel1);
            //apply the bars values as cGL.ScrollValue[..] properties 
                                         //!!!
            hScrollBarScroll(hScrollBar1, null);
            hScrollBarScroll(hScrollBar2, null);
            hScrollBarScroll(hScrollBar3, null);
            hScrollBarScroll(hScrollBar4, null);
            hScrollBarScroll(hScrollBar5, null);
            hScrollBarScroll(hScrollBar6, null);
            hScrollBarScroll(hScrollBar7, null);
            hScrollBarScroll(hScrollBar8, null);
            hScrollBarScroll(hScrollBar9, null);
            hScrollBarScroll(hScrollBar10, null);
            hScrollBarScroll(hScrollBar11, null);
            hScrollBarScroll(hScrollBar12, null);
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            cGL.Draw();
            timer1.Start();
            
            //panel1.BorderStyle==None !! provides an exact half-height views
                                    //!!!!!! 
            //e.Graphics.DrawLine(new Pen(BackColor), 0, panel1.Height / 2.0f, Width, panel1.Height / 2.0f);
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
          //  cGL.OnResize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void hScrollBarScroll(object sender, ScrollEventArgs e)
        {
            cGL.intOptionC = 0;
            HScrollBar hb = (HScrollBar)sender;
            int n;
            if (hb.Name.Length == 11)//look-at scrolls
            {
                n = int.Parse(hb.Name.Substring(hb.Name.Length - 1));
                cGL.ScrollValue[n - 1] = (hb.Value - 100) / 10.0f;
            }
            else
            if (hb.Name.Length == 12)//light control scrools
            {
                n = int.Parse(hb.Name.Substring(hb.Name.Length - 2));
                cGL.ScrollValue[n - 1] = hb.Value;
            }
            else
            if (hb.Name.Length == 13)//lake control scroll
            {
                n = int.Parse(hb.Name.Substring(hb.Name.Length - 2));
                cGL.lake_size = (float)(hb.Value);
              //  Console.WriteLine("scroolll lake {0} ", hb.Value);
            }
            else
            if (hb.Name.Length == 14)//birs speed control scroll
            {
          //      Console.WriteLine("scroolll  ");
                n = int.Parse(hb.Name.Substring(hb.Name.Length - 2));
                cGL.speed_inc = hb.Value / 10;
            //    Console.WriteLine("scroolll speed {0} ", hb.Value);
            }
            else
            if (hb.Name.Length == 15)//birs scale size control scroll
            {
              //  Console.WriteLine("scroolll  ");
                n = int.Parse(hb.Name.Substring(hb.Name.Length - 2));
                cGL.scale_inc = hb.Value / 10;
                //Console.WriteLine("scroolll speed {0} ", hb.Value);
            }
            else
            if (hb.Name.Length == 16)//birs scale size control scroll
            {
              //  Console.WriteLine("scroolll  ");
                n = int.Parse(hb.Name.Substring(hb.Name.Length - 2));
                cGL.spiral_inc = hb.Value / 10;
              //  Console.WriteLine("scroolll speed {0} ", hb.Value);
            }
            if (e != null)
            {
                cGL.Draw();
            }
        }

        public float[] oldPos = new float[7];

        private void numericUpDownValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nUD = (NumericUpDown)sender;
            int i = int.Parse(nUD.Name.Substring(nUD.Name.Length - 1));
            int pos = (int)nUD.Value; 
            switch(i)
            {
                case 1:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.xShift += 2f;
                        cGL.intOptionC = 4;
                    }
                    else
                    {
                        cGL.xShift -= 2f;
                        cGL.intOptionC = -4;
                    }
                    break;
                case 2:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.yShift += 2f;
                        cGL.intOptionC = 5;
                    }
                    else
                    {
                        cGL.yShift -= 2f;
                        cGL.intOptionC = -5;
                    }
                    break;
                case 3:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.zShift += 2f;
                        cGL.intOptionC = 6;
                    }
                    else
                    {
                        cGL.zShift -= 2f;
                        cGL.intOptionC = -6;
                    }
                    break;
                case 4:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.xAngle += 5;
                        cGL.intOptionC = 1;
                    }
                    else
                    {
                        cGL.xAngle -= 5;
                        cGL.intOptionC = -1;
                    }
                    break;
                case 5:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.yAngle += 5;
                        cGL.intOptionC = 2;
                    }
                    else
                    {
                        cGL.yAngle -= 5;
                        cGL.intOptionC = -2;
                    }
                    break;
                case 6: 
	                if (pos>oldPos[i-1]) 
	                {
		                cGL.zAngle+=5;
		                cGL.intOptionC=3;
	                }
	                else
	                {
                        cGL.zAngle -= 5;
                        cGL.intOptionC = -3;
                    }
                    break;
            }
            cGL.Draw();

            oldPos[i - 1] = pos;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cGL.Draw();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            cGL.checkBox = checkBox1.Checked;
            cGL.Draw();
            
        }
    }
}