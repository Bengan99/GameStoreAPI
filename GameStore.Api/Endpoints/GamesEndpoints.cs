using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndPointName = "GetGame";
    private static readonly List<GameDto> games = [
    new (1, "Red Dead Redemption 2", "Action-adventure", 59.99m, new DateOnly(2018, 10, 26)),
    new (2, "The Witcher 3: Wild Hunt", "Action RPG", 39.99m, new DateOnly(2015, 5, 19)),
    new (3, "Hades", "Roguelike", 24.99m, new DateOnly(2020, 9, 17)),
    new (4, "Celeste", "Platformer", 19.99m, new DateOnly(2018, 1, 25)),
    new (5, "Stardew Valley", "Simulation", 14.99m, new DateOnly(2016, 2, 26)),
    new (6, "Elden Ring", "Action RPG", 59.99m, new DateOnly(2022, 2, 25)),
    new (7, "Hollow Knight", "Metroidvania", 14.99m, new DateOnly(2017, 2, 24)),
    new (8, "Doom Eternal", "First-person shooter", 39.99m, new DateOnly(2020, 3, 20)),
    new (9, "Among Us", "Party", 4.99m, new DateOnly(2018, 6, 15)),
    new (10, "Portal 2", "Puzzle-platformer", 9.99m, new DateOnly(2011, 4, 19))
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/1
        group.MapGet("/{id}", (int id) =>
        {
            var result = games.Find(game => game.Id == id);

            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName(GetGameEndPointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );
            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndPointName, new { id = game.Id }, game);
        });

        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1) return Results.NotFound();

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });
    }
}
