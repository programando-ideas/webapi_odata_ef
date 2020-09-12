# OData + ASPNET Core WebApi 3.1 + Entity Framework Core
**OData** aplicado a **ASP.NET Core WebApi 3.1** utilizando **Entity Framework Core**. 📇 📋 📂

## Video Youtube
[📚 𝖮𝖣𝖺𝗍𝖺 + 𝖤𝗇𝗍𝗂𝗍𝗒 𝖥𝗋𝖺𝗆𝖾𝗐𝗈𝗋𝗄 + 𝖠𝖲𝖯.𝖭𝖤𝖳 𝖢𝗈𝗋𝖾 𝖶𝖾𝖻𝖠𝗉𝗂 𝟥.𝟣 📝🔎](https://youtu.be/xsCiXMzAVQ8)

## Herramientas necesarias ejecutar y probar este proyecto
- [x] SQL Server Developer Edition
- [x] SQL Server Management Studio
- [x] Visual Studio 2019 Community
- [x] .net Core 3.1
- [x] PostMan

## Referencias de importancia
 - Extension para Visual Studio
    - https://marketplace.visualstudio.com/items?itemName=unchase.UnchaseODataConnectedService
    - https://devblogs.microsoft.com/odata/tutorial-sample-how-to-use-odata-client-code-generator-to-generate-client-side-proxy-class/
 - Cliente OData con HttpClient: https://docs.microsoft.com/es-es/odata/client/using-httpclient
 
## Configuraciones del proyecto
### ClienteOData
Cliente de ejemplo utilizando HttpClient
### ServerOData
WebApi con OData

#### Agregar al archivo del proyecto webapi_odata_ef.csproj
```xml
<GenerateRuntimeConfigurationFiles>True</GenerateRuntimeConfigurationFiles>
```
#### Paquetes de Nuget
- [x] Microsoft.AspNetCore.OData
- [x] Microsoft.EntityFrameworkCore
- [x] Microsoft.EntityFrameworkCore.SqlServer
- [x] Microsoft.EntityFrameworkCore.Design
- [x] Microsoft.VisualStudio.Web.CodeGeneration.Design

## Entity Framework Core (code-first)

```bash
# Para instalar la herramienta dotnet-ef
dotnet tool install --global dotnet-ef

# Para generar la base de datos
dotnet-ef migrations add Initial
dotnet-ef database update
```

## Consultas para realizar con OData
#### GET
- http://localhost:5000/odata/personas?$select=NombreYApellido
- http://localhost:5000/odata/personas?$filter=Id eq 1
- http://localhost:5000/odata/personas?$expand=Telefono,Direccion
- http://localhost:5000/odata/personas?$expand=telefono,direccion&$filter=Id eq 1
- http://localhost:5000/api/personasapi?$expand=telefono,direccion&$filter=Id eq 2

#### POST
- http://localhost:5000/odata/personas%281%29/CrearPersona
```json
{
  "NombreYApellido": "Luciano",
  "Edad": 23,
  "Telefonos": [
  {
    "TelefonoDesc": "+528885555444"
  },
  {
    "TelefonoDesc": "+551234567890"
  }],
  "Direcciones": [
  {
    "DireccionDesc": "Calle 23"
  }]
}
```
- http://localhost:5000/odata/Personas
```markdown
NombreYApellido: Nueva Persona
Edad: 23
Telefono[0].TelefonoDesc: +545555444477
Telefono[1].TelefonoDesc: +559879879879
Direccion[0].DireccionDesc: Avenida 293 #445
```

## Contulta para verificar las personas en SQL Server
```sql
SELECT per.Id, per.NombreYApellido, per.Edad, 
  Telefonos =  STUFF((SELECT ' || ' + tel.TelefonoDesc
                      FROM Telefonos tel
                      WHERE tel.IdPersona = per.id
                      FOR XML PATH('')), 1, 2, ''),
  Direcciones = STUFF((SELECT ' || ' + dir.DireccionDesc
                       FROM Direcciones dir
                       WHERE dir.IdPersona = per.id
                       FOR XML PATH('')), 1, 2, '')
FROM Personas per
```
## Seguridad
[Documento de Microsoft](https://docs.microsoft.com/en-us/odata/webapi/odata-security)

## Links del video
- Ejemplos OData: https://github.com/OData/ODataSamples
- URL OData: http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html#sec_URLComponents



------------
#### Programando Ideas 2020
<p>
  <a href="https://paypal.me/lp8126" target="_blank">
    <img src="https://www.paypalobjects.com/es_XC/MX/i/btn/btn_donateCC_LG.gif" border="0" alt="Donar con PayPal" />
  </a>
</p>
