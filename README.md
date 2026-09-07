# Shan Distribution – Wholesale ERP System

> **Enterprise Resource Planning · Print-Optimised Invoicing · Real-Time Financials**
>
> Built with ASP.NET Core · Entity Framework Core · SQL Server · Bootstrap 5 · JavaScript

![.NET](https://img.shields.io/badge/.NET-8.0%20%2F%209.0-purple)
![C%23](https://img.shields.io/badge/C%23-12.0-blue)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC%20%2F%20Razor-indigo)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-red)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-purple)
![Currency](https://img.shields.io/badge/Standard-LKR%20(Sri%20Lanka)-emerald)
![Licence](https://img.shields.io/badge/Licence-MIT-yellow)

---

## Table of Contents

1. [Project Description](#1-project-description)
2. [System Architecture & Design](#2-system-architecture--design)
3. [Technologies Used](#3-technologies-used)
4. [Installation Instructions](#4-installation-instructions)
5. [Environment Variables & Configuration](#5-environment-variables--configuration)
6. [Usage Instructions](#6-usage-instructions)
7. [API & Controller Endpoints](#7-api--controller-endpoints)
8. [Project Structure](#8-project-structure)
9. [Features](#9-features)
10. [Contact Information](#10-contact-information)

---

## 1. Project Description

### Overview

**Shan Distribution** is an enterprise-grade wholesale distribution ERP web application engineered specifically for the regional supply of sweets, confectionery, and packaged retail fast-moving consumer goods (FMCG). The system replaces manual ledgers and static spreadsheets with a centralized, responsive web platform featuring real-time financial recalculations, atomic transaction handling, and print-ready corporate documentation.

### Problem Statement

Managing high-velocity wholesale confectionery trade across diverse retail accounts poses significant operational hurdles:

* **Manual Pricing Errors:** Negotiating variable bulk discounts and unit prices at the counter creates human calculation mistakes.
* **Inventory Drift:** Billed items failing to instantly synchronize with warehouse stocks leads to overselling.
* **Non-Standardized Paperwork:** Standard web receipts include browser headers, URLs, and cluttered layouts that fail formal commercial audit standards.
* **Rolling Credit Complexities:** Tracking partial settlements, upfront deposits, and multi-stage invoice balances across rolling credit terms creates debt recovery bottlenecks.

### Objectives

- **Interactive Invoicing:** Enable fast dynamic billing with inline price overrides, live inventory checks, and automatic item total recalculation.
- **Transactional Consistency:** Execute stock balance decrements, invoice records, line items, and payment logs within atomic database transactions.
- **Standardized Presentation:** Enforce clean Sri Lankan Rupee (`LKR`) formatting with two-decimal precision and build `@media print` letterhead templates.
- **Separation of Concerns:** Transition from controller-heavy routines to a service-oriented architecture (`ICustomerInvoiceService`) to ensure testability and safety.

---

## 2. System Architecture & Design

### Application Topology

The application follows a decoupled N-Tier MVC architecture utilizing ASP.NET Core Dependency Injection, separating presentation, business coordination, and relational data access.

```
+-------------------+       +-------------------+       +-------------------+
|     Frontend      |       |      Backend       |       |     Database      |
|  Razor Views      | ----> | ASP.NET Core MVC   | ----> | Microsoft         |
|  Bootstrap 5      | <---- | Controller +       | <---- | SQL Server        |
|  Vanilla JS (ES6) |       | Service Layer      |       | (MARS Enabled)    |
|  Port 5000 / 5001 |       | (C# / EF Core)      |       | ShanDistributionDb|
+-------------------+       +-------------------+       +-------------------+
```

### Core Components

**Backend (`ShanDistribution/`)**

| File / Module | Responsibility |
|---|---|
| `Program.cs` | Application entry point — DI registrations, Identity configs, DB contexts, middleware pipeline |
| `Data/ApplicationDbContext.cs` | Entity Framework Core context — DB sets, model relations, constraints |
| `Services/Implementations/CustomerInvoiceService.cs` | Service engine — atomic transactions, stock reductions, payment logs |
| `Controllers/CustomerInvoicesController.cs` | Endpoint routing — AJAX order payload ingestion, redirect handling |
| `Controllers/SupplierBillsController.cs` | Supplier billing — vendor delivery records, payment settlements |

**Frontend (`Views/`)**

| View File | Responsibility |
|---|---|
| `Views/CustomerInvoices/Create.cshtml` | Dynamic billing layout — datalist search, inline editable price/count, live totals |
| `Views/CustomerInvoices/Details.cshtml` | Printable invoice — corporate letterhead, 3-point audit sign-off, PDF export |
| `Views/Customers/Details.cshtml` | Customer statement — lifetime sales, settled totals, credit/debit balances |
| `Views/Products/Index.cshtml` | Inventory management — wholesale price sheet and stock monitoring |

### Data Flow

```
Browser (Client)            CustomerInvoicesController          CustomerInvoiceService             SQL Server
    │                                    │                                  │                            │
  1 ├── Select Products & Rates ─────────┼──────────────────────────────────┼────────────────────────────┤
    │   Dynamic recalculation (JS)       │                                  │                            │
    │                                    │                                  │                            │
  2 ├── Click "Submit Invoice" ──────────► POST /CustomerInvoices/Create    │                            │
    │   JSON Payload (DTO)               │  Pass DTO to Service ───────────►│                            │
    │                                    │                                  ├── Begin Transaction ──────►│
    │                                    │                                  │   Verify & Decrement Stock │
    │                                    │                                  │   Insert Invoice & Items ──►
    │                                    │                                  │   Commit Transaction ─────►│
    │                                    │                                  │                            │
  3 ├── Redirect /Details/:id ◄──────────┴─ Return Result & Redirect URL ───┤                            │
    │   Display Letterhead View          │                                  │                            │
    │                                    │                                  │                            │
  4 ├── Press "Print / Save PDF" ────────┼──────────────────────────────────┼────────────────────────────┤
    │   CSS @media print triggers        │                                  │                            │
    │   Hides UI, prints letterhead A4   │                                  │                            │
```

---

## 3. Technologies Used

| Category | Technology | Version | Purpose |
|---|---|---|---|
| Web Framework | ASP.NET Core | 8.0 / 9.0 | Server-side runtime and MVC architecture |
| Language | C# | 12.0 | Application backend logic, models, and DTO contracts |
| ORM | Entity Framework Core | 8.0+ | Code-First migrations, relational LINQ queries |
| Database | Microsoft SQL Server | 2019+ | Relational persistence with MARS support |
| Frontend Engine | Razor Views | Latest | Server-side rendered dynamic markup |
| Styling | Bootstrap | 5.3+ | Responsive layout system and UI elements |
| Iconography | Bootstrap Icons | Latest | Dashboard and button visual indicators |
| Client Scripting | JavaScript (ES6+) | Vanilla | DOM updates, client validation, real-time math |
| Print System | CSS3 `@media print` | W3C Standard | A4 pagination and UI chrome removal |

---

## 4. Installation Instructions

### Prerequisites

- .NET SDK 8.0 or higher
- Microsoft SQL Server (LocalDB, Express, or standard edition)
- Visual Studio 2022 / VS Code
- Git

### 1. Clone the Repository

```bash
git clone https://github.com/NimshanDilunika/ShanDistribution.git
cd ShanDistribution
```

### 2. Configure Local Database Connection

Open `appsettings.json` and set your local SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ShanDistributionDb;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;"
  }
}
```

### 3. Run Database Migrations

Apply the Entity Framework migrations to construct the database schema:

```bash
dotnet ef database update
```

### 4. Run the Application

```bash
dotnet run
```

Access the ERP dashboard in your browser:

- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

---

## 5. Environment Variables & Configuration

Configuration keys are managed within `appsettings.json` and `appsettings.Development.json`:

| Key | Description | Example |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string (requires MARS) | `Server=localhost;Database=ShanDistributionDb;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;` |
| `Logging:LogLevel:Default` | Core log output verbosity | `Information` |

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ShanDistributionDb;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

> ⚠️ **Do not** commit production database passwords or confidential keys to version control. Keep `appsettings.Development.json` in your `.gitignore`.

---

## 6. Usage Instructions

**Step 1 — Start the Application**

```bash
dotnet run
```

**Step 2 — Open the Dashboard**

Navigate to `https://localhost:5001` in your browser.

**Step 3 — Use the System**

| Action | How |
|---|---|
| Add Products | Navigate to Products → Create New → Set wholesale base price & opening stock |
| Create Invoice | Go to Customer Invoices → Create New → Select customer, products, and counts |
| Negotiate Price | Directly edit the numeric value in the Price (LKR) cell inside the invoice item row |
| Record Payment | Enter customer cash deposit in the Payment input box to calculate net balance due |
| Print / Save PDF | On the invoice details page, click Print Invoice or press Ctrl + P |
| Settle Supplier Bill | Navigate to Supplier Bills → Select Bill → Register payment disbursement |

---

## 7. API & Controller Endpoints

**Base URL:** `https://localhost:5001`

| Method | Endpoint | Description | Payload / Query |
|---|---|---|---|
| GET | `/CustomerInvoices` | List customer invoices | — |
| GET | `/CustomerInvoices/Create` | Render dynamic invoicing page | — |
| POST | `/CustomerInvoices/Create` | Submit new customer invoice (Atomic) | Body: `CustomerInvoiceCreateDto` (JSON) |
| GET | `/CustomerInvoices/Details/:id` | View printable letterhead invoice | Route Param: `id` |
| GET | `/Products` | Browse inventory and wholesale price list | — |
| POST | `/Products/Create` | Add a new confectionery item | Form Body: `Product` |
| GET | `/SupplierBills` | List supplier purchasing bills | — |
| POST | `/SupplierBills/AddPayment` | Record partial payment to supplier | Form Body: `billId`, `amount` |

**HTTP Status Codes Used**

| Code | Meaning |
|---|---|
| 200 | Success / OK |
| 302 | Redirect to details view |
| 400 | Invalid request / insufficient inventory error |
| 404 | Entity record not found |
| 500 | Internal server or database transaction failure |

---

## 8. Project Structure

```
ShanDistribution/
│
├── Controllers/
│   ├── CustomerInvoicesController.cs   # Invoicing workflows and JSON API endpoints
│   ├── CustomersController.cs          # Customer profile & ledger statements
│   ├── ProductsController.cs           # Product stock catalog and price list
│   ├── SupplierBillsController.cs      # Supplier purchases and payment recording
│   └── SuppliersController.cs          # Supplier statements and account tracking
│
├── Data/
│   └── ApplicationDbContext.cs         # Entity Framework database context
│
├── DTOs/
│   └── CustomerInvoiceCreateDto.cs     # Payload transfer models for AJAX submission
│
├── Models/
│   ├── Customer.cs                     # Customer profile entity
│   ├── CustomerInvoice.cs              # Parent sales invoice entity
│   ├── InvoiceProduct.cs               # Billed sales line item entity
│   ├── InvoicePayment.cs               # Customer payment transaction entity
│   ├── Product.cs                      # Inventory stock entity
│   ├── Supplier.cs                     # Supplier profile entity
│   ├── SupplierBill.cs                 # Supplier purchase bill entity
│   ├── BillProduct.cs                  # Purchased item entity
│   └── BillPayment.cs                  # Supplier disbursement entity
│
├── Services/
│   ├── Interfaces/
│   │   └── ICustomerInvoiceService.cs  # Invoicing service abstraction contract
│   └── Implementations/
│       └── CustomerInvoiceService.cs   # Atomic transaction logic and stock deductions
│
├── Views/
│   ├── CustomerInvoices/
│   │   ├── Create.cshtml               # Interactive billing line calculator
│   │   ├── Details.cshtml              # Print-ready customer invoice view
│   │   └── Index.cshtml                # Sales invoice ledger table
│   ├── Customers/
│   │   └── Details.cshtml              # Customer ledger and financial history
│   ├── Products/
│   │   └── Index.cshtml                # Inventory table and wholesale price sheet
│   └── SupplierBills/
│       ├── Details.cshtml              # Print-ready supplier delivery sheet
│       └── Index.cshtml                # Supplier bill ledger table
│
├── wwwroot/                            # Static CSS, JavaScript libraries, icons
│   ├── css/
│   └── js/
├── appsettings.json                    # Configuration, DB connection strings
├── appsettings.Development.json        # Development configuration
├── Program.cs                          # Application startup, DI services, pipeline
└── ShanDistribution.sln                # Visual Studio Solution file
```

---

## 9. Features

### Core ERP Features

- **Interactive Dynamic Billing:** Real-time calculation of line totals, trade discounts, and balance dues without full page reloads.
- **Editable Inline Pricing:** Allows negotiated wholesale rates directly inside table rows while tracking original unit pricing.
- **Atomic Stock Management:** Automatically decrements inventory quantities upon order confirmation and blocks transactions exceeding current stock.
- **Supplier Dispatch & Accounts Payable:** Tracks incoming consignments, trade discount deductions, and partial disbursement logs.
- **Standardized Currency Engine:** Native Sri Lankan Rupee (LKR) formatting across all views, tables, and exported ledgers.
- **Corporate `@media print` Layouts:** Eliminates web UI chrome to output standardized, letterhead-branded A4 invoices and 3-stage audit sign-offs.

### Data Model — Core Entities

| Entity Model | Primary Key | Key Attributes | Relationships |
|---|---|---|---|
| Customer | CustomerId | Name, Phone, Address | 1-to-Many with CustomerInvoice |
| Product | ProductId | ProductName, WholesalePrice, StockCount | 1-to-Many with InvoiceProduct |
| CustomerInvoice | CIId | InvoiceNo, Date, TotalAmount, PaidAmount, Status | Belongs to Customer, 1-to-Many with InvoiceProduct & InvoicePayment |
| InvoiceProduct | InvoiceProductId | Price, Count, TotalPrice | Belongs to CustomerInvoice & Product |
| SupplierBill | SBId | BillNo, Date, TotalAmount, BalanceAmount, Status | Belongs to Supplier, 1-to-Many with BillProduct & BillPayment |

---

## 10. Contact Information

| Name | Email | Institution / Role |
|---|---|---|
| T.P.D. Nimshan Tharamasinghe | nimshandilunika@gmail.com | University of Jaffna, Department of Computer Science |

## Licence

This project is licensed under the MIT Licence.

```
MIT License

Copyright (c) 2026 Shan Distribution

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
