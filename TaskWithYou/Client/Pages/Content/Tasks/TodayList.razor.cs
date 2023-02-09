using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using TaskWithYou.Shared.Model;

namespace TaskWithYou.Client.Pages.Content.Tasks
{
    public partial class TodayList
    {
        [Inject]
        private HttpClient Http { get; set; }

        private bool CanRender { get => TodayTasks != null && TodayTasks.Count() != 0; }

        // Modals
        private _EditTask EditModal { get; set; }
        private _DeleteTask DeleteModal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            OnRefleshAsync();
        }

        private async Task OnRefleshAsync()
        {
            TodayTasks = await Http.GetFromJsonAsync<TaskTicket[]>("api/taskticket/today");
            StateHasChanged();
        }

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

        private TaskTicket[] TodayTasks { get; set; }        
    }
}
