## ✨ Integrantes

* Leon Flores, Celedonio

* Ramirez, Luján

* Gonzalez, Sofia.

## 📝 Descripcion General del Proyecto

El proyecto **SVE (Sistema de Venta de Entradas)** es una aplicación backend diseñada para gestionar eventos, funciones, clientes, órdenes de compra y generación de entradas con códigos QR. El sistema implementa una arquitectura por capas clara, utilizando **.NET**, **Dapper** y **PostgreSQL**.

El sistema permite:

* Registrar **clientes**, **usuarios**, **locales**, **eventos** y **funciones**.
* Gestionar **órdenes de compra**.
* Generar **entradas** con código QR y validación.
* Administrar sectores, tarifas y disponibilidad.
* Validar accesos mediante QR.

---

## 🧱 Arquitectura del Proyecto

El backend está organizado en 3 capas principales:

### 1. **SVE.Core (Dominio)**

Incluye:

* Modelos y entidades.
* DTOs y enums.
* Interfaces de repositorios y servicios.
* Reglas y contratos del dominio.

### 2. **SVE.Dapper (Acceso a Datos)**

* Implementación de repositorios.
* Consultas SQL en PostgreSQL mediante Dapper.
* Fábrica de conexiones basada en roles.

### 3. **SVE (API - Presentación)**

* Controladores.
* Servicios y validaciones.
* AutoMapper para mapeos.
* Exposición de endpoints REST.

---

## 🔄 Flujo General del Sistema

1. Cliente externo envía solicitud.
2. Controller la recibe y la envía al service.
3. El service procesa lógica de negocio.
4. Repositorios ejecutan SQL.
5. Los datos regresan hacia la API.
6. API devuelve un DTO ordenado.

---

## 🎟️ Módulos del Sistema

### ✔ Clientes

### ✔ Usuarios

### ✔ Eventos / Funciones / Locales

### ✔ Órdenes de Compra

### ✔ Entradas (QR + Validación)

---

## 🛠 Tecnologías

* .NET 8+
* Web API
* Dapper
* PostgreSQL
* FluentValidation
* AutoMapper
* JWT (opcional)

---

## 🚧 Estado del Proyecto

* Controladores completos
* Servicios implementados
* Repositorios funcionales
* Flujo de venta funcional

Pendiente:

* Frontend
---

## 📌 Estructura

```txt
SVE
├── Controllers
├── DTOs
├── Helpers
├── Requests
├── Profiles
└── Program.cs

SVE.Core
├── Models
├── DTOs
├── Enums
├── Interfaces
└── Exceptions

SVE.Dapper
├── Repositories
└── ConnectionFactory
```

---

## 🔐 Configuración de Conexión por Roles

```json
"Users": {
  "Administrador": "Server=localhost;Uid=administrador;Pwd=contraseniaNueva;Database=bd_boleteria;",
  "Cliente": "Server=localhost;Uid=cliente;Pwd=contraseniaNueva;Database=bd_boleteria;",
  "Organizador": "Server=localhost;Uid=organizador;Pwd=contraseniaNueva;Database=bd_boleteria;",
  "Default": "Server=localhost;Uid=default;Pwd=contraseniaNueva;Database=bd_boleteria;"
}
```

---

🌐 Acceso Externo con `0.0.0.0 : Es una dirección "comodín" utilizada en `launchSettings.json` para permitir que la aplicación escuche en todas las interfaces de red. Esto se emplea para que otros dispositivos dentro de la misma red local puedan acceder al Swagger o a la API.

Sin embargo, su funcionamiento depende completamente de la configuración del router y del firewall. Si estos bloquean puertos externos, el acceso no será posible aunque se utilice 0.0.0.0.

**Alternativa recomendada**
Usar la IP local:

```
http://10.120.x.x:5257
```

---


