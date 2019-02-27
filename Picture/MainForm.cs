/*
 * Created by SharpDevelop.
 * User: Piotr
 * Date: 26.02.2019
 * Time: 13:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Picture
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		Bitmap bmp;
			public static Bitmap MakeGrayscale3(Bitmap original)
{
   //create a blank bitmap the same size as original
   Bitmap newBitmap = new Bitmap(original.Width, original.Height);

   //get a graphics object from the new image
   Graphics g = Graphics.FromImage(newBitmap);

   //create the grayscale ColorMatrix
   ColorMatrix colorMatrix = new ColorMatrix(
      new float[][] 
      {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
      });

   //create some image attributes
   ImageAttributes attributes = new ImageAttributes();

   //set the color matrix attribute
   attributes.SetColorMatrix(colorMatrix);

   //draw the original image on the new image
   //using the grayscale color matrix
   g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
      0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

   //dispose the Graphics object
   g.Dispose();
   return newBitmap;
}
		public static bool ColorsAreClose(Color a, Color z, int threshold = 50)
{
    int r = (int)a.R - z.R,
        g = (int)a.G - z.G,
        b = (int)a.B - z.B;
    return (r*r + g*g + b*b) <= threshold*threshold;
}
		public static Bitmap MaskGray(Bitmap b,Bitmap g,int number,int backgroundint){
			Bitmap outp = new Bitmap(g.Width,g.Height);
			Color back = b.GetPixel(0,0);
			for(int x= 0; x < b.Width;x++){
				for(int y= 0; y < b.Height;y++){
					Color col = b.GetPixel(x,y);
					Color gray = g.GetPixel(x,y);
					Color output = Color.FromArgb(col.A,(col.R /2)+(gray.R /number),(col.G /2)+(gray.G /number),(col.B /2)+(gray.B /number));
					outp.SetPixel(x,y,output);
			}
			}
			Color bck = outp.GetPixel(0,0);
			for(int xa= 0; xa < b.Width;xa++){
				for(int ya= 0; ya < b.Height;ya++){
					if(ColorsAreClose(bck, outp.GetPixel(xa,ya),backgroundint)){
						outp.SetPixel(xa,ya,back);
					}
			}
			}
			return outp;
		}
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			
		}
		
		void TrackBar1Scroll(object sender, EventArgs e)
		{
				pictureBox1.Image = MaskGray(bmp,MakeGrayscale3(bmp),trackBar2.Value,trackBar1.Value);
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			OpenFileDialog of = new OpenFileDialog();
			of.ShowDialog();
			bmp = new Bitmap(Bitmap.FromFile(of.FileName));
			pictureBox1.Image = MaskGray(bmp,MakeGrayscale3(bmp),trackBar2.Value,trackBar1.Value);
		}
		
		void TrackBar2Scroll(object sender, EventArgs e)
		{
				pictureBox1.Image = MaskGray(bmp,MakeGrayscale3(bmp),trackBar2.Value,trackBar1.Value);
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			SaveFileDialog sv = new SaveFileDialog();
			sv.ShowDialog();
			pictureBox1.Image.Save(sv.FileName);
		}
	}
}
