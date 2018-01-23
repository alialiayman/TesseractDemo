using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Tesseract;

namespace TesseractDemo
{
    public partial class Form1 : Form
    {
        //http://www.waytoumrah.com/prj_umrah/include/image.aspx?imgtext=538
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            var result = fd.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(fd.FileName))
                pictureBox1.ImageLocation = fd.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lblResult.Text = RunTesseract(new Bitmap(pictureBox1.Image));
        }


        private string RunTesseract(Bitmap inputImage)
        {
            var res = "";
            using (var engine = new TesseractEngine(@"../../tessdata", "eng", EngineMode.Default))
            {
                //engine.SetVariable("tessedit_char_whitelist", "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                engine.SetVariable("tessedit_char_whitelist", "1234567890");
                engine.SetVariable("tessedit_unrej_any_wd", true);

                using (var page = engine.Process(inputImage, PageSegMode.SingleLine))
                {
                    res = page.GetText();
                }
            }
            return res;
        }

        private void btnDownloadWTU_Click(object sender, EventArgs e)
        {
            var wc = new WebClient();
            var result = wc.DownloadData("http://www.waytoumrah.com/prj_umrah/include/image.aspx?imgtext=538");

            Bitmap bmp;
            using (var ms = new MemoryStream(result))
            {
                bmp = new Bitmap(ms);
            }

            pictureBox1.Image = bmp;
        }
    }
}