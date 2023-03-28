using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TaskWithYou.Shared.Model;

namespace TaskWithYou.Client.Pages.Content.Tasks
{
    public partial class Card
    {
        [Inject]
        private HttpClient _Http { get; set; }

        [Parameter]
        public TicketCard CardContent { get; set; }

        protected override Task OnInitializedAsync()
        {
            //TicketCard = _Http.GetFromJsonAsync<TicketCard>();
            return base.OnInitializedAsync();
        }

        //private TicketCard TicketCard { get; set; }
    }
}
