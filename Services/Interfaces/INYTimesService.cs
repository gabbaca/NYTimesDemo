using NYTimesDemo.Helpers;
using NYTimesDemo.Models;

namespace NYTimesDemo.Services.Interfaces
{
    public interface INYTimesService
    {
        Task<NYTimesArticlesModel> GetTopRecentArticles(NYTimesPeriodEnum period);
    }
}
