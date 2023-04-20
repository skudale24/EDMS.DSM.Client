namespace EDMS.DSM.Client.Managers.Feedback;

public interface IFeedbackManager : IManager
{
    Task<IApiResult> FeedbackSubmit<TIn>(TIn feedbackSubmit);
    Task<IApiResult<UserInfoDto>> GetUserData();
}
