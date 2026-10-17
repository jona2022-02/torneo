# Mundialito

Sistema para operar un torneo de fútbol de un solo grupo: equipos, jugadores, calendario, resultados, tabla de posiciones y goleadores.

## Cómo correrlo

Hace falta Docker y el SDK de .NET 10.

Todo junto:

```bash
docker compose up --build
```

- Frontend: http://localhost:3000
- API: http://localhost:8080
- Swagger: http://localhost:8080/swagger
- Usuario: `admin`
- Contraseña: `TorneoDev_2026!`

La API espera a SQL Server, aplica las migraciones y, si la base está vacía, carga la data: Real Madrid, Barcelona, Atlético de Madrid y Bayern Múnich, con seis partidos y tres resultados.

Para trabajar en local, deja solo la base en Docker y corre API y frontend en la máquina:

```bash
docker compose up db
dotnet run --project backend/src/Torneo.Api/Torneo.Api.csproj --launch-profile http
```

```bash
cd frontend
npm ci
npm run dev
```

El frontend lee `NEXT_PUBLIC_API_URL`. En local el valor es `http://localhost:8080` (`frontend/.env.local`).

La colección de Postman está en `postman/`. El entorno `Local` apunta a `http://localhost:8080`. El primer request es el login y guarda el token.

## Decisiones

- Clean Architecture: Domain, Application, Infrastructure y Api. Domain no referencia a nadie. Application solo a Domain.
- CQRS sin MediatR. Los comandos escriben con EF Core y Unit of Work. Las lecturas usan Dapper, con paginación en SQL.
- Los errores de negocio viajan como `Result`, no como excepciones. La API los traduce a 400, 401, 404 o 409.
- Todo POST exige `Idempotency-Key`. La misma clave y el mismo cuerpo repiten la respuesta guardada. La misma clave con otro cuerpo responde 409.
- Cada request lleva `X-Correlation-Id`. Si no viene, la API lo genera y lo escribe en el log.
- La tabla ordena por puntos, diferencia de gol, goles a favor y nombre. Solo cuentan los partidos finalizados.
- El frontend es Next.js y habla con la API por HTTP. El inicio de sesión entrega un JWT.

El diagrama está en [docs/arquitectura.md](docs/arquitectura.md).


