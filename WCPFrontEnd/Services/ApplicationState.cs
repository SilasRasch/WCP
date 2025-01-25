using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WCPShared.Interfaces;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;

namespace WCPFrontEnd.Services
{
    public class ApplicationState
    {
        private readonly IWcpDbContext _context;

        public ApplicationState(IWcpDbContext context)
        {
            _context = context;
        }

        public User CurrentUser { get; private set; }
        public Language Language { get; private set; }

        public bool IsDataLoaded { get; private set; } = false;

        public event Action? OnChange;

        public async Task LoadDataAsync(Claim? userId)
        {
            if (IsDataLoaded) return; // Prevent duplicate loading

            if (userId is null)
            {
                IsDataLoaded = true;
                NotifyStateChanged();
                return;
            }

            CurrentUser = await _context.Users.Include(x => x.Language).SingleOrDefaultAsync(x => x.Id.ToString() == userId.Value);
            Language = CurrentUser!.Language;
            IsDataLoaded = true;

            NotifyStateChanged();
        }

        public void UnloadData()
        {
            CurrentUser = null;
            Language = null;
            IsDataLoaded = false;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
