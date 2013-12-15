using System;
using System.IO;
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

namespace ChineseReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Kreacja slownika wymowy
        Dictionary<string, string> pinyin = new Dictionary<string, string>();
        // Kreacja slownika znaczen
        Dictionary<string, string> meaning = new Dictionary<string, string>();


        public MainWindow()
        {
            CreateDictionary();
            InitializeComponent();
        }

        private void CreateDictionary()
        {
            // Wczytanie bazy hanzi-pinyin
            StreamReader reader = new StreamReader("Data\\pinyin.db");
            string line;
            line = reader.ReadLine();
            while((line = reader.ReadLine()) != null)
            {
                string [] entry = line.Split('\t');
                pinyin.Add(entry[0], entry[1]);
                meaning.Add(entry[0], entry[2]);
            }
        }

        private void Debug_Click(object sender, RoutedEventArgs e)
        {
            string selected = HZText.SelectedText;
            if (pinyin.ContainsKey(selected.ToString()) == true)
            {
                ExplainBox.Text = meaning[selected];
            }
        }

        private void Speech_Click(object sender, RoutedEventArgs e)
        {
            // Ustawienie elementu audio
            audioElement.LoadedBehavior = System.Windows.Controls.MediaState.Manual;

            // Wybranie zaznaczonego tekstu
            string selected = HZText.SelectedText;
            // Sprawdzenie czy znak istnieje w słowniku
            if (pinyin.ContainsKey(selected.ToString()) == true)
            {
                string a = pinyin[selected.ToString()];
                // Wybór właściwego MP3
                string SelectedHanzi = "Data\\Audio\\" + pinyin[selected.ToString()] + ".mp3";
                // Odtworzenie właściwego MP3
                audioElement.Source = new Uri(SelectedHanzi, UriKind.RelativeOrAbsolute);
                audioElement.Play();
            }
        }

        private void HZText_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Wybranie zaznaczonego tekstu
            string selected = HZText.SelectedText;

            // Wyszukanie w słowniku
            if (pinyin.ContainsKey(selected.ToString()) == true)
            {
                ExplainBox.Text = meaning[selected];
            }

            // Ustawienie elementu audio
            audioElement.LoadedBehavior = System.Windows.Controls.MediaState.Manual;


            // Sprawdzenie czy znak istnieje w słowniku
            if (pinyin.ContainsKey(selected.ToString()) == true)
            {
                string a = pinyin[selected.ToString()];
                // Wybór właściwego MP3
                string SelectedHanzi = "Data\\Audio\\" + pinyin[selected.ToString()] + ".mp3";
                // Odtworzenie właściwego MP3
                audioElement.Source = new Uri(SelectedHanzi, UriKind.RelativeOrAbsolute);
                audioElement.Play();
            }
        }

        private void LoadClip_Click(object sender, RoutedEventArgs e)
        {
            HZText.Text = Clipboard.GetText();
        }
    }
  }
