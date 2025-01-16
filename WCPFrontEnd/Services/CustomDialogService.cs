using MudBlazor;
using WCPFrontEnd.Components.Pages.ProjectPages.Dialoges;
using WCPShared.Models.Entities.ProjectModels;
using WCPShared.Models.Enums;

namespace WCPFrontEnd.Services
{
    public class CustomDialogService
    {
        public IDialogService DialogService { get; set; }

        public CustomDialogService(IDialogService dialogService)
        {
            DialogService = dialogService;
        }

        public async Task OpenStatusDialog(Project project, ProjectStatus newStatus)
        {
            var parameters = new DialogParameters<ProjectStatusDialog>
            {
                { x => x.Project, project },
                { x => x.NewStatus, newStatus }
            };

            var options = new DialogOptions { CloseOnEscapeKey = true };

            var dialog = await DialogService.ShowAsync<ProjectStatusDialog>("Status dialog", parameters, options);
            var result = await dialog.Result;
        }

        public async Task OpenStatusDialogAdmin(Project project)
        {
            var parameters = new DialogParameters<ProjectStatusDialog_Admin>
            {
                { x => x.Project, project }
            };

            var options = new DialogOptions { CloseOnEscapeKey = true };

            var dialog = await DialogService.ShowAsync<ProjectStatusDialog_Admin>("Status admin dialog", parameters, options);
            var result = await dialog.Result;
        }
    }
}
