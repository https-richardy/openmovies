global using System.Text;
global using System.Text.Json.Serialization;
global using System.Linq.Expressions;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity;

global using OpenMovies.WebApi.Models;
global using OpenMovies.WebApi.Models.InputModels;
global using OpenMovies.WebApi.Data;
global using OpenMovies.WebApi.Data.Repositories;
global using OpenMovies.WebApi.Extensions;
global using OpenMovies.WebApi.Services;
global using OpenMovies.WebApi.Services.Exceptions;
global using OpenMovies.WebApi.Utils;
global using OpenMovies.WebApi.Validators;

global using Nelibur.ObjectMapper;
global using FluentValidation;