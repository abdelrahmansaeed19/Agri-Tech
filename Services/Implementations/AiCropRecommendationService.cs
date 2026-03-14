using AgriculturalTech.API.Services.Interfaces;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using AgriculturalTech.API.DTOs;

namespace AgriculturalTech.API.Services.Implementations
{
    public class AiCropRecommendationService : IAiCropRecommendationService
    {
        private readonly InferenceSession _session;

        public AiCropRecommendationService()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string modelPath = Path.Combine(basePath, "Services/Resources", "Naive_Bayes.onnx");

            _session = new InferenceSession(modelPath);
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
            }
            else if (predictionOutput is IEnumerable<long> longResult)
            {
                recommendedCrop = longResult.First().ToString();
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
