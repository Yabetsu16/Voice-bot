using System.Diagnostics;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace Voice_bot
{
    public partial class Form1 : Form
    {
        static string projectDirectory = Environment.CurrentDirectory;
        string[] grammarFile = File.ReadAllLines(@$"{Directory.GetParent(projectDirectory).Parent.Parent.FullName}\grammar.txt");
        string[] responseFile = File.ReadAllLines(@$"{Directory.GetParent(projectDirectory).Parent.Parent.FullName}\response.txt");

        SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

        Choices grammarList = new Choices();
        SpeechRecognitionEngine speechRecognitionEngine = new SpeechRecognitionEngine();
        public Form1()
        {
            grammarList.Add(grammarFile);
            Grammar grammar = new Grammar(new GrammarBuilder(grammarList));

            try
            {
                speechRecognitionEngine.RequestRecognizerUpdate();
                speechRecognitionEngine.LoadGrammar(grammar);
                speechRecognitionEngine.SpeechRecognized += recSpeechRecognized;
                speechRecognitionEngine.SetInputToDefaultAudioDevice();
                speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                return;
            }

            speechSynthesizer.SelectVoiceByHints(VoiceGender.Female);
            InitializeComponent();
        }

        public void say(String text)
        {
            speechSynthesizer.SpeakAsync(text);
        }

        private void recSpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
        {
            String result = e.Result.Text;
            int resp = Array.IndexOf(grammarFile, result);
            Debug.WriteLine(resp);
            say(responseFile[resp]);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}