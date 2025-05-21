using System;
using F1_Web.Data;
using F1_Web.ModelDTO;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using F1_Web.Ergast;
using F1_Web.Model;

namespace F1_Web.EndPoints;

public static class TeamEndPoints
{
    public static RouteGroupBuilder MapTeamEndPoints(this RouteGroupBuilder group)
    {
        group.MapGet("/teams", async (F1DbContext db) =>
        {
            var teams = await db.Constructors.Select(c => new ConstructorDTO(c)).AsNoTracking().ToListAsync();
            return Results.Ok(teams);
        });
        // Endpoint per importare tutti i team dall'API Ergast e salvarli nel database
        group.MapPost("/import-teams", async (F1DbContext db) =>
        {
            var client = new HttpClient();
            int offset = 0;
            int limit = 100;
            bool hasMoreData = true;

            while (hasMoreData)
            {
                // Chiamata API Ergast per ottenere i team
                var response = await client.GetStringAsync($"https://ergast.com/api/f1/constructors.json?limit={limit}&offset={offset}");
                var data = JsonSerializer.Deserialize<ErgastResponse>(response);
                var teams = data?.MRData?.ConstructorTable?.Constructors;

                if (teams.Count == 0)
                {
                    hasMoreData = false;
                    break;
                }

                // Controllo duplicati ed inserimento nel database
                foreach (var team in teams)
                {
                    if (!await db.Constructors.AnyAsync(c => c.name == team.name))
                    {
                        db.Constructors.Add(team);
                    }
                }

                await db.SaveChangesAsync();
                offset += limit; // Avanzamento per la prossima richiesta API
            }

            return Results.Ok(new { message = "Tutti i team Ergast sono stati importati!" });
        });

        group.MapPost("/teams", async (F1DbContext db, ConstructorDTO ConstructorDTO) =>
        {
            Constructor Constructor = new()
            {
                constructorId = ConstructorDTO.constructorId,
                name = ConstructorDTO.name,
                nationality = ConstructorDTO.nationality
            };

            db.Constructors.Add(Constructor);
            await db.SaveChangesAsync();

            return Results.Created($"/teams/{Constructor.constructorId}", new ConstructorDTO(Constructor));
        });

        group.MapGet("/teams/{id}", async (F1DbContext db, string id) =>
        {
            Constructor? Constructor = await db.Constructors.FindAsync(id);
            if (Constructor is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(new ConstructorDTO(Constructor));
        });

        group.MapPut("/teams/{id}", async (F1DbContext db, string id, ConstructorDTO ConstructorDTO) =>
		{
			Constructor? Constructor = await db.Constructors.FindAsync(id);
			if (Constructor is null)
			{
				return Results.NotFound();
			}

			Constructor.name = ConstructorDTO.name;
			Constructor.nationality = ConstructorDTO.nationality;

			await db.SaveChangesAsync();
			return Results.NoContent();
		});

        group.MapDelete("/teams/{id}", async (F1DbContext db, string id) =>
        {

            Constructor? Constructor = await db.Constructors.FindAsync(id);
            if (Constructor is null)
            {
                return Results.NotFound();
            }

            db.Constructors.Remove(Constructor);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}
