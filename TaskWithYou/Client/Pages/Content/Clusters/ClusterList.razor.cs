using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TaskWithYou.Shared.Model;

namespace TaskWithYou.Client.Pages.Content.Clusters
{
    public partial class ClusterList
    {
        [Inject]
        private HttpClient _Http { get; set; }
        
        private bool CanRender
        {
            get => ListItems != null && ListItems.Count() > 0; 
        }

        protected override async Task OnInitializedAsync()
        {
            ListItems = await _Http.GetFromJsonAsync<Cluster[]>("api/cluster");
            StateHasChanged();
        }

        private Cluster[] ListItems { get; set; }
    }
}
