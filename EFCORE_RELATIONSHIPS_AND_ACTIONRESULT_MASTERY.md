# MASTER GUIDE: EF Core Relationships, Navigation Properties, Querying & ActionResult
### Using YOUR PlaygroundArenaApp as the live example

---

# SECTION A — EF CORE RELATIONSHIPS (THE 3 TYPES)

## The Mental Model First

A **relationship** is just "how two tables connect." EF Core needs 3 things to understand it:
1. **Primary Key (PK)** — unique ID of a row (e.g., `ArenaId`)
2. **Foreign Key (FK)** — a column that *points to* another table's PK (e.g., `Court.ArenaId`)
3. **Navigation Property** — the C# *object reference* so you can write `court.Arena.Name` instead of a JOIN

> **Golden rule:** FK is the *database* concept (an int column). Navigation property is the *C#* concept (an object). You usually have BOTH.

---

## TYPE 1: ONE-TO-MANY (most common — 80% of real schemas)

**"One Arena has MANY Courts. Each Court belongs to ONE Arena."**

### In YOUR code — `Arena.cs` + `Court.cs`:
```csharp
// PARENT (the "one" side)
public class Arena
{
	public int ArenaId { get; set; }                 // PRIMARY KEY
	public string Name { get; set; }
	public ICollection<Court> Courts { get; set; }   // NAVIGATION to many children
		= new List<Court>();
}

// CHILD (the "many" side)
public class Court
{
	public int CourtId { get; set; }   // PRIMARY KEY
	public string Name { get; set; }

	public int ArenaId { get; set; }   // FOREIGN KEY (the int)
	public Arena Arena { get; set; }   // NAVIGATION to single parent (REFERENCE nav)
}
```

### The 3 building blocks explained:
| Piece | Type | Purpose |
|---|---|---|
| `ICollection<Court> Courts` | **Collection navigation** | "Give me all my children" |
| `int ArenaId` (in child) | **Foreign Key** | The actual DB column storing the link |
| `Arena Arena` (in child) | **Reference navigation** | "Give me my one parent" |

**EF Core Convention (magic, no config needed):** EF *auto-detects* the relationship because it sees `ArenaId` matches `Arena`'s PK, and sees the navigation properties. This is called **relationship by convention**.

### Resulting SQL schema:
```
TABLE Arenas:   ArenaId (PK), Name, Location, Email
TABLE Courts:   CourtId (PK), Name, CourtType, ArenaId (FK → Arenas)
```
---

## TYPE 2: ONE-TO-ONE

**"One Booking has ONE Payment. One Payment belongs to ONE Booking."**

### In YOUR code — `Booking.cs` + `Payment.cs`:
```csharp
public class Booking
{
	public int BookingId { get; set; }       // PK
	public Payment? Payment { get; set; }     // NAV (nullable — a booking may not be paid yet)
}

public class Payment
{
	public int PaymentId { get; set; }        // PK
	public int BookingId { get; set; }        // FK  ← FK lives on the DEPENDENT side
	public Booking Booking { get; set; }      // NAV back to parent
}
```

### Why you needed Fluent API here (in `PlaygroundArenaDbContext.OnModelCreating`):
```csharp
modelBuilder.Entity<Payment>()
	.HasOne(p => p.Booking)              // Payment has ONE Booking
	.WithOne(b => b.Payment)             // Booking has ONE Payment
	.HasForeignKey<Payment>(p => p.BookingId)  // FK is on Payment
	.OnDelete(DeleteBehavior.Cascade);   // deleting Booking deletes its Payment
```
> **One-to-one often needs explicit config** because EF can't always guess which side holds the FK. `HasForeignKey<Payment>` tells EF "Payment is the dependent."

---

## TYPE 3: MANY-TO-MANY

**"One Student takes MANY Courses; one Course has MANY Students."**

You **don't** have this in your app (worth adding for practice!). Two ways:

### Way A — Skip navigation (EF Core 5+, easiest, EF hides the join table):
```csharp
public class Student { public int Id; public List<Course> Courses; }
public class Course  { public int Id; public List<Student> Students; }
// EF auto-creates hidden join table "CourseStudent(StudentId, CourseId)"
```

### Way B — Explicit join entity (when the join itself has data, e.g., "EnrolledDate"):
```csharp
public class Enrollment {
	public int StudentId; public Student Student;
	public int CourseId;  public Course Course;
	public DateTime EnrolledDate;   // extra payload on the relationship
}
// = really just TWO one-to-many relationships through Enrollment
```

> **Interview insight:** `TimeSlot` in your app is *almost* a many-to-many join between `Court` and `Booking` — but you modeled it as one-to-many (`TimeSlot.BookingId` nullable). That's a design decision worth explaining in interviews.

---

# SECTION B — CONFIGURING RELATIONSHIPS: 3 WAYS

| Approach | How | When to use |
|---|---|---|
| **1. Conventions** | EF guesses from names/types | 90% of the time (your Arena→Court) |
| **2. Data Annotations** | `[Key]`, `[ForeignKey]`, `[Required]` on props | Simple overrides (you used `[Key]` on `CourtRules.RuleId`) |
| **3. Fluent API** | `OnModelCreating` with `modelBuilder` | Complex cases: one-to-one, delete behavior, composite keys (most powerful) |

**Fluent API full template (memorize the pattern):**
```csharp
modelBuilder.Entity<Child>()
	.HasOne(c => c.Parent)          // or HasMany
	.WithMany(p => p.Children)      // or WithOne
	.HasForeignKey(c => c.ParentId)
	.OnDelete(DeleteBehavior.Cascade);
```

## DeleteBehavior (INTERVIEW FAVORITE) — what happens to children when parent is deleted:
| Behavior | Meaning | Your usage |
|---|---|---|
| **Cascade** | Delete children too | Payment when Booking deleted |
| **Restrict** | Block delete if children exist | Payment→User (you used this) |
| **SetNull** | Set child's FK to null | optional relationships |
| **NoAction** | Do nothing (DB may error) | rarely used |

---

# SECTION C — QUERYING: Loading Related Data

## The N+1 Problem (MUST understand)
Without `.Include()`, navigation properties are NOT loaded. If you loop and access them, EF fires a NEW query per row → 100 arenas = 101 queries. **Fix = Eager Loading with Include.**

## 1. EAGER LOADING — `.Include()` (load NOW with a JOIN)
```csharp
// One level: Arena + its Courts
var arena = await _context.Arenas
	.Include(a => a.Courts)
	.FirstOrDefaultAsync(a => a.ArenaId == id);

// MULTI-LEVEL (your code!): Arena → Courts → TimeSlots
_context.Arenas
	.Include(a => a.Courts)
		.ThenInclude(c => c.TimeSlots)   // ← ThenInclude = go one level deeper
	.FirstOrDefaultAsync(a => a.ArenaId == id);
```
*This is exactly what you wrote in `ArenaRepository.GetArenaByIdWithCourtAndSlots`.*

**Filtered Include (EF Core 5+):**
```csharp
.Include(a => a.Courts.Where(c => c.CourtType == "Badminton"))
```

## 2. LAZY LOADING (load on ACCESS — generally AVOID in web apps)
Requires `Microsoft.EntityFrameworkCore.Proxies` + `virtual` navigation props. Convenient but causes N+1 silently. **Senior answer: avoid it; prefer explicit Include.**

## 3. EXPLICIT LOADING (load on DEMAND, manually)
```csharp
var arena = await _context.Arenas.FindAsync(id);
await _context.Entry(arena).Collection(a => a.Courts).LoadAsync();  // load children now
```

## 4. PROJECTION with `.Select()` (BEST for read APIs — most performant)
Instead of loading whole entities + Includes, select exactly the shape you need:
```csharp
var dto = await _context.Courts
	.Where(c => c.ArenaId == arenaId)
	.Select(c => new CourtDetailsDTO {
		CourtId = c.CourtId,
		Name = c.Name,
		SlotCount = c.TimeSlots.Count()   // EF translates to SQL COUNT
	})
	.ToListAsync();
```
> **This is what pros do for GET endpoints** — one query, only needed columns, no change tracking.

---

# SECTION D — THE ESSENTIAL QUERY METHODS (your daily toolkit)

## FINDING BY KEY
```csharp
_context.Arenas.Find(id)          // SYNC. Checks LOCAL change-tracker cache FIRST, then DB. PK only. Returns null if missing.
await _context.Arenas.FindAsync(id) // async version
```
> `Find()` is special: it looks in memory before hitting the DB. Only works with Primary Keys.

## SINGLE ROW
```csharp
.FirstAsync(pred)        // first match; THROWS if none
.FirstOrDefaultAsync(pred)  // first match; returns null if none  ← YOU use this
.SingleAsync(pred)       // exactly one; THROWS if 0 or >1
.SingleOrDefaultAsync(pred) // one or null; THROWS if >1
```
**First vs Single:** Use `FirstOrDefault` when duplicates are possible and you just want one. Use `SingleOrDefault` to *enforce* uniqueness (e.g., email lookup).

## ANY / EXISTENCE CHECK (fastest — use instead of fetching then checking null)
```csharp
bool exists = await _context.Users.AnyAsync(u => u.UserId == id);  // SELECT EXISTS — you used this!
```

## LISTS
```csharp
.ToListAsync()                    // all rows
.Where(pred).ToListAsync()        // filtered
.OrderBy(x => x.Price).ToListAsync()
.Skip(20).Take(10).ToListAsync()  // PAGINATION (page 3, size 10)
.CountAsync(pred)                 // COUNT(*)
.SumAsync(x => x.Price)           // SUM — you used .Sum() for payment total
.MaxAsync / .MinAsync / .AverageAsync
```

## AGGREGATES / GROUPING
```csharp
.GroupBy(c => c.CourtType)
.Select(g => new { Type = g.Key, Total = g.Count() })
```

## WRITE OPERATIONS
```csharp
await _context.Arenas.AddAsync(arena);   // INSERT (staged)
_context.Arenas.AddRange(list);          // INSERT many — you used AddAllSlots
_context.Arenas.Update(arena);           // UPDATE (marks all props modified)
_context.Arenas.Remove(arena);           // DELETE — you used this in DeleteArena
_context.Arenas.RemoveRange(list);       // DELETE many — you used for TimeSlots
await _context.SaveChangesAsync();       // COMMITS all staged changes in ONE transaction
```

**CRITICAL understanding:** `Add/Update/Remove` only STAGE changes in memory. **Nothing touches the DB until `SaveChangesAsync()`.** That's why it's the "Unit of Work."

## AsNoTracking (PERFORMANCE — read-only queries)
```csharp
_context.Arenas.AsNoTracking().ToListAsync();
```
Skips change-tracking → faster, less memory. **Use for every GET that you won't modify.**

## ExecuteUpdate / ExecuteDelete (EF Core 7+ — bulk ops WITHOUT loading entities)
```csharp
await _context.TimeSlots
	.Where(s => s.Date < DateTime.Today)
	.ExecuteDeleteAsync();   // one DELETE SQL, no entities loaded
```

## Raw SQL (when LINQ isn't enough)
```csharp
_context.Arenas.FromSqlRaw("SELECT * FROM Arenas WHERE Location = {0}", city);
_context.Database.ExecuteSqlRawAsync("DELETE FROM ...");
```

---

# SECTION E — IActionResult vs ActionResult vs Task vs List
## (Return types for controller actions — a HUGE interview topic)

### The 4 layers, from simplest to most flexible:

**1. Returning raw DATA (`List<Arena>`, `CourtSlotsDTO`)**
```csharp
[HttpGet]
public List<GetArenaDTO> GetAll() => _service.GetAll();
// Always HTTP 200. Simple. But you CAN'T return NotFound() or BadRequest().
```

**2. `ActionResult<T>` — data OR a status code (the sweet spot)**
```csharp
[HttpGet("{id}")]
public ActionResult<CourtDetailsDTO> Get(int id)
{
	var court = _service.Get(id);
	if (court == null)
		return NotFound();          // ← can return a status code
	return court;                    // ← or the data (auto-200)
}
```
`ActionResult<T>` = "either the typed data `T`, or any IActionResult error." **This is Microsoft's recommended default for APIs.**

**3. `IActionResult` — full flexibility, no specific data type**
```csharp
[HttpGet("{id}")]
public IActionResult Get(int id)
{
	if (id < 0) return BadRequest();
	var court = _service.Get(id);
	if (court == null) return NotFound();
	return Ok(court);               // you wrap data in Ok() yourself
}
```
Use when the action returns *different* data types on different paths, or non-data results.

**4. `Task<...>` — making any of the above ASYNC (you need this for DB calls)**
```csharp
public async Task<IActionResult> Get(int id)          // async + flexible
public async Task<ActionResult<CourtDetailsDTO>> Get(int id)  // async + typed (BEST)
public async Task<List<GetArenaDTO>> GetAll()         // async + raw data
```

### THE KEY INSIGHT:
- `Task` is **orthogonal** to `IActionResult` — they solve different problems.
- `Task` = "this runs asynchronously" (needed because DB calls are async).
- `IActionResult`/`ActionResult<T>`/`List` = "what HTTP response do I produce."
- So you **combine** them: `Task<ActionResult<Dto>>` is the professional standard.

### Decision table:
| Return type | Can return status codes? | Typed data? | When to use |
|---|---|---|---|
| `List<T>` / `T` | ❌ (always 200) | ✅ | Simple reads that never fail |
| `ActionResult<T>` | ✅ | ✅ | **Default for most API actions** |
| `IActionResult` | ✅ | ❌ (wrapped in Ok) | Multiple return types / NoContent etc. |
| `Task<ActionResult<T>>` | ✅ | ✅ | **Async version — your go-to** |

### Common status-code helpers (memorize):
`Ok(x)` 200 · `CreatedAtAction(...)` 201 · `NoContent()` 204 (DELETE — you used it) · `BadRequest()` 400 · `Unauthorized()` 401 · `NotFound()` 404 · `Conflict()` 409 · `StatusCode(500)`

> **Your code note:** You return `IActionResult` mostly and throw exceptions for errors (which your `GlobalExceptionHandlerMiddleware` converts to 404/400). That's a valid "exception-driven" style. The alternative is returning `NotFound()` directly. Both are acceptable — be ready to defend either in an interview.

---

# SECTION F — HANDS-ON MASTERY EXERCISES (do these IN your repo)

1. **Draw your schema on paper:** Arenas → Courts → TimeSlots → Bookings → Payments. Label every PK, FK, and nav property.
2. **Write 5 queries** in a scratch controller: (a) all courts with arena names, (b) available slots for a court using projection, (c) a booking with its payment via Include, (d) count of bookings per court via GroupBy, (e) `FindAsync` vs `FirstOrDefaultAsync` — log the SQL each generates (enable `LogTo` or check console).
3. **Add a Many-to-Many:** Create `Amenity` (Wifi, Parking) ↔ `Arena` many-to-many with skip navigations. Run a migration. See the auto join table.
5. **Convert 3 controller actions** to `Task<ActionResult<Dto>>` and use `NotFound()` instead of throwing — see how it changes your exception handler's workload.

> Master these 4 and you'll answer EF Core + Web API return-type questions better than most mid-level devs.
