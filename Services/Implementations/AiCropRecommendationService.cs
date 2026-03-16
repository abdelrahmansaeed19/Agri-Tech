using AgriculturalTech.API.Services.Interfaces;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using AgriculturalTech.API.DTOs;
using System.Text.Json;

namespace AgriculturalTech.API.Services.Implementations
{
    public class AiCropRecommendationService : IAiCropRecommendationService
    {
        private readonly InferenceSession _session;
        private readonly List<string> _cropNames;
        public AiCropRecommendationService()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string modelPath = Path.Combine(basePath, "Services/Resources", "Naive_Bayes.onnx");

            _session = new InferenceSession(modelPath);

            // 2. تحميل وقراءة ملف الـ JSON
            try
            {
                string cropNamesPath = Path.Combine(basePath, "Services/Resources", "crop_names.json");

                string jsonContent = File.ReadAllText(cropNamesPath);

                // تحويل الـ JSON إلى List من الـ Strings
                _cropNames = JsonSerializer.Deserialize<List<string>>(jsonContent);

                if (_cropNames == null || _cropNames.Count == 0)
                    throw new Exception("Crop names file is empty or invalid.");

                Console.WriteLine($"✅ Loaded {_cropNames.Count} classes from JSON.");
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Failed to load class names: {ex.Message}");
            }
        }

        public async Task<CropResponseDto> PredictCropAsync(CropRecommendationRequestDto request)
        {
            // 1. Preprocess: Convert the 7 properties into a Tensor
            DenseTensor<float> tensor = PreprocessData(request);

            // 2. Prepare Inputs
            // Dynamically get the input name (usually "float_input")
            string inputName = _session.InputMetadata.Keys.First();

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(inputName, tensor)
            };

            // 3. Run Inference
            // We use Task.Run to keep the async signature, though ONNX runs this instantly
            using var results = await Task.Run(() => _session.Run(inputs));

            // 4. Extract Output
            // Scikit-learn ONNX models usually output the predicted label as the FIRST result.
            var predictionOutput = results.First().Value;
            string recommendedCrop = "Unknown";

            // Depending on how your Python model was trained, the output label might be a string or a long (int)
            if (predictionOutput is IEnumerable<string> stringResult)
            {
                recommendedCrop = stringResult.First();

                Console.WriteLine("✅ Predicted crop (string output): " + recommendedCrop);
            }
            else if (predictionOutput is IEnumerable<long> longResult)
            {
                int index = (int)longResult.First();

                if(index >= 0 && index < _cropNames.Count)
                {
                    recommendedCrop = _cropNames[index];

                    Console.WriteLine("✅ Predicted crop (long index output): " + recommendedCrop);
                }
                else
                {
                    Console.WriteLine($"⚠️ Warning: Predicted index {index} is out of bounds for crop names list.");

                    throw new Exception("Predicted index is out of bounds for crop names list.");
                }
            }
            else if (predictionOutput is IEnumerable<int> intResult)
            {
                int index = intResult.First();

                if(index >= 0 && index < _cropNames.Count)
                {
                    recommendedCrop = _cropNames[index];

                    Console.WriteLine("✅ Predicted crop (int index output): " + recommendedCrop);
                }
                else
                {
                    Console.WriteLine($"⚠️ Warning: Predicted index {index} is out of bounds for crop names list.");
                    throw new Exception("Predicted index is out of bounds for crop names list.");
                }
            }

            return new CropResponseDto
            {
                RecommendedCrop = recommendedCrop
            };
        }

        private DenseTensor<float> PreprocessData(CropRecommendationRequestDto req)
        {
            // Create a float array mapping exactly to the 7 features
            // ONNX expects float32 (float in C#), so we must cast the doubles.
            float[] data = new float[]
            {
                (float)req.Nitrogen,
                (float)req.Phosphorous,
                (float)req.Potassium,
                (float)req.Temperature,
                (float)req.Humidity,
                (float)req.Ph,
                (float)req.Rainfall
            };

            // Create a 2D Tensor with shape [1 row, 7 columns]
            return new DenseTensor<float>(data, new[] { 1, 7 });
        }
    }
}
