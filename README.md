# Resetting Migrations and Database (for breaking changes)

If you need to reset the migration history and database (for example, after renaming columns or making breaking changes in your models), follow these steps:

1. **Delete all migration files:**
   Delete all files in the folder:
   ```
   src/Ambev.DeveloperEvaluation.ORM/Migrations/
   ```

2. **Drop all tables and reset the schema:**
   Run this command to drop all tables and recreate the schema (this will erase all data!):
   ```powershell
   docker exec -i ambev_developer_evaluation_database psql -U developer -d developer_evaluation -c "DROP SCHEMA public CASCADE; CREATE SCHEMA public;"
   ```

3. **Generate a new initial migration:**
   ```powershell
   dotnet ef migrations add InitialCreate --project ./src/Ambev.DeveloperEvaluation.ORM/ --startup-project ./src/Ambev.DeveloperEvaluation.WebApi/
   ```

4. **Apply the migration to the database:**
   ```powershell
   dotnet ef database update --project ./src/Ambev.DeveloperEvaluation.ORM/ --startup-project ./src/Ambev.DeveloperEvaluation.WebApi/ --connection "Host=localhost;Port=<mapped_port>;Database=developer_evaluation;Username=developer;Password=ev@luAt10n"
   ```
   Replace `<mapped_port>` with the port mapped to Postgres as shown in `docker ps` or `docker-compose ps` (e.g., 60277).

**When to use this:**
- If you changed property/column names in your models and did a global replace (e.g., OrderId → SaleNumber),
- If you get migration errors about missing columns or tables,
- If you want a clean start for your database and migrations.

**Warning:** This process will erase all data in the database. Only use it in development environments!
# Running Migration Locally (using mapped port)

If you need to apply migrations directly from your host (for example, when the Docker tools container cannot resolve the database service name), use the following command with the mapped port:

```powershell
dotnet ef database update --project ./src/Ambev.DeveloperEvaluation.ORM/ --startup-project ./src/Ambev.DeveloperEvaluation.WebApi/ --connection "Host=localhost;Port=60277;Database=developer_evaluation;Username=developer;Password=ev@luAt10n"
```

Replace `60277` with the port mapped to Postgres as shown in `docker ps` or `docker-compose ps`.

**Description:**
This command forces the connection string to use your local machine's mapped port for the Postgres container, ensuring migrations are applied even if the Docker network name resolution fails. Use this if you can connect to the database with DBeaver or another tool using `localhost:<mapped_port>`, but migrations via Docker are not working.
# Note on Connection Strings for Migrations

When running migrations via Docker (tools container), use this connection string in your appsettings or as an environment variable:

```
Host=ambev.developerevaluation.database;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n
```

When running locally (Visual Studio/IIS), use:

```
Host=localhost;Port=<mapped_port>;Database=developer_evaluation;Username=developer;Password=ev@luAt10n
```

Replace `<mapped_port>` with the port shown in `docker-compose ps`.
> **Note:**
> The HTTPS port for the WebApi container is dynamically assigned each time you run Docker Compose, unless you set a fixed port in your docker-compose.yml. Always check the current port with:
>
> ```
> docker-compose --project-name ambevdev ps
> ```
>
> Then access the API in Postman or your browser using the format:
>
> `https://127.0.0.1:<WebApiHttpsPort>/`
>
> For example, if the mapped port is 62770, use:
>
> `https://127.0.0.1:62770/`

# Quick Start: Running, Migrating, and Testing the API

Follow these steps to run the project with Docker, apply migrations, and test the API:

1. **Start all containers (WebApi, database, etc):**
   ```powershell
   docker-compose --project-name ambevdev up -d --build
   ```

2. **Apply database migrations:**
   ```powershell
   docker-compose --project-name ambevdev run --rm tools bash -c 'dotnet tool install --global dotnet-ef --version 8.* && export PATH="$PATH:/root/.dotnet/tools" && dotnet ef database update --project ./src/Ambev.DeveloperEvaluation.ORM/ --startup-project ./src/Ambev.DeveloperEvaluation.WebApi/'
   ```

3. **Test the API:**
   - **Swagger:**
     - Open your browser and go to: `http://localhost:<WebApiPort>/swagger` (replace `<WebApiPort>` with the mapped port, e.g., 55105)
     - Use the Swagger UI to explore and test endpoints interactively.
   - **Postman:**
     - Make requests to: `http://localhost:<WebApiPort>/api/<YourEndpoint>` (e.g., `http://localhost:55105/api/Sales`)
     - For HTTPS, use the mapped HTTPS port (e.g., 55106): `https://localhost:55106/api/Sales`
     - If you get a redirect (307), use the HTTPS URL and accept the self-signed certificate if prompted.

4. **Check logs (optional):**
   ```powershell
   docker logs ambev_developer_evaluation_webapi
   ```

5. **Stop all containers:**
   ```powershell
   docker-compose --project-name ambevdev down
   ```

This workflow ensures your environment is up, the database is migrated, and you can test the API via Swagger or Postman.
## Connecting to the Database from Visual Studio (Localhost)

When running the WebApi locally (for example, using Visual Studio with IIS Express), you must use `localhost` and the mapped port for the database in your connection string.

**Important (for Visual Studio/IIS Express):**

1. Run the following command to check which port Postgres is mapped to on your host:

   ```
   docker-compose --project-name ambevdev ps
   ```

2. Use the command below to list all running containers and their mapped ports:SELECT * FROM "Sales";

   ```
   docker-compose --project-name ambevdev ps
   ```

   Find the line with `ambev_developer_evaluation_database` and note the port before `->5432/tcp` (for example, `54387`).

3. Edit your `appsettings.Development.json` file and update the `DefaultConnection` string to use this port:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=54387;Database=developer_evaluation;Username=developer;Password=ev@luAt10n"
   }
   ```
   (Replace `54387` with the actual port from step 2.)

This step is required so your local WebApi (when running via Visual Studio/IIS Express) can connect to the Postgres database running in Docker. If you change the mapped port, always update the configuration accordingly.
# Recommended Development Workflow with Visual Studio and Docker

To develop and test in Visual Studio using Docker for the database (without table errors due to missing migrations), follow this workflow:

1. **Start Docker containers (including the database):**
   ```powershell
   docker-compose --project-name ambevdev up -d --build
   ```

2. **Apply database migrations using the tools service:**
   ```powershell
   docker-compose --project-name ambevdev run --rm tools bash -c 'dotnet tool install --global dotnet-ef --version 8.* && export PATH="$PATH:/root/.dotnet/tools" && dotnet ef database update --project ./src/Ambev.DeveloperEvaluation.ORM/ --startup-project ./src/Ambev.DeveloperEvaluation.WebApi/'
   ```

3. **Keep the database container running at all times.**

4. **In Visual Studio, configure your WebApi connection string** to point to the Docker database container (host: `database`, port: the one exposed in docker-compose).

5. **Run and debug the WebApi project in Visual Studio as usual.** You will not get table errors, since the migration has already been applied.

6. **If you change the model or need a new migration,** generate the migration and repeat step 2.

This workflow ensures you can develop and test in Visual Studio, using the persistent Docker database, without losing data or encountering missing table errors.
## How to run Visual Studio locally using Docker database (without losing volumes)

If you want to debug or run the WebApi project in Visual Studio, but keep your database and other services running in Docker (without losing data), follow these steps:

1. **Stop only the WebApi container (keep the database running):**
   ```powershell
   docker-compose stop ambev.developerevaluation.webapi
   ```
2. **Keep the database and other containers running.**

3. **Configure your connection string in Visual Studio** to point to the Docker database container. Use the hostname of the service (e.g., `database`) and the mapped port (check with `docker ps`). Example:
   - Host: `localhost` or the Docker network IP
   - Port: (the port mapped to Postgres, e.g., `65485`)
   - User/Password: as defined in your `docker-compose.yml`

4. **Run the WebApi project in Visual Studio** (F5 or Debug). Your API will run locally, but will use the database in Docker, keeping all data persistent.

**Note:**
- Do not use `docker-compose down --volumes` unless you want to delete all data.
- You can restart the WebApi container anytime with:
  ```powershell
  docker-compose up -d ambev.developerevaluation.webapi
  ```
## Stopping all containers

To stop all containers started by docker-compose, run:

```
docker-compose --project-name ambevdev down
```

This will stop and remove all containers, networks, and by default, keep the volumes (database data will be preserved unless you add the `-v` flag).
## Checking if migrations were applied successfully

After running the migration command, you can check if the tables were created correctly by executing:

```
docker exec -it ambev_developer_evaluation_database psql -U developer -d developer_evaluation -c '\dt'
```

The result should list the created tables, such as `Sales`, `SaleItem`, `Users`, etc.