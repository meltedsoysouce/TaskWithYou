using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TaskWithYou.Client.Pages.Content.Tasks;
using TaskWithYou.Shared.Model;

namespace TaskWithYou.Client.Pages.Content
{
    public partial class CardView : ComponentBase
    {
        [Inject]
        private HttpClient _Http { get; set; }

        private bool CanRender { get => Cards != null && Cards.Count > 0; }
        //private List<Card> Cards { get; set; }
        private List<TicketCard> Cards { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //await base.OnInitializedAsync();
            Cards = await _Http.GetFromJsonAsync<List<TicketCard>>("/api/TicketCard");
            StateHasChanged();
        }
    }
}
