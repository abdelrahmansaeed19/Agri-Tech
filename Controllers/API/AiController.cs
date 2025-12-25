using AgriculturalTech.API.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AgriculturalTech.API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        public AiController(IUnitOfWork unitOfWork, IMapper mapper, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        [Authorize]
        [HttpPost("upload_img")]
        public async Task<ActionResult<ApiResponse<AIResponse>>> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            using var content = new MultipartFormDataContent();
            using var stream = image.OpenReadStream();
            using var streamContent = new StreamContent(stream);

            // "file" matches the Python variable name

            content.Add(streamContent, "file", image.FileName);

            var pythonApiUrl = "http://127.0.0.1:8000/predict";

            try
            {
                var response = await _httpClient.PostAsync(pythonApiUrl, content);

                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var aiResult = JsonSerializer.Deserialize<AIResponse>(responseString, options);

                return Ok(ApiResponse<AIResponse>.SuccessResponse(aiResult, "Image processed successfully"));

            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Error communicating with AI service", new List<string> { ex.Message }));
            }
        }
    }
}
