using BlazorStrap.V5;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using TaskWithYou.Shared;
using TaskWithYou.Shared.Model;

namespace TaskWithYou.Client.Pages.Content.Clusters
{
    public partial class _EditCluster
    {
        private BSModal BsModal { get; set; } = new();

        [Inject]
        private HttpClient _Http { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }

        private EditClusterMode EditModel { get; set; }

        private bool CanRender { get => EditModel != null; }

        public async Task OpenAddAsync()
        {
            EditModel = new() { Mode = EditMode.Add };
            var _ = BsModal.ShowAsync();
        }

        public async Task OpenEditAsync(Guid pGid)
        {
            var entity = await _Http.GetFromJsonAsync<Cluster>($"api/cluster/{pGid}");
            EditModel = new()
            {
                Gid = entity.Gid,
                Name = entity.Name,
                Detail = entity.Detail,
                Mode = EditMode.Edit
            };

            var _ = BsModal.ShowAsync();
        }

        private async Task OnValidSubmitAsync()
        {
            Cluster entity = new()
            {
                Gid = EditModel.Gid,
                Name = EditModel.Name,
                Detail = EditModel.Detail,
            };

            switch (EditModel.Mode)
            {
                case EditMode.Edit:
                    await _Http.PutAsJsonAsync("api/cluster", entity);
                    break;
                case EditMode.Add:
                    await _Http.PostAsJsonAsync("api/cluster", entity);
                    break;
                default:
                    return;
            }

            _ = OnClose.InvokeAsync();
            _ = BsModal.HideAsync();
        }

        private class EditClusterMode
        {
            public Guid Gid { get; set; }

            [Required]
            public string Name { get; set; }

            public string Detail { get; set; }

            public EditMode Mode { get; set; }
        }
    }
}
