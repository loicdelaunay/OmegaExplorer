using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services.Databases;

namespace OmegaExplorer.Server.Services._Game.Orders;

public class OrderRepository
{
    private readonly DatabaseContext _databaseContext;

    public OrderRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task DeleteById(Guid orderId, Guid userId)
    {
        //TODO IMMEDIATE add user check 
        var order = await _databaseContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.OwnerUserId == userId);

        if (order == null) throw new Exception("Order not found.");

        _databaseContext.Orders.Remove(order);
        await _databaseContext.SaveChangesAsync();
    }
}