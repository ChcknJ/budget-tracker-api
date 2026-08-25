BudgetTracker API
Expense tracking API for personal budgets, expenses, categories, subscriptions and monthly summaries.
Tech stack
•	.NET: net10.0 (TargetFramework)
•	ASP.NET Core Web API
•	EF Core 10.0.9 (Microsoft.EntityFrameworkCore / Tools / Design)
•	Npgsql EF provider (Npgsql.EntityFrameworkCore.PostgreSQL 10.0.3) — PostgreSQL
•	JWT authentication (Microsoft.AspNetCore.Authentication.JwtBearer 10.0.10)
•	FluentValidation 12.1.1 (with DI extensions)
•	BCrypt.Net-Next 4.2.0 (password hashing)
•	EFCore.NamingConventions 10.0.1 (snake_case DB naming)
•	OpenAPI / swagger support (Microsoft.AspNetCore.OpenApi 10.0.9, Microsoft.OpenApi 2.9.0)
•	Built-in ASP.NET Core rate limiting (configured in Program.cs)
Features
•	User registration and login (JWT)
•	Create / edit / list / delete expenses (with pagination & filtering)
•	Category CRUD
•	Monthly budgets (create / edit / get / list)
•	Subscriptions (create / edit / cancel / list)
•	Monthly summary with category breakdown
•	Server-side validation (FluentValidation)
•	Rate limiting: "Auth" (5 req/min) and "General" (10 req/min)
Getting started
Prerequisites
•	.NET 10 SDK
•	PostgreSQL (any recent version)
•	(Optional) dotnet-ef for migrations (dotnet tool install --global dotnet-ef)
Quick setup
1.	Clone git clone https://github.com/ChcknJ/budget-tracker-api.git
2.	Restore dotnet restore
3.	Configure secrets (recommended: user-secrets or environment variables)
•	Required configuration keys:
•	ConnectionStrings:DefaultConnection (Postgres connection string)
•	Jwt:Key
•	Jwt:Issuer
•	Jwt:Audience
•	Jwt:ExpiryMinutes (integer) Example (user-secrets): dotnet user-secrets init dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=...;Database=...;Username=...;Password=..." dotnet user-secrets set "Jwt:Key" "your-very-secret-key" dotnet user-secrets set "Jwt:Issuer" "your-issuer" dotnet user-secrets set "Jwt:Audience" "your-audience" dotnet user-secrets set "Jwt:ExpiryMinutes" "60"
4.	Apply EF migrations dotnet ef database update
5.	Run dotnet run
Notes
•	DefaultConnection key is used in Program.cs to configure Npgsql.
•	Swagger/OpenAPI is enabled in Development (see Program.cs).
API Endpoints
Auth | Method | Endpoint | Description | Auth Required | Request body (summary) | |---|---:|---|:---:|---| | POST | /api/auth/register | Register new user → returns LoginResponse (token) | No | RegisterRequest { username: string, password: string } | | POST | /api/auth/login | Login → returns LoginResponse (token + expiry) | No | LoginRequest { username: string, password: string } |
Expenses (controller requires [Authorize]) | Method | Endpoint | Description | Auth Required | Request body / query (summary) | |---|---:|---|:---:|---| | POST | /api/expense/create-expense | Create expense | Yes | ExpenseRequest { CategoryId: Guid, SubscriptionId?: Guid, Amount: decimal, Description?: string, Date: DateOnly } | | PATCH | /api/expense/edit-expense/{expenseId} | Edit expense by id | Yes | ExpenseRequest (same) + route param expenseId: Guid | | GET | /api/expense/get-expenses | Query & paginate expenses | Yes | ExpenseQueryRequest (query params): { CategoryId?, SubscriptionId?, FromDate?, ToDate?, MinimumAmount?, MaximumAmount?, PageNumber, PageSize, SortBy, SortOrder } | | DELETE | /api/expense/delete-expense/{expenseId} | Delete expense | Yes | route param expenseId: Guid |
Categories | Method | Endpoint | Description | Auth Required | Request body | |---|---:|---|:---:|---| | POST | /api/category/create-category | Create category | Yes | CategoryRequest { Name: string } | | PATCH | /api/category/edit-category/{categoryId} | Edit category | Yes | CategoryRequest + route param categoryId: Guid | | GET | /api/category/get-categories | List categories for user | Yes | none | | DELETE | /api/category/delete-category/{categoryId} | Delete category | Yes | route param categoryId: Guid |
Budgets | Method | Endpoint | Description | Auth Required | Request body / params | |---|---:|---|:---:|---| | POST | /api/budget/create-budget | Create monthly budget | Yes | BudgetRequest { Month: DateOnly, Amount: decimal } | | PATCH | /api/budget/edit-budget/{month} | Edit budget for month | Yes | BudgetRequest + route param month (DateOnly) | | GET | /api/budget/get-budget/{month} | Get budget for month | Yes | route param month (DateOnly) | | GET | /api/budget/get-all-budgets | Get all budgets | Yes | none |
Subscriptions | Method | Endpoint | Description | Auth Required | Request body | |---|---:|---|:---:|---| | POST | /api/subscription/create-subscription | Create subscription | Yes | SubscriptionRequest { CategoryId: Guid, Name: string, Amount: decimal, StartDate: DateOnly, EndDate?: DateOnly, BillingCycle: string } | | PATCH | /api/subscription/edit-subscription/{subscriptionId} | Edit subscription | Yes | SubscriptionRequest + route param subscriptionId: Guid | | DELETE | /api/subscription/cancel-subscription/{subscriptionId} | Cancel subscription | Yes | route param subscriptionId: Guid | | GET | /api/subscription/get-subscriptions | List subscriptions | Yes | none |
Summary | Method | Endpoint | Description | Auth Required | Params | |---|---:|---|:---:|---| | GET | /api/summary/get-summary | Get monthly summary (totals & category breakdown) | Yes | month: DateOnly (query param) |
Auth / Tokens
•	Register and Login endpoints return a LoginResponse containing a JWT access token (Token), TokenType ("Bearer") and ExpiresAt.
•	Use Authorization: Bearer <token> for protected endpoints.
•	No refresh-token / HttpOnly cookie flow is implemented in the current codebase (token is returned in JSON response).
Rate limiting
•	"Auth" limiter: 5 requests per minute
•	"General" limiter: 10 requests per minute (see Program.cs for exact settings)
Configuration keys
•	ConnectionStrings:DefaultConnection — PostgreSQL connection string
•	Jwt:Key, Jwt:Issuer, Jwt:Audience, Jwt:ExpiryMinutes — JWT options (configured via builder.Configuration.GetSection("Jwt"))
Project structure (key folders)
•	Controllers/ — API endpoints (AuthController, ExpenseController, CategoryController, BudgetController, SubscriptionController, SummaryController)
•	DTO/ — request and response shapes for endpoints
•	Services/ — business logic implementations (AuthService, ExpenseService, etc.)
•	Interfaces/ — service interfaces (IAuthService, IExpenseService, ...)
•	Database/ — AppDbContext and EF Core configuration
•	Models/ — EF entities (User, Expense, Category, Budget, Subscription)
•	Migrations/ — EF migrations
•	Validators/ — FluentValidation validators for requests
•	Configs/ — configuration models (JwtSettings)
•	Exceptions/ — global exception handler