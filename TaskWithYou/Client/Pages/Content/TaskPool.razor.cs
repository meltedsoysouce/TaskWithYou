using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TaskWithYou.Shared.Model;
using TaskWithYou.Client.Pages.Content.Tasks;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;

namespace TaskWithYou.Client.Pages.Content
{
    public partial class TaskPool
    {
        [Inject]
        private HttpClient Http { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private bool CanRender
        {
            get => TaskTickets != null && TaskTickets.Count() > 0;
        }
        
        private bool IsUpdate { get; set; } = false;

        // modals this page have
        private _EditTask EditModal { get; set; } = new();
        private _DeleteTask DeleteModal { get; set; } = new();

        private async void OpenModal(Func<Task> pModalOpenFunc)
        {
            if (UpdateList.Count() > 0)
                await UpdateTodayTask();

            Task.Run(pModalOpenFunc);
        }

        private async void JumpToOtherPage(string pURL)
        {
            if (UpdateList.Count() > 0)
                await UpdateTodayTask();

            NavigationManager.NavigateTo(pURL);
        }

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
            var entitis = await Http.GetFromJsonAsync<TaskTicket[]>("api/taskticket");
            TaskTickets = entitis;
            UpdateList.Clear();

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
            //var ticket = 
            //await Http.PutAsJsonAsync("api/taskticket", pTask);
            UpdateList.Add(pTask);
        }

        private async Task UpdateTodayTask()
        {
            await Http.PostAsJsonAsync("api/taskticket/updatetasks", UpdateList.ToArray());
        }

        // to update today task?
        private List<TaskTicket> UpdateList { get; set; } = new();

        private TaskTicket[] TaskTickets { get; set; }
    }
}
