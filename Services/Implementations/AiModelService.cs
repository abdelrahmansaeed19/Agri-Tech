using AgriculturalTech.API.DTOs;
using AgriculturalTech.API.Services.Interfaces;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text.Json;


namespace AgriculturalTech.API.Services.Implementations
{
    public class AiModelService : IAiModelService
    {
        private readonly InferenceSession _session;

        private readonly List<string> _classNames;

        public AiModelService()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string modelPath = Path.Combine(basePath, "Services/Resources", "xception_plant_model_22.onnx");

            _session = new InferenceSession(modelPath);

            // 2. تحميل وقراءة ملف الـ JSON
            try
            {
                string classNamesPath = Path.Combine(basePath, "Services/Resources", "class_names.json");

                string jsonContent = File.ReadAllText(classNamesPath);

                // تحويل الـ JSON إلى List من الـ Strings
                _classNames = JsonSerializer.Deserialize<List<string>>(jsonContent);

                if (_classNames == null || _classNames.Count == 0)
                    throw new Exception("Class names file is empty or invalid.");

                Console.WriteLine($"✅ Loaded {_classNames.Count} classes from JSON.");
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Failed to load class names: {ex.Message}");
            }
        }


        public async Task<AIResponse> PredictAsync(Stream imageStream)
        {
            // 1. معالجة الصورة (Preprocessing)
            // Xception يتوقع صور بحجم 299x299 وقيم بيكسل بين -1 و 1
            DenseTensor<float> tensor = await PreprocessImage(imageStream);

            // 2. تجهيز المدخلات
            string inputName = _session.InputMetadata.Keys.First();

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(inputName, tensor)
            };

            // 3. تشغيل الموديل (Inference)
            using var results = _session.Run(inputs);

            // 4. استخراج النتائج
            // Xception يخرج مصفوفة احتمالات (Softmax)
            var output = results.First().AsEnumerable<float>().ToArray();

            // البحث عن أعلى احتمال
            var maxProbability = output.Max();
            var maxIndex = Array.IndexOf(output, maxProbability);

            return new AIResponse
            {
                ClassName = _classNames[maxIndex],
                Confidence = $"{maxProbability:P2}", // تحويل لنسبة مئوية
                ClassId = maxIndex
            };
        }


        public async Task<DenseTensor<float>> PreprocessImage(Stream imageStream)
        {
            // تحميل الصورة
            using var image = Image.Load<Rgb24>(imageStream);

            // تغيير الحجم لـ 299x299
            image.Mutate(x => x.Resize(299, 299));

            // إنشاء التنسور (BatchSize, Height, Width, Channels)
            // ملاحظة: تأكد من ترتيب الأبعاد حسب ما يطلبه الموديل بعد التحويل (غالباً NHWC في Keras المحول)
            var tensor = new DenseTensor<float>(new[] { 1, 299, 299, 3 });

            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < accessor.Width; x++)
                    {
                        var pixel = pixelRow[x];

                        // معادلة Xception Preprocessing:
                        // (Pixel / 127.5) - 1.0
                        // لتحويل القيم من [0, 255] إلى [-1, 1]

                        tensor[0, y, x, 0] = (pixel.R / 127.5f) - 1.0f;
                        tensor[0, y, x, 1] = (pixel.G / 127.5f) - 1.0f;
                        tensor[0, y, x, 2] = (pixel.B / 127.5f) - 1.0f;
                    }
                }
            });

            return tensor;
        }
    }
}
