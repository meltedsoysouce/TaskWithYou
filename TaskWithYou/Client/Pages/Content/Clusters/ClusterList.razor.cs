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

        private _EditCluster EditModal { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await OnReflesh();
            StateHasChanged();
        }

        private async Task OnReflesh()
        {
            ListItems = await _Http.GetFromJsonAsync<Cluster[]>("api/cluster");
        }

        private Cluster[] ListItems { get; set; }
    }
}
