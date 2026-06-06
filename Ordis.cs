using System.Text.Json;
using NAudio.Wave;
using Vosk;

namespace OrdisVosk;

class Ordis
{
    private static VoskRecognizer? recognizer;
    private static WaveInEvent? waveSource;
    private static bool isAwake = false;

    static void Main(string[] args)
    {
        string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "vosk-model-small-ru-0.22");
        modelPath = Path.GetFullPath(modelPath);

        Console.WriteLine("Загрузка модели...");
        Console.WriteLine($"Путь: {modelPath}");

        if (!Directory.Exists(modelPath))
        {
            Console.WriteLine($"Ошибка: модель не найдена!");
            Console.WriteLine("Скачай и распакуй папку vosk-model-small-ru-0.22 в папку проекта");
            Console.WriteLine("Ссылка: https://alphacephei.com/vosk/models/vosk-model-small-ru-0.22.zip");
            Console.ReadLine();
            return;
        }

        var model = new Model(modelPath);
        recognizer = new VoskRecognizer(model, 16000.0f);

        Console.WriteLine("Модель загружена!");
        Console.WriteLine("Скажи 'компьютер' или 'робот' для активации");
        Console.WriteLine("----------------------------------------------");

        waveSource = new WaveInEvent
        {
            DeviceNumber = 0,
            WaveFormat = new WaveFormat(16000, 1)
        };
        waveSource.DataAvailable += OnDataAvailable;
        waveSource.StartRecording();

        Console.WriteLine("Слушаю... Нажми Enter для выхода, нажми tab для настроек");
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Tab)
                {
                    Settings settings = new Settings();
                    settings.SettingsMenu();
                }
            }
            Thread.Sleep(100);
        }

        waveSource.StopRecording();
        waveSource.Dispose();
        recognizer.Dispose();
        model.Dispose();
    }

    private static void OnDataAvailable(object? sender, WaveInEventArgs e)
    {
        if (recognizer == null) return;

        if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
        {
            string result = recognizer.Result();
            using var doc = JsonDocument.Parse(result);
            string text = doc.RootElement.GetProperty("text").GetString() ?? "";

            if (!string.IsNullOrEmpty(text))
            {
                Console.WriteLine($"[Распознано] {text}");

                if (!isAwake && (text.Contains("ардис")))
                {
                    ActivateAssistant();
                    isAwake = true;
                }
                else if (isAwake)
                {
                    ProcessCommand(text);
                    isAwake = false;
                }
            }
        }
        else
        {
            string partial = recognizer.PartialResult();
            using var doc = JsonDocument.Parse(partial);
            string partialText = doc.RootElement.GetProperty("partial").GetString() ?? "";
            if (!string.IsNullOrEmpty(partialText))
            {
                Console.Write($"\r[Слышу: {partialText,-40}]");
            }
        }
    }

    private static void ActivateAssistant()
    {
        Console.WriteLine("\n=== АССИСТЕНТ АКТИВИРОВАН! ===");
    }

    private static void ProcessCommand(string command)
    {
        Console.WriteLine($"\nВыполняю команду: {command}");
        // Здесь ты можешь добавить свою логику
    }
}