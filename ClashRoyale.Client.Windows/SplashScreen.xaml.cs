namespace ClashRoyale.Client.Windows
{
    using System.IO;
    using System.Media;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    using NAudio.Wave;

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            TextElement.FontFamilyProperty.OverrideMetadata(typeof(TextElement), new FrameworkPropertyMetadata(new FontFamily("Supercell-Magic")));
            TextBlock.FontFamilyProperty.OverrideMetadata(typeof(TextBlock),     new FrameworkPropertyMetadata(new FontFamily("Supercell-Magic")));
        }

        /// <summary>
        /// Indique que le processus d’initialisation pour l’élément est terminé.
        /// </summary>
        public override async void EndInit()
        {
            base.EndInit();

            await Task.Delay(700);

            using (SoundPlayer Sound = new SoundPlayer(Directory.GetCurrentDirectory() + @"\Gamefiles\sfx\supercell_jingle.wav"))
            {
                Sound.Play();
            }

            while (this.ScLogo.Opacity < 1.0)
            {
                await Task.Delay(20);
                this.ScLogo.Opacity += 0.02;
            }

            this.ScLogo.Opacity = 1;

            await Task.Delay(1000);

            this.ScLogo.Opacity     = 0;

            WaveOutEvent WaveDevice = new WaveOutEvent();
            AudioFileReader Reader  = new AudioFileReader(Directory.GetCurrentDirectory() + @"\Gamefiles\sfx\scroll_loading_01.wav");

            WaveDevice.Init(Reader);
            WaveDevice.Play();

            while (this.ScBackground.Opacity < 1.0)
            {
                await Task.Delay(15);
                this.ScBackground.Opacity += 0.05;
            }
        }
    }
}
