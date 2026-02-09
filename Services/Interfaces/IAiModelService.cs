using AgriculturalTech.API.DTOs;

using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AgriculturalTech.API.Services.Interfaces
{
    public interface IAiModelService
    {
        public Task<AIResponse> PredictAsync(Stream imageStream);

        public Task<DenseTensor<float>> PreprocessImage(Stream imageStream);

    }
}
