using AgriculturalTech.API.DTOs;
using AgriculturalTech.API.Services.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Org.BouncyCastle.Security;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Globalization;
using System.Text.Json;


namespace AgriculturalTech.API.Services.Implementations
{
    public class AiModelService : IAiModelService
    {
        private readonly InferenceSession _session;

        private readonly List<string> _classNames;

        private readonly Dictionary<string, DiseaseInfoDto> _diseaseInfo;

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

            _diseaseInfo = new Dictionary<string, DiseaseInfoDto>(StringComparer.OrdinalIgnoreCase);

            LoadExcelDataIntoMemory();
        }

        private void LoadExcelDataIntoMemory()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string excelPath = Path.Combine(basePath, "Services/Resources", "disease_info.xlsx");

            if (!File.Exists(excelPath))
                throw new FileNotFoundException($"Excel file not found at {excelPath}");

            // Open the file as a stream
            using var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read);

            // Create the Excel Reader
            using var reader = ExcelReaderFactory.CreateReader(stream);

            // 1. Read the first row (the Headers) to skip them
            reader.Read();

            // 2. Loop through all remaining rows
            while (reader.Read())
            {
                // reader.GetString(columnIndex) grabs the data.
                // Column 1 is "disease_name"
                var diseaseName = reader.GetString(1)?.Trim();

                if (string.IsNullOrEmpty(diseaseName)) continue;

                var info = new DiseaseInfoDto
                {
                    Description = reader.GetString(2),          // Column C
                    PossibleSteps = reader.GetString(3),        // Column D
                    DiseaseImageUrl = reader.GetString(4),      // Column E
                    SupplementName = reader.GetString(5),       // Column F
                    SupplementImageUrl = reader.GetString(6)    // Column G
                };

                // Add to dictionary. TryAdd prevents crashes on duplicate rows.
                _diseaseInfo.TryAdd(diseaseName, info);
            }

            Console.WriteLine($"✅ Loaded {_diseaseInfo.Count} diseases/crops into RAM.");
        }


        public async Task<DiseaseInfoDto> GetDiseaseInfo(string diseaseName)
        {
            if (string.IsNullOrEmpty(diseaseName))
                throw new ArgumentException("Disease name cannot be null or empty.");

            if (_diseaseInfo.TryGetValue(diseaseName, out var info))
                return info;

            throw new KeyNotFoundException($"No information found for disease: {diseaseName}");
        }

        public async Task<AIResponseDto> PredictAsync(Stream imageStream)
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

            return new AIResponseDto
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
