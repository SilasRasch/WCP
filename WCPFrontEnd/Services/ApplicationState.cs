using Microsoft.EntityFrameworkCore;
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

        public async Task LoadDataAsync(int userId)
        {
            if (IsDataLoaded) return; // Prevent duplicate loading

            CurrentUser = await _context.Users.Include(x => x.Language).SingleOrDefaultAsync(x => x.Id == userId);
            Language = CurrentUser.Language;
            IsDataLoaded = true;

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
