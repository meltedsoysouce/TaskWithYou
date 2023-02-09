using Microsoft.AspNetCore.Components;
using System.Runtime;

namespace TaskWithYou.Client.Pages
{
    public partial class Index
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            NavigationManager.NavigateTo("/Content/TaskPool");
        }
    }
}
