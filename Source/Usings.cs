/* add global usage directives for the System namespaces here */
global using System.Reflection;
global using System.Net;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Linq.Expressions;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;

/* add global usage directives for the Microsoft namespaces here */
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity;

/* add global usage directives for application namespaces here */
global using OpenMovies.WebApi.Models;
global using OpenMovies.WebApi.Models.InputModels;
global using OpenMovies.WebApi.Models.ViewModels;
global using OpenMovies.WebApi.Data;
global using OpenMovies.WebApi.Data.Repositories;
global using OpenMovies.WebApi.Extensions;
global using OpenMovies.WebApi.Services;
global using OpenMovies.WebApi.Services.Exceptions;
global using OpenMovies.WebApi.Operations.Commands.Handlers;
global using OpenMovies.WebApi.Operations.Queries.Handlers;
global using OpenMovies.WebApi.Identity;
global using OpenMovies.WebApi.Utils;
global using OpenMovies.WebApi.Exceptions;
global using OpenMovies.WebApi.Validators;

/* add global usage directives for third-party namespaces here */
global using Nelibur.ObjectMapper;
global using FluentValidation;
global using MediatR;