# OmegaExplorer

Jeu de stratégie spatiale en cours de développement. Le joueur explore des cartes dynamiques, développe ses ressources, ses technologies et ses vaisseaux, puis interagit avec un univers persistant.

## Jouer

La version publique est accessible depuis la [marketplace de Loïc Delaunay](https://loicdelaunay.com/). Le jeu utilise un client web et une API ; une connexion au serveur est nécessaire pour les fonctions persistantes.

## Architecture

| Projet | Rôle |
| --- | --- |
| `OmegaExplorer.Client` | Client Blazor WebAssembly, interface et cartes interactives |
| `OmegaExplorer.Server` | API ASP.NET Core, logique du jeu, SignalR et persistance SQLite |
| `OmegaExplorer.Shared` | Modèles et utilitaires partagés |
| `OmegaExplorer.Aspire.*` | Orchestration et services de développement |
| `OmegaExplorer.Server.Test` | Tests serveur |

Le client et le serveur ciblent .NET 9. Les packages NuGet sont restaurés avec le SDK indiqué dans `global.json`.

## Lancer en développement

Dans deux terminaux :

```powershell
dotnet run --project OmegaExplorer.Server --urls http://localhost:5000
dotnet run --project OmegaExplorer.Client --urls http://localhost:5280
```

Ouvrez ensuite `http://localhost:5280`. Le client de développement pointe par défaut vers l'API `http://localhost:5000` dans `OmegaExplorer.Client/wwwroot/appsettings.json`. Le serveur crée sa base SQLite et applique ses migrations au démarrage.

Pour un déploiement, fournissez une configuration serveur propre à l'environnement et une clé JWT générée pour cette installation. Ne publiez pas de fichiers de configuration contenant des secrets.
