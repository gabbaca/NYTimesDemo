using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MudBlazor;
using NYTimesDemo.Helpers;
using NYTimesDemo.Models;
using NYTimesDemo.Services.Interfaces;

namespace NYTimesDemo.Components
{
    public partial class NYTimesDashboardComponent
    {
        [Inject] public INYTimesService NYTimesService { get; set; }
        private NYTimesPeriodEnum NYTimesPeriodEnum { get; set; } = NYTimesPeriodEnum.LastDay;
        private NYTimesArticlesModel NYTimesDataModel { get; set; }
        private Result[] NYTimesResults { get; set; }
        private int NYTimesPeriodInt { get; set; } = (int)NYTimesPeriodEnum.LastDay;
        private List<IGrouping<string, Result>> GroupedResults { get; set; }
        private bool IsLoading = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetMostPopularArticles(NYTimesPeriodEnum.LastDay);
            }
        }

        async Task GetMostPopularArticles(NYTimesPeriodEnum period)
        {
            IsLoading = true;
            NYTimesDataModel = await NYTimesService.GetTopRecentArticles(period);

            if (NYTimesDataModel != null)
            {
                NYTimesResults = NYTimesDataModel.Results.OrderByDescending(o => o.PublishedDate).ToArray();                
            }
            IsLoading = false;
            this.StateHasChanged();
        }

        async void OnSelectedValuesChanged(IEnumerable<int> values)
        {
            await GetMostPopularArticles((NYTimesPeriodEnum)values.FirstOrDefault());
        }

        string GetImageUrlFromMedia(Media[] media)
        {
            if (media != null)
            {
                foreach (var item in media)
                {
                    if (item.MediaMetadata != null)
                    {
                        // Return first non-null image
                        return item.MediaMetadata[0].Url.ToString();
                    }
                }
            }
            return String.Empty;
        }

        string FormatShortDate(DateTimeOffset dateTimeOffset)
        {
            DateTimeOffset dateTime = dateTimeOffset.ToLocalTime();
            return dateTime.ToString("MM/dd/yyyy");
        }      
    }
}
