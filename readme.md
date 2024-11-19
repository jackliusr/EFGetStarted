## Intro

This repository is created to get information how the linq expression are translated to sql by entity framework core. 

## Start database

```bash
docker run -d \
  -e POSTGRES_PASSWORD=PFG2cpm~sJ.WC0rGyqFKf0 \
  -e POSTGRES_HOST_AUTH_METHOD=scram-sha-256 \
  -e POSTGRES_HOST_AUTH_METHOD=trust \
  -e POSTGRES_DB=blog \
  -p 5432:5432 \
  -v TestFhirServerPostgres.AppHost-postgres-data:/var/lib/postgresql/data docker.io/library/postgres:16.4	
```

## Init database

```bash
dotnet ef database update
```