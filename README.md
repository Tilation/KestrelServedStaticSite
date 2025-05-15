# ✨ Kestrel Served Static Site
Permite desarrollar una api en Asp Net Core y generar los servicios en TypeScript para consumir desde una aplicacion web estatica, diseñado para Angular.
Separando lo que es backend de lo frontend, pero manteniendo la infraestructura de ambos proyectos sincronizada.
Diseñado enteramente desde cero y con una mentalidad clara, aprovechar el rendimiento de dotnet y la versatilidad de typescript/javascript.

## ⁉️ ¿Cómo funciona?
Se usa [Kestrel](https://learn.microsoft.com/es-es/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-8.0) como columna principal del proyecto, que nos permite crear nuestra API usando [Controllers](https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0) y tambien servir los archivos estaticos de nuestra pagina web hecha con Angular 19 CSR.
Sobre eso, [NSwag](https://github.com/RicoSuter/NSwag) nos ayuda y genera automaticamente servicios typescript para poder consumir desde nuestra aplicacion Angular. También genera las entidades de transporte de datos.
Esto permite que ambos proyectos estén sincronizados automaticamente y los cambios hechos en la API siempre estén en nuestra aplicacion Angular.

## 🚀 Roadmap

-[ ] Personalizar el perfil debug para que también ejecute "ng serve" y que Kestrel rutee a ese puerto para permitir hot reload del codigo de Angular
-[x] Usar NSwag para generar los servicios TypeScript que consumirán la API
-[ ] Crear un servicio configurable para la autenticacion y autorizacion Angular-C#
  -[ ] Hacer que si se usa seguridad, se puedan refrescar automaticamente los tokens
  -[ ] Permitir persistir en cookies o storage un token para no tener que inicar sesion cada vez.
	
