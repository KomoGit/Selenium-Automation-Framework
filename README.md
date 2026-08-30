# 🚀 Selenium Automation Framework (.NET 10 / C#)

An enterprise-grade, high-performance UI & BDD automation framework built on **.NET 10**, **Selenium 4**, **Reqnroll**, **NUnit**, and **Allure Reporting**. 

Designed for parallel execution, cross-browser grid testing (Chrome, Firefox, Safari), dynamic test data generation via Bogus, and automated CI/CD reporting on GitHub Pages.

📊 **Live Allure Test Report:** [https://komogit.github.io/Selenium-Automation-Framework/](https://komogit.github.io/Selenium-Automation-Framework/)

---

## 📐 Project Architecture & Directory Structure

The framework is structured using a clean, layered architecture separating core infrastructure, business domain Page Object Models (POMs), and test scenarios:

```
SeleniumWebFramework/
├── .github/
│   └── workflows/
│       └── ci.yml                      # GitHub Actions CI/CD Pipeline (Build, Test, Allure Report to GitHub Pages)
├── docker-compose.yml                  # Local Selenium Grid 4 Hub & Node composition (Chrome & Firefox)
├── SeleniumWebFramework.sln
│
├── SeleniumWebFramework.Core/          # Framework Core Infrastructure & Utilities
│   ├── Base/
│   │   ├── BaseTest.cs                 # Base class for standard NUnit test fixtures
│   │   └── UITestHooks.cs              # Reqnroll BDD lifecycle hooks ([BeforeScenario], [AfterScenario])
│   ├── Constants/                      # Path and framework constant helpers
│   ├── Drivers/
│   │   └── DriverManager.cs            # Thread-safe AsyncLocal<IWebDriver> context & driver factory
│   ├── Models/                         # Configuration and schema models
│   │   ├── ConfigurationModel.cs
│   │   ├── DriverConfigurationOptions.cs
│   │   └── GridConfigurationOptions.cs
│   └── Utilities/                      # Framework engine utilities
│       ├── ConfigurationLoader.cs      # Loads appsettings.json and handles ENV variable overrides
│       ├── JsonUtils.cs
│       ├── PathUtils.cs                # AppDomain and project root path helper
│       └── TestDataGenerator.cs        # Thread-safe Bogus factory for dynamic e-commerce test data
│
├── SeleniumWebFramework.Business/      # Business Domain & Page Object Models (POMs)
│   └── POMs/
│       ├── BasePage.cs                 # Base POM with explicit retry loops and locator wrappers
│       ├── HomePage.cs                 # Home page POM
│       ├── ContactPage.cs              # Contact Us page POM
│       └── Components/                 # Modular, re-usable UI component POMs
│           ├── FilterSideBarComponent.cs
│           └── ProductCardComponent.cs
│
└── SeleniumWebFramework.Tests/         # Test Suites & BDD Specifications
    ├── appsettings.json                # Execution configuration (Browser, Headless, Grid, BaseUrl)
    ├── allureConfig.json               # Allure reporting configuration
    ├── reqnroll.json                   # Reqnroll BDD engine configuration
    ├── NUnitConfig.cs                  # Assembly-level NUnit parallel execution settings
    ├── Features/                       # Gherkin Feature files (.feature)
    │   ├── Contact.feature
    │   └── Sidebar.feature
    └── StepDefinitions/                # Reqnroll Binding Step Definitions
        ├── ContactStepDefinitions.cs
        ├── HomeStepDefinitions.cs
        ├── ProductStepDefinitions.cs
        ├── SidebarStepDefinitions.cs
        └── TabStepDefinitions.cs
```

---

## 🛠️ Used Libraries & Tech Stack

| Technology / Package | Version | Purpose |
| :--- | :--- | :--- |
| **.NET SDK** | `net10.0` | Target C# framework runtime |
| **Selenium.WebDriver** | `v4.46.0` | Browser automation engine & DevTools Protocol (CDP) |
| **Reqnroll / Reqnroll.NUnit** | `v3.3.4` | Open-source Gherkin BDD framework for .NET |
| **NUnit** | `v4.3.2` | Primary test runner framework |
| **Allure.NUnit / Allure.Reqnroll** | `v2.15.0` | HTML test reporting engine |
| **Bogus** | `v35.6.5` | Dynamic fake data generator (Names, Addresses, Credit Cards, Emails) |
| **Azure.Security.KeyVault.Secrets** | `v4.11.0` | Secrets management integration |
| **Newtonsoft.Json** | `v13.0.4` | JSON serialization helper |
| **dotenv.net** | `v4.0.2` | Environment file configuration loader |
| **Docker / Docker Compose** | `Selenium 4` | Local & grid containerization engine |
| **GitHub Actions** | `v4` | Automated CI/CD pipeline & GitHub Pages deployment |

---

## 🔥 Key Capabilities & Features

### 1. 🧵 Thread-Safe `AsyncLocal<IWebDriver>` Context
- Built using `AsyncLocal<IWebDriver?>` to ensure complete driver isolation across asynchronous continuations, parallel NUnit fixtures, and Reqnroll scenarios.

### 2. ⚡ Parallel Execution & Thread Control
- Supports parallel scenario execution configured via `appsettings.json` or `MAX_PARALLEL_THREADS` environment variables to optimize test execution speed.

### 3. 🌐 Selenium Grid 4 & Docker Support
- Supports execution against both **local browsers** and **Selenium Grid Hubs** (`RemoteWebDriver`). Includes a pre-configured `docker-compose.yml` for Chrome and Firefox nodes.

### 4. 🎲 Dynamic Test Data Generator (`TestDataGenerator`)
- Integrated with **Bogus** to create realistic, localized fake data models for e-commerce testing:
  - `CustomerData` (First/Last name, Email, Password, Phone)
  - `AddressData` (Street, City, State, Country, Postal Code)
  - `PaymentData` (Credit card numbers, CVV, Expiry dates)
  - `ContactFormData` & `ProductData`

---

## 🚀 Getting Started & Execution Guide

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Google Chrome](https://www.google.com/chrome/) or [Mozilla Firefox](https://www.mozilla.org/firefox/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) *(optional, for grid testing)*
- [Allure CLI](https://allurereport.org/) *(optional, for local HTML report generation)*

---

### 🏃 Running Tests

#### 1. Standard Local Execution
Run all tests using the default settings in `appsettings.json`:
```bash
dotnet test
```

#### 2. Running Specific Features or Tags
Run tests filtered by Reqnroll `@ui` tag:
```bash
dotnet test --filter "Category=ui"
```

#### 3. Environment Variable Overrides
Override browser, execution mode, or thread counts without modifying files:
```bash
# Run on Firefox
BROWSER=firefox dotnet test

# Run on Headless Chrome with 4 parallel NUnit worker threads
BROWSER=chrome IS_HEADLESS=true dotnet test -- NUnit.NumberOfTestWorkers=4
```

---

### 🐳 Running with Selenium Grid 4 (Docker)

1. Start the Selenium Grid Hub and Nodes:
```bash
docker compose up -d
```

2. Verify Grid Status:
Open `http://localhost:4444/ui` in your browser.

3. Execute tests against the Grid:
```bash
EXECUTION_MODE=grid BROWSER=chrome dotnet test
EXECUTION_MODE=grid BROWSER=firefox dotnet test
```

4. Stop the Grid:
```bash
docker compose down
```

---

### 📊 Local Allure Report Generation

Generate and view local Allure reports:
```bash
allure generate SeleniumWebFramework.Tests/bin/Debug/net10.0/allure-results --clean -o allure-report
allure open allure-report
```

---

## ⚙️ CI/CD & GitHub Pages Integration

The workflow in `.github/workflows/ci.yml` performs the following steps automatically on every `push` and `pull_request`:

1. Restores dependencies & compiles the solution in `Release` mode.
2. Runs the test suite headlessly.
3. Consolidates `allure-results` test execution metadata.
4. Generates the HTML report using official Allure CLI binaries.
5. Deploys the interactive Allure HTML report directly to **GitHub Pages** (`actions/deploy-pages@v4`).