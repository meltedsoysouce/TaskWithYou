using BlazorStrap.V5;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TaskWithYou.Shared.Model;

namespace TaskWithYou.Client.Pages.Content.Clusters
{
    public partial class _DeleteCluster
    {
        private BSModal BsModal { get; set; }

        [Inject]
        private HttpClient _Http { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }

        private DeleteModel Model { get; set; }

        private bool CanRender { get => Model != null; }

        public async Task OnShowAsync(Guid pGid)
        {
            var entity = await _Http.GetFromJsonAsync<Cluster>($"api/cluster/{pGid}");

            Model = new()
            {
                Gid = entity.Gid,
                Name = entity.Name,
                Detail = entity.Detail,
            };

            var _ = BsModal.ShowAsync();
        }

        private async Task OnValidSubmitAsync()
        {
            await _Http.DeleteAsync($"api/cluster/{Model.Gid}");
            var _ = OnClose.InvokeAsync();
            _ = BsModal.HideAsync();
        }

        private class DeleteModel
        {
            public Guid Gid { get; set; }
            public string Name { get; set; }
            public string Detail { get; set; }
        }
    }
}
