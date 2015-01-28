using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using SharpLZW;
using TestWPF.Models;

namespace TestWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml//
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Przeliczanie statystyk dla zakladki nr1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            #region old
            //Dictionary<string, int> charsDictionary = new Dictionary<string, int>();
            //List<StatisticModel> statisticList = new List<StatisticModel>();
            //string textInBox = myText.Text;
            //int textLength = textInBox.Length;
            //string result = "";

            //for (int i = 0; i < textLength; i++)
            //{
            //    string currentChar = textInBox[i].ToString();
            //    bool isInDictionary = charsDictionary.ContainsKey(currentChar);

            //    if (isInDictionary)
            //    {
            //        int currentValue = charsDictionary[currentChar];
            //        currentValue++;
            //        charsDictionary[currentChar] = currentValue;
            //    }
            //    else
            //    {
            //        charsDictionary.Add(currentChar, 1);
            //    }
            //}

            //foreach (var dict in charsDictionary)
            //{
            //    double frequency = (double)dict.Value / (double)textLength;
            //    StatisticModel statistic = new StatisticModel
            //    {
            //        Character = dict.Key,
            //        AppearInTextCounter = dict.Value,
            //        FrequencyAppearInText = Math.Round(frequency,3)
            //    };
            //    statisticList.Add(statistic);

            //    result += "Znak: " + statistic.Character + " Czestotliwosc: " + statistic.FrequencyAppearInText + " Ilosc wystapien: " + statistic.AppearInTextCounter + "\n";
            //}

            //double entropiaResult = Entropia(statisticList);

            //result += "\nEntropia: " + entropiaResult;
            //statisticBox.Text = "";
            //statisticBox.Text = result;
            #endregion

            Dictionary<string, int> charsDictionary = CreateDictionary(myText.Text);
            List<StatisticModel> statisticList = CreateStatistics(charsDictionary, myText.Text.Length);
            double entropiaResult = Entropia(statisticList);
            string result = "";

            foreach (var statistic in statisticList)
            {
                result += "Znak: " + statistic.Character + " Czestotliwosc: " + Math.Round(statistic.FrequencyAppearInText,3) + " Ilosc wystapien: " + statistic.AppearInTextCounter + "\n";
            }

            result += "\nEntropia: " + entropiaResult;

            statisticBox.Text = result;
            
        }

        /// <summary>
        /// Wybieranie pliku tekstowego zakladka nr 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                string readText = System.IO.File.ReadAllText(filename);
                textFromFile.Text = readText;

            }
        }

        /// <summary>
        /// Statystki zakladka nr 2 dla pliku wczytanego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> charsDictionary = CreateDictionary(textFromFile.Text);
            List<StatisticModel> statisticList = CreateStatistics(charsDictionary, textFromFile.Text.Length);
            double entropiaResult = Entropia(statisticList);
            string result = "";

            foreach (var statistic in statisticList)
            {
                result += "Znak: " + statistic.Character + ", Czestotliwość: " + Math.Round(statistic.FrequencyAppearInText, 3) + ", Ilość wystąpień: " + statistic.AppearInTextCounter + "\n";
            }

            result += "\nEntropia: " + entropiaResult;

            result += "\nSuma prawodpodbieństwa * ilość występowania: " + Math.Round(SumprawdXIloscWyst(statisticList),3);

            result += "\nIlość znaków w tekscie: " + statisticList.Sum(x => x.AppearInTextCounter);

            statisticBox.Text = result;
        }

        private double Entropia(List<StatisticModel> list)
        {
            double result = 0.0;

            foreach (var element in list)
            {
                result += element.FrequencyAppearInText * Math.Log(element.FrequencyAppearInText, 2);
            }

            result = result * (-1);

            return Math.Round(result,3);
        } 

        private Dictionary<string,int> CreateDictionary(string text)
        {
            Dictionary<string, int> charsDictionary = new Dictionary<string, int>();
            //List<StatisticModel> statisticList = new List<StatisticModel>();
            
            int textLength = text.Length;

            for (int i = 0; i < textLength; i++)
            {
                string currentChar = text[i].ToString(CultureInfo.InvariantCulture);
                bool isInDictionary = charsDictionary.ContainsKey(currentChar);

                if (isInDictionary)
                {
                    int currentValue = charsDictionary[currentChar];
                    currentValue++;
                    charsDictionary[currentChar] = currentValue;
                }
                else
                {
                    charsDictionary.Add(currentChar, 1);
                }
            }

            return charsDictionary;
        }

        private List<StatisticModel> CreateStatistics(Dictionary<string, int> charsDictionary, int textLength)
        {
            List<StatisticModel> statisticList = new List<StatisticModel>();

            foreach (var dict in charsDictionary)
            {
                double frequency = dict.Value / (double)textLength;
                StatisticModel statistic = new StatisticModel
                {
                    Character = dict.Key,
                    AppearInTextCounter = dict.Value,
                    FrequencyAppearInText = frequency
                };
                statisticList.Add(statistic);                
            }

            return statisticList;
        }

        private double CreateL(List<StatisticModel> list)
        {
            double result = 0;

            return result;
        }

        private double SumprawdXIloscWyst(List<StatisticModel> list)
        {
            double sum = 0;

            foreach (var element in list)
            {
                sum += (element.FrequencyAppearInText*element.AppearInTextCounter);
            }

            return sum;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string input = textFromHuffman.Text;
            HuffmanTree huffmanTree = new HuffmanTree();

            huffmanTree.Build(input);


            BitArray encoded = huffmanTree.Encode(input);
            int ii = 0;
            string encodeResult = "";
            foreach (bool bit in encoded)
            {
                encodeResult += (bit ? 1 : 0);
                ii++;
            }
           
            // Decode
            string decodeResult = huffmanTree.Decode(encoded);


            statisticBox.Text = "Encoded: " + encodeResult + "\n" + "Decoded: " + decodeResult + "\n";
        }

        public void Huffmann2(List<StatisticModel> stat)
        {
            var list = new List<StatisticModel>();

            foreach (var statisticModel in stat)
            {
                
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            statisticBox.Text = "";
            string text = textFromLZW.Text;

            string encodedFile = "EncodedFromWindow";
            string decodedFile = "DecodedFromWindow";

            var ascii = new ANSI();
            ascii.WriteToFile();
          
            var encoder = new LZWEncoder();
            byte[] b = encoder.EncodeToByteList(text);
            File.WriteAllBytes(encodedFile, b);

            var decoder = new LZWDecoder();
           
            byte[] bo = File.ReadAllBytes(encodedFile);
            string decodedOutput = decoder.DecodeFromCodes(bo);

            string byteText = "";
            foreach (var i in b)
            {
                byteText += i;
            }

            File.WriteAllText(decodedFile, decodedOutput, System.Text.Encoding.Default);

            statisticBox.Text = "zakodowany: \n" + byteText  + "\nodkodowany: \n" +
                                decodedOutput;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string fileToCompress = "Test.txt";
            string encodedFile = "Output.txt";
            string decodedFile = "DecodedOutput.txt";

            var ascii = new ANSI();
            ascii.WriteToFile();

            string text = File.ReadAllText(fileToCompress, System.Text.ASCIIEncoding.Default);
            var encoder = new LZWEncoder();
            byte[] b = encoder.EncodeToByteList(text);
            File.WriteAllBytes(encodedFile, b);

            var decoder = new LZWDecoder();
            byte[] bo = File.ReadAllBytes(encodedFile);
            string decodedOutput = decoder.DecodeFromCodes(bo);
            File.WriteAllText(decodedFile, decodedOutput, System.Text.Encoding.Default);

            statisticBox.Text = "Wyniki w plikach txt we wskazanej lokalizacji";
        }
    }
}
