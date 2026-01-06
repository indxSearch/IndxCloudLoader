# IndxCloudLoader

A C# console application for loading and configuring datasets in **IndxCloudApi**, the search server. This helper tool streamlines the process of importing data, configuring search fields, and setting up filters for optimal search performance.

## Related Projects

- **[IndxCloudApi](https://github.com/yourusername/IndxCloudApi)** - The search server API that this loader connects to
- **[indx-interface](https://github.com/yourusername/indx-interface)** - React UI for interacting with the search server

## Features

- Automatic dataset import and indexing
- Pre-configured field settings (searchable, filterable, facetable, sortable)
- Support for multiple datasets
- Field weight configuration for search relevance
- Real-time status monitoring during import
- Example search queries to verify data loading

## Included Datasets

### TMDB Top 10,000 Movies

The project includes a pre-configured dataset of 10,000 top movies from The Movie Database (TMDB) with the following field configuration:

**Searchable Fields:**
- `title` (High weight)
- `original_title` (Medium weight)
- `description` (Medium weight)
- `actors` (Low weight)

**Filterable & Facetable Fields:**
- `release_year`
- `vote_average`
- `vote_count_tier`
- `genres`
- `decade`
- `actors`
- `language`

**Sortable Fields:**
- `popularity`
- `vote_average`

### Pokedex Dataset

Also includes a Pokemon dataset with searchable, filterable, and sortable fields configured for quick testing.

## Prerequisites

- .NET 9.0 or later
- Running instance of IndxCloudApi (local or Azure-deployed)
- Valid authentication credentials for IndxCloudApi

## Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/IndxCloudLoader.git
   cd IndxCloudLoader
   ```

2. Create a `.env.local` file in the root directory with your configuration:
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

3. Choose your dataset by editing `LoadAPI.cs:18`:
   ```csharp
   private const string dataset = "tmdb"; // Options: "tmdb", "pokedex"
   ```

## Usage

Run the application to load and configure your chosen dataset:

```bash
dotnet run
```

The loader will:
1. Connect to IndxCloudApi
2. Create or open the dataset
3. Analyze the data structure
4. Configure searchable, filterable, facetable, and sortable fields
5. Load the data
6. Index the dataset
7. Run a test search query

## Configuration

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `API_URI` | URL of your IndxCloudApi instance | `https://localhost:5001` |
| `BEARER_TOKEN` | JWT authentication token (recommended) | - |
| `USER_EMAIL` | Your IndxCloudApi account email (alternative auth) | - |
| `USER_PASSWORD` | Your IndxCloudApi account password (alternative auth) | - |

## API Endpoints

The loader interacts with the following IndxCloudApi endpoints:
- `/api/CreateOrOpen/{dataSetName}/{configuration}` - Initialize dataset
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
2. Add a new configuration block in `LoadAPI.cs` in the `LoadDataset()` method
3. Define your searchable, filterable, facetable, and sortable fields
4. Update the dataset constant to use your new dataset name

## License

[Your License Here]

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
