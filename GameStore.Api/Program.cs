using GameStore.Api.Dtos;

const string GetGameEndPointName = "GetGame";
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = new List<GameDto>
{
    new GameDto(1, "Red Dead Redemption 2", "Action-adventure", 59.99m, new DateOnly(2018, 10, 26)),
    new GameDto(2, "The Witcher 3: Wild Hunt", "Action RPG", 39.99m, new DateOnly(2015, 5, 19)),
    new GameDto(3, "Hades", "Roguelike", 24.99m, new DateOnly(2020, 9, 17)),
    new GameDto(4, "Celeste", "Platformer", 19.99m, new DateOnly(2018, 1, 25)),
    new GameDto(5, "Stardew Valley", "Simulation", 14.99m, new DateOnly(2016, 2, 26)),
    new GameDto(6, "Elden Ring", "Action RPG", 59.99m, new DateOnly(2022, 2, 25)),
    new GameDto(7, "Hollow Knight", "Metroidvania", 14.99m, new DateOnly(2017, 2, 24)),
    new GameDto(8, "Doom Eternal", "First-person shooter", 39.99m, new DateOnly(2020, 3, 20)),
    new GameDto(9, "Among Us", "Party", 4.99m, new DateOnly(2018, 6, 15)),
    new GameDto(10, "Portal 2", "Puzzle-platformer", 9.99m, new DateOnly(2011, 4, 19))
};

// GET /games
app.MapGet("/games", () => games);

// GET /games/1
app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id) is GameDto game ? Results.Ok(game) : Results.NotFound())
    .WithName(GetGameEndPointName);

// POST /games
app.MapPost("/games", (CreateGameDto newGame) =>
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
app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var index = games.FindIndex(game => game.Id == id);

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
app.MapDelete("/games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);
    return Results.NoContent();
});


app.Run();
