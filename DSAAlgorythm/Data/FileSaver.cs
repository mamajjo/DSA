using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace DSAAlgorythm.Data
{
    public static class FileSaver
    {
        public static void SaveTextToFile(string text)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text File|*.txt";
            saveFileDialog.Title = "Create or choose sign file";
            saveFileDialog.ShowDialog();

            try
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                    Console.WriteLine("");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
            //else if (File.Exists(saveFileDialog.FileName))
            //{
            //    System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
            //    using (StreamWriter sw = new StreamWriter(fs))
            //    {
            //        sw.Write(text);
            //        Console.WriteLine("");
            //    }
            //}
        }
        public static string ReadTextToStrign(string filePath)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();

            if (File.Exists(filePath))
            {
                System.IO.FileStream fs = new FileStream(filePath, FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
            }
            else
                return null;
            
        }
        //public static void SaveBytesToFile(byte[] Rarray, byte[] Sarray)
        //{
        //    SaveFileDialog saveFileDialog = new SaveFileDialog();
        //    saveFileDialog.Filter = "Text File|*.txt";
        //    saveFileDialog.Title = "Create sign file";
        //    saveFileDialog.ShowDialog();
        //    if(saveFileDialog.FileName != "")
        //    {
        //        System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
        //        File.WriteAllBytes(saveFileDialog.FileName, Rarray);
        //        File.W
        //    }
        //    //if (!File.Exists(saveFileDialog.FileName))
        //    //{
        //    //    System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();

        //    //}
        //    //switch (saveFileDialog.FilterIndex)
        //    //{
        //    //    case 1:
        //    //        File.Create(saveFileDialog.FileName);
        //    //        TextWriter tw = new StreamWriter(saveFileDialog.FileName);
        //    //        tw.WriteLine(text);
        //    //        break;
        //    //}
        //}
    }
}
