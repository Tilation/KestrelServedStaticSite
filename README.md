# ✨ Kestrel Served Static Site
![Badge](https://img.shields.io/badge/Project_status-working_on_dev_mode-orange)

Allows you to develop an API in ASP.NET Core and generate TypeScript services to be consumed from a static web application, designed for Angular (but you can use whatever you like!).
It separates the backend from the frontend, while keeping both projects' infrastructure in sync.
Entirely designed from scratch with a clear mindset: take advantage of .NET’s performance and reliability along with the flexibility and rapid iteration of TypeScript/JavaScript development.

## ⁉️ How does it work?
[Kestrel](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-8.0) is used as the backbone of the project, allowing us to create our API using [Controllers](https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0) and also serve the static files of our web page.
On top of that, [NSwag](https://github.com/RicoSuter/NSwag) helps by automatically generating TypeScript services to consume the API from our static web application. It also generates the data transport entities.
This allows both projects to remain automatically synchronized, and changes made in the API are always reflected in our Angular application.

## 🚀 Personal Roadmap
- [ ] Customize the debug profile to also run "ng serve" and route requests from Kestrel to that port, enabling hot reload for Angular code
- [x] Use NSwag to generate TypeScript services that will consume the API
- [ ] Create a configurable authentication and authorization service for Angular-C#
  - [ ] Ensure that if security is enabled, tokens can be automatically refreshed
  - [ ] Allow persisting a token in cookies or storage to avoid logging in every time
- [ ] Compatibility so that the CSR site can be a GitHub submodule
- [ ] Create a script to easily set up the project
  - [ ] Allow adding a GitHub submodule as the CSR site
- [ ] Docker support

## 👌 Ideal workflow
- Clone the repository
- Run the configuration script to customize the template (doesn’t exist yet 🤭)
  - (Optional) Make the static web page a GitHub submodule in case you want to decouple it from Kestrel in the future
- Start developing!
  - Create or modify controllers
  - Generate code with NSwag
  - Use the services from the web page
  - Keep on developing!

## 🧱 Production-Ready Roadmap

### 🔧 Infrastructure & DevOps
- [ ] Add Dockerfile and docker-compose for dev & prod
- [ ] Configure CI/CD pipeline (e.g., GitHub Actions, GitLab CI)
- [ ] Environment-based configuration support (dev/staging/prod)
- [ ] Enable static Angular build output in publish profile
- [ ] Add secrets management via environment variables or vault

### 🔐 Security & Auth
- [ ] Implement JWT-based authentication and Angular guards
- [ ] Add role-based and policy-based authorization
- [ ] Secure cookies or token storage (HttpOnly, Secure)
- [ ] Enable CSRF, XSS, and input validation protections
- [ ] Configure security headers (CSP, HSTS, etc.)
- [ ] Add audit logging for sensitive actions

### 🧠 Observability & Reliability
- [ ] Add centralized logging (e.g., Serilog, Seq, or Elastic)
- [ ] Add ASP.NET Health Checks (`/healthz`)
- [ ] Add frontend error logging/interception
- [ ] Add Prometheus or OpenTelemetry metrics support
- [ ] Enable readiness/liveness probes for Docker/Kubernetes

### 🌐 Frontend UX & Internationalization
- [ ] Add global Angular HTTP interceptor for error & token handling
- [ ] Add offline/timeout fallback display for API downtime
- [ ] Add i18n support using Angular i18n or ngx-translate

### 🧪 Testing & Quality
- [ ] Add unit and integration tests for API and frontend
- [ ] Add end-to-end tests (Cypress or Playwright)
- [ ] Enforce code linting and format rules
- [ ] Add test coverage tracking

### 📚 DX and Documentation
- [ ] Create setup script or CLI for scaffolding new projects
- [ ] Create docs site or markdown-based documentation
- [ ] Include demo/test accounts for local development
