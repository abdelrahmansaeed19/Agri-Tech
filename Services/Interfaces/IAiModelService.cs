using AgriculturalTech.API.DTOs;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace AgriculturalTech.API.Services.Interfaces
{
    public interface IAiModelService
    {
        public Task<AIResponse> PredictAsync(Stream imageStream);

        public Task<DenseTensor<float>> PreprocessImage(Stream imageStream);

    }
}
