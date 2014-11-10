using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestWPF.Models;
using System.IO;

namespace TestWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
                result += "Znak: " + statistic.Character + " Czestotliwosc: " + statistic.FrequencyAppearInText + " Ilosc wystapien: " + statistic.AppearInTextCounter + "\n";
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
               result += "Znak: " + statistic.Character + " Czestotliwosc: " + statistic.FrequencyAppearInText + " Ilosc wystapien: " + statistic.AppearInTextCounter + "\n";
            }

            result += "\nEntropia: " + entropiaResult;

            statisticBox.Text = result;
        }

        private double Entropia(List<StatisticModel> list)
        {
            double result = 0;

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
            List<StatisticModel> statisticList = new List<StatisticModel>();
            
            int textLength = text.Length;

            for (int i = 0; i < textLength; i++)
            {
                string currentChar = text[i].ToString();
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
                double frequency = (double)dict.Value / (double)textLength;
                StatisticModel statistic = new StatisticModel
                {
                    Character = dict.Key,
                    AppearInTextCounter = dict.Value,
                    FrequencyAppearInText = Math.Round(frequency, 3)
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

        
        // dodać liczbę bitów porzebnych do zakodowania jednego znaku i dodać suma prawdopodobieństw * ilość wystąpień każdego z osobna
        //wybrać 2 dowolne programy kompresujące, wybrać 3 pliki tekstowe bez polskich znaków (mały (pojedyncze litery), średniej wielkości (kB) i duży), wyznaczyć statystyki
        //skompresować pliki wybranymi programami i porównać te kompresje z sytuacją idealną wynikającą z entropii

        //wstawić to na gita i pobrać Visio
    }
}
