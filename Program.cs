using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public class Word
    {
        string value;
        int frequency;

        public Word(string value, int frequency)
        {
            this.value = value;
            this.frequency = frequency;
        }

        public string Value { get { return value; } }
        public int Frequency { get { return frequency; } set { frequency = value; } }
    }

    public class CommonWord
    {
        string value;
        int freq1;
        int freq2;

        public CommonWord(string value, int freq1, int freq2)
        {
            this.value = value;
            this.freq1 = freq1;
            this.freq2 = freq2;
        }

        public string Value { get { return value; } }
        public int Freq1 { get { return freq1; } set { freq1 = value; } }
        public int Freq2 { get { return freq2; } set { freq2 = value; } }
    }
}
