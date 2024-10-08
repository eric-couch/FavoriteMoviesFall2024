﻿using Blazored.LocalStorage;
using FavoriteMoviesFall2024.Client.Pages;
using FavoriteMoviesFall2024.Shared;
using FavoriteMoviesFall2024.Shared.Wrapper;
using Microsoft.AspNetCore.Components.Authorization;
using Syncfusion.Blazor.PivotView;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace FavoriteMoviesFall2024.Client.HttpRepo;

public class UserMoviesHttpRepository : IUserMoviesHttpRepository
{
    public readonly HttpClient _httpClient;
    public readonly ILocalStorageService _localStorageService;
    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?";
    private readonly string OMDBAPIKey = "apikey=86c39163";

    public UserMoviesHttpRepository(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _localStorageService = localStorage;
        _httpClient = httpClient;
    }
    
    public async Task<DataResponse<List<OMDBMovie>>> GetMovies()
    {
        var MovieDetails = new List<OMDBMovie>();
        DataResponse<UserDto> res = await _httpClient.GetFromJsonAsync<DataResponse<UserDto>>($"api/User");  // this is our server (FavoriteMoviesFall2024.Server Project)

        if (res.Succeeded)
        {
            UserDto User = res.Data;
            if (User?.OMDBMovies?.Any() ?? false)
            {
                return new DataResponse<List<OMDBMovie>>(User.OMDBMovies);

                //foreach (var movie in User.FavoriteMovies)
                //{
                //    // check if movie is in local storage.  If so, get it from local storage, otherwise call the api
                //    OMDBMovie omdbMovie = new OMDBMovie();
                //    omdbMovie = await _localStorageService.GetItemAsync<OMDBMovie>(movie.imdbId);



                //    if (omdbMovie is null)
                //    {
                //        // OMDB API is a microservice
                //        omdbMovie = await _httpClient.GetFromJsonAsync<OMDBMovie>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbId}");
                //        if (omdbMovie is not null)
                //        {
                //            MovieDetails.Add(omdbMovie);
                //            await _localStorageService.SetItemAsync<OMDBMovie>(movie.imdbId, omdbMovie);
                //        }

                //    }
                //    else
                //    {
                //        MovieDetails.Add(omdbMovie);
                //    }
                //}
            }
            else
            {
                return new DataResponse<List<OMDBMovie>>()
                {
                    Succeeded = false,
                    Message = "Movies not found",
                    Data = new List<OMDBMovie>()
                };
            }
        } else
        {
            return new DataResponse<List<OMDBMovie>>()
            {
                Succeeded = false,
                Message = "Movies not found",
                Data = new List<OMDBMovie>()
            };
        }

        
        
    }

    public async Task<DataResponse<List<UserEditDto>>> GetUsers()
    {
        try
        {
            var res = await _httpClient.GetFromJsonAsync<DataResponse<List<UserEditDto>>>("api/users");
            if (res?.Succeeded ?? false)
            {
                return new DataResponse<List<UserEditDto>>(res.Data);
            } else
            {
                return new DataResponse<List<UserEditDto>>()
                {
                    Succeeded = false,
                    Message = "Users list not found",
                    Data = new List<UserEditDto>()
                };
            }
        } catch (Exception ex)
        {
            return new DataResponse<List<UserEditDto>>()
            {
                Succeeded = false,
                Message = ex.Message,
                Data = new List<UserEditDto>()
            };
        }
    }

    public async Task<Response> RemoveMovie(string imdbId)
    {
        try
        {
            Movie newMovie = new Movie { imdbId = imdbId };
            var res = await _httpClient.PostAsJsonAsync("api/remove-movie", newMovie);
            Response response = await res.Content.ReadFromJsonAsync<Response>();
            return response;
        } catch (Exception ex)
        {
            return new Response("Remove movie failed.");
        }
        

    }

    public async Task<Response> ToggleEnabledUser(string userId)
    {
        try
        {
            var res = await _httpClient.GetFromJsonAsync<Response>($"api/toggle-enabled-user/{userId}");
            if (!res.Succeeded)
            {
                return new Response("Enable User Failed");
            } else
            {
                return new Response() { Succeeded = true, Message="Enabled User." };
            }

        }
        catch (HttpRequestException ex)
        {
            return new Response("Http exception occured.");
        }
        catch(NotSupportedException ex)
        {
            return new Response("Not Supported exception occured.");
        }
        catch(JsonException ex)
        {
            return new Response("Json exception occured.");
        }
        catch (Exception ex)
        {
            return new Response("Enable User Failed");
        }
    }

    public async Task<Response> ToggleAdminRole(string userId)
    {
        try
        {
            var res = await _httpClient.GetFromJsonAsync<Response>($"api/toggle-admin-role/{userId}");
            if (!res.Succeeded)
            {
                return new Response("Toggle Admin Role Failed");
            }
            else
            {
                return new Response() { Succeeded = true, Message = "User added to admin." };
            }

        }
        catch (HttpRequestException ex)
        {
            return new Response("Http exception occured.");
        }
        catch (NotSupportedException ex)
        {
            return new Response("Not Supported exception occured.");
        }
        catch (JsonException ex)
        {
            return new Response("Json exception occured.");
        }
        catch (Exception ex)
        {
            return new Response("Enable User Failed");
        }
    }
}
