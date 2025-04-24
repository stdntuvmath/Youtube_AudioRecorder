using System;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Lame;
using System.Threading;




namespace Audio_Output_Recorder
{
    public partial class Form1 : Form
    {

        private WaveFileWriter RecordedAudioWriter = null;
        private WasapiLoopbackCapture CaptureInstance = null;
        private static string userName = Environment.UserName;

        public string outputFilePath = @"C:\Users\" + userName + @"\Desktop\audioFile1.wav";

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)//start recording
        {
            

            //Thread.Sleep(3000);
            //MessageBox.Show("Que the music!");

            // Redefine the capturer instance with a new instance of the LoopbackCapture class
            this.CaptureInstance = new WasapiLoopbackCapture();

            // Redefine the audio writer instance with the given configuration
            this.RecordedAudioWriter = new WaveFileWriter(outputFilePath, CaptureInstance.WaveFormat);

            // When the capturer receives audio, start writing the buffer into the mentioned file
            this.CaptureInstance.DataAvailable += (s, a) =>
            {

                this.RecordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);
                //Thread.Sleep(1000);
                //int value = Math.Abs(BitConverter.ToInt16(a.Buffer, 0));
                //MessageBox.Show(value.ToString());
                //if (value == 0)
                    
                //    this.RecordedAudioWriter.Dispose();
                //    ConvertWaveToMP3File(outputFilePath);
                //if (value > 0)
                //{
                //    this.RecordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);
                //}

            };




            //int stuff = (int)CaptureInstance.CaptureState;
            //MessageBox.Show(CaptureInstance.CaptureState.ToString());

            // When the Capturer Stops
            this.CaptureInstance.RecordingStopped += (s, a) =>
            {
                //MessageBox.Show(CaptureInstance.GetType().ToString());
                this.RecordedAudioWriter.Dispose();
                this.RecordedAudioWriter = null;
                CaptureInstance.Dispose();

            };



            // Enable "Stop button" and disable "Start Button"
            this.button1.Enabled = false;
            this.button2.Enabled = true;

            // Start recording !
            this.CaptureInstance.StartRecording();



        }

        private void button2_Click(object sender, EventArgs e)//stop
        {
            // Stop recording !
            this.CaptureInstance.StopRecording();
            

            // Enable "Start button" and disable "Stop Button"
            this.button1.Enabled = true;
            this.button2.Enabled = false;
            this.button3.Enabled = true;
            //byte[] array = ConvertWavToMp3("");
            Thread.Sleep(3000);
            ConvertWaveToMP3File(outputFilePath);

            if (File.Exists(@"C:\Users\" + userName + @"\Desktop\audioFile1.wav"))
            {
                File.Delete(@"C:\Users\" + userName + @"\Desktop\audioFile1.wav");
            }

            //this.CaptureInstance.RecordingStopped += (s, a) =>
            //{
            //    //MessageBox.Show(CaptureInstance.GetType().ToString());
            //    this.RecordedAudioWriter.Dispose();
            //    this.RecordedAudioWriter = null;
            //    CaptureInstance.Dispose();
            //    //button3.PerformClick();

            //};

        }



        public void ConvertWaveToMP3File(string waveFilePath)
        {
            
            Thread.Sleep(3000);
            string mp3FileName = @"C:\Users\" + userName + @"\Desktop\audioFile1.mp3";

            WaveFileReader waveFileReader = new WaveFileReader(waveFilePath);

            NAudio.MediaFoundation.MediaFoundationApi.Startup();
            MediaFoundationEncoder.EncodeToMp3(waveFileReader, mp3FileName, 128000);
            NAudio.MediaFoundation.MediaFoundationApi.Shutdown();
            waveFileReader.Dispose();
            
            this.button3.Enabled = false;
            //using (WaveFileReader waveFileReader = new WaveFileReader(waveFilePath))
            //{
            //    var mediaType = NAudio.Wave.MediaFoundationEncoder.SelectMediaType(AudioSubtypes.MFAudioFormat_MP3,
            //    waveFileReader.WaveFormat, desiredBitRate);
            //    using (var encoder = new MediaFoundationEncoder(mediaType))
            //    {
            //        MediaFoundationApi.Startup();
            //        MediaFoundationEncoder.EncodeToMp3(waveFileReader, outputFilePath, desiredBitRate);
            //        MediaFoundationApi.Shutdown();
            //    }
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ConvertWaveToMP3File(outputFilePath);
        }

        private void SaveMP3(string SaveToFolder, string VideoURL, string MP3Name)
        {
            var source = @SaveToFolder;
            var youtube = YouTube.Default;
            var vid = youtube.GetVideo(VideoURL);
            File.WriteAllBytes(source + vid.FullName, vid.GetBytes());

            var inputFile = new MediaFile { Filename = source + vid.FullName };
            var outputFile = new MediaFile { Filename = $"{MP3Name}.mp3" };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                engine.Convert(inputFile, outputFile);
            }
        }
    }
}
