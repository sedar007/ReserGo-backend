﻿using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface INotificationDataAccess {
    Task<Notification> Create(Notification notification);
    Task<IEnumerable<Notification>> GetLatestNotifications(Guid userId, int count);
    Task<bool> Update(Notification notification);

    Task<Notification?> GetById(Guid id);
}