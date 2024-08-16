using RichardSzalay.MockHttp;
using FavoriteMoviesFall2024.Client.HttpRepo;

namespace FavoriteMoviesFall2024.Client.Test;

public class HttpUserMoviesRepositoryTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test_GetMovies_ReturnOMDBMovieList()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        string testUserResponse = """
            {
            "favoriteMovies": [
                {
                    "id": 1,
                    "imdbId": "tt0816692"
                },
                {
                    "id": 2,
                    "imdbId": "tt0109686"
                },
                {
                    "id": 3,
                    "imdbId": "tt0468569"
                }
            ],
            "id": "0d77e2f5-8af5-47d3-9975-5e394068a900",
            "userName": "eric.couch@example.net",
            "normalizedUserName": "ERIC.COUCH@EXAMPLE.NET",
            "email": "eric.couch@example.net",
            "normalizedEmail": "ERIC.COUCH@EXAMPLE.NET",
            "emailConfirmed": true,
            "passwordHash": "AQAAAAIAAYagAAAAEGUuGUSoneuP1w7AZhrqi8kcu+eTTK4HqGgnzdbSiR5+Zn0bv1IpPK8xpFQO/6BEmw==",
            "securityStamp": "MHMQKBOG6UP5X7WCAY2KBXNOJXQTYCZ7",
            "concurrencyStamp": "7fb37c60-1a5e-4f5e-bdfc-50a14087671a",
            "phoneNumber": null,
            "phoneNumberConfirmed": false,
            "twoFactorEnabled": false,
            "lockoutEnd": null,
            "lockoutEnabled": true,
            "accessFailedCount": 0
        }
        """;
        string testOMDBApiInterstellarResponse = """
            {
                "Title": "Interstellar",
                "Year": "2014",
                "Rated": "PG-13",
                "Released": "07 Nov 2014",
                "Runtime": "169 min",
                "Genre": "Adventure, Drama, Sci-Fi",
                "Director": "Christopher Nolan",
                "Writer": "Jonathan Nolan, Christopher Nolan",
                "Actors": "Matthew McConaughey, Anne Hathaway, Jessica Chastain",
                "Plot": "When Earth becomes uninhabitable in the future, a farmer and ex-NASA pilot, Joseph Cooper, is tasked to pilot a spacecraft, along with a team of researchers, to find a new planet for humans.",
                "Language": "English",
                "Country": "United States, United Kingdom, Canada",
                "Awards": "Won 1 Oscar. 44 wins & 148 nominations total",
                "Poster": "https://m.media-amazon.com/images/M/MV5BZjdkOTU3MDktN2IxOS00OGEyLWFmMjktY2FiMmZkNWIyODZiXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_SX300.jpg",
                "Ratings": [
                    {
                        "Source": "Internet Movie Database",
                        "Value": "8.7/10"
                    },
                    {
                        "Source": "Rotten Tomatoes",
                        "Value": "73%"
                    },
                    {
                        "Source": "Metacritic",
                        "Value": "74/100"
                    }
                ],
                "Metascore": "74",
                "imdbRating": "8.7",
                "imdbVotes": "2,139,855",
                "imdbID": "tt0816692",
                "Type": "movie",
                "DVD": "N/A",
                "BoxOffice": "$188,020,017",
                "Production": "N/A",
                "Website": "N/A",
                "Response": "True"
            }
            """;
        string testOMDBApiDumbAndDumberResponse = """
                        {
                "Title": "Dumb and Dumber",
                "Year": "1994",
                "Rated": "PG-13",
                "Released": "16 Dec 1994",
                "Runtime": "107 min",
                "Genre": "Comedy",
                "Director": "Peter Farrelly, Bobby Farrelly",
                "Writer": "Peter Farrelly, Bennett Yellin, Bobby Farrelly",
                "Actors": "Jim Carrey, Jeff Daniels, Lauren Holly",
                "Plot": "After a woman leaves a briefcase at the airport terminal, a dumb limo driver and his dumber friend set out on a hilarious cross-country road trip to Aspen to return it.",
                "Language": "English, Swedish, German",
                "Country": "United States",
                "Awards": "5 wins & 3 nominations",
                "Poster": "https://m.media-amazon.com/images/M/MV5BZDQwMjNiMTQtY2UwYy00NjhiLTk0ZWEtZWM5ZWMzNGFjNTVkXkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_SX300.jpg",
                "Ratings": [
                    {
                        "Source": "Internet Movie Database",
                        "Value": "7.3/10"
                    },
                    {
                        "Source": "Rotten Tomatoes",
                        "Value": "68%"
                    },
                    {
                        "Source": "Metacritic",
                        "Value": "41/100"
                    }
                ],
                "Metascore": "41",
                "imdbRating": "7.3",
                "imdbVotes": "417,018",
                "imdbID": "tt0109686",
                "Type": "movie",
                "DVD": "N/A",
                "BoxOffice": "$127,190,327",
                "Production": "N/A",
                "Website": "N/A",
                "Response": "True"
            }
            """;
        string testOMDBApiTheDarkKnightResponse = """
            {
                "Title": "The Dark Knight",
                "Year": "2008",
                "Rated": "PG-13",
                "Released": "18 Jul 2008",
                "Runtime": "152 min",
                "Genre": "Action, Crime, Drama",
                "Director": "Christopher Nolan",
                "Writer": "Jonathan Nolan, Christopher Nolan, David S. Goyer",
                "Actors": "Christian Bale, Heath Ledger, Aaron Eckhart",
                "Plot": "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman, James Gordon and Harvey Dent must work together to put an end to the madness.",
                "Language": "English, Mandarin",
                "Country": "United States, United Kingdom",
                "Awards": "Won 2 Oscars. 164 wins & 164 nominations total",
                "Poster": "https://m.media-amazon.com/images/M/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_SX300.jpg",
                "Ratings": [
                    {
                        "Source": "Internet Movie Database",
                        "Value": "9.0/10"
                    },
                    {
                        "Source": "Rotten Tomatoes",
                        "Value": "94%"
                    },
                    {
                        "Source": "Metacritic",
                        "Value": "84/100"
                    }
                ],
                "Metascore": "84",
                "imdbRating": "9.0",
                "imdbVotes": "2,907,659",
                "imdbID": "tt0468569",
                "Type": "movie",
                "DVD": "N/A",
                "BoxOffice": "$534,987,076",
                "Production": "N/A",
                "Website": "N/A",
                "Response": "True"
            }
            """;

        mockHttp.When("https://localhost:7088/api/User")
            .Respond("application/json", testUserResponse);
        mockHttp.When("https://www.omdbapi.com/?apikey=86c39163&i=tt0816692")
            .Respond("application/json", testOMDBApiInterstellarResponse);
        mockHttp.When("https://www.omdbapi.com/?apikey=86c39163&i=tt0109686")
            .Respond("application/json", testOMDBApiDumbAndDumberResponse);
        mockHttp.When("https://www.omdbapi.com/?apikey=86c39163&i=tt0468569")
            .Respond("application/json", testOMDBApiTheDarkKnightResponse);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("https://localhost:7088/");
        var userMoviesHttpRepo = new UserMoviesHttpRepository(client);


        // Act
        var res = await userMoviesHttpRepo.GetMovies();

        var movies = res.Data;
        
        // Assert 
        Assert.That(movies.Count(), Is.EqualTo(3));
        Assert.That(movies[0].Title, Is.EqualTo("Interstellar"));
        Assert.That(movies[0].Year, Is.EqualTo("2014"));
        Assert.That(movies[1].Title, Is.EqualTo("Dumb and Dumber"));
    }
}