/* add global usage directives for the System namespaces here */

global using System.Reflection;
global using System.Net;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Linq.Expressions;
global using System.Security.Claims;

/* add global usage directives for the Microsoft namespaces here */

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;

/* add global usage directives for application namespaces here */

global using OpenMovies.WebApi.Data;
global using OpenMovies.WebApi.Data.Repositories;
global using OpenMovies.WebApi.Extensions;
global using OpenMovies.WebApi.Entities;
global using OpenMovies.WebApi.Identity;
global using OpenMovies.WebApi.Handlers;
global using OpenMovies.WebApi.Policies;
global using OpenMovies.WebApi.Exceptions;
global using OpenMovies.WebApi.Services;
global using OpenMovies.WebApi.Services.Exceptions;
global using OpenMovies.WebApi.Payloads;
global using OpenMovies.WebApi.Validators;
global using OpenMovies.WebApi.Helpers;
global using OpenMovies.TestingSuite.Helpers;
global using OpenMovies.WebApi.Controllers;
global using OpenMovies.WebApi.Middlewares;

/* add global usage directives for third-party namespaces here */

global using Moq;
global using AutoFixture;
global using FluentValidation;
global using FluentValidation.Results;
global using Nelibur.ObjectMapper;
global using MediatR;