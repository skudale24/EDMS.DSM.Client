using System.ComponentModel.DataAnnotations;

namespace EDMS.DSM.Client.DTO;

public class SubmitFeedbackUploadDto
{
    public string Name { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;

    [Required]
    [StringLength(500, ErrorMessage = "Feedback text must be greater than 4 characters", MinimumLength = 5)]
    public string Feedback { get; set; } = string.Empty;
}
