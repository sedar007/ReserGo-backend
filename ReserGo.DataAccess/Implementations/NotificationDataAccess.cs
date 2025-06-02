using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class NotificationDataAccess : INotificationDataAccess {
    private readonly ReserGoContext _context;

    public NotificationDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<Notification> Create(Notification notification) {
        var entry = await _context.AddAsync(notification);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }


    public async Task<IEnumerable<Notification>> GetLatestNotifications(Guid userId, int count) {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<bool> Update(Notification notification) {
        _context.Notifications.Update(notification);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Notification?> GetById(Guid id) {
        return await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> SaveChanges() {
        return await _context.SaveChangesAsync();
    }
}