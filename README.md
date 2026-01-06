# IndxCloudLoader

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/)
[![Version](https://img.shields.io/badge/version-1.0.0-green.svg)](https://github.com/indxSearch/IndxCloudLoader/releases)

**Version 1.0** - Compatible with IndxCloudApi 1.0

A C# console application for loading and configuring datasets in **IndxCloudApi**, the search server. This helper tool streamlines the process of importing data, configuring search fields, and setting up filters for optimal search performance.

## Related Projects

- **[IndxCloudApi](https://github.com/indxSearch/IndxCloudApi)** - The search server API that this loader connects to
- **[indx-interface](https://github.com/indxSearch/indx-interface)** - React UI for interacting with the search server

## Features

- Automatic dataset import and indexing
- Pre-configured field settings (searchable, filterable, facetable, sortable)
- Support for multiple datasets
- Field weight configuration for search relevance
- Real-time status monitoring during import
- Example search queries to verify data loading

## Project Structure

```
IndxCloudLoader/
├── Program.cs              # Entry point with CLI args and interactive menu
├── LoadAPI.cs              # Main data loading logic with API calls
├── DatasetConfig.cs        # Dataset configurations (tmdb, pokedex)
├── ApiModels.cs            # API data contracts (DTOs for JSON serialization)
├── ConsoleHelper.cs        # Console output formatting utilities
├── Login.cs                # Authentication helpers
├── .env.local.example      # Environment configuration template
└── data/                   # Dataset JSON files
    ├── tmdb_top10k.json
    └── pokedex.json
```

**Key Files:**
- **ApiModels.cs** - Contains all the data transfer objects (DTOs) that match IndxCloudApi's JSON contracts. These are required for C# to serialize/deserialize API requests and responses.
- **DatasetConfig.cs** - Add new datasets here by defining searchable, filterable, facetable, and sortable fields.
- **LoadAPI.cs** - The main workflow with comprehensive inline comments explaining each step.

## Included Datasets

### TMDB Top 10,000 Movies

The project includes a pre-configured dataset of 10,000 top movies from The Movie Database (TMDB). Data sourced from [TMDB](https://www.themoviedb.org/).

### Pokedex Dataset

Includes a Pokemon dataset for testing purposes. Pokemon data is property of Nintendo/Game Freak.

## Prerequisites

- .NET 9.0 or later
- Running instance of IndxCloudApi (local or Azure-deployed)
- Valid authentication credentials for IndxCloudApi

## Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/indxSearch/IndxCloudLoader.git
   cd IndxCloudLoader
   ```

2. Create your configuration file:
   ```bash
   cp .env.local.example .env.local
   ```

   Then edit `.env.local` with your configuration:
   ```env
   API_URI=https://localhost:5001
   USER_EMAIL=your-email@example.com
   USER_PASSWORD=your-password
   BEARER_TOKEN=your-jwt-token
   ```

   **Authentication (Recommended):** Use a bearer token for authentication. To obtain a bearer token:
   1. Navigate to your IndxCloudApi instance (e.g., `https://localhost:5001` or your Azure URL)
   2. Log in to the search API
   3. Click "API Key" to generate and copy your bearer token
   4. Paste the token into the `BEARER_TOKEN` field in `.env.local`

   Note: While email/password authentication is supported, bearer token authentication is recommended for better security.

   **For local development:** Use `https://localhost:5001` as the API_URI
   **For Azure deployment:** Replace with your Azure API URL

3. You're ready! No code changes needed - dataset selection is now done via command-line or interactive menu.

## Usage

### Interactive Mode (Recommended for First Time)

Simply run without arguments to see an interactive menu:

```bash
dotnet run
```

You'll see:
```
━━━ Dataset Selection ━━━

Available datasets:
  1. tmdb
  2. pokedex
  0. Exit

Select dataset (enter number):
```

### Command-Line Mode

Specify the dataset directly:

```bash
# Load TMDB dataset
dotnet run --dataset tmdb

# Or use short form
dotnet run -d pokedex
```

### Get Help

```bash
dotnet run --help
```

### What Happens

The loader will:
1. ✓ Validate data file exists
2. ✓ Connect to IndxCloudApi and verify authentication
3. ✓ Create or open the dataset
4. ✓ Analyze the JSON data structure
5. ✓ Configure searchable, filterable, facetable, and sortable fields
6. ✓ Stream and load the data
7. ✓ Build the search index
8. ✓ Run a test search query
9. ✓ Display a summary with statistics

## API Endpoints

The loader interacts with the following IndxCloudApi endpoints:
- `/api/CreateOrOpen/{dataSetName}/{configuration}` - Initialize dataset session
- `/api/AnalyzeString/{dataSetName}` - Analyze data structure
- `/api/SetSearchableFields/{dataSetName}` - Configure searchable fields
- `/api/SetFilterableFields/{dataSetName}` - Configure filterable fields
- `/api/SetFacetableFields/{dataSetName}` - Configure facetable fields
- `/api/SetSortableFields/{dataSetName}` - Configure sortable fields
- `/api/LoadStream/{dataSetName}` - Load data
- `/api/IndexDataSet/{dataSetName}` - Build search index
- `/api/Search/{dataSetName}` - Execute search queries

## Dataset Configuration

To add your own dataset:

1. Place your JSON file in the `data/` directory
2. Add a new configuration case in `DatasetConfig.cs` in the `GetConfig()` method
3. Define your searchable, filterable, facetable, and sortable fields
4. Add the dataset name to the `GetAvailableDatasets()` method

Example:
```csharp
"products" => new DatasetConfig
{
    Name = "products",
    FilePath = "data/products.json",
    SearchableFields = new[]
    {
        ("name", (int)Weight.High),
        ("description", (int)Weight.Med)
    },
    FilterableFields = new[] { "price", "category", "in_stock" },
    FacetableFields = new[] { "category", "brand" },
    SortableFields = new[] { "price", "name" },
    TestQuery = "laptop"
}
```

## Troubleshooting

### "Data file not found"
- Ensure the JSON file exists in the `data/` directory
- Check that the filename matches exactly (case-sensitive)
- Verify the path in DatasetConfig.cs is correct

### "Network error: Cannot connect"
- Verify IndxCloudApi is running
- Check the API_URI in `.env.local`
- For localhost, ensure you're using HTTPS: `https://localhost:5001`
- Check your firewall settings

### "Failed to create or open dataset"
- Verify API_URI in .env.local is correct
- Ensure IndxCloudApi is running
- Check that BEARER_TOKEN is valid (not expired)
- Verify your user account has permission to create datasets

### "Failed to analyze data file"
- Ensure the data file is valid JSON
- Check that the file is not corrupted
- Verify the file is not empty

### Authentication Issues
- Your bearer token may have expired - generate a new one by logging into IndxCloudApi and clicking "API Key"
- Verify the BEARER_TOKEN in `.env.local` is correct and complete
- Ensure you're authenticated to the API with valid credentials

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
