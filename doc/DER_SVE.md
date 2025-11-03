```mermaid
erDiagram
    Usuario {
        int IdUsuario PK
        string Username
        string Email
        string Password
        int Rol
        string RefreshToken
        datetime RefreshTokenExpiracion
    }

    Cliente {
        int IdCliente PK
        string DNI
        string Nombre
        string Telefono
        int IdUsuario FK
    }

    Evento {
        int IdEvento PK
        string Nombre
        string Descripcion
        datetime FechaInicio
        datetime FechaFin
        int Estado
    }

    Local {
        int IdLocal PK
        string Nombre
        string Direccion
        int CapacidadTotal
    }

    Sector {
        int IdSector PK
        string Nombre
        int Capacidad
        int IdLocal FK
    }

    Funcion {
        int IdFuncion PK
        int IdEvento FK
        int IdLocal FK
        datetime FechaHora
        int Estado
    }

    Tarifa {
        int IdTarifa PK
        int IdFuncion FK
        int IdSector FK
        decimal Precio
        int Stock
        int Estado
    }

    Orden {
        int IdOrden PK
        int IdTarifa FK
        int IdCliente FK
        decimal Total
        datetime Fecha
        int Estado
    }

    Entrada {
        int IdEntrada PK
        decimal Precio
        int Estado
        int IdOrden FK
        int IdTarifa FK
        int IdCliente FK
        int IdFuncion FK
    }

    %% Relaciones
    Usuario ||--o{ Cliente : "tiene"
    Cliente ||--o{ Orden : "realiza"
    Cliente ||--o{ Entrada : "posee"
    Evento ||--o{ Funcion : "incluye"
    Local ||--o{ Funcion : "se realiza en"
    Local ||--o{ Sector : "contiene"
    Funcion ||--o{ Tarifa : "define"
    Sector ||--o{ Tarifa : "asigna"
    Tarifa ||--o{ Orden : "vende"
    Tarifa ||--o{ Entrada : "genera"
    Funcion ||--o{ Entrada : "asocia"
