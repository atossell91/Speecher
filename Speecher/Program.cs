using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Speecher
{
    class Program
    {
        private static bool checkInput(string[] args)
        {
            if (args.Length < 1)
            {
                return false;
            }

            return File.Exists(args[0]);
        }
        private static void programEnding(string message)
        {
            if (!String.IsNullOrEmpty(message))
            {
                Console.WriteLine(message);
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
        private static string generateOutputPath(string inputFilepath)
        {
            FileInfo fileInfo = new FileInfo(inputFilepath);
            int extLen = fileInfo.Extension.Length;
            string output = fileInfo.FullName.Substring(0, inputFilepath.Length - extLen) + ".wav";
            return output;
        }
        static void Main(string[] args)
        {
            if (!checkInput(args))
            {
                programEnding("Error - No file found. Terminating.");
                return;
            }

            string filepath = args[0];

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer())
                {
                    speechSynthesizer.SetOutputToWaveStream(memoryStream);


                    string text = File.ReadAllText(filepath);
                    speechSynthesizer.SpeakSsml(text);

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    //memoryStream.Position = 0;

                    string output = generateOutputPath(filepath);
                    if (File.Exists(output))
                    {
                        File.Delete(output);
                    }
                    using (FileStream fileStream = new FileStream(output, FileMode.CreateNew))
                    {
                        memoryStream.CopyTo(fileStream);
                    }
                }
            }

                programEnding("Program terminating.");
        }
    }
}
