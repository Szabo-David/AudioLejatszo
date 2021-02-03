using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;



namespace mp3player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += new EventHandler(lejatszashelyzet_Tick);
            timer.Start();
            helyzet1.IsEnabled = false;
        }
        private void lejatszashelyzet_Tick(object sender, EventArgs e)
        {
            if ((mediaPlayer.Source != null) && (mediaPlayer.NaturalDuration.HasTimeSpan))
            {
                helyzet1.Value = mediaPlayer.Position.TotalSeconds;
            }
            if (helyzet1.Value == helyzet1.Maximum)
            {

            }
        }
        MediaPlayer mediaPlayer = new MediaPlayer();
        string[] filenamee;
        List<string> lejatszasilista = new List<string>();
        
        


        private void Lejatszas_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
            fajlnev.Content = zenek.SelectedItem;
            helyzet1.IsEnabled = true;
        }

        private void kivalaszt_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            filenamee = fileDialog.FileNames;
            fileDialog.Multiselect = true;
            fileDialog.Filter = ("All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV");
            fileDialog.ShowDialog();
            foreach (var item in fileDialog.FileNames)
            {
                lejatszasilista.Add(item);
                zenek.Items.Add(item);
                
            }
            try
            {
                mediaPlayer.Open(new Uri(lejatszasilista[0]));
            }
            catch
            {

            }

            
        }

        private void Leallitas_Click(object sender, RoutedEventArgs e)
        {

            mediaPlayer.Stop();
            
        }

        private void Megallitas_Click(object sender, RoutedEventArgs e)
        {

            mediaPlayer.Pause();
        }


        private void zenek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                mediaPlayer.Open(new Uri(lejatszasilista[zenek.SelectedIndex]));
            }
            catch
            {

            }

        }

        private void hangero_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            mediaPlayer.Volume = (double)hangero.Value;
        }

        private void helyzet1_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            helyzet1.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;            
        }

        private void torles_Click(object sender, RoutedEventArgs e)
        {
            zenek.Items.Remove(lejatszasilista[zenek.SelectedIndex]);
        }

        private void Tekeres(object sender, MouseEventArgs e)
        {
            mediaPlayer.Position = new TimeSpan(0, 0, Convert.ToInt32(helyzet1.Value));
        }

        private void Listamentes_Click(object sender, RoutedEventArgs e)
        {
            
            StreamWriter sw = new StreamWriter("Lejátszásilista.txt");
            foreach (var item in lejatszasilista)
            {
                sw.WriteLine(item);
            }
            sw.Close();
            zenek.Items.Clear();
        }

        private void Listabetoltes_Click(object sender, RoutedEventArgs e)
        {
            StreamReader sr = new StreamReader("Lejátszásilista.txt");
            while (!sr.EndOfStream)
            {
                string row = sr.ReadLine();
                zenek.Items.Add(row);
                lejatszasilista.Add(row);
            }

            sr.Close();
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mediaPlayer.Open(new Uri(lejatszasilista[zenek.SelectedIndex + 1]));
                zenek.SelectedIndex += 1;
            }
            catch (ArgumentOutOfRangeException)
            {

                mediaPlayer.Open(new Uri(lejatszasilista[0]));
                zenek.SelectedIndex = 0;
            }
            mediaPlayer.Play();
        }

        private void prev_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mediaPlayer.Open(new Uri(lejatszasilista[zenek.SelectedIndex - 1]));
                zenek.SelectedIndex -= 1;
            }
            catch (ArgumentOutOfRangeException)
            {

                mediaPlayer.Open(new Uri(lejatszasilista[lejatszasilista.Count-1]));
                zenek.SelectedIndex = -1;
            }
            mediaPlayer.Play();
        }


    }
}
