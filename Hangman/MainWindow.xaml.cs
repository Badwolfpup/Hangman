using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hangman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        string[] svenskaOrd = {
            "Banan", "Bästis", "Bokhylla", "Cyklop", "Drömma", "Elefant", "Fjäder", "Grannar", "Högskola", "Isberg",
            "Julgran", "Kaktus", "Långsam", "Mössa", "Nyckel", "Översätt", "Päron", "Quinoa", "Ryggsäck", "Sköldpadda",
            "Tågstation", "Ugglan", "Vattenfall", "Xylofon", "Yrkesutbildning", "Zeppelinare", "Åskmoln", "Äventyr",
            "Överraska", "Göteborg", "Hallon", "Insekt", "Järnväg", "Kakel", "Lägenhet", "Målning", "Nötkärna",
            "Ostron", "Påskägg", "Qigong", "Regnjacka", "Sjöfart", "Tandläkare", "Underbar", "Vindruta", "Växthuseffekt",
            "Xylografi", "Yoghurt", "Åskväder", "Älgstek", "Ökenvandring", "Guldklimp", "Hotellrum", "Igelkott",
            "Jaktmark", "Karamell", "Lövskog", "Mjölkpaket", "Nattmössa", "Örtagård", "Parkbänk", "Quiltning",
            "Rymdskepp", "Snöstorm", "Tidskrift", "Ullstrumpa", "Väderkvarn", "Xylofonist", "Yrsel", "Återvinningsstation",
            "Äppelträd", "Ölprovning", "Godisklubba", "Havsutsikt", "Isoleucin", "Jättesnigel", "Kombination",
            "Lyxkryssning", "Månskenspromenad", "Nyckelord", "Odlingsmark", "Pusselbit", "Quesadilla", "Regnbåge",
            "Solros", "Träskulptur", "Utsiktsplats", "Vattenkanna", "Vinterdäck", "Yoghurtdryck", "Ånglok", "Äventyrslust",
            "Överlevnad", "Guldgruva", "Historiebok", "Inbrottstjuv", "Jättegryta", "Kamouflage", "Ljuskrona", "Myskväll"
        };

        string valtOrd;
        string xcr;
        int counter = 1;

        bool nyrunda = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<char> _bokstäver;
        public ObservableCollection<char> Bokstäver
        {
            get { return _bokstäver; }
            set
            {
                if(_bokstäver != value)
                {
                    _bokstäver = value;
                    OnPropertyChanged(nameof(Bokstäver));
                }
            }
        }

        private ObservableCollection<string> _gissadebokstäver;
        public ObservableCollection<string> GissadeBokstäver
        {
            get { return _gissadebokstäver; }
            set
            {
                if (_gissadebokstäver != value)
                {
                    _gissadebokstäver = value;
                    OnPropertyChanged(nameof(GissadeBokstäver));
                }
            }
        }

        private ObservableCollection<SolidColorBrush> _teckenfärg;

        public ObservableCollection<SolidColorBrush> Teckenfärg
        {
            get { return _teckenfärg; }
            set
            {
                if (_teckenfärg != value)
                {
                    _teckenfärg = value;
                    OnPropertyChanged(nameof(Teckenfärg));
                }
            }
        }

        public ObservableCollection<char> Alfabetet { get; set; } = new ObservableCollection<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N' , 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'Å', 'Ä', 'Ö' };
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Bokstäver = new ObservableCollection<char>();
            GissadeBokstäver = new ObservableCollection<string>();
            Teckenfärg = new ObservableCollection<SolidColorBrush>();
            SättTFärg();
            SlumpaOrd();
        }

        private void SättTFärg()
        {
            if (Teckenfärg.Count < 1)
            {
                for (int i = 0; i < Alfabetet.Count; i++)
                {
                    Teckenfärg.Add(new SolidColorBrush(Colors.Black));
                }
            } else
            {
                for(int i = 0;i < Teckenfärg.Count; i++)
                {
                    Teckenfärg[i] = new SolidColorBrush(Colors.Black);
                }
            }
        }

        private void SlumpaOrd()
        {
            Bokstäver.Clear();
            GissadeBokstäver.Clear();
            SättTFärg();
            Random r = new Random();
            valtOrd = svenskaOrd[r.Next(svenskaOrd.Length)].ToLower();
            foreach (char c in valtOrd)
            {
                Bokstäver.Add(c);
                GissadeBokstäver.Add("___");
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (nyrunda)
            {
                if (e.Key == Key.Enter)
                {
                    counter = 0;
                    SlumpaOrd();
                    nyrunda = false;
                    nyrundatext.Visibility = Visibility.Collapsed;
                    GömGubbe();
                }
            }
            else
            {
                if (e.Key >= Key.A && e.Key <= Key.Z)
                {
                    if (!Bokstäver.Contains((char)e.Key))
                    {
                        Bokstäver.Add((char)e.Key);
                        if (valtOrd.Contains(e.Key.ToString().ToLower()))
                        {

                            for (int i = 0; i < valtOrd.Length; i++)
                            {
                                if (e.Key.ToString().ToLower() == valtOrd[i].ToString()) GissadeBokstäver[i] = char.ToUpper(valtOrd[i]).ToString();
                            }
                            ÄndraTeckenFärg(e.Key, true);
                            klar();
                        }
                        else { ÄndraTeckenFärg(e.Key, false); VisaGubbe(); }
                    }
                }
                else if (e.Key == Key.Oem6)
                {
                    if (!Bokstäver.Contains('Å'))
                    {
                        Bokstäver.Add('Å');
                        if (valtOrd.Contains('å'))
                        {
                            for (int i = 0; i < valtOrd.Length; i++)
                            {
                                if (valtOrd[i] == 'å') GissadeBokstäver[i] = 'Å'.ToString();
                            }
                            ÄndraTeckenFärgÅÄÖ('Å', true);
                            klar();
                        }
                        else { ÄndraTeckenFärgÅÄÖ('Å', false); VisaGubbe(); }
                    }
                }
                else if (e.Key == Key.Oem7)
                {
                    if (!Bokstäver.Contains('Ä'))
                    {
                        Bokstäver.Add('Ä');
                        if (valtOrd.Contains('ä'))
                        {
                            for (int i = 0; i < valtOrd.Length; i++)
                            {
                                if (valtOrd[i] == 'ä') GissadeBokstäver[i] = 'Ä'.ToString();
                            }
                            ÄndraTeckenFärgÅÄÖ('Ä', true);
                            klar();
                        }
                        else { ÄndraTeckenFärgÅÄÖ('Ä', false); VisaGubbe(); }
                    }
                }
                else if ((e.Key == Key.Oem3))
                {
                    if (!Bokstäver.Contains('Ö'))
                    {
                        Bokstäver.Add('Ö');
                        if (valtOrd.Contains('ö'))
                        {
                            for (int i = 0; i < valtOrd.Length; i++)
                            {
                                if (valtOrd[i] == 'ö') GissadeBokstäver[i] = 'Ö'.ToString();
                            }
                            ÄndraTeckenFärgÅÄÖ('Ö', true);
                            klar();
                        }
                        else { ÄndraTeckenFärgÅÄÖ('Ö', false); VisaGubbe(); }
                    }
                }
            }
        }

        private void ÄndraTeckenFärg(Key e, bool sant)
        {
            
            for (int i = 0; i < Alfabetet.Count; i++) 
            {
                if (Alfabetet[i].ToString() == e.ToString())
                {
                    if (sant) Teckenfärg[i].Color = Colors.LimeGreen;
                    else Teckenfärg[i].Color = Colors.Red;
                }
            }
        }

        private void ÄndraTeckenFärgÅÄÖ(char c, bool sant)
        {

            for (int i = 0; i < Alfabetet.Count; i++)
            {
                if (Alfabetet[i] == c)
                {
                    if (sant) Teckenfärg[i].Color = Colors.LimeGreen;
                    else Teckenfärg[i].Color = Colors.Red;
                }
            }
        }

        private void VisaGubbe()
        {
            switch(counter)
            {
                case 1: baseofhangman.Visibility = Visibility.Visible; break;
                case 2: structure.Visibility = Visibility.Visible; break;
                case 3: roof.Visibility = Visibility.Visible; break;
                case 4: diagonal.Visibility = Visibility.Visible; break;
                case 5: rope.Visibility = Visibility.Visible; break;
                case 6: head.Visibility = Visibility.Visible; break;
                case 7: body.Visibility = Visibility.Visible; break;
                case 8: leftArm.Visibility = Visibility.Visible; break;
                case 9: rightArm.Visibility = Visibility.Visible; break;
                case 10: leftLeg.Visibility = Visibility.Visible; break;
                case 11: rightLeg.Visibility = Visibility.Visible; break;
                case 12: 
                    for( int i = 0; i< valtOrd.Length; i++ ) 
                    {
                        GissadeBokstäver[i] = char.ToUpper(valtOrd[i]).ToString();
                        nyrunda = true;
                        nyrundatext.Visibility = Visibility.Visible;
                    } break;
            }
            counter++;
        }

        private void klar()
        {
            string lösning = "";

            foreach(string c in GissadeBokstäver)
            {
                lösning += c;
            }
            if (valtOrd.Equals(lösning.ToLower()))
            {
                nyrunda = true;
                nyrundatext.Visibility = Visibility.Visible;
            }
        }

        private void GömGubbe()
        {
            baseofhangman.Visibility = Visibility.Hidden;
            structure.Visibility = Visibility.Hidden;
            roof.Visibility = Visibility.Hidden; 
            diagonal.Visibility = Visibility.Hidden; 
            rope.Visibility = Visibility.Hidden; 
            head.Visibility = Visibility.Hidden; 
            body.Visibility = Visibility.Hidden; 
            leftArm.Visibility = Visibility.Hidden; 
            rightArm.Visibility = Visibility.Hidden; 
            leftLeg.Visibility = Visibility.Hidden; 
            rightLeg.Visibility = Visibility.Hidden; 
        }
    }
}