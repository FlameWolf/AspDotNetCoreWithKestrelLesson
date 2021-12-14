﻿global using System;
global using System.Collections.Concurrent;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
global using System.IO;
global using System.Linq;
global using System.Linq.Expressions;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Security.Claims;
global using System.Threading.Tasks;
global using AspDotNetCoreWithKestrelLesson;
global using AspDotNetCoreWithKestrelLesson.Attributes;
global using AspDotNetCoreWithKestrelLesson.Controllers;
global using AspDotNetCoreWithKestrelLesson.Conventions;
global using AspDotNetCoreWithKestrelLesson.Database;
global using AspDotNetCoreWithKestrelLesson.Extensions;
global using AspDotNetCoreWithKestrelLesson.Filters;
global using AspDotNetCoreWithKestrelLesson.Middleware;
global using AspDotNetCoreWithKestrelLesson.Models;
global using AspDotNetCoreWithKestrelLesson.Providers;
global using AspDotNetCoreWithKestrelLesson.Repositories;
global using Autofac.Extensions.DependencyInjection;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.JsonPatch;
global using Microsoft.AspNetCore.JsonPatch.Operations;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ApplicationModels;
global using Microsoft.AspNetCore.Mvc.ApplicationParts;
global using Microsoft.AspNetCore.Mvc.Controllers;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Mvc.Formatters;
global using Microsoft.AspNetCore.Mvc.Infrastructure;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.IO;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Linq;
global using Newtonsoft.Json.Serialization;
global using Swashbuckle.AspNetCore.Filters;