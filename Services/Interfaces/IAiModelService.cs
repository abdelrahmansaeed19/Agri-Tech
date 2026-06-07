using AgriculturalTech.API.DTOs;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace AgriculturalTech.API.Services.Interfaces
{
    public interface IAiModelService
    {
        public Task<AIResponseDto> PredictAsync(Stream imageStream);

        public Task<DiseaseInfoDto> GetDiseaseInfo(string diseaseName);

        public Task<DenseTensor<float>> PreprocessImage(Stream imageStream);

    }
}
