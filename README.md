# ReserGo

ReserGo est une plateforme de gestion de réservations pour hôtels et restaurants, développée en .NET (C#) avec une architecture multi-couches (API, Business, DataAccess, Shared, Common, Tiers).

## Fonctionnalités principales
- **Gestion des réservations** : Permet aux utilisateurs de réserver des chambres d'hôtel, des tables de restaurant, ou des salles d'événement.
- **Gestion des offres** : Les hôtels, restaurants, et salles d'événement peuvent proposer des offres spéciales.
- **Recherche de disponibilités** : Recherche avancée de disponibilités selon des critères (dates, nombre de personnes, type de cuisine, etc.).
- **Notifications** : Système de notifications en temps réel via SignalR.
- **Authentification et sécurité** : Gestion des utilisateurs, rôles, et sécurité des accès.
- **Administration** : Interfaces pour la gestion des établissements, des offres, et des disponibilités.

## Technologies utilisées
- **.NET 9** (C#)
- **ASP.NET Core Web API**
- **Entity Framework Core** (accès aux données, migrations)
- **PostgreSQL**
- **SignalR** (notifications temps réel)
- **Swagger / OpenAPI** (documentation interactive de l’API)
- **Docker** (déploiement et conteneurisation)
- **NLog** (logs)
<!-- - **xUnit** (tests unitaires) -->

## Architecture
- **ReserGo.WebApi** : API REST principale, expose les endpoints pour toutes les opérations (réservations, recherches, administration, etc.).
- **ReserGo.Business** : Logique métier (services de réservation, gestion des offres, notifications, etc.).
- **ReserGo.DataAccess** : Accès aux données via Entity Framework Core (contextes, requêtes, migrations).
- **ReserGo.Common** : Objets partagés (DTO, entités, requêtes, réponses, helpers).
- **ReserGo.Shared** : Constantes, exceptions, utilitaires partagés.
- **ReserGo.Tiers** : Intégration avec des services tiers (ex : FranceGouv, Google).

## Démarrage rapide
1. **Prérequis** :
   - .NET 9 SDK
   - PostgreSQL (base de données)
   - Docker (optionnel)

2. **Configuration** :
   - Renseignez les chaînes de connexion dans `appsettings.json`. (Contactez les contributeurs pour les informations de connexion à la base de données PostgreSQL).
   - Configurez les paramètres nécessaires (authentification, notifications, etc.).

3. **Migration de la base** :
   - Appliquez les migrations Entity Framework pour créer la base de données :
     ```bash
     dotnet ef database update --project ReserGo.DataAccess
     ```

4. **Lancement de l'API** :
   - Depuis le dossier racine :
     ```bash
     dotnet run --project ReserGo.WebApi
     ```
   - L'API sera accessible sur `https://localhost:5001` (par défaut).
<!--
5. **Tests** :
   - Les tests unitaires sont dans le dossier `ReserGo.WebApi.Tests`.
   - Pour lancer les tests :
     ```bash
     dotnet test
     ```
-->
## Points techniques
- **Entity Framework Core** pour l'accès aux données.
- **SignalR** pour les notifications temps réel.
- **Swagger** pour la documentation de l'API (générée automatiquement).
- **Séparation stricte** des responsabilités (API, métier, accès données).

## Contribution
- Forkez le projet, créez une branche, proposez vos Pull Requests.
- Respectez la structure et les conventions du projet.

## Auteurs
- Projet développé par l'équipe ReserGo.
- Contributeurs :
    - [Sedar](https://github.com/sedar007)
    - [Lala Britta](https://github.com/laurrnci22)
---

Pour toute question ou contribution, ouvrez une issue sur le dépôt ou contactez l'équipe.

