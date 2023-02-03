using BlazorStrap.V5;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TaskWithYou.Shared.Model;

namespace TaskWithYou.Client.Pages.Content
{
    public partial class DeleteTask
    {
        // ViewModel
        private ViewModel viewModel { get; set; }

        // HttpClient
        [Inject]
        private HttpClient Http { get; set; }

        private bool CanRender
        {
            get => viewModel != null;
        }

        // Modal Field
        private BSModal BsModal { get; set; }

        // EventCallBack. When this modal close, this calla parent componet's method.
        [Parameter]
        public EventCallback OnClose { get; set; }


        /// <summary>
        /// Open when call this modal by another component
        /// </summary>
        /// <param name="pGid">Task Gid</param>
        /// <returns></returns>
        public async Task OnOpenAsync(Guid pGid)
        {
            var task = await Http.GetFromJsonAsync<TaskTicket>($"api/taskticket/{pGid}");
            viewModel = new()
            {
                Gid = task.Gid,
                TourokuBi = task.TourokuBi,
                Name = task.Name,
                KigenBi = task.KigenBi,
                Detail = task.Detail,
            };
            viewModel.StateItem = new()
            {
                Gid = task.State.Gid,
                Name = task.State.StateName,
                State = task.State.State
            };

            await BsModal.ShowAsync();
        }

        private async Task OnValidSubmitAsync()
        {
            await Http.DeleteAsync($"api/taskticket/{viewModel.Gid}");
            BsModal.HideAsync();
            OnClose.InvokeAsync();
        }

        private class ViewModel
        {
            public Guid Gid { get; set; }

            public int TourokuBi { get; set; }

            public string Name { get; set; }

            public int KigenBi { get; set; }

            public string Detail { get; set; }

            public DeleteStateItem StateItem { get; set; }
        }

        private class DeleteStateItem
        {
            public Guid Gid { get; set;}

            public string Name { get; set; }

            public State State { get; set; }
        }
    }
}
