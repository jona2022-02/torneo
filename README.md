# Mundialito

Sistema para operar un torneo de fútbol de un solo grupo: equipos, jugadores, calendario, resultados, tabla de posiciones y goleadores.

## Cómo correrlo

Hace falta Docker.

```bash
docker compose up --build
```

- Frontend: http://localhost:3000
- API: http://localhost:8080
- Swagger: http://localhost:8080/swagger

La API espera a SQL Server, aplica las migraciones y, si la base está vacía, carga la semilla: 4 equipos, 5 jugadores por equipo, 6 partidos y 3 con resultado.

La colección de Postman está en `postman/`. El entorno `Local` apunta a `http://localhost:8080`. Los POST envían `Idempotency-Key`.

## Decisiones

- Clean Architecture: Domain, Application, Infrastructure y Api. Domain no referencia a nadie. Application solo a Domain.
- CQRS sin MediatR. Los comandos escriben con EF Core y Unit of Work. Las lecturas usan Dapper, con paginación en SQL.
- Los errores de negocio viajan como `Result`, no como excepciones. La API los traduce a 200, 201, 400, 404 o 409.
- Todo POST exige `Idempotency-Key`. La misma clave y el mismo cuerpo repiten la respuesta guardada. La misma clave con otro cuerpo responde 409.
- Cada request lleva `X-Correlation-Id`. Si no viene, la API lo genera y lo escribe en el log.
- La tabla ordena por puntos, diferencia de gol y goles a favor. Solo cuentan los partidos finalizados.
- El frontend es Next.js y habla con la API por HTTP.

El diagrama está en [docs/arquitectura.md](docs/arquitectura.md).

## Fuera de alcance

- Broker de mensajería externo. Los eventos de dominio se registran en el log.
- OpenTelemetry y Kubernetes.
