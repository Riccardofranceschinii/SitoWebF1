using System;
using F1_Web.Data;
using F1_Web.ModelDTO;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using F1_Web.Ergast;
using F1_Web.Model;

namespace F1_Web.EndPoints;

public static class PilotaEndPoints
{
    public static RouteGroupBuilder MapPilotaEndPoints(this RouteGroupBuilder group)
    {
        group.MapGet("/drivers", async (F1DbContext db) =>
        {
            var drivers = await db.Drivers.Select(d => new DriverDTO(d)).AsNoTracking().ToListAsync();
            return Results.Ok(drivers);
        });
        // Endpoint per importare tutti i piloti dall'API Ergast e salvarli nel database
        group.MapPost("/import-drivers", async (F1DbContext db) =>
        {
            var client = new HttpClient();
            int offset = 0;
            int limit = 100;
            bool hasMoreData = true;

            while (hasMoreData)
            {
                // Chiamata API Ergast per ottenere i piloti
                var response = await client.GetStringAsync($"https://ergast.com/api/f1/drivers.json?limit={limit}&offset={offset}");
                var data = JsonSerializer.Deserialize<ErgastResponse>(response);
                var drivers = data?.MRData?.DriverTable?.Drivers;

                if (drivers.Count == 0)
                {
                    hasMoreData = false;
                    break;
                }

                // Controllo duplicati ed inserimento nel database
                foreach (var driver in drivers)
                {
                    if (!await db.Drivers.AnyAsync(d => d.givenName == driver.givenName && d.familyName == driver.familyName))
                    {
                        db.Drivers.Add(driver);
                    }
                }

                await db.SaveChangesAsync();
                offset += limit; // Avanzamento per la prossima richiesta API
            }

            return Results.Ok(new { message = "Tutti i piloti Ergast sono stati importati!" });
        });

        group.MapPost("/drivers", async (F1DbContext db, DriverDTO driverDTO) =>
        {
            Driver driver = new()
            {
                driverId = driverDTO.driverId,
                givenName = driverDTO.givenName,
                familyName = driverDTO.familyName,
                dateOfBirth = driverDTO.dateOfBirth,
                nationality = driverDTO.nationality,
            };

            db.Drivers.Add(driver);
            await db.SaveChangesAsync();

            return Results.Created($"/drivers/{driver.driverId}", new DriverDTO(driver));
        });

        group.MapGet("/drivers/{id}", async (F1DbContext db, string id) =>
        {
            Driver? driver = await db.Drivers.FindAsync(id);
            if (driver is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(new DriverDTO(driver));
        });

        group.MapPut("/drivers/{id}", async (F1DbContext db, string id, DriverDTO driverDTO) =>
		{
			Driver? driver = await db.Drivers.FindAsync(id);
			if (driver is null)
			{
				return Results.NotFound();
			}
			driver.givenName = driverDTO.givenName;
			driver.familyName = driverDTO.familyName;
			driver.nationality = driverDTO.nationality;

			await db.SaveChangesAsync();
			return Results.NoContent();
		});

        group.MapDelete("/drivers/{id}", async (F1DbContext db, string id) =>
        {
            Driver? driver = await db.Drivers.FindAsync(id);
            if (driver is null)
            {
                return Results.NotFound();
            }

            db.Drivers.Remove(driver);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}
