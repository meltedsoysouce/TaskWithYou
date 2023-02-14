using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TaskWithYou.Shared.Model;
using TaskWithYou.Client.Pages.Content.Tasks;
using System.Net.Sockets;

namespace TaskWithYou.Client.Pages.Content
{
    public partial class TaskPool
    {
        [Inject]
        private HttpClient Http { get; set; }

        private bool CanRender
        {
            get => TaskTickets != null && TaskTickets.Count() > 0;
        }

        private bool IsUpdate { get; set; } = false;

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
            TaskTickets = entitis;

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

        private async Task OnChangeTodayTask(TaskTicket pTask)
        {
            pTask.IsTodayTask = !pTask.IsTodayTask;
            var ticket = 
            await Http.PutAsJsonAsync("api/taskticket", pTask);
        }

        private TaskTicket[] TaskTickets { get; set; }
    }
}
