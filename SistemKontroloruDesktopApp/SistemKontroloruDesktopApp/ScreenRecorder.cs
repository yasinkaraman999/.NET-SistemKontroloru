using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using Accord.Video.FFMPEG;
using System.Windows.Forms;

namespace ScreenRecord
{
    class ScreenRecorder
    {
        //Video Degişkenleri
        private Rectangle bounds;
        private string outputPath = "";
        private string tempPath = "";
        private int fileCount = 1;
        private List<string> inputImageSequence = new List<string>();

        //Dosya Degişkenleri
        public string videoName = "";



       //zaman degişkenleri
       Stopwatch watch = new Stopwatch();

        public ScreenRecorder(Rectangle b, string outPath)
        {
            CreateTempFolder("tempScreenshots");
            bounds = b;
            outputPath = outPath;
        }
        private void CreateTempFolder(string name)
        {
            if (Directory.Exists("D://"))
            {
                string pathName = $"D://{name}";
                Directory.CreateDirectory(pathName);
                tempPath = pathName;
            }
            else
            {
                string pathName = $"C://{name}";
                Directory.CreateDirectory(pathName);
                tempPath = pathName;
            }
        }
        private void DeletePath(string targetDir)
        {
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            foreach (string dir in dirs)
            {
                DeletePath(dir);
            }
            Directory.Delete(targetDir, false);
        }
        private void DeleteFilesExcept(string targetFile, string excFile)
        {
            string[] files = Directory.GetFiles(targetFile);

            foreach (string file in files)
            {
                if (file != excFile)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }
        }
        public void cleanUp()
        {
            if (Directory.Exists(tempPath))
            {
                DeletePath(tempPath);
            }
        }

        public string getElapsed()
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", watch.Elapsed.Hours, watch.Elapsed.Minutes, watch.Elapsed.Seconds);
        }

        public void RecordVideo()
        {
            watch.Start();

            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                string name = tempPath + "//screenshot-" + fileCount + ".png";
                bitmap.Save(name, ImageFormat.Png);
                inputImageSequence.Add(name);
                fileCount++;

                bitmap.Dispose();
            }
        }

        private void SaveVideo(int width, int height, int frameRate, string video_adi)
        {
            using (VideoFileWriter vFWriter = new VideoFileWriter())
            {
                vFWriter.Open(outputPath + "//" + video_adi, width, height, frameRate, VideoCodec.H264);
                foreach (string imageLoc in inputImageSequence)
                {
                    Bitmap imageFrame = System.Drawing.Image.FromFile(imageLoc) as Bitmap;
                    vFWriter.WriteVideoFrame(imageFrame);
                    imageFrame.Dispose();
                }
                vFWriter.Close();
            }
        }




        public void Stop(string video_adi)
        {
            watch.Stop();

            int width = bounds.Width;
            int height = bounds.Height;
            int frameRate = 10;

            SaveVideo(width, height, frameRate, video_adi);


            DeletePath(tempPath);



        }
    }
}
