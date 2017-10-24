using System.Threading.Tasks;

namespace BlogPlatform.Hubs
{
    public interface ICommentsHub
    {
        Task RefreshCommentsCount();
    }
}
