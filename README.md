# BackendConsultorioSeguros

## Proyecto .NET Consultorio Seguros
Este es el backend del proyecto Consultorio Seguros, desarrollado en ASP.NET Core 6.0.

## Instrucciones de Clonación y Ejecución
Siga estos pasos para clonar y ejecutar el proyecto en su máquina local.

## Prerrequisitos
Asegúrese de tener instalados en su sistema:

- .NET SDK (v6.0.0)

## Pasos
1. Clonar el Repositorio:
```
git clone https://github.com/joguisa/backendconsultorioseguros.git
```

2. Crear la base de datos y los respecticos procedimientos que están separados por archivos.
3. **Los scripts están en la carpeta DB-SP**
  
4. Migrar la Base de Datos:

En el directorio del proyecto, ejecute el siguiente comando para aplicar las migraciones y crear la base de datos:
```
dotnet ef database update
```
4. Ejecutar la aplicación

## Dependencias
### Paquetes NuGet
- AutoMapper: 12.0.1
- AutoMapper.Extensions.Microsoft.DependencyInjection: 12.0.1
- ClosedXML: 0.102.1
- Microsoft.EntityFrameworkCore: 6.0.0
- Microsoft.EntityFrameworkCore.Design: 6.0.0
- Microsoft.EntityFrameworkCore.SqlServer: 6.0.0
- Microsoft.EntityFrameworkCore.Tools: 6.0.0
- Swashbuckle.AspNetCore: 6.5.0
