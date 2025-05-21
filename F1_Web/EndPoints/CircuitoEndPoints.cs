using System;
using F1_Web.Model;
using F1_Web.ModelDTO;
using F1_Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using F1_Web.Ergast;
using Microsoft.AspNetCore.Mvc;

namespace F1_Web.EndPoints;
public static class CircuitoEndPoints
{
    public static RouteGroupBuilder MapCircuitoEndPoints(this RouteGroupBuilder group)
    {
        group.MapGet("/circuits", async (F1DbContext db) =>
        {
            var circuits = await db.Circuits.Select(c => new CircuitDTO(c)).AsNoTracking().ToListAsync();
            return Results.Ok(circuits);
        });
        // Endpoint per importare tutti i circuiti dall'API Ergast e salvarli nel database
        group.MapPost("/import-circuits", async (F1DbContext db) =>
        {
            var client = new HttpClient();

            // Chiamata API Ergast per ottenere i circuiti
            var response = await client.GetStringAsync($"https://ergast.com/api/f1/circuits.json?limit=100");
            var data = JsonSerializer.Deserialize<ErgastResponse>(response);
            var circuits = data?.MRData?.CircuitTable?.Circuits;

            // Controllo duplicati ed inserimento nel database
            foreach (var circuit in circuits)
            {
                if (!await db.Circuits.AnyAsync(c => c.circuitName == circuit.circuitName && c.circuitName == circuit.circuitName))
                {
                    db.Circuits.Add(circuit);
                }
           }

            await db.SaveChangesAsync();
            return Results.Ok(new { message = "Tutti i circuiti Ergast sono stati importati!" });
        });

        group.MapPost("/circuits", async (F1DbContext db, [FromBody] CircuitDTO circuitDTO) =>
        {
            Location location = new()
            {
                country = circuitDTO.Location.country,
                locality = circuitDTO.Location.locality
            };

            Circuit circuit = new()
            {
                circuitId = circuitDTO.circuitId,
                circuitName = circuitDTO.circuitName,
                Location = location
            };

            db.Circuits.Add(circuit);
            await db.SaveChangesAsync();

            return Results.Created($"/circuits/{circuit.circuitId}", new CircuitDTO(circuit));
        });

        group.MapGet("/circuits/{id}", async (F1DbContext db, string id) =>
        {
            Circuit? circuit = await db.Circuits.FindAsync(id);
            if (circuit is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(new CircuitDTO(circuit));
        });

        group.MapPut("/circuits/{id}", async (F1DbContext db, string id, CircuitDTO CircuitDTO) =>
		{

			Circuit? circuit = await db.Circuits.FindAsync(id);
			if (circuit is null)
			{
				return Results.NotFound();
			}

			circuit.circuitName = CircuitDTO.circuitName;
			circuit.Location.country = CircuitDTO.Location.country;
			circuit.Location.locality = CircuitDTO.Location.locality;

			await db.SaveChangesAsync();
			return Results.NoContent();
		});

        group.MapDelete("/circuits/{id}", async (F1DbContext db, string id) =>
        {

            Circuit? circuit = await db.Circuits.FindAsync(id);
            if (circuit is null)
            {
                return Results.NotFound();
            }

            db.Circuits.Remove(circuit);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}