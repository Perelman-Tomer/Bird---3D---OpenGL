using System;
using System.Collections.Generic;
using System.Windows.Forms;

/* Some <math.h> files do not define M_PI... */


//2
using System.Drawing;



namespace OpenGL
{
    class cOGL
    {

        GLUquadric obj;
        GLUquadric obj2;

        GLUquadric quad;


        public float[] ScrollValue = new float[14];
        public bool checkBox=false;

        float M_PI = 3.14159265f;
        float M_PI_2 = 1.57079632f;

        float flap=1;
        public double speed = 0;
        public double speed_inc = 1;
        public double spiral_inc = 1;
        public double scale_inc = 15;

        int f = 0;
        int spirala = 45;
        double hh = 1;
        double headMov = 1;
        double tilt = 1;


        double updown = 1;
        double ascend = 1;
        bool tiltFlg = false;
        bool heightFlg = false;
        bool spirlFlg = false;
        bool flapFlg = false;
        bool flapBox = false;
        public double lake_size = 100;


        //#define glVertex3f glVertex3f  /* glVertex3f was the short IRIS GL name for
        //                           glVertex3f*/

        Control p;
        int Width;
        int Height;

        public cOGL(Control pb)
        {
            p=pb;
            Width = p.Width;
            Height = p.Height;
            obj = GLU.gluNewQuadric();

            InitializeGL();
            
            //3 points of ground plane
            ground[0, 0] = 1;
            ground[0, 1] = 0f;
            ground[0, 2] = 0;

            ground[1, 0] = -1;
            ground[1, 1] = 0f;
            ground[1, 2] = 0;

            ground[2, 0] = 0;
            ground[2, 1] = 0f;
            ground[2, 2] = -1;

            //light position

            pos[0] = light_position[0] = ScrollValue[9];
            pos[1] = light_position[1] = ScrollValue[10];
            pos[2] = light_position[2] = ScrollValue[11];
            pos[3] = light_position[3] = 0;


            light_position_reflected[0] = -ScrollValue[9];
            light_position_reflected[1] = -ScrollValue[10];
            light_position_reflected[2] = -ScrollValue[11];
            light_position_reflected[3] = 0;


            GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, light_ambient);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, light_diffuse);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, light_specular);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, light_position);
            GL.glLightfv(GL.GL_LIGHT1, GL.GL_AMBIENT, light_ambient);
            GL.glLightfv(GL.GL_LIGHT1, GL.GL_DIFFUSE, light_diffuse);
            GL.glLightfv(GL.GL_LIGHT1, GL.GL_SPECULAR, light_specular);
            GL.glLightfv(GL.GL_LIGHT1, GL.GL_POSITION, light_position_reflected);
            GL.glLightModelfv(GL.GL_LIGHT_MODEL_AMBIENT, lmodel_ambient);

            
        }

        ~cOGL()
        {
            WGL.wglDeleteContext(m_uint_RC);
        }

		uint m_uint_HWND = 0;

        public uint HWND
		{
			get{ return m_uint_HWND; }
		}
		
        uint m_uint_DC   = 0;

        public uint DC
		{
			get{ return m_uint_DC;}
		}
		uint m_uint_RC   = 0;

        public uint RC
		{
			get{ return m_uint_RC; }
		}
       

        public float zShift = 0.0f;
        public float yShift = 0.0f;
        public float xShift = 0.0f;

        public float zAngle = 0.0f;
        public float yAngle = 0.0f;
        public float xAngle = 0.0f;
        public int intOptionC = 0;

        //Light properties..
        float[] light_ambient = { 0.0f, 0.0f, 0.0f, 1.0f };
        float[] light_diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] light_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] light_position = { 0.0f, 100.0f, 0.0f, 0.0f };
        float[] light_position_reflected = { 0.0f, 100.0f, 0.0f, 0.0f };
        float[] lmodel_ambient = { 0.4f, 0.4f, 0.4f, 1.0f };


        public float sun = 0;
        public float planet = 0;
        float[] cubeXform = new float[16];
        float[] planeCoeff = { 1, 1, 1, 1 };
        float[,] ground = new float[3, 3];
        public float[] pos = new float[4];

        public void Drawlake()
        {
            float[] lake_ambuse = { 0.0117f, 0.4296f, 0.6562f, 0.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, lake_ambuse);

            GL.glPushMatrix();
            GL.glColor4f(0.0117f, 0.4296f, 0.6562f, 0.5f);
            GL.glRotatef(90, 1, 0, 0);
            GLU.gluDisk(obj, 0, lake_size, 30, 30);// size of lake
            GL.glPopMatrix();
        }

        public void Drawfloor()
        {
            GL.glPushMatrix();
            GL.glColor3f(0.0f, 1.0f,0.0f);
            GL.glTranslatef(0, -0.001f, 0);
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3f(0, 1, 0);
            GL.glVertex3f(-100, 0, -100);
            GL.glVertex3f(-100, 0, 100);
            GL.glVertex3f(100, 0, 100);
            GL.glVertex3f(100, 0, -100);
            GL.glEnd();
            GL.glPopMatrix();
        }

        public void drawSun()
        {
            GL.glPushMatrix();
            GL.glColor3d(1, 1, 1);
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]);


            quad = GLU.gluNewQuadric();
             GLU.gluQuadricTexture(quad, 40);

            GL.glTranslatef(pos[0], pos[1], pos[2]);

           // rotating sun as well as all planets to Y axis
            GL.glRotatef((float)sun++, 0.0f, 1.0f, 0.0f);
            GLU.gluSphere(quad, 10, 100, 100);

           // GL.FreeCreatedTexture(texture);

            //Draw_Planets();

            GL.glPopMatrix();

           // GL.glutSwapBuffers();

            sun += 0.04f;
        }

        public void drawaxe()
        {

            //GL.glBegin(GL.GL_LINES);
            ////x  RED
            //GL.glColor3f(1.0f, 0.0f, 0.0f);
            //GL.glVertex3f(0.0f, 0.0f, 0.0f);
            //GL.glVertex3f(150.0f, 0.0f, 0.0f);
            ////y  GREEN 
            //GL.glColor3f(0.0f, 1.0f, 0.0f);
            //GL.glVertex3f(0.0f, 0.0f, 0.0f);
            //GL.glVertex3f(0.0f, 150.0f, 0.0f);
            ////z  BLUE
            //GL.glColor3f(0.0f, 0.0f, 1.0f);
            //GL.glVertex3f(0.0f, 0.0f, 0.0f);
            //GL.glVertex3f(0.0f, 0.0f, 150.0f);
            //GL.glEnd();
        }

        public void Draw()
        {


            //if (tilt < 10 && tiltFlg == false)
            //{
            //    tilt += 0.5f;
            //}
            //else
            //{
            //    tiltFlg = true;
            //}

            //if (tiltFlg == true)
            //{
            //    tilt -= 0.5f;
            //    if (tilt == 1)
            //    {
            //        tiltFlg = false;
            //    }
            //}




            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;

            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);

            //TRIVIAL
            GL.glViewport(0, 0, Width, Height);
            GL.glLoadIdentity();
            GL.glEnable(GL.GL_NORMALIZE);

            GLU.gluLookAt(ScrollValue[0], ScrollValue[1], ScrollValue[2],
                           ScrollValue[3], ScrollValue[4], ScrollValue[5],
                           ScrollValue[6], ScrollValue[7], ScrollValue[8]);



            pos[0] = light_position[0] = ScrollValue[9];
            pos[1] = light_position[1] = ScrollValue[10];
            pos[2] = light_position[2] = ScrollValue[11];
            pos[3] = light_position[3] = 0;

            light_position_reflected[0] = -ScrollValue[9];
            light_position_reflected[1] = -ScrollValue[10];
            light_position_reflected[2] = -ScrollValue[11];
            light_position[3] = 0;

            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, light_position);
            GL.glEnable(GL.GL_LIGHT0);

            GL.glLightfv(GL.GL_LIGHT1, GL.GL_POSITION, light_position);
            GL.glEnable(GL.GL_LIGHT1);

            //beascender look angle
            GL.glTranslatef(0.0f,- 50.0f, -340.0f);//how far from the lake,-far +close.
            GL.glTranslatef( 0.0f, 1.0f, 0.0f);//height from the lake
            GL.glRotatef(25, 1.0f, 0, 0);//look at lake angle ,+up -down

            GL.glRotatef(xAngle, 1.0f, 0.0f, 0.0f);
            GL.glRotatef(yAngle, 0.0f, 1.0f, 0.0f);
            GL.glRotatef(zAngle, 0.0f, 0.0f, 1.0f);
            GL.glTranslatef(xShift, yShift, zShift);
            

            /*
             * 
             * Reflection drawing section 
             * 
             */ 

            GL.glPushMatrix();

            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);

            //draw only to STENCIL buffer
            GL.glEnable(GL.GL_STENCIL_TEST);
            GL.glStencilOp(GL.GL_REPLACE, GL.GL_REPLACE, GL.GL_REPLACE);
            GL.glStencilFunc(GL.GL_ALWAYS, 1, 0xFFFFFFFF);
            GL.glColorMask((byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE);
            GL.glDisable(GL.GL_DEPTH_TEST);   
            Drawlake();//Draw area when we want to see reflect

            // restore regular seascendings
            GL.glColorMask((byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE);
            GL.glEnable(GL.GL_DEPTH_TEST);

            // reflection is drawn only where STENCIL buffer value equal to 1
            GL.glStencilFunc(GL.GL_EQUAL, 1, 0xFFFFFFFF);
            GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_KEEP);


            /*
             * draw reflected scene 
             */
             
            GL.glScalef(1, -1, 1); //swap axes down 

            //GL.glShadeModel(GL.GL_FLAT);
            //light_position[0] = -pos[0];
            //light_position[1] = -pos[1];
            //light_position[2] = -pos[2];

            //GL.glDisable(GL.GL_LIGHT0);
            //GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, light_position);
            //GL.glEnable(GL.GL_LIGHT0);


            drawTrees();


            drawBird();

            drawSun();


            //light_position[0] = pos[0];
            //light_position[1] = pos[1];
            //light_position[2] = pos[2];

            //GL.glDisable(GL.GL_LIGHT0);
            //GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, light_position);
            //GL.glEnable(GL.GL_LIGHT0);

            GL.glPopMatrix();
            GL.glEnable(GL.GL_LIGHTING);

            Drawlake();

            GL.glDisable(GL.GL_LIGHTING);


            GL.glStencilFunc(GL.GL_NOTEQUAL, 1, 0xFFFFFFFF);
            GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_KEEP);

            GL.glDepthMask((byte)GL.GL_FALSE);
            GL.glDepthMask((byte)GL.GL_TRUE);

            drawFloorTextured();

            drawSun();


            GL.glDisable(GL.GL_STENCIL_TEST);

            DrawTexturedCube();    //SKY BOX
         


            /*
             * 
             * paint main scene 
             * 
             */

            drawaxe();



            GL.glShadeModel(GL.GL_FLAT);

            GL.glEnable(GL.GL_LIGHTING);
            //Main bird
            drawBird();
            drawTrees();
        
            /////////

            GL.glPushMatrix();
            GL.glTranslated(0, 1, 0);
           // GL.glTranslated(hh, 1, 0);

            GL.glRotated(sun, 0, 1, 0);

            drawNofarleaf(); //center

            for (int k = 0; k < 8; k++) //leafs
            {
                GL.glTranslated(12, 0, 0);
                drawNofarleaf();
                GL.glTranslated(-12, 0, 0);
                GL.glRotated(45, 0, 1, 0);
            }




            GL.glTranslated(0, -1, 0);
            GL.glPopMatrix();


            GL.glPushMatrix();
            GL.glRotated(sun, 0, 1, 0);

            GL.glTranslated(0, 1, 0);

         //   drawNofarFlower();

            for (int j = 0; j < 8; j++) //leafs
            {
                drawNofarFlower();
                GL.glRotated(45, 0, 1, 0);
            }


            GL.glTranslated(0, -1, 0);
            GL.glPopMatrix();


            float[] flowerBase_ambuse = { 0.9117f, 0.2296f, 0.1562f, 0.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, flowerBase_ambuse);

            GL.glPushMatrix();
            GL.glRotated(sun, 0, 1, 0);

            GL.glRotatef(-90, 1, 0, 0);
            GLUT.glutSolidSphere(3*lake_size/20, 10, 10);
            GL.glPopMatrix();

            GL.glTranslated(0, 10+ (lake_size/10), 0);

            float[] flowerHeart_ambuse = { 0.0117f, 0.2196f, 0.9562f, 0.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, flowerHeart_ambuse);

            GL.glPushMatrix();
            GL.glRotated(sun, 0, 1, 0);

            GL.glRotatef(-90, 1, 0, 0);
            GLUT.glutSolidSphere(8*lake_size/100, 32, 32);
            GL.glPopMatrix();

            GL.glTranslated(0, -10 -(lake_size/10), 0);

            /////////

            GL.glDisable(GL.GL_LIGHTING);

            /*
             * 
             * Draw shadows
             * 
             */
            GL.glDisable(GL.GL_LIGHTING);
            

            GL.glPushMatrix();

            MakeShadowMatrix(ground);
            GL.glMultMatrixf(cubeXform);

            GL.glShadeModel(GL.GL_FLAT);
            GL.glColor3d(0, 0, 0);//black
            drawBirdShade();

            GL.glPopMatrix();





            GL.glFlush();
            WGL.wglSwapBuffers(m_uint_DC);
        }

 /*
 * 
 * SHADOWS FUNCS
 * 
 */
        void ReduceToUnit(float[] vector)
        {
            float length;

            // Calculate the length of the vector		
            length = (float)Math.Sqrt((vector[0] * vector[0]) +
                                (vector[1] * vector[1]) +
                                (vector[2] * vector[2]));

            // Keep the program from blowing up by providing an exceptable
            // value for vectors that may calculated too close to zero.
            if (length == 0.0f)
                length = 1.0f;

            // Dividing each element by the length will result in a
            // unit normal vector.
            vector[0] /= length;
            vector[1] /= length;
            vector[2] /= length;
        }

        const int x = 0;
        const int y = 1;
        const int z = 2;

        // Points p1, p2, & p3 specified in counter clock-wise order
        void calcNormal(float[,] v, float[] outp)
        {
            float[] v1 = new float[3];
            float[] v2 = new float[3];

            // Calculate two vectors from the three points
            v1[x] = v[0, x] - v[1, x];
            v1[y] = v[0, y] - v[1, y];
            v1[z] = v[0, z] - v[1, z];

            v2[x] = v[1, x] - v[2, x];
            v2[y] = v[1, y] - v[2, y];
            v2[z] = v[1, z] - v[2, z];

            // Take the cross product of the two vectors to get
            // the normal vector which will be stored in out
            outp[x] = Math.Abs(v1[y] * v2[z] - v1[z] * v2[y]);
            outp[y] = Math.Abs(v1[z] * v2[x] - v1[x] * v2[z]);//Abs added..
            outp[z] = Math.Abs(v1[x] * v2[y] - v1[y] * v2[x]);

            // Normalize the vector (shorten length to one)
            ReduceToUnit(outp);
        }

        // Creates a shadow projection matrix out of the plane equation
        // coefficients and the position of the light. The return value is stored
        // in cubeXform[,]
        void MakeShadowMatrix(float[,] points)
        {
            float dot;

            // Find the plane equation coefficients
            // Find the first three coefficients the same way we
            // find a normal.
            calcNormal(points, planeCoeff);

            // Find the last coefficient by back substitutions
            planeCoeff[3] = -(
                (planeCoeff[0] * points[2, 0]) + (planeCoeff[1] * points[2, 1]) +
                (planeCoeff[2] * points[2, 2]));


            // Dot product of plane and light position
            dot = planeCoeff[0] * pos[0] +
                    planeCoeff[1] * pos[1] +
                    planeCoeff[2] * pos[2] +
                    planeCoeff[3];

            // Now do the projection
            // First column
            cubeXform[0] = dot - pos[0] * planeCoeff[0];
            cubeXform[4] = 0.0f - pos[0] * planeCoeff[1];
            cubeXform[8] = 0.0f - pos[0] * planeCoeff[2];
            cubeXform[12] = 0.0f - pos[0] * planeCoeff[3];

            // Second column
            cubeXform[1] = 0.0f - pos[1] * planeCoeff[0];
            cubeXform[5] = dot - pos[1] * planeCoeff[1];
            cubeXform[9] = 0.0f - pos[1] * planeCoeff[2];
            cubeXform[13] = 0.0f - pos[1] * planeCoeff[3];

            // Third Column
            cubeXform[2] = 0.0f - pos[2] * planeCoeff[0];
            cubeXform[6] = 0.0f - pos[2] * planeCoeff[1];
            cubeXform[10] = dot - pos[2] * planeCoeff[2];
            cubeXform[14] = 0.0f - pos[2] * planeCoeff[3];

            // Fourth Column
            cubeXform[3] = 0.0f - pos[3] * planeCoeff[0];
            cubeXform[7] = 0.0f - pos[3] * planeCoeff[1];
            cubeXform[11] = 0.0f - pos[3] * planeCoeff[2];
            cubeXform[15] = dot - pos[3] * planeCoeff[3];
        }


        protected virtual void InitializeGL()
		{
			m_uint_HWND = (uint)p.Handle.ToInt32();
			m_uint_DC   = WGL.GetDC(m_uint_HWND);

            // Not doing the following WGL.wglSwapBuffers() on the DC will
			// result in a failure to subsequently create the RC.
			WGL.wglSwapBuffers(m_uint_DC);

			WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
			WGL.ZeroPixelDescriptor(ref pfd);
			pfd.nVersion        = 1; 
			pfd.dwFlags         = (WGL.PFD_DRAW_TO_WINDOW |  WGL.PFD_SUPPORT_OPENGL |  WGL.PFD_DOUBLEBUFFER); 
			pfd.iPixelType      = (byte)(WGL.PFD_TYPE_RGBA);
			pfd.cColorBits      = 32;
			pfd.cDepthBits      = 32;
			pfd.iLayerType      = (byte)(WGL.PFD_MAIN_PLANE);

			int pixelFormatIndex = 0;
			pixelFormatIndex = WGL.ChoosePixelFormat(m_uint_DC, ref pfd);
			if(pixelFormatIndex == 0)
			{
				MessageBox.Show("Unable to retrieve pixel format");
				return;
			}

			if(WGL.SetPixelFormat(m_uint_DC,pixelFormatIndex,ref pfd) == 0)
			{
				MessageBox.Show("Unable to set pixel format");
				return;
			}
			//Create rendering context
			m_uint_RC = WGL.wglCreateContext(m_uint_DC);
			if(m_uint_RC == 0)
			{
				MessageBox.Show("Unable to get rendering context");
				return;
			}
			if(WGL.wglMakeCurrent(m_uint_DC,m_uint_RC) == 0)
			{
				MessageBox.Show("Unable to make rendering context current");
				return;
			}


            initRenderinspeedL();
        }

        public void OnResize()
        {
            Width = p.Width;
            Height = p.Height;
            GL.glViewport(0, 0, Width, Height);
            Draw();
        }

        protected virtual void initRenderinspeedL()
		{
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;
            if (this.Width == 0 || this.Height == 0)
                return;
            //GL.glClearColor(0.5f, 0.9f, 1.0f, 1.0f);

            GL.glClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDepthFunc(GL.GL_LEQUAL);

            GL.glViewport(0, 0, this.Width, this.Height);
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();


            GL.glShadeModel(GL.GL_SMOOTH);

            GLU.gluPerspective(60, (float)Width / (float)Height, 0.45f, 1000.0f);

            GenerateTextures(1);

            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();

        }

        public uint[] Textures = new uint[7];
        public void GenerateTextures(int texture)
        {
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glGenTextures(7, Textures);

            string[] imagesName ={ "posz.jpg","negz.jpg",
                                    "posx.jpg","negx.jpg","posy.jpg","negy.jpg","sun.jpg",};
            for (int i = 0; i < 7; i++)
            {
                Bitmap image = new Bitmap(imagesName[i]);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY); //Y axis in Windows is directed downwards, while in OpenGL-upwards
                System.Drawing.Imaging.BitmapData bitmapdata;
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

                bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[i]);
                //2D for XYZ
                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, image.Width, image.Height,
                                                              0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapdata.Scan0);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);

                image.UnlockBits(bitmapdata);
                image.Dispose();
            }
        }

        void drawFloorTextured()
        {
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glDisable(GL.GL_BLEND);
            GL.glColor3d(1, 1, 1);
            GL.glDisable(GL.GL_LIGHTING);
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3f(0, 1, 0);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-300, -0.01f, 300);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(300, -0.01f, 300);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(300, -0.01f, -300);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-300, -0.01f, -300);
            GL.glEnd();
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glEnable(GL.GL_BLEND);
           // GL.glEnable(GL.GL_LIGHTING);

        }

        void DrawTexturedCube()
        {
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glDisable(GL.GL_BLEND);
            GL.glColor3d(1, 1, 1);
            GL.glDisable(GL.GL_LIGHTING);
            // front
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(300.0f, -0.01f, 300.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(-300.0f, -0.01f, 300.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(-300.0f, 300.0f, 300.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(300.0f, 300.0f, 300.0f);
            GL.glEnd();
            // back
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-300.0f, -0.01f, -300.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(300.0f, -0.01f, -300.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(300.0f, 300.0f, -300.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-300.0f, 300.0f, -300.0f);
            GL.glEnd();
            // left
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-300.0f, -0.01f, 300.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(-300.0f, -0.01f, -300.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(-300.0f, 300.0f, -300.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-300.0f, 300.0f, 300.0f);
            GL.glEnd();
            // right
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[3]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(300.0f, -0.01f, -300.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(300.0f, -0.01f, 300.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(300.0f, 300.0f, 300.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(300.0f, 300.0f, -300.0f);
            GL.glEnd();
            // top
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[4]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-300.0f, 300.0f, -300.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(300.0f, 300.0f, -300.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(300.0f, 300.0f, 300.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-300.0f, 300.0f, 300.0f);
            GL.glEnd();

            GL.glDisable(GL.GL_TEXTURE_2D);
        }


            public void drawNofarleaf()
        {
            float[] nofarBase_ambuse = { 0.0117f, 0.8296f, 0.2562f, 0.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, nofarBase_ambuse);

            GL.glPushMatrix();
            GL.glColor4f(0.0117f, 0.8296f, 0.2562f, 0.0f);
            GL.glRotatef(90, 1, 0, 0);
            GLU.gluDisk(obj, 0, lake_size/6, 30, 30);// size of lake
            GL.glPopMatrix();
        }

        public void drawNofarFlower()
        {

            GL.glPushMatrix();
            GL.glRotatef(-90, 1, 0, 0);
            GL.glRotatef(40, 1, 0, 0);


            float[] flower_ambuse = { 0.183f, 0.914f, 0.213f, 1.0f };
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, flower_ambuse);

            //// Flower-Top
            ///

            GL.glScaled(lake_size / 60, lake_size / 60, lake_size / 60);
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0f, 0.0f, 0.0f);//1
            GL.glVertex3f(-5.0f, 2.0f, 10.0f);//2
            GL.glVertex3f(0.0f, 2.0f, 13.0f);//3
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0f, 0.0f, 0.0f);//1
            GL.glVertex3f(5.0f, 2.0f, 10.0f);//2
            GL.glVertex3f(0.0f, 2.0f, 13.0f);//3
            GL.glEnd();



            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(-5.0f, 4.0f, 10.0f);//1
            GL.glVertex3f(0.0f, 4.0f, 13.0f);//2
            GL.glVertex3f(0.0f, 0.0f, 22.0f);//3
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(5.0f, 4.0f, 10.0f);//1
            GL.glVertex3f(0.0f, 4.0f, 13.0f);//2
            GL.glVertex3f(0.0f, 0.0f, 22.0f);//3
            GL.glEnd();

            GL.glRotatef(5, 1, 0, 0);

            //// Flower-Top2
            ///

            GL.glScaled(lake_size / 80, lake_size / 80, lake_size / 80);
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0f, 0.0f, 0.0f);//1
            GL.glVertex3f(-5.0f, 2.0f, 10.0f);//2
            GL.glVertex3f(0.0f, 2.0f, 13.0f);//3
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0f, 0.0f, 0.0f);//1
            GL.glVertex3f(5.0f, 2.0f, 10.0f);//2
            GL.glVertex3f(0.0f, 2.0f, 13.0f);//3
            GL.glEnd();



            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(-5.0f, 4.0f, 10.0f);//1
            GL.glVertex3f(0.0f, 4.0f, 13.0f);//2
            GL.glVertex3f(0.0f, 0.0f, 22.0f);//3
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(5.0f, 4.0f, 10.0f);//1
            GL.glVertex3f(0.0f, 4.0f, 13.0f);//2
            GL.glVertex3f(0.0f, 0.0f, 22.0f);//3
            GL.glEnd();


            GL.glRotatef(5, 1, 0, 0);

            //// Flower-Top2
            
            float[] flower2_ambuse = { 0.183f, 0.814f, 0.283f, 1.0f };
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, flower2_ambuse);

            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0f, 0.0f, 0.0f);//1
            GL.glVertex3f(-5.0f, 2.0f, 10.0f);//2
            GL.glVertex3f(0.0f, 2.0f, 13.0f);//3
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0f, 0.0f, 0.0f);//1
            GL.glVertex3f(5.0f, 2.0f, 10.0f);//2
            GL.glVertex3f(0.0f, 2.0f, 13.0f);//3
            GL.glEnd();



            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(-5.0f, 4.0f, 10.0f);//1
            GL.glVertex3f(0.0f, 4.0f, 13.0f);//2
            GL.glVertex3f(0.0f, 0.0f, 22.0f);//3
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(5.0f, 4.0f, 10.0f);//1
            GL.glVertex3f(0.0f, 4.0f, 13.0f);//2
            GL.glVertex3f(0.0f, 0.0f, 22.0f);//3
            GL.glEnd();

            GL.glPopMatrix();
        }



        public void drawTrees()
        {


            GL.glPushMatrix();

            GL.glRotatef(-90, 1.0f, 0.0f, 0.0f);


            GL.glTranslated(200.0f, 0.0f, 0.0f);

            GL.glColor4d(0.75f, 0.55f, 0.15f, 1.0f);
            float[] trunk_ambuse = { 0.7f, 0.5f, 0.1f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, trunk_ambuse);

            //    GLU.GLUquadricObj* obj2;
            obj2 = GLU.gluNewQuadric();
            GLU.gluCylinder(obj2, 15f, 10f, 18.0f, 32, 32);

            GL.glTranslated(0.0f, 0.0f, 10.0f);





            for (int i = 0; i < 45; i++)
            {
                if(i%2==0)
                {
                    float[] trees_ambuse = { 0.1f, 0.3f, 0.0f, 1.0f };
                    GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, trees_ambuse);

                    GL.glRotatef(8, 0.0f, 3.0f, 20.0f);

                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    GL.glNormal3d(0, 0, 1);
                    GL.glColor3f(0f, 0.5f, 0.0f);  //green
                    GL.glVertex3f(-45f, 0.0f, 28.0f);//1
                    GL.glVertex3f(0f, 0.0f, 190.0f);//2
                    GL.glVertex3f(45.0f, 0.0f, 28.0f);//3
                    GL.glEnd();
                }
                else
                {
                    float[] trees_ambuse2 = { 0.0f, 0.55f, 0.0f, 1.0f };
                    GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, trees_ambuse2);

                    GL.glRotatef(8, 0.0f, -3.0f, 20.0f);

                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    GL.glNormal3d(0, 0, 1);
                    GL.glColor3f(0f, 0.5f, 0.0f);  //green
                    GL.glVertex3f(-45f, 0.0f, 18.0f);//1
                    GL.glVertex3f(0f, 0.0f, 190.0f);//2
                    GL.glVertex3f(45.0f, 0.0f, 18.0f);//3
                    GL.glEnd();
                }

            }
            //// Tree1
            ///
            //GL.glBegin(GL.GL_TRIANGLE_STRIP);
            //GL.glNormal3d(0, 0, 1);
            //GL.glColor3f(0f, 0.5f, 0.0f);  //green
            //GL.glVertex3f(-30f, 0.0f, 0.0f);//1
            //GL.glVertex3f(0f, 0.0f, 90.0f);//2
            //GL.glVertex3f(30.0f, 0.0f, 0.0f);//3
            //GL.glEnd();

            //float[] trees_ambuse2 = { 0.0f, 0.65f, 0.0f, 1.0f };

            //GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, trees_ambuse2);

            ////// Tree2
            /////
            //GL.glBegin(GL.GL_TRIANGLE_STRIP);
            //GL.glNormal3d(0, 0, 1);
            //GL.glColor3f(0f, 0.65f, 0.0f);  //green
            //GL.glVertex3f(0.0f, -30f, 0.0f);//1
            //GL.glVertex3f(0f, 0.0f, 90.0f);//2
            //GL.glVertex3f(0.0f, 30f, 0.0f);//3
            //GL.glEnd();

            GL.glPopMatrix();


        }


        public void drawBird()
        {
            Console.WriteLine("speed inc= {0} ", speed_inc);
                   
                    GL.glPushMatrix();
                    //  drawaxe();

                    //  GL.glTranslatef(planesArr[i].x, planesArr[i].y, planesArr[i].z);
                    GL.glRotatef(-90, 1.0f, 0.0f, 0.0f);
                    //   GL.glRotatef(45, 0.0f, 0.0f, 1.0f);
                    // GL.glRotatef(25, 1.0f, 0.0f, 0.0f);
                    GL.glScalef(1.0f / 3.0f, 1.0f / 4.0f, 1.0f / 4.0f);
                   // drawaxe();

                    GL.glRotated((++speed) * speed_inc, 0.0, 0.0, 1.0);

                    drawaxe();
                    GL.glTranslated(0.0, spirala* spiral_inc, 0);
                   // drawaxe();
                    GL.glRotated(90, 0.0, 0.0, 1.0);

                    if (!tiltFlg)
                    {
                        if (tilt > -12)
                        {
                            tilt -= 1.5;
                        }
                        if (tilt <= -12)
                            tiltFlg = true;
                    }
                    if (tiltFlg)
                    {
                         if(tilt <= -3)
                             tilt += 0.4;
                         if (tilt < 0  && tilt > -3)
                             tilt += 0.000001;
                         if (tilt >= 0)
                            tiltFlg = false;
                    }

            if (checkBox)
            {
                if (!flapFlg)
                {
                    flap += 0.5f;
                    if (flap >= 5)
                        flapFlg = true;
                }
                if (flapFlg)
                {
                    flap -= 0.4f;
                    if (flap <= -4)
                        flapFlg = false;
                }
            }
            if (!checkBox)
                flap = 0;



            if (!heightFlg)
                    {
                        if (ascend < 50)
                        {
                            ascend += 0.2;
                        }
                        if (ascend >= 25)
                            heightFlg = true;
                    }
                    if (heightFlg)
                    {
                        ascend -= 0.4;
                        if (ascend <= 2)
                            heightFlg = false;
                    }


                    GL.glRotated(tilt, 0.0, 1.0, 0.0);  //hh=tilt

                    //drawaxe();



                    GL.glTranslated(0.0, -(spirala), 0);

                    if (!spirlFlg)
                    {
                        if (spirala < 200)
                        {
                            spirala++;
                            if (spirala <= 100)
                                updown += 0.2;
                  //          if(scale_inc<35)
                  //  scale_inc *=1.007;



                    if (spirala > 100 && spirala <= 155)
                                updown -= 0.2;

                            if (!heightFlg)
                            {
                                if (ascend < 15)
                                {
                                   // ascend += 0.25;
                                }
                                if (ascend >= 15)
                                    heightFlg = true;
                            }

                        }
                        if (spirala == 200)
                            spirlFlg = true;
                    }
                    if (spirlFlg)
                    {
                //if (scale_inc > 1)
                //    scale_inc *=0.992;

                        spirala--;

                        if (spirala >= 150)
                            updown -= 0.2;
                        if (spirala < 150 && spirala > 100)
                            updown += 0.2;

                        if (heightFlg)
                        {
                          //  ascend -= 0.25;
                            if (ascend <= 2)
                                heightFlg = false;
                        }

                        if (spirala <= 45)
                            spirlFlg = false;
                    }



                    GL.glTranslatef(0.0f, 0.0f, 18.5f);


                    GL.glTranslatef(0.0f, 32.0f, 0.0f);
                    // drawaxe();

                     GL.glRotated(updown, 1.0, 0.0, 0.0);
                    GL.glTranslatef(0.0f, -32.0f, 0.0f);


                    GL.glPushMatrix();

                    GL.glScaled(scale_inc, scale_inc, scale_inc); //size of plane 
                    //drawaxe();

                    float[] wing_ambuse = { 0.183f, 0.914f, 0.613f, 1.0f };
                    float[] wing_shininess = { 8 };

                    GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, wing_ambuse);
                    GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_SHININESS, wing_shininess);

                    //// WINGS
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    /* left wing */
                    GL.glNormal3d(0, 0, 1);
                    GL.glColor3f(0f, 0.2f, 0.5f);  //blue
                    GL.glVertex3f(-14.5f, 0.0f, 5.0f + (float)flap + (float)ascend);//1   //hh= flap
                    GL.glVertex3f(-4.0f, -3.0f, 5.0f + ((float)flap * -0.15f) + (float)ascend);//2

                    GL.glVertex3f(-4.0f, 3.0f, 5.0f + ((float)flap * -0.15f) + (float)ascend);//3

                    /* left to down */
                    GL.glColor3f(0.0f, 0.1f, 0.65f);
                    GL.glVertex3f(-1.5f, -2.5f, 4.5f + (float)ascend);//4
                    GL.glVertex3f(-1.5f, 2.5f, 4.5f + (float)ascend);//5

                    GL.glEnd();


                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    /* right wing */





                    GL.glNormal3d(0, 0,1);

                    GL.glColor3f(0f, 0.2f, 0.5f);  //blue
                    GL.glVertex3f(14.5f, 0.0f, 5.0f + (float)flap + (float)ascend);//1
                    GL.glVertex3f(4.0f, -3.0f, 5.0f + ((float)flap * -0.15f) + (float)ascend);//2
                    GL.glVertex3f(4.0f, 3.0f, 5.0f + ((float)flap * -0.15f) + (float)ascend);//3
                    /* left to down */
                    GL.glColor3f(0.0f, 0.1f, 0.65f);
                    GL.glVertex3f(1.5f, -2.5f, 4.5f + (float)ascend);//4
                    GL.glVertex3f(1.5f, 2.5f, 4.5f + (float)ascend);//5

                    GL.glEnd();


                    float[] body_ambuse = { 0.042f, 0.492f, 0.355f, 1.0f };

                    GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, body_ambuse);

                    //// Center body
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);

                    GL.glNormal3d(0, 0, 1);

                    GL.glColor3f(0.1f, 0.3f, 0.6f);  //blue
                    GL.glVertex3f(-1.5f, -2.5f, 4.5f + (float)ascend);
                    GL.glVertex3f(0f, 0.0f, 6.5f + (float)ascend);
                    GL.glVertex3f(-1.5f, 2.5f, 4.5f + (float)ascend);


                    GL.glColor3f(0.0f, 0.4f, 0.6f);  //blue
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(-1.5f, 2.5f, 0.5f + (float)ascend);
                    GL.glEnd();

                    //// Center body
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);

                    GL.glNormal3d(0, 0, 1);

                    GL.glColor3f(0.1f, 0.3f, 0.6f);  //blue
                    GL.glVertex3f(1.5f, -2.5f, 4.5f + (float)ascend);
                    GL.glVertex3f(0f, 0.0f, 6.5f + (float)ascend);
                    GL.glVertex3f(1.5f, 2.5f, 4.5f + (float)ascend);

                    GL.glColor3f(0.0f, 0.4f, 0.6f);  //blue
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(1.5f, 2.5f, 0.5f + (float)ascend);
                    GL.glEnd();

                    //// Back body
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    /* back right */
                    GL.glNormal3d(0, 0, 1);

                    GL.glColor3f(0.4f, 0.4f, 0.6f);  //blue
                    GL.glVertex3f(1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(1.5f, -2.5f, 4.5f + (float)ascend);//4
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(0f, 0.0f, 6.5f + (float)ascend);//6
                    GL.glEnd();

                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    /* back left */
                    GL.glNormal3d(0, 0, 1);

                    GL.glColor3f(0.4f, 0.4f, 0.6f);  //blue
                    GL.glVertex3f(-1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(-1.5f, -2.5f, 4.5f + (float)ascend);//4
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(0f, 0.0f, 6.5f + (float)ascend);//6
                    GL.glEnd();




                      float[] tail_ambuse1 = { 0.4f, 0.25f, 0.99f, 1.0f };

                      GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, tail_ambuse1);

                      //// Tail
                      GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    GL.glNormal3d(0, 0, 1);

                    GL.glColor3f(0.8f, 0.9f, 0.8f);  //
                    GL.glVertex3f(-1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(0.0f, -14.0f + ((float)hh * -0.05f), 8.5f + (float)ascend*1.1f);//4  //tailMovement
                    GL.glVertex3f(1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glEnd();

            float[] tail_ambuse2 = { 0.4f, 0.20f, 0.97f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, tail_ambuse2);

            //// Tail2
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    GL.glNormal3d(0, 0, 1);

                    GL.glColor3f(0.8f, 0.9f, 0.8f);  //
                    GL.glVertex3f(-1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(0.0f, -8.0f + ((float)hh * -0.05f), 7.5f + (float)ascend);//4
                    GL.glVertex3f(1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glEnd();

            float[] tail_ambuse3 = { 0.4f, 0.15f, 0.95f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, tail_ambuse3);

            //// Tail3
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    GL.glNormal3d(0, 0, 1);

                    GL.glColor3f(0.8f, 0.9f, 0.8f);  //
                    GL.glVertex3f(-1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(0.0f, -5.0f + ((float)hh * -0.05f), 6.5f + (float)ascend);//4
                    GL.glVertex3f(1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glEnd();

            float[] tail_ambuse4 = { 0.7f, 0.9f, 0.8f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, tail_ambuse4);

            //// TailOpenleft
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    GL.glNormal3d(0.2, 0, 1);

                    GL.glColor3f(0.2f, 0.7f, 0.2f);  //
                    GL.glVertex3f(-1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(0.0f, -14.0f + ((float)hh * -0.05f), 8.5f + +(float)ascend * 1.1f);//4
                    GL.glVertex3f(-2.0f - ((float)hh * -0.15f), -15.5f + ((float)hh * -0.05f), 9.0f + ((float)hh * -0.10f) + +(float)ascend * 1.1f);
                    GL.glEnd();

                    //// TailOpenright
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    GL.glNormal3d(-0.2, 0, 1);

                    GL.glColor3f(0.2f, 0.7f, 0.2f);  //
                    GL.glVertex3f(1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(0.0f, -14.0f + ((float)hh * -0.05f), 8.5f +(float)ascend * 1.1f);//4
                    GL.glVertex3f(2.0f - ((float)hh * 0.15f), -15.5f + ((float)hh * -0.05f), 9.0f + ((float)hh * -0.10f) +(float)ascend * 1.1f);
                    GL.glEnd();

            float[] head_ambuse = { 0.3f, 0.9f, 0.4f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, head_ambuse);


            //// Head (neck)
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    GL.glNormal3d(0, -0.5, 1);

                    GL.glColor3f(0.7f, 0.8f, 0.7f);  //
                    GL.glVertex3f(-1.5f, 2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(0.0f, 8.0f + ((float)hh * -0.05f), 8.0f + ((float)hh * 0.05f) + (float)ascend);//4  //Head
                    GL.glVertex3f(1.5f, 2.5f, 0.5f + (float)ascend);
                    GL.glEnd();


            float[] beak_ambuse = { 0.579f, 0.198f, 0.105f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, beak_ambuse);

            //// Beak left
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    GL.glNormal3d(-0.2, 0.5, 1);

                    GL.glColor3f(0.5f, 0.5f, 0.6f);  //color
                    GL.glVertex3f(0.0f, 8.0f + ((float)hh * -0.05f), 8.0f + ((float)hh * 0.05f) + (float)ascend);//4
                    GL.glVertex3f(-0.4f, 6.84f + ((float)hh * -0.05f), 6.42f + ((float)hh * 0.05f) + (float)ascend);
                    GL.glVertex3f(0.0f, 10.0f + ((float)hh * -0.05f), 6.0f + ((float)hh * 0.05f) + (float)ascend);//4
                    GL.glEnd();
                    //// Beak right
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    GL.glNormal3d(0.2, 0.5, 1);

                    GL.glColor3f(0.5f, 0.5f, 0.6f);  //color
                    GL.glVertex3f(0.0f, 8.0f + ((float)hh * -0.05f), 8.0f + ((float)hh * 0.05f) + (float)ascend);//4
                    GL.glVertex3f(0.4f, 6.84f + ((float)hh * -0.05f), 6.42f + ((float)hh * 0.05f) + (float)ascend);
                    GL.glVertex3f(0.0f, 10.0f + ((float)hh * -0.05f), 6.0f + ((float)hh * 0.05f) + (float)ascend);//4
                    GL.glEnd();



                    GL.glPopMatrix();
                    GL.glPopMatrix();
    
        }

        public void drawBirdShade()
        {
            
                    GL.glPushMatrix();
                    //  drawaxe();

                    //  GL.glTranslatef(planesArr[i].x, planesArr[i].y, planesArr[i].z);
                    GL.glRotatef(-90, 1.0f, 0.0f, 0.0f);
                    //   GL.glRotatef(45, 0.0f, 0.0f, 1.0f);
                    // GL.glRotatef(25, 1.0f, 0.0f, 0.0f);
                    GL.glScalef(1.0f / 3.0f, 1.0f / 4.0f, 1.0f / 4.0f);
                    // drawaxe();

                    GL.glRotated((++speed) * speed_inc, 0.0, 0.0, 1.0);

                    drawaxe();
                    GL.glTranslated(0.0, spirala * spiral_inc, 0);
                    // drawaxe();
                    GL.glRotated(90, 0.0, 0.0, 1.0);
                    if (!tiltFlg)
                    {
                        if (hh > -12)
                        {
                            hh -= 1.5;
                        }
                        if (hh <= -12)
                            tiltFlg = true;
                    }
                    if (tiltFlg)
                    {
                        hh += 0.5;
                        if (hh >= 0)
                            tiltFlg = false;
                    }

                    if (!heightFlg)
                    {
                        if (ascend < 15)
                        {
                            ascend += 0.25;
                        }
                        if (ascend >= 15)
                            heightFlg = true;
                    }
                    if (heightFlg)
                    {
                        ascend -= 0.25;
                        if (ascend <= 2)
                            heightFlg = false;
                    }


                    GL.glRotated(hh, 0.0, 1.0, 0.0);

                    //drawaxe();



                    GL.glTranslated(0.0, -(spirala), 0);

                    if (!spirlFlg)
                    {
                        if (spirala < 200)
                        {
                            spirala++;
                            if (spirala <= 100)
                                updown += 0.2;
                            if (spirala > 100 && spirala <= 155)
                                updown -= 0.2;

                            if (!heightFlg)
                            {
                                if (ascend < 15)
                                {
                                    ascend += 0.25;
                                }
                                if (ascend >= 15)
                                    heightFlg = true;
                            }

                        }
                        if (spirala == 200)
                            spirlFlg = true;
                    }
                    if (spirlFlg)
                    {

                        if (!tiltFlg)
                        {
                            if (hh > -28)
                            {
                                hh -= 2.5;
                            }
                            if (hh <= -28)
                                tiltFlg = true;
                        }
                        if (tiltFlg)
                        {
                            hh += 0.5;
                            if (hh >= 0)
                                tiltFlg = false;
                        }

                        spirala--;

                        if (spirala >= 150)
                            updown -= 0.2;
                        if (spirala < 150 && spirala > 100)
                            updown += 0.2;

                        if (heightFlg)
                        {
                            ascend -= 0.25;
                            if (ascend <= 2)
                                heightFlg = false;
                        }

                        if (spirala <= 45)
                            spirlFlg = false;
                    }




                    //if (!flapFlg)
                    //{
                    //    f++;
                    //    flap += 0.2f;
                    //    if (f == 100)
                    //        flapFlg = true;
                    //}
                    //if (flapFlg)
                    //{
                    //    f--;
                    //    flap -= 0.2f;
                    //    if (f == 0)
                    //        flapFlg = false;
                    //}


                    GL.glTranslatef(0.0f, 0.0f, 18.5f);


                    GL.glTranslatef(0.0f, 32.0f, 0.0f);
                    // drawaxe();

                    GL.glRotated(updown, 1.0, 0.0, 0.0);
                    GL.glTranslatef(0.0f, -32.0f, 0.0f);


                    GL.glPushMatrix();

                    GL.glScaled(scale_inc, scale_inc, scale_inc); //size of plane 
                    drawaxe();

                    //// WINGS
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    /* left wing */
                    GL.glColor3f(0f, 0.0f, 0.0f);  //
                    GL.glVertex3f(-14.5f, 0.0f, 5.0f + (float)hh + (float)ascend);//1
                    GL.glVertex3f(-4.0f, -3.0f, 5.0f + ((float)hh * -0.1f) + (float)ascend);//2
                    GL.glVertex3f(-4.0f, 3.0f, 5.0f + ((float)hh * -0.1f) + (float)ascend);//3
                    /* left to down */
                    //GL.glColor3f(0.0f, 0.1f, 0.65f);
                    GL.glVertex3f(-1.5f, -2.5f, 4.5f + (float)ascend);//4
                    GL.glVertex3f(-1.5f, 2.5f, 4.5f + (float)ascend);//5

                    GL.glEnd();

                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    /* right wing */
                    //GL.glColor3f(0f, 0.2f, 0.5f);  //blue
                    GL.glVertex3f(14.5f, 0.0f, 5.0f + (float)hh + (float)ascend);//1
                    GL.glVertex3f(4.0f, -3.0f, 5.0f + ((float)hh * -0.1f) + (float)ascend);//2
                    GL.glVertex3f(4.0f, 3.0f, 5.0f + ((float)hh * -0.1f) + (float)ascend);//3
                    /* left to down */
                    //GL.glColor3f(0.0f, 0.1f, 0.65f);
                    GL.glVertex3f(1.5f, -2.5f, 4.5f + (float)ascend);//4
                    GL.glVertex3f(1.5f, 2.5f, 4.5f + (float)ascend);//5

                    GL.glEnd();



                    //// Center body
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    //GL.glColor3f(0.1f, 0.3f, 0.6f);  //blue
                    GL.glVertex3f(-1.5f, -2.5f, 4.5f + (float)ascend);
                    GL.glVertex3f(0f, 0.0f, 6.5f + (float)ascend);
                    GL.glVertex3f(-1.5f, 2.5f, 4.5f + (float)ascend);

                    //GL.glColor3f(0.0f, 0.4f, 0.6f);  //blue
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(-1.5f, 2.5f, 0.5f + (float)ascend);
                    GL.glEnd();

                    //// Center body
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    //GL.glColor3f(0.1f, 0.3f, 0.6f);  //blue
                    GL.glVertex3f(1.5f, -2.5f, 4.5f + (float)ascend);
                    GL.glVertex3f(0f, 0.0f, 6.5f + (float)ascend);
                    GL.glVertex3f(1.5f, 2.5f, 4.5f + (float)ascend);

                    //GL.glColor3f(0.0f, 0.4f, 0.6f);  //blue
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(1.5f, 2.5f, 0.5f + (float)ascend);
                    GL.glEnd();

                    //// Back body
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    /* back right */
                    //GL.glColor3f(0.4f, 0.4f, 0.6f);  //blue
                    GL.glVertex3f(1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(1.5f, -2.5f, 4.5f + (float)ascend);//4
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(0f, 0.0f, 6.5f + (float)ascend);//6
                    GL.glEnd();

                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    /* back left */
                    //GL.glColor3f(0.4f, 0.4f, 0.6f);  //blue
                    GL.glVertex3f(-1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(-1.5f, -2.5f, 4.5f + (float)ascend);//4
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(0f, 0.0f, 6.5f + (float)ascend);//6
                    GL.glEnd();

                    //// Tail
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    //GL.glColor3f(0.8f, 0.9f, 0.8f);  //
                    GL.glVertex3f(-1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(0.0f, -14.0f + ((float)hh * -0.05f), 8.5f + (float)ascend);//4
                    GL.glVertex3f(1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glEnd();
                    //// Tail2
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    //GL.glColor3f(0.8f, 0.9f, 0.8f);  //
                    GL.glVertex3f(-1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(0.0f, -8.0f + ((float)hh * -0.05f), 7.5f + (float)ascend);//4
                    GL.glVertex3f(1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glEnd();
                    //// Tail3
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    //GL.glColor3f(0.8f, 0.9f, 0.8f);  //
                    GL.glVertex3f(-1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(0.0f, -5.0f + ((float)hh * -0.05f), 6.5f + (float)ascend);//4
                    GL.glVertex3f(1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glEnd();

                    //// TailOpenleft
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    //GL.glColor3f(0.2f, 0.7f, 0.2f);  //
                    GL.glVertex3f(-1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(0.0f, -14.0f + ((float)hh * -0.05f), 8.5f + (float)ascend);//4
                    GL.glVertex3f(-2.0f + ((float)hh * -0.15f), -15.5f + ((float)hh * -0.05f), 9.0f + ((float)hh * -0.10f) + (float)ascend);
                    GL.glEnd();
                    //// TailOpenright
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    //GL.glColor3f(0.2f, 0.7f, 0.2f);  //
                    GL.glVertex3f(1.5f, -2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(0.0f, -14.0f + ((float)hh * -0.05f), 8.5f + (float)ascend);//4
                    GL.glVertex3f(2.0f + ((float)hh * 0.15f), -15.5f + ((float)hh * -0.05f), 9.0f + ((float)hh * -0.10f) + (float)ascend);
                    GL.glEnd();



                    //// Head
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    //GL.glColor3f(0.7f, 0.8f, 0.7f);  //
                    GL.glVertex3f(-1.5f, 2.5f, 0.5f + (float)ascend);
                    GL.glVertex3f(0.0f, 0.0f, 1.0f + (float)ascend);
                    GL.glVertex3f(0.0f, 8.0f + ((float)hh * -0.05f), 8.0f + ((float)hh * 0.05f) + (float)ascend);//4
                    GL.glVertex3f(1.5f, 2.5f, 0.5f + (float)ascend);
                    GL.glEnd();

                    //// Beak left
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    //GL.glColor3f(0.5f, 0.5f, 0.6f);  //color
                    GL.glVertex3f(0.0f, 8.0f + ((float)hh * -0.05f), 8.0f + ((float)hh * 0.05f) + (float)ascend);//4
                    GL.glVertex3f(-0.4f, 6.84f + ((float)hh * -0.05f), 6.42f + ((float)hh * 0.05f) + (float)ascend);
                    GL.glVertex3f(0.0f, 10.0f + ((float)hh * -0.05f), 6.0f + ((float)hh * 0.05f) + (float)ascend);//4
                    GL.glEnd();
                    //// Beak right
                    GL.glBegin(GL.GL_TRIANGLE_STRIP);
                    //GL.glColor3f(0.5f, 0.5f, 0.6f);  //color
                    GL.glVertex3f(0.0f, 8.0f + ((float)hh * -0.05f), 8.0f + ((float)hh * 0.05f) + (float)ascend);//4
                    GL.glVertex3f(0.4f, 6.84f + ((float)hh * -0.05f), 6.42f + ((float)hh * 0.05f) + (float)ascend);
                    GL.glVertex3f(0.0f, 10.0f + ((float)hh * -0.05f), 6.0f + ((float)hh * 0.05f) + (float)ascend);//4
                    GL.glEnd();



                    GL.glPopMatrix();
                    GL.glPopMatrix();
        }

    }



}


