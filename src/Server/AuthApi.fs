module AuthApi

open System
open Giraffe
open Saturn
open Fable.Remoting.Giraffe
open Fable.Remoting.Server
open Common.AuthStore
open System.Security.Claims
open System.IdentityModel.Tokens.Jwt
open Microsoft.IdentityModel.Tokens
open Microsoft.IdentityModel.JsonWebTokens
open System.Text

let secret = "some very strong secret"
let issuer = "FSharping"
let securityKey = SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
let validationParameters = TokenValidationParameters()
validationParameters.ValidAudience <- issuer
validationParameters.ValidIssuer <- issuer
validationParameters.IssuerSigningKey <- securityKey

let generateToken username =
    let claims = [|
        Claim(JwtRegisteredClaimNames.Sub, username);
        Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) |]
    claims
    |> Auth.generateJWT (secret, SecurityAlgorithms.HmacSha256) issuer (DateTime.UtcNow.AddHours(1.0))


type Movie = {
    Id : Guid
    Title : string
    Username : string
}

let favoriteMovies = [
    { Id = Guid.Parse("2150E333-8FDC-42A3-9474-1A3956D46DE1"); Title = "The Matrix"; Username = "mikhail" }
    { Id = Guid.Parse("2150E333-8FDC-42A3-9474-1A3956D46DE2"); Title = "Terminator 2"; Username = "mikhail" }
    { Id = Guid.Parse("2150E333-8FDC-42A3-9474-1A3956D46DE3"); Title = "Titanic"; Username = "anotheruser" }
    { Id = Guid.Parse("2150E333-8FDC-42A3-9474-1A3956D46DE4"); Title = "Gladiator"; Username = "yetanotheruser" }
]

let login (loginInfo : LoginInfo) = 
    async {
        match loginInfo.Username, loginInfo.Password with
        | "mikhail", "password" ->
            return generateToken loginInfo.Username |> AuthToken |> Ok
        | _ ->
            return Error "Wrong credentials"
    }

let authenticateUser (AuthToken token) =
    try
        let tokenHandler = JwtSecurityTokenHandler()
        let (principal, _) = tokenHandler.ValidateToken(token, validationParameters)
        Ok principal
    with
    | e ->
        Error e.Message

let extractUsername (principal : ClaimsPrincipal) =
    match principal.FindFirst ClaimTypes.NameIdentifier with
    | null ->
        Error "No username found in token"
    | claim ->
        Ok claim.Value

let selectFavoriteMovies username =
    favoriteMovies
    |> List.filter (fun m -> m.Username = username)
    |> Ok

let toFavoriteMovies movies =
    movies
    |> List.map (fun m -> { Id = m.Id; Title = m.Title })
    |> Ok

let getFavoriteMovies token =
    async {
        let result =
            token
            |> authenticateUser
            |> Result.bind extractUsername
            |> Result.bind selectFavoriteMovies
            |> Result.bind toFavoriteMovies
        return result
    }

let authMovieStore = {
    login = login
    favoriteMovies = getFavoriteMovies
}

let api : HttpHandler =
    Remoting.createApi()
    |> Remoting.withRouteBuilder (sprintf "/auth/%s/%s")
    |> Remoting.fromValue authMovieStore
    |> Remoting.buildHttpHandler