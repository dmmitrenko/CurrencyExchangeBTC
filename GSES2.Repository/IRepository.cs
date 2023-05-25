namespace GSES2.Repository;
public interface IRepository
{
    public Task<IEnumerable<string>> GetSubscribedEmailsAsync();
    public Task SubscribeEmailAsync(string email);
    public Task<bool> IsEmailExists(string email);
}