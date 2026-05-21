# 🏗️ MODERN ARCHITECTURE BLUEPRINT - LUZ GRAND HOTEL v2.0

## Project Structure

```
LuzReception-Net10/
├── .github/workflows/              # CI/CD pipelines
│   ├── build.yml                  # Build on push
│   ├── test.yml                   # Run tests
│   └── release.yml                # Release automation
│
├── src/
│   ├── LuzReception.Models/
│   │   ├── LuzReception.Models.csproj
│   │   ├── Entities/
│   │   │   ├── Reservation.cs
│   │   │   ├── Accompagnant.cs
│   │   │   └── AppSetting.cs
│   │   ├── ValueObjects/
│   │   │   ├── LanguageDetermination.cs
│   │   │   ├── CivilitesDictionnaire.cs
│   │   │   └── ReservationId.cs
│   │   └── Exceptions/
│   │       ├── DataAccessException.cs
│   │       ├── OPERAImportException.cs
│   │       └── ValidationException.cs
│   │
│   ├── LuzReception.DataAccess/
│   │   ├── LuzReception.DataAccess.csproj
│   │   ├── DbContext/
│   │   │   └── LuzReceptionDbContext.cs
│   │   ├── Repositories/
│   │   │   ├── IReservationRepository.cs
│   │   │   ├── ReservationRepository.cs
│   │   │   └── IAccompagnantRepository.cs
│   │   ├── Migrations/
│   │   │   ├── 001_InitialCreate.cs
│   │   │   └── 002_AddIndexes.cs
│   │   └── GlobalUsings.cs
│   │
│   ├── LuzReception.Services/
│   │   ├── LuzReception.Services.csproj
│   │   ├── Abstractions/
│   │   │   ├── IReservationService.cs
│   │   │   ├── IOPERAImportService.cs
│   │   │   ├── ILanguageDetectionService.cs
│   │   │   ├── IPrintService.cs
│   │   │   └── IExportService.cs
│   │   ├── Implementations/
│   │   │   ├── ReservationService.cs
│   │   │   ├── OPERAImportService.cs
│   │   │   ├── LanguageDetectionService.cs
│   │   │   ├── PrintService.cs
│   │   │   └── ExportService.cs
│   │   └── GlobalUsings.cs
│   │
│   └── LuzReception.UI/
│       ├── LuzReception.UI.csproj
│       ├── App.xaml
│       ├── App.xaml.cs
│       ├── Views/
│       │   ├── MainWindow.xaml
│       │   ├── MainWindow.xaml.cs
│       │   ├── ReceptionView.xaml
│       │   ├── ReservationsView.xaml
│       │   └── StatisticsView.xaml
│       ├── ViewModels/
│       │   ├── MainWindowViewModel.cs
│       │   ├── ReceptionViewModel.cs
│       │   ├── ReservationsViewModel.cs
│       │   └── StatisticsViewModel.cs
│       ├── Converters/
│       │   ├── DateToStringConverter.cs
│       │   └── BoolToVisibilityConverter.cs
│       ├── Behaviors/
│       │   └── TextBoxBehavior.cs
│       └── Resources/
│           ├── Themes/
│           │   └── Modern.xaml
│           └── Strings/
│               ├── Strings.fr.xaml
│               ├── Strings.en.xaml
│               └── Strings.es.xaml
│
├── tests/
│   ├── LuzReception.Tests.Unit/
│   │   ├── LuzReception.Tests.Unit.csproj
│   │   ├── Services/
│   │   │   ├── ReservationServiceTests.cs
│   │   │   ├── OPERAImportServiceTests.cs
│   │   │   └── LanguageDetectionServiceTests.cs
│   │   ├── DataAccess/
│   │   │   └── ReservationRepositoryTests.cs
│   │   ├── Models/
│   │   │   └── LanguageDeterminationTests.cs
│   │   └── Fixtures/
│   │       └── ReservationFixture.cs
│   │
│   └── LuzReception.Tests.Integration/
│       ├── LuzReception.Tests.Integration.csproj
│       └── Services/
│           └── ReservationServiceIntegrationTests.cs
│
├── docs/
│   ├── ARCHITECTURE.md
│   ├── API.md
│   ├── DEPLOYMENT.md
│   └── TROUBLESHOOTING.md
│
├── LuzReception.sln
├── Directory.Build.props
├── README.md
└── VERSION
```

## Dependency Graph

```
┌──────────────────────────────────────────────┐
│         LuzReception.UI (WPF)                │
│  ┌──────────────────────────────────────────┐│
│  │  MainWindow (minimal code-behind)       ││
│  │  └─ ReceptionViewModel                  ││
│  │     ├─ IReservationService              ││
│  │     ├─ IOPERAImportService              ││
│  │     └─ ILogger<ReceptionViewModel>      ││
│  └──────────────────────────────────────────┘│
└──────────────────────────────────────────────┘
    ↓ (ServiceCollection.AddScoped)
┌──────────────────────��───────────────────────┐
│    LuzReception.Services (Business Logic)    │
│  ┌──────────────────────────────────────────┐│
│  │ ReservationService                       ││
│  │ ├─ IReservationRepository                ││
│  │ ├─ ILanguageDetectionService             ││
│  │ └─ ILogger<ReservationService>           ││
│  │                                           ││
│  │ OPERAImportService                       ││
│  │ ├─ IReservationService                   ││
│  │ └─ ILogger<OPERAImportService>           ││
│  └──────────────────────────────────────────┘│
└──────────────────────────────────────────────┘
    ↓ (ServiceCollection.AddScoped)
┌──────────────────────────────────────────────┐
│  LuzReception.DataAccess (EF Core)           │
│  ┌──────────────────────────────────────────┐│
│  │ ReservationRepository                    ││
│  │ ├─ LuzReceptionDbContext                 ││
│  │ └─ ILogger<ReservationRepository>        ││
│  │                                           ││
│  │ LuzReceptionDbContext                    ││
│  │ └─ DbContextOptions<...>                 ││
│  └──────────────────────────────────────────┘│
└──────────────────────────────────────────────┘
    ↓ (ServiceCollection.AddDbContext)
┌──────────────────────────────────────────────┐
│   LuzReception.Models (Immutable Records)    │
│  ┌──────────────────────────────────────────┐│
│  │ Reservation (EF entity)                  ││
│  │ Accompagnant (EF entity)                 ││
│  │ LanguageDetermination (value object)     ││
│  └──────────────────────────────────────────┘│
└──────────────────────────────────────────────┘
```

## Initialization Flow (Dependency Injection)

```csharp
// Program.cs
public static void Main(string[] args)
{
    var app = new App();
    app.Run();
}

// App.xaml.cs
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;
    
    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }
    
    private void ConfigureServices(IServiceCollection services)
    {
        // Configuration
        services.AddSingleton(_ => LoadConfiguration());
        
        // Logging
        services.AddLogging(builder =>
            builder
                .AddSerilog(new LoggerConfiguration()
                    .WriteTo.File("logs/app-.txt", rollingInterval: RollingInterval.Day)
                    .WriteTo.Console()
                    .CreateLogger()));
        
        // Database
        services.AddDbContext<LuzReceptionDbContext>(options =>
            options.UseSqlite("Data Source=LuzReception.db"));
        
        // Repositories
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IAccompagnantRepository, AccompagnantRepository>();
        
        // Services
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IOPERAImportService, OPERAImportService>();
        services.AddScoped<ILanguageDetectionService, LanguageDetectionService>();
        services.AddScoped<IPrintService, PrintService>();
        services.AddScoped<IExportService, ExportService>();
        
        // ViewModels
        services.AddTransient<ReceptionViewModel>();
        services.AddTransient<ReservationsViewModel>();
        services.AddTransient<StatisticsViewModel>();
        services.AddTransient<MainWindowViewModel>();
        
        // Views
        services.AddTransient<MainWindow>();
    }
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        // Initialize database
        var dbContext = _serviceProvider.GetRequiredService<LuzReceptionDbContext>();
        dbContext.Database.EnsureCreated();
        
        // Show window
        var window = _serviceProvider.GetRequiredService<MainWindow>();
        window.Show();
    }
}
```

## Data Flow in Reservation Save Operation

```
User Input (UI)
    ↓
ReceptionView (XAML)
    ↓
ReceptionViewModel.SaveReservationCommand
    ↓
IReservationService.SaveReservationAsync()
    ├─ ILanguageDetectionService.DetermineLanguage()
    │  └─ Analyzes civilité → FR/EN/ES + M/F/Mixte
    │
    ├─ GracefulNessFormula.GenerateFormula()
    │  └─ Creates politeness formula based on language/gender
    │
    └─ IReservationRepository.SaveAsync()
       ├─ LuzReceptionDbContext.Reservations.Add()
       │  └─ EF Core adds entity to change tracking
       │
       ├─ SaveChangesAsync()
       │  └─ EF Core executes INSERT SQL
       │
       └─ Returns reservation ID
            ↓
        UI Updated (MVVM binding)
            ↓
        StatusText = "✓ Réservation enregistrée"
```

## File Structure for Each Layer

### Models Layer (Immutable Records)

```csharp
// Reservation.cs - EF Entity
public record Reservation
{
    public int Id { get; init; }
    public required string NumeroReservation { get; init; }  // required!
    public required string NomClient { get; init; }
    // ... init-only properties for immutability
    public List<Accompagnant> Accompagnants { get; init; } = [];
    
    // Business logic can go here (if needed)
    public bool IsVIP => VIP;
    public bool IsStaying => DateTime.UtcNow >= DateArrivee && DateTime.UtcNow <= DateDepart;
}
```

### Data Access Layer (EF Core)

```csharp
// IReservationRepository.cs - Interface
public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetByDateAsync(
        DateTime date, 
        CancellationToken ct = default);
    Task<int> SaveAsync(
        Reservation reservation, 
        CancellationToken ct = default);
}

// ReservationRepository.cs - Implementation
public class ReservationRepository : IReservationRepository
{
    private readonly LuzReceptionDbContext _context;
    private readonly ILogger<ReservationRepository> _logger;
    
    public async Task<IEnumerable<Reservation>> GetByDateAsync(
        DateTime date, 
        CancellationToken ct = default)
    {
        return await _context.Reservations
            .Where(r => r.DateArrivee.Date == date.Date)
            .Include(r => r.Accompagnants)
            .OrderBy(r => r.NumerosChambre)
            .ToListAsync(ct);
    }
    
    public async Task<int> SaveAsync(
        Reservation reservation, 
        CancellationToken ct = default)
    {
        if (reservation.Id == 0)
        {
            _context.Reservations.Add(reservation);
        }
        else
        {
            _context.Reservations.Update(reservation);
        }
        
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation(
            "Reservation saved: {ReservationNumber}", 
            reservation.NumeroReservation);
        
        return reservation.Id;
    }
}
```

### Services Layer (Business Logic)

```csharp
// IReservationService.cs - Interface
public interface IReservationService
{
    Task<int> SaveReservationAsync(
        Reservation reservation, 
        CancellationToken ct = default);
}

// ReservationService.cs - Implementation
public class ReservationService : IReservationService
{
    private readonly IReservationRepository _repository;
    private readonly ILanguageDetectionService _languageService;
    private readonly ILogger<ReservationService> _logger;
    
    public async Task<int> SaveReservationAsync(
        Reservation reservation, 
        CancellationToken ct = default)
    {
        // 1. Validate input
        ValidateReservation(reservation);
        
        // 2. Auto-detect language
        var langDetection = _languageService.DetermineLanguage(
            reservation.CiviliteClient);
        
        // 3. Generate politeness formula
        var formula = GracefulNessFormula.GenerateFormula(
            langDetection.Langue,
            langDetection.Genre,
            new List<string> { reservation.NomClient },
            reservation.Accompagnants);
        
        // 4. Create updated reservation (immutable)
        var updatedReservation = reservation with
        {
            LangueAuto = langDetection.Langue,
            GenreGrammatical = langDetection.Genre,
            FormulePolitesse = formula,
            DateModification = DateTime.UtcNow
        };
        
        // 5. Save to database
        return await _repository.SaveAsync(updatedReservation, ct);
    }
    
    private void ValidateReservation(Reservation reservation)
    {
        if (string.IsNullOrWhiteSpace(reservation.NomClient))
            throw new ValidationException("Nom client requis");
        
        if (reservation.DateArrivee >= reservation.DateDepart)
            throw new ValidationException("Date départ doit être après date arrivée");
    }
}
```

### UI Layer (MVVM)

```csharp
// ReceptionViewModel.cs - ViewModel
public partial class ReceptionViewModel : ObservableObject
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReceptionViewModel> _logger;
    
    [ObservableProperty]
    private string nomClient = string.Empty;
    
    [ObservableProperty]
    private string civilite = "Monsieur";
    
    [ObservableProperty]
    private string formulePolitesse = string.Empty;
    
    [ObservableProperty]
    private string statusText = "Prêt";
    
    [RelayCommand]
    private async Task SaveReservation()
    {
        try
        {
            StatusText = "Sauvegarde en cours...";
            
            var reservation = new Reservation
            {
                NomClient = NomClient.ToUpperInvariant(),
                CiviliteClient = Civilite,
                NumeroReservation = Guid.NewGuid().ToString(),
                // ... other properties
            };
            
            var id = await _reservationService.SaveReservationAsync(reservation);
            
            StatusText = $"✓ Réservation sauvegardée (ID: {id})";
            ClearForm();
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error");
            StatusText = $"✗ {ex.Message}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error saving reservation");
            StatusText = "✗ Erreur inattendue. Veuillez réessayer.";
        }
    }
    
    [RelayCommand]
    private void ClearForm()
    {
        NomClient = string.Empty;
        Civilite = "Monsieur";
        FormulePolitesse = string.Empty;
    }
}

// MainWindow.xaml.cs - View (minimal code-behind)
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

// MainWindow.xaml - View (XAML)
<Window x:Class="LuzReception.MainWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    Title="LUZ GRAND HOTEL - Réception" Width="1600" Height="900">
    
    <Grid>
        <StackPanel Padding="15">
            <TextBlock Text="Nom:" FontWeight="Bold" />
            <TextBox Text="{Binding ReceptionViewModel.NomClient, UpdateSourceTrigger=PropertyChanged}" 
                     Padding="8" Height="35" />
            
            <Button Command="{Binding ReceptionViewModel.SaveReservationCommand}" 
                    Content="Enregistrer" 
                    Padding="10,8" 
                    Margin="0,10,0,0" 
                    Background="#28A745" 
                    Foreground="White" />
            
            <TextBlock Text="{Binding ReceptionViewModel.StatusText}" Margin="0,10,0,0" />
        </StackPanel>
    </Grid>
</Window>
```

---

**Architecture designed for:**
- ✅ Testability (every layer independently testable)
- ✅ Maintainability (clear responsibilities)
- ✅ Scalability (easy to add new features)
- ✅ Reusability (services can be used by multiple clients)
- ✅ Flexibility (can swap implementations)
