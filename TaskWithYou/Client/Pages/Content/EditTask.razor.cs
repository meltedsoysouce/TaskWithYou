using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using TaskWithYou.Shared.Model;
using BlazorStrap.V5;
using TaskWithYou.Shared;

namespace TaskWithYou.Client.Pages.Content
{
    public partial class EditTask
    {
        // HttoClient
        [Inject]
        private HttpClient Http { get; set; }

        // ViewModel
        private ViewModel viewModel { get; set; }

        // We use async method to initilaize.
        // so we must block the null expception with this field.
        private bool CanRender { get => viewModel != null && viewModel.StateItems != null; }

        // modal field
        private BSModal BsModal { get; set; } = new();

        // EventCallBack
        [Parameter]
        public EventCallback OnClose { get; set; }

        protected override async Task OnInitializedAsync()
        {
            viewModel = new();
        }

        /// <summary>
        /// Intialize ViewModel to add task.
        /// </summary>
        private async Task InitializeViewModelToAdd(EditMode pMode)
        {
            viewModel = new() { Mode = EditMode.Add };
            await InitializeStateItems();
            viewModel.SelectedState = viewModel
                .StateItems
                .First(a => a.State == State.BeforeDoing);
        }

        /// <summary>
        /// Initialize ViewModel to edit task.
        /// </summary>
        private async Task InitializeViewModelToEdit(Guid pGid, EditMode pMode)
        {
            var targetTask = await Http.GetFromJsonAsync<TaskTicket>($"api/taskticket/{pGid}");
            if (targetTask == null)
            {
                // Todo: Must handle the case targetTask is null.
            }
            else
            {
                viewModel = new()
                {
                    Gid = targetTask.Gid,
                    Name = targetTask.Name,
                    TourokuBi = targetTask.TourokuBi,
                    StrTourokuBi = IntDateToString(targetTask.TourokuBi),
                    KigenBi = targetTask.KigenBi,
                    StrKigenBi = IntDateToString(targetTask.KigenBi),
                    Detail = targetTask.Detail,
                    Mode = EditMode.Edit
                };

                viewModel.SelectedState = new()
                {
                    Gid = targetTask.State.Gid,
                    Name = targetTask.State.StateName,
                    State = targetTask.State.State
                };

                await InitializeStateItems();
            }
        }

        private async Task InitializeStateItems()
        {
            var states = await Http.GetFromJsonAsync<TaskState[]>("api/taskstate");
            viewModel.StateItems =  states
                .OrderBy(a => a.State)
                .Select(a =>
                {
                    return new ViewModel.StateItem()
                    {
                        Gid = a.Gid,
                        Name = a.StateName,
                        State = a.State
                    };
                })
                .ToArray();            
        }

        /// <summary>
        /// open by outer page
        /// </summary>
        public async Task OnOpenAsync(Guid pGid ,EditMode pMode)
        {
            switch (pMode)
            {
                case EditMode.Add:
                    await InitializeViewModelToAdd(pMode);
                    break;
                case EditMode.Edit:
                    await InitializeViewModelToEdit(pGid, pMode);
                    break;
                default:
                    throw new NotSupportedException($"EditModeエラー：このモードは存在しません {pMode}");
            }
            await BsModal.ShowAsync();
        }

        private void OnChangeSelectedItem(ChangeEventArgs pArg)
        {
            var state = (State)Enum.Parse(typeof(State), pArg.Value.ToString());

            viewModel.SelectedState = viewModel
                .StateItems
                .First(a => a.State == state);
        }

        /// <summary>
        /// this method is called when editform valication is all ok
        /// </summary>
        /// <returns></returns>
        private async Task OnValidSubmitAsync()
        {
            var ticket = new TaskTicket()
            {
                Gid = viewModel.Gid,
                Name = viewModel.Name,
                TourokuBi = viewModel.TourokuBi,
                KigenBi = viewModel.KigenBi,
                Detail = viewModel.Detail,
            };

            var state = new TaskState()
            {
                Gid = viewModel.SelectedState.Gid,
                StateName = viewModel.SelectedState.Name,
                State = viewModel.SelectedState.State
            };
            ticket.State = state;

            switch (viewModel.Mode)
            {
                case EditMode.Add:
                    await Http.PostAsJsonAsync("api/taskticket", ticket);
                    break;
                case EditMode.Edit:
                    await Http.PutAsJsonAsync("api/taskticket", ticket);
                    break;
                default:
                    throw new NotSupportedException($"EditModeエラー：このモードは存在しません {viewModel.Mode}");
            }
            await BsModal.HideAsync();
            await OnClose.InvokeAsync();
        }

        /// <summary>
        /// Int date convert to string date.
        /// </summary>
        /// <param name="pIntDate">Date by int</param>
        /// <returns> converted Date as string </returns>
        private string IntDateToString(int pIntDate)
        {
            var _strdate = $"{pIntDate}";
            var year = _strdate.Substring(0, 4);
            var month = _strdate.Substring(4, 2);
            var day = _strdate.Substring(6, 2);

            return $"{year}-{month}-{day}";
        }

        /// <summary>
        /// class of ViewModel
        /// </summary>
        private class ViewModel
        {
            // this field need to keep working edit function. 
            public Guid Gid { get; set; } = Guid.NewGuid();

            // Mode of this page
            public EditMode Mode { get; set; }

            public int TourokuBi { get; set; }

            [Required]
            [RegularExpression("[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])",
                                ErrorMessage = "日付の形式で入力してください。")]
            public string StrTourokuBi { get; set; }

            [Required]
            public string Name { get; set; }

            public int KigenBi { get; set; }
            [RegularExpression("[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])",
                                ErrorMessage = "日付の形式で入力してください。")]
            public string StrKigenBi { get; set; }

            public string Detail { get; set; }

            public StateItem SelectedState { get; set; } = new();

            public StateItem[] StateItems { get; set; }            

            internal class StateItem
            {
                public Guid Gid { get; set; }

                public string Name { get; set; }

                public State State { get; set; }
            }

            internal ViewModel() 
            {
                Name = "";
                var today = DateTime.Today.ToString("yyyy-MM-dd");
                var int_today = int.Parse(today.Replace("-", ""));
                StrTourokuBi = today;
                TourokuBi = int_today;
                StrKigenBi = today;
                KigenBi = int_today;
                Detail = "";               
            }
        }
    }
}
