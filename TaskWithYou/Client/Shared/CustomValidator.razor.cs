using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace TaskWithYou.Client.Shared
{
    public partial class CustomValidator : ComponentBase
    {
        private ValidationMessageStore? MessageStore { get; set; }

        [CascadingParameter]
        private EditContext? CurrentEditContext { get; set; }

        //protected override void OnInitialized()
        //{
        //    if (CurrentEditContext is null)
        //    {
        //        throw new InvalidOperationException("EditContextを設定してください");
        //    }

        //    MessageStore = new(CurrentEditContext);

        //    CurrentEditContext.OnValidationRequested += (sender, eventArgs) => MessageStore?.Clear();
        //    CurrentEditContext.OnFieldChanged += (sender, eventArgs) => 
        //        MessageStore?.Clear(eventArgs.FieldIdentifier);

        //    base.OnInitialized();
        //}

        protected override void OnParametersSet()
        {
            base.OnParametersSet();        
        
            if (CurrentEditContext is null)
            {
                throw new InvalidOperationException("EditContextを設定してください");
            }

            MessageStore = new(CurrentEditContext);

            CurrentEditContext.OnValidationRequested += (sender, eventArgs) => MessageStore?.Clear();
            CurrentEditContext.OnFieldChanged += (sender, eventArgs) => 
                MessageStore?.Clear(eventArgs.FieldIdentifier);
        }

        public void DisplayError(Dictionary<string, List<string>> errors)
        {
            if (CurrentEditContext is not null) 
            {
                foreach (var err in errors) 
                {
                    MessageStore?.Add(CurrentEditContext.Field(err.Key), err.Value);
                }

                CurrentEditContext.NotifyValidationStateChanged();
            }
        }

        public void ClearErrors()
        {
            MessageStore?.Clear();
            CurrentEditContext.NotifyValidationStateChanged();
        }
    }
}
