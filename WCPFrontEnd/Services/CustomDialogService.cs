using MudBlazor;
using WCPFrontEnd.Components.Pages.Account.Dialoges;
using WCPFrontEnd.Components.Pages.Brand.Dialoges;
using WCPFrontEnd.Components.Pages.ProductRelated.Dialoges;
using WCPFrontEnd.Components.Pages.ProjectPages;
using WCPFrontEnd.Components.Pages.ProjectPages.Dialoges;
using WCPFrontEnd.Components.Pages.Users.Dialoges;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.ProjectModels;
using WCPShared.Models.Entities.UserModels;
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

        public async Task OpenProductDialog(Product product)
        {
            var parameters = new DialogParameters<ProductDetailDialog>
            {
                { x => x.Product, product }
            };

            var options = new DialogOptions { CloseOnEscapeKey = true };

            var dialog = await DialogService.ShowAsync<ProductDetailDialog>("Product details", parameters, options);
            var result = await dialog.Result;
        }

        public async Task OpenBrandDetailDialog(Brand brand)
        {
            var parameters = new DialogParameters<EditBrandDialog>
            {
                { x => x.Brand, brand }
            };

            var options = new DialogOptions { CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<EditBrandDialog>("Delete dialog", parameters, options);
            var result = await dialog.Result;
        }

        public async Task OpenCreatorDetailDialog(Creator creator)
        {
            var parameters = new DialogParameters<CreatorDetailDialog>
            {
                { x => x.Creator, creator }
            };

            var options = new DialogOptions { CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<CreatorDetailDialog>("Creator detail dialog", parameters, options);
            var result = await dialog.Result;
        }

        public async Task OpenAddPaymentMethodDialog(User user)
        {
            var parameters = new DialogParameters<AddPaymentDialog>
            {
                { x => x.User, user }
            };

            var options = new DialogOptions { CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<AddPaymentDialog>("Creator detail dialog", parameters, options);
            var result = await dialog.Result;
        }

        public async Task OpenProjectCreation()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };
            var dialog = await DialogService.ShowAsync<CreateProject>("Creator detail dialog", options);
            var result = await dialog.Result;
        }
    }
}
