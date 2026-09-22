# .NET Backend Revision Guide — From YOUR PlaygroundArenaApp
### A Senior-Mentor Walkthrough: What you built, Why it works, and What the industry adds on top

> **How to use this:** Read top-to-bottom once. Every concept has: (1) Easy definition, (2) Where YOU used it (file reference), (3) Interview one-liner, (4) What's next in industry.

---

# PART 1 — WHAT IS IMPLEMENTED IN YOUR PROJECT

## 1. Clean Architecture / Layered Architecture (N-Tier)

**Easy concept:** Separate your code into layers by responsibility. Each layer only talks to the one below it. Like a restaurant: Customers → Waiters → Kitchen → Pantry. Customers never walk into the pantry.

**Your 4 layers (single project, folder-separated):**

| Layer | Folder | Responsibility | Analogy |
|---|---|---|---|
| **Presentation** | `Presentation/Controllers/` | HTTP in/out. No logic. | Waiter |
| **Application** | `Application/Services/` | Business logic / use-cases | Kitchen |
| **Core (Domain)** | `Core/Models/`, `Core/DTO/` | Entities + DTOs (pure, no dependencies) | Recipes & Ingredients |
| **Infrastructure** | `Infrastructure/Data/`, `Repository/` | DB, EF Core, external concerns | Pantry / Suppliers |

**Where you see it:** `Program.cs` wires all of these together.

**Interview one-liner:** *"Clean Architecture enforces the Dependency Rule — inner layers (Domain) know nothing about outer layers (DB, API). This makes the core testable and swappable."*

**Industry note:** Big companies split these into **separate projects (.csproj)** — `App.Domain`, `App.Application`, `App.Infrastructure`, `App.API` — to *physically* prevent a controller from touching EF Core. You did it in one project with folders, which is fine for small apps.

---

## 2. Dependency Injection (DI) & IoC Container

**Easy concept:** Don't `new` up your dependencies. Ask for them in the constructor, and the framework hands them to you. "Don't call us, we'll call you."

**Where you used it — everywhere in `Program.cs`:**
```csharp
builder.Services.AddScoped<IArenaRepository, ArenaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
```
And receiving it via constructor in `AdminArenaService`:
```csharp
public AdminArenaService(IUnitOfWork unit, PlaygroundArenaDbContext context, ILogger<...> logger, IMapper mapper)
```

**The 3 Lifetimes (MEMORIZE — asked in 99% of interviews):**

| Lifetime | Meaning | Your example |
|---|---|---|
| **Transient** | New instance every single time | Lightweight stateless helpers |
| **Scoped** | One instance **per HTTP request** | Your repos, UoW, DbContext |
| **Singleton** | One instance for app lifetime | Caching, config, `Log.Logger` |

**Why Scoped for DbContext?** A DbContext is a "unit of work" — you want one per request so all operations in a request share one transaction/change-tracker, then it's disposed.

**Interview one-liner:** *"DI gives loose coupling and testability. Scoped is the default for EF Core contexts; Singleton must be thread-safe; Transient for cheap stateless services."*

---

## 3. Repository Pattern + Unit of Work (UoW)

**Easy concept:**
- **Repository** = a class that wraps ALL database queries for one entity. Controllers/services never write SQL/LINQ-to-DB directly — they ask the repository. Like a librarian: you ask for a book, you don't search the shelves yourself.
- **Unit of Work** = one object that coordinates ALL repositories and commits them in a SINGLE `SaveChanges()` — so everything succeeds or fails together (a transaction).

**Your files:**
- Interfaces: `Infrastructure/Repository/ArenaRepository/IArenaRepository.cs`, `IBookingRepository`, etc.
- Implementations: `ArenaRepository.cs` (uses `_context.Arenas.Include(...)`)
- UoW: `IUnitOfWork.cs` exposes `Arena`, `Court`, `Book`, `Slot` + `SaveAsync()`; `UnitOfWork.cs` implements it.

**Why interface + implementation (`IArenaRepository`/`ArenaRepository`)?** So in unit tests you can swap the real DB repo with a fake in-memory one (mocking).

**Honest senior note (good interview discussion):** EF Core's `DbContext` **already IS a Unit of Work**, and `DbSet<T>` **already IS a repository**. Wrapping them adds abstraction that's debated. Many teams now use **CQRS + MediatR** and skip the extra repository layer. Being able to SAY this in an interview = senior-level signal.

---

## 4. Entity Framework Core (ORM) + Code-First Migrations

**Easy concept:** EF Core = an ORM (Object-Relational Mapper). You write C# classes; it writes SQL. **Code-First** = your C# models are the source of truth; the DB schema is *generated* from them.

**Where you used it:**
- `PlaygroundArenaDbContext.cs` — `DbSet<Arena> Arenas` etc. = your tables.
- `OnModelCreating` — Fluent API to configure relationships. You set `DeleteBehavior.Restrict` for Payment→User and `Cascade` for Payment→Booking.
- `Migrations/` folder — `20250912_InitialCreation.cs`, `UpdateTimeSlotStructure`, `ChangeStartEndTimeToTimeSpan`... each is a versioned schema change.

**Migration commands you ran (revise these):**
```powershell
dotnet ef migrations add InitialCreation      # create migration
dotnet ef database update                     # apply to DB
dotnet ef migrations remove                   # undo last (unapplied)
```

**Key EF concepts to revise:**
- **Navigation properties:** `Arena.Courts`, `Booking.TimeSlots`, `Booking.Payment` — let you traverse relationships.
- **Eager loading:** `.Include(a => a.Courts).ThenInclude(t => t.TimeSlots)` in `ArenaRepository` — loads related data in ONE query (avoids N+1).
- **The N+1 problem** (INTERVIEW FAVORITE): Loading 100 arenas, then lazily loading each arena's courts = 101 queries. Fix: `.Include()`.
- **Change tracking:** EF watches entities you load; `SaveChangesAsync()` generates UPDATE/INSERT for what changed.

---

## 5. Multiple DbContexts (Two Databases)

**You registered TWO contexts in `Program.cs`:**
```csharp
builder.Services.AddDbContext<PlaygroundArenaDbContext>(... "DefaultConnection");
builder.Services.AddDbContext<CompanyDbContext>(... "CompanyConnection");
```
Each has its **own migrations folder** (`Migrations/` and `Data/Migrations/Company/`). Run migrations per-context with `--context CompanyDbContext`. This is how you'd talk to two separate SQL databases from one app.

---

## 6. ASP.NET Core Web API — Controllers, Routing, Model Binding

**Easy concept:** The framework maps an HTTP request → a C# method (an "action"), converts JSON body → your DTO, runs it, and converts the return value → an HTTP response.
**Your attributes (revise each meaning):**

| Attribute | Meaning | Example |
|---|---|---|
| `[ApiController]` | Enables auto model-validation (400 on bad input), binding inference | both controllers |
| `[Route("api/[controller]")]` | `[controller]` = class name minus "Controller" | `/api/AdminControl` |
| `[HttpPost("arenas")]` | Verb + sub-route | `POST /api/AdminControl/arenas` |
| `[FromBody]` | Read DTO from JSON body | `CreateCourtTimeSlotsAPI([FromBody] dto)` |
| `[FromQuery]` | Read from `?date=...` | `GetBookingByCourtIdAPI(int id, [FromQuery] DateTime date)` |
| Route param `{id}` | From URL path | `[HttpGet("courts/{id}")]` |

**Return types you used:** `IActionResult` (`Ok()`, `NoContent()`), and returning DTO directly (`Task<CourtSlotsDTO>`) which auto-200s.

**`async Task` (revise):** Every action is `async`/`await` so the thread returns to the pool while awaiting DB I/O → your API handles more concurrent requests. You did this correctly everywhere.

---

## 7. DTOs (Data Transfer Objects) + AutoMapper

**Easy concept:** NEVER send your DB entity straight to the client. A DTO is a "view" shaped exactly for that API response/request — hides internal fields (passwords, IDs, nav props), flattens nested data, prevents over-posting.

**Your DTOs:** `Core/DTO/` — `AddUserDTO`, `GetArenaDTO`, `CourtDetailsDTO`, `BookingResponseDTO`...

**AutoMapper** (`Application/Mapping/AutoMapping.cs`): auto-copies matching property names between Entity ↔ DTO:
```csharp
CreateMap<Arena, GetArenaDTO>();
// usage: _mapper.Map<List<GetArenaDTO>>(arenas);
```
Registered in `Program.cs`: `builder.Services.AddAutoMapper(typeof(AutoMapping));`

**Validation (Data Annotations):** `[Required]` on `AddUserDTO`. Combined with `[ApiController]`, bad input → automatic 400. (`ModelState.IsValid` in your controllers.)

**Interview note:** AutoMapper is convenient but in performance-critical code, manual mapping / extension methods are preferred (no reflection overhead, compile-time safety). Mention **Mapster** or source-generated mappers as modern alternatives.

---

## 8. Middleware & The Request Pipeline

**Easy concept:** Middleware = an assembly line each HTTP request passes through, in ORDER. Each station can inspect/modify the request, call the next station, then inspect the response on the way back.

**Your pipeline (order MATTERS — classic interview Q):**
```csharp
app.UseSwagger();
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseMiddleware<SerilogLoggingMiddleware>();  // custom
app.UseExceptionHandler();                      // global errors
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
```

**Your custom middleware** `SerilogLoggingMiddleware.cs` — logs "Incoming Request" → calls `await _next(context)` → logs "Outgoing Response", with try/catch that re-throws so the global handler catches it. This pattern (`_next`, `InvokeAsync`) is THE middleware template — memorize it.

**Fix to know:** `UseExceptionHandler()` should come EARLY (before your logging middleware) to catch everything downstream.

---

## 9. Global Exception Handling (.NET 8's IExceptionHandler)

**Easy concept:** One central place catches ALL unhandled exceptions → converts them to proper HTTP status codes + clean JSON, instead of leaking stack traces.

**Your file:** `GlobalExceptionHandlerMiddleware.cs` implements **`IExceptionHandler`** (the modern .NET 8 way) with `TryHandleAsync`. You map:
- `KeyNotFoundException` → 404
- `BadHttpRequestException` / `ArgumentNullException` → 400
- `UnauthorizedAccessException` → 401
- `SqlException` / default → 500

Registered: `builder.Services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>(); builder.Services.AddProblemDetails();`

**Interview one-liner:** *"Centralized exception handling avoids try/catch in every controller. .NET 8's IExceptionHandler + ProblemDetails gives RFC 7807-standard error responses."*

---

## 10. Logging — Serilog (Structured Logging)

**Easy concept:** Structured logging = log *data fields* (`{UserId}`), not just strings — so logs are searchable/queryable in tools like Seq/Elasticsearch.

**Your setup in `Program.cs`:**
```csharp
Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();
builder.Host.UseSerilog();  // replace built-in logger
```
**Sinks** = destinations (Console, File). **Rolling interval** = new file daily. You inject `ILogger<T>` and log like:
```csharp
_logger.LogInformation("Arena {ID} added at {Time}", arena.ArenaId, DateTime.UtcNow);
```
Note the `{ID}` named placeholders = structured. **Log levels (order):** Verbose < Debug < Information < Warning < Error < Fatal.

---

## 11. Action Filters

**Easy concept:** Code that runs before/after a controller action — for cross-cutting concerns (logging, caching, validation). Different from middleware: filters run INSIDE MVC with access to action context.

**Your file:** `LoggingActionFilter.cs` — `IAsyncActionFilter`, uses `Stopwatch` to time each action. Registered globally: `builder.Services.AddControllers(o => o.Filters.Add<LoggingActionFilter>());`

**Filter pipeline order:** Authorization → Resource → **Action** → Exception → Result.

---

## 12. CORS (Cross-Origin Resource Sharing)

**Easy concept:** Browsers block a frontend on domain A from calling an API on domain B. CORS = your API explicitly whitelisting which origins may call it.

**Yours:**
```csharp
builder.Services.AddCors(o => o.AddPolicy("AllowFrontend",
	p => p.WithOrigins("https://book-n-playyyapp.vercel.app").AllowAnyHeader().AllowAnyMethod()));
...
app.UseCors("AllowFrontend");
```
**Interview:** `AllowAnyOrigin()` + credentials is a security no-no. Always whitelist specific origins in production.

---

## 13. Swagger / OpenAPI

**Easy concept:** Auto-generates interactive API documentation + a test UI from your controllers. `AddEndpointsApiExplorer()` + `AddSwaggerGen()`, then `UseSwagger()`/`UseSwaggerUI()`. Open at `/swagger`.

**Industry note:** You should gate it: `if (app.Environment.IsDevelopment()) { UseSwagger... }` so it's not exposed in production.

---

## 14. Configuration & appsettings.json

`appsettings.json` holds `ConnectionStrings`, logging, etc. Accessed via `builder.Configuration.GetConnectionString("DefaultConnection")`. Also `appsettings.Development.json` overrides per environment.

**Industry additions:** **Options pattern** (`IOptions<T>`), **User Secrets** (dev), **Azure Key Vault / env vars** (prod — never commit real connection strings to Git!).

---

# PART 2 — WHAT'S NOT HERE, BUT INDUSTRY EXPECTS YOU TO KNOW

These are the gaps. For each: what it is, why it matters, and where it'd plug into YOUR app.

## 15. Authentication & Authorization (JWT) — **BIGGEST GAP**

Your app has `UseAuthorization()` but NO auth implemented. **The #1 thing to learn next.**

- **Authentication** = WHO are you? (login)
- **Authorization** = WHAT can you do? (roles/permissions)
- **JWT (JSON Web Token):** Stateless token = `Header.Payload.Signature`. Server signs it at login; client sends `Authorization: Bearer <token>` on every request. Server validates the signature — no DB lookup needed.
- **Flow:** Login → issue JWT (with claims: userId, role) → `[Authorize]` on controllers → `[Authorize(Roles="Admin")]` on admin endpoints.
- **Concepts:** Claims, Refresh tokens, Token expiry.
- **Industry also uses:** ASP.NET Core **Identity** (full user management), **OAuth2/OpenID Connect** (login with Google), **Azure AD / Entra ID, Auth0, Keycloak**.

## 16. CQRS + MediatR

- **CQRS** = Command Query Responsibility Segregation. Separate "writes" (Commands) from "reads" (Queries) into different handlers.
- **MediatR** = in-process messaging; controller sends a `CreateBookingCommand` → MediatR routes to its handler. Removes fat controllers/services.
- This is the MODERN replacement for the big Service class + Repository debate. Learn it — very common in interviews.

## 17. FluentValidation

Replaces `[Required]` attributes with expressive, testable rules:
```csharp
RuleFor(x => x.Email).NotEmpty().EmailAddress();
```
Far more powerful than Data Annotations for complex rules. Industry standard.

## 18. Response Caching, Redis & Distributed Caching

- `IMemoryCache` (in-process) / `IDistributedCache` (**Redis**) to cache expensive queries (e.g., "available slots").
- Output caching, `ResponseCache` attribute.
- Huge performance topic. Know when to cache, cache invalidation, TTL.

## 19. Pagination, Filtering, Sorting

Your `GetArenasAPI()` returns ALL arenas — fine for 10 rows, fatal for 10,000. Learn `Skip()/Take()`, a `PagedResult<T>` wrapper, and query params (`?page=2&pageSize=20`).

## 20. API Versioning

`Asp.Versioning.Mvc` — run `/api/v1/arenas` and `/api/v2/arenas` side by side so you don't break old clients.

## 21. Health Checks

`builder.Services.AddHealthChecks()` → `/health` endpoint. Essential for Docker/Kubernetes/load balancers to know your app is alive.

## 22. Testing — **Industry non-negotiable**

- **Unit tests:** xUnit + **Moq** (mock `IUnitOfWork`) + **FluentAssertions**. Test services in isolation.
- **Integration tests:** `WebApplicationFactory<T>` spins up your whole API in-memory to test endpoints end-to-end.
- Your project has ZERO tests — the single biggest "junior vs senior" differentiator.

## 23. Docker & Containerization

Package the app + SQL Server in containers. Know: `Dockerfile`, `docker-compose`, images vs containers, multi-stage builds. Gateway to cloud/Kubernetes.

## 24. CI/CD (GitHub Actions / Azure DevOps)

Automate: on every push → build → run tests → deploy. You already have GitHub — GitHub Actions is the natural next step.

## 25. Minimal APIs (.NET 6+)

Controller-less endpoints:
```csharp
app.MapGet("/arenas", async (IUnitOfWork u) => await u.Arena.GetArenaList());
```
Lightweight, fast, great for microservices. Know both styles.

## 26. gRPC & Microservices (awareness level)

- **gRPC:** high-performance contract-first RPC using Protocol Buffers (binary, faster than JSON) — for service-to-service.
- **Microservices:** split the monolith; communicate via message brokers (**RabbitMQ, Azure Service Bus, Kafka**). Understand CAP theorem, eventual consistency, sagas — conceptually.

## 27. Resilience — Polly

Retry policies, circuit breakers, timeouts for calling external APIs/DBs. `Polly` integrates with `HttpClientFactory`. Senior-level robustness.

## 28. Rate Limiting & API Security

.NET 7+ built-in `AddRateLimiter` — prevent abuse. Also know: HTTPS enforcement, input sanitization, SQL injection (EF Core protects you), OWASP top 10.

---

# PART 3 — RAPID-FIRE INTERVIEW CHEAT SHEET

| Term | 5-word answer |
|---|---|
| DI | Constructor-provided dependencies, loose coupling |
| Scoped | One instance per HTTP request |
| Middleware | Request pipeline assembly-line stages |
| Repository | Wraps data access for one entity |
| Unit of Work | Single transaction across repositories |
| DTO | Shape data for the API boundary |
| EF Core | ORM — C# classes to SQL |
| Migration | Versioned DB schema change |
| N+1 problem | Lazy loading = many queries; use Include |
| JWT | Signed stateless token with claims |
| CQRS | Separate read and write logic |
| CORS | Whitelist allowed frontend origins |
| IExceptionHandler | Central .NET 8 error handling |
| Serilog | Structured logging with sinks |
| Filter | Code before/after MVC actions |

---

# PART 4 — YOUR 30-DAY REVISION PLAN (using THIS repo)

**Week 1 — Foundations:** DI lifetimes, middleware pipeline, async/await. Re-read your own `Program.cs` and explain every line aloud.
**Week 2 — Data:** EF Core relationships, migrations (add a new one!), Include vs lazy loading, write a raw-SQL comparison.
**Week 3 — Architecture & Quality:** Add **JWT auth**, write **3 unit tests** (xUnit+Moq), add **FluentValidation**, add **pagination** to GetArenas.
**Week 4 — Industry:** **Dockerize** the app, add a **GitHub Action**, add **health checks** + **rate limiting**, and spike a **MediatR** handler for one endpoint.

> Do these IN this repo — hands-on beats reading 10x.
