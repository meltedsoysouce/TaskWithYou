using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using TaskWithYou.Shared.Model;
using BlazorStrap.V5;
using TaskWithYou.Shared;
using TaskWithYou.Client.Shared;

namespace TaskWithYou.Client.Pages.Content.Tasks
{
    public partial class _EditTask
    {
        // HttoClient
        [Inject]
        private HttpClient Http { get; set; }

        // ViewModel
        private ViewModel viewModel { get; set; } = new();

        // We use async method to initilaize.
        // so we must block the null expception with this field.
        private bool CanRender { get => viewModel != null && viewModel.StateItems != null; }

        // modal field
        private BSModal BsModal { get; set; } = new();

        // EventCallBack
        [Parameter]
        public EventCallback OnClose { get; set; }

        // CustomValidator
        private CustomValidator CustomValidator { get; set; }

        protected override async Task OnInitializedAsync()
        {
            viewModel = new();
        }

        /// <summary>
        /// Intialize ViewModel to add task.
        /// </summary>
        private async Task InitializeViewModelToAdd(EditMode pMode)
        {
            //viewModel = new() { Mode = EditMode.Add };
            viewModel.Mode = EditMode.Add;
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
                //viewModel = new()
                //{
                //    Gid = targetTask.Gid,
                //    Name = targetTask.Name,
                //    IsTodayTask = targetTask.IsTodayTask,
                //    TourokuBi = targetTask.TourokuBi,
                //    StrTourokuBi = IntDateToString(targetTask.TourokuBi),
                //    KigenBi = targetTask.KigenBi,
                //    StrKigenBi = IntDateToString(targetTask.KigenBi),
                //    Detail = targetTask.Detail,
                //    Mode = EditMode.Edit
                //};
                viewModel.Gid = targetTask.Gid;
                viewModel.Name = targetTask.Name;
                viewModel.IsTodayTask = targetTask.IsTodayTask;
                viewModel.TourokuBi = targetTask.TourokuBi;
                viewModel.StrTourokuBi = IntDateToString(targetTask.TourokuBi);
                viewModel.KigenBi = targetTask.KigenBi;
                viewModel.StrKigenBi = IntDateToString(targetTask.KigenBi);
                viewModel.Detail = targetTask.Detail;
                viewModel.Mode = EditMode.Edit;

                viewModel.SelectedState = new()
                {
                    Gid = targetTask.State.Gid,
                    Name = targetTask.State.Name,
                    State = targetTask.State.State
                };
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
                        Name = a.Name,
                        State = a.State
                    };
                })
                .ToArray();            
        }

        private async Task InitializeClusterItems()
        {
            viewModel.ClusterItems = await Http.GetFromJsonAsync<Cluster[]>("api/cluster");
        }

        /// <summary>
        /// open by outer page
        /// </summary>
        public async Task OnOpenAsync(Guid pGid ,EditMode pMode)
        {
            await InitializeStateItems();
            await InitializeClusterItems();

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

            var _ = BsModal.ShowAsync();
        }

        private void OnChangeSelectedItem(ChangeEventArgs pArg)
        {
            var state = (State)Enum.Parse(typeof(State), pArg.Value.ToString());

            viewModel.SelectedState = viewModel
                .StateItems
                .First(a => a.State == state);
        }

        private void OnChangeCluster(ChangeEventArgs pArg)
        {
            var gid = Guid.Parse(pArg.Value.ToString());
            viewModel.SelectedCluster = viewModel
                .ClusterItems
                .First(a => a.Gid == gid);
        }

        /// <summary>
        /// this method is called when editform valication is all ok
        /// </summary>
        /// <returns></returns>
        private async Task OnValidSubmitAsync()
        {
            // Intialize CustomValidator
            CustomValidator.ClearErrors();

            var isCheckOk = InputCheck(out var errors);
            if (isCheckOk is true)
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
                    Name = viewModel.SelectedState.Name,
                    State = viewModel.SelectedState.State
                };
                ticket.State = state;

                Cluster cluster = new();
                if (viewModel.SelectedCluster != null)
                {
                    cluster = new Cluster()
                    {
                        Gid = viewModel.SelectedCluster.Gid,
                        Name = viewModel.SelectedCluster.Name,
                        Detail = viewModel.SelectedCluster.Detail,
                    };
                }
                ticket.Cluster = cluster;

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
            }
            else
            {
                CustomValidator.DisplayError(errors);
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

        private bool InputCheck(out Dictionary<string, List<string>> errors)
        {
            errors = new();

            if (viewModel.SelectedState is null)
            {
                errors.Add("SelectedState", new() { "Stateがセットされていません。選択してください。" });
            }

            return errors.Count() > 0;
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

            public bool IsTodayTask { get; set; }

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

            public Cluster SelectedCluster { get; set; } = new();
            public Cluster[] ClusterItems { get; set; }

            internal class StateItem
            {
                public Guid Gid { get; set; }

                public string Name { get; set; }

                public State State { get; set; }
            }

            internal ViewModel() 
            {
                IsTodayTask = false;
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
