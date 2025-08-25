namespace GoToWorkContracts.BindingModels
{
    public class EmailBindingModel
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string? AttachmentPath { get; set; }
    }
}
