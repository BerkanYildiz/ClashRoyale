namespace ClashRoyale.Files.Sound
{
    using System.IO;
    using System.Threading.Tasks;
    using NAudio.Wave;

    public class SoundFile
    {
        public FileInfo File;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SoundFile" /> class.
        /// </summary>
        /// <param name="FileInfo">The file information.</param>
        public SoundFile(FileInfo FileInfo)
        {
            this.File = FileInfo;
        }

        /// <summary>
        ///     Gets the trimmed file name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.File.Name;
            }
        }

        /// <summary>
        ///     Plays this instance.
        /// </summary>
        public async Task Play()
        {
            using (WaveOutEvent WaveDevice = new WaveOutEvent())
            {
                using (AudioFileReader Reader = new AudioFileReader(this.File.FullName))
                {
                    WaveDevice.Init(Reader);
                    WaveDevice.Play();

                    while (WaveDevice.PlaybackState == PlaybackState.Playing)
                    {
                        await Task.Delay(1000);
                    }
                }
            }
        }
    }
}