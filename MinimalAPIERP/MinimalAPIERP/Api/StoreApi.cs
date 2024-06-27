using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ERP;
using ERP.Data;

namespace MinimalAPIERP.Api
{
    internal static class StoreApi
    {
        public static RouteGroupBuilder MapStoreApi(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/erp").WithTags("Store Api");

            group.MapGet("/stores", async (AppDbContext db) =>
                Results.Ok(await db.Stores.Include(s => s.Rainchecks).ToListAsync()))
                .WithOpenApi();

            group.MapGet("/store/{storeid}", async (int storeid, AppDbContext db) =>
            {
                var store = await db.Stores.Include(s => s.Rainchecks).FirstOrDefaultAsync(s => s.StoreId == storeid);
                return store != null ? Results.Ok(store) : Results.NotFound();
            })
                .WithOpenApi();

            group.MapPost("/store", async (Store newStore, AppDbContext db) =>
            {
                using var transaction = await db.Database.BeginTransactionAsync();
                try
                {
                    newStore.Code = GenerateRandomCode();
                    db.Stores.Add(newStore);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new CreatedResult($"/erp/store/{newStore.StoreId}", new
                    {
                        newStore.StoreId,
                        newStore.Name,
                        newStore.Code
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear el store: {ex.Message}");
                    await transaction.RollbackAsync();
                    return new BadRequestObjectResult("Error al crear el store");
                }
            })
            .Produces<object>()
            .WithOpenApi();

            group.MapPut("/store/{storeid}", async (int storeid, Store updatedStore, AppDbContext db) =>
            {
                var existingStore = await db.Stores.FindAsync(storeid);
                if (existingStore == null)
                    return Results.NotFound();

                existingStore.Name = updatedStore.Name; // Actualizar otros campos según sea necesario
                db.Stores.Update(existingStore);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithOpenApi();

            group.MapDelete("/store/{storeid}", async (int storeid, AppDbContext db) =>
            {
                var existingStore = await db.Stores.FindAsync(storeid);
                if (existingStore == null)
                    return Results.NotFound();

                db.Stores.Remove(existingStore);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithOpenApi();

            return group;
        }

        private static string GenerateRandomCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
