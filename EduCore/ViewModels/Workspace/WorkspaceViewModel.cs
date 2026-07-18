namespace EduCore.ViewModels.Workspace
{
    public class WorkspaceViewModel
    {
        public string Title { get; set; } = "";

        public string Subtitle { get; set; } = "";

        public string Icon { get; set; } = "";

        public string PrimaryActionText { get; set; } = "";

        public string PrimaryActionController { get; set; } = "";

        public string PrimaryActionAction { get; set; } = "Index";

        public string SecondaryActionText { get; set; } = "";

        public string SecondaryActionController { get; set; } = "";

        public string SecondaryActionAction { get; set; } = "Index";
    }
}