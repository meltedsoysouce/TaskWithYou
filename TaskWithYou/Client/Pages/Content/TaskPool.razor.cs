using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TaskWithYou.Shared.Model;
using TaskWithYou.Client.Pages.Content.Tasks;

namespace TaskWithYou.Client.Pages.Content
{
    public partial class TaskPool
    {
        private ViewModel viewModel { get; set; } = new();

        [Inject]
        private HttpClient Http { get; set; }

        private bool CanRender {
            get => viewModel != null && viewModel.TaskItems != null && viewModel.TaskItems.Count() != 0; 
        }

        // modals this page have
        private _EditTask EditModal { get; set; } = new();
        private _DeleteTask DeleteModal { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await InitializeViewModel();
        }      

        /// <summary>
        /// ViewModelの初期化
        /// </summary>
        /// <returns></returns>
        private async Task InitializeViewModel()
        {
            OnRefleshList();
        }

        /// <summary>
        /// リストのリフレッシュ
        /// </summary>
        /// <returns>なし</returns>
        private async Task OnRefleshList()
        {
            //var entitis = await Http.GetFromJsonAsync<TaskTicket[]>("api/taskticket");
            var entitis = await Http.GetFromJsonAsync<TaskTicket[]>("api/taskticket");
            viewModel.TaskItems = entitis.Select(a =>
            {
                ViewModel.TaskItem item = new()
                {
                    Gid = a.Gid,
                    TourokuBi = a.TourokuBi,
                    Name = a.Name,
                    KigenBi = a.KigenBi,
                    Detail = a.Detail
                };

                ViewModel.ListStateItem state = new()
                {
                    Gid = a.State.Gid,
                    Name = a.State.StateName,
                    State = a.State.State
                };

                item.State = state;

                return item;
            })
            .ToArray();

            StateHasChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pState"></param>
        /// <returns></returns>
        private string GetColorClass(State pState)
        {
            switch (pState)
            {
                case State.BeforeDoing:
                    return "before-doing";
                case State.Doing:
                    return "doing";
                case State.Finished:
                    return "finished";
                default:
                    return "";
            }
        }

        /// <summary>
        /// ViewModelクラス
        /// </summary>
        private class ViewModel
        {
            public TaskItem[] TaskItems { get; set; } = Array.Empty<TaskItem>();

            internal class TaskItem
            {
                public Guid Gid { get; set; }

                public int TourokuBi { get; set; }

                public string Name { get; set; }

                public int KigenBi { get; set; }

                public string Detail { get; set; }

                public ListStateItem State { get; set; }
            }

            internal class ListStateItem
            {
                public Guid Gid { get; set; }

                public string Name { get; set; }

                public State State { get; set; }
            }
        }
    }
}
