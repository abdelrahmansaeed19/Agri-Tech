namespace AgriculturalTech.API.Repositories.Interfaces
{
    public interface IAiAuthorizationRepository
    {
        /// <summary>
        /// Checks if the user has access to the AI feature based on their subscription status and usage limits.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <returns>True if the user is authorized to use the AI feature, false otherwise.</returns>
        /// 
        Task ToggleUserPremium(string userId);
        Task<bool> CanUserRunAiScanAsync(string userId);

        Task RecordSuccessfulScanAsync(string userId);
    }
}
