# Shan Distribution – Wholesale ERP System

> **Enterprise Resource Planning · Print-Optimised · Real-Time Financials**
>
> Built with ASP.NET Core · Entity Framework Core · SQL Server · Bootstrap 5 · JavaScript

![.NET](https://img.shields.io/badge/.NET-8.0%20%2F%209.0-purple)
![C%23](https://img.shields.io/badge/C%23-12.0-blue)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC%20%2F%20Razor-indigo)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-red)
![Currency](https://img.shields.io/badge/Standard-LKR%20(Sri%20Lanka)-emerald)
![Licence](https://img.shields.io/badge/Licence-MIT-yellow)

---

## Table of Contents

1. [Project Overview](#1-project-overview)
2. [Key Features](#2-key-features)
3. [System Architecture & Design](#3-system-architecture--design)
4. [Technologies Used](#4-technologies-used)
5. [Installation & Setup](#5-installation--setup)
6. [Usage Guide](#6-usage-guide)
7. [Print & PDF Document Standards](#7-print--pdf-document-standards)
8. [Project Structure](#8-project-structure)
9. [Author](#9-author)
10. [Licence](#10-licence)

---

## 1. Project Overview

### Business Background
**Shan Distribution** is a wholesale distribution enterprise based in Sri Lanka specializing in the regional supply of sweets, confectionery, and packaged goods. The company manages high-velocity trade with hundreds of local retail outlets and major manufacturing suppliers.

### Problem Statement
Wholesale confectionery distribution demands strict credit tracking, real-time inventory adjustments, and rapid invoicing at varying wholesale price points. Managing operations via manual paper ledgers or rigid, legacy off-the-shelf software leads to:
* Human calculation errors during manual trade discount negotiations.
* Discrepancies between physical warehouse stock and billed customer orders.
* Cluttered, unstandardized receipts that fail Sri Lankan commercial presentation standards.
* Delays in tracking accounts receivable and payable across rolling credit terms.

### Solution & Objectives
This ERP system provides an end-to-end management pipeline designed specifically around wholesale operations:
* **Interactive Dynamic Invoicing:** Calculate line totals, custom tier discounts, upfront cash payments, and rolling balance dues on the fly.
* **Strict Currency Compliance:** Native Sri Lankan Rupee (`LKR`) formatting across all views, reports, and calculations.
* **Corporate PDF/Print Engine:** Specialized CSS `@media print` layouts that generate borderless, letterhead-branded invoices, statements, and wholesale price catalogs without browser headers or URLs.
* **Atomic Transactions:** Automated stock decrement and balance adjustments with database-level rollback protection.

---

## 2. Key Features

* **Dynamic Customer Billing:** Fast item selection via searchable datalists with live inventory balance validation, editable inline prices for negotiated wholesale rates, and percentage-based discounting.
* **Supplier Bill & Dispatch Management:** Itemized tracking of bulk vendor deliveries, gross payables, applied vendor deductions, and post-delivery partial settlement records.
* **Customer & Supplier Statements:** Consolidated financial profiles featuring lifetime billed amounts, cumulative settlements, and formatted outstanding debit/credit balances (`(LKR XX.XX)` for negative/credit balances).
* **Print-Optimized Letterhead Reports:** Clean A4 portrait reports designed for physical printout and digital PDF export with baseline-aligned company branding and 3-stage corporate sign-off blocks (*Prepared By*, *Accounts Dept*, *Authorized Signatory*).
* **Inventory Control & Stock Protection:** Real-time stock availability verification preventing over-commitments during invoice creation.

---

## 3. System Architecture & Design

### Architecture Diagram

The system uses an N-Tier decoupled MVC architecture with Dependency Injection, enforcing separation of concerns across Data, Services, and Presentation layers:

```text
+-------------------------------------------------------------------------------+
|                             Presentation Layer                                |
|       Razor Views / Bootstrap 5 / Vanilla JS Dynamic Dom Recalculation        |
|             Dedicated @media print Corporate Letterhead Layouts               |
+-------------------------------------------------------------------------------+
                                      │
                                      ▼
+-------------------------------------------------------------------------------+
|                             Controllers Layer                                 |
|   CustomerInvoicesController · SupplierBillsController · ProductsController   |
+-------------------------------------------------------------------------------+
                                      │
                                      ▼
+-------------------------------------------------------------------------------+
|                       Business & Service Layer (DTOs)                         |
|     ICustomerInvoiceService  <───────>  CustomerInvoiceService (Atomic Tx)    |
|   - Stock Verification       - Discount Engines      - Settlement Allocation  |
+-------------------------------------------------------------------------------+
                                      │
                                      ▼
+-------------------------------------------------------------------------------+
|                         Data Access Layer & Storage                           |
|       Entity Framework Core (Code-First) · MultipleActiveResultSets (MARS)    |
|                          Microsoft SQL Server Database                        |
+-------------------------------------------------------------------------------+                        4. Technologies UsedCategoryTechnologyVersionPurposeBackend FrameworkASP.NET Core8.0 / 9.0High-performance enterprise web application platformLanguageC#12.0Core backend, domain logic, and DTO contractsData AccessEntity Framework CoreLatestORM for database migrations, relationships, and LINQ queriesDatabaseMicrosoft SQL Server2019+Relational storage for transactions and accountingConnection PoolingMARS (MultipleActiveResultSets)EnabledPrevents data reader collisions during nested query executionFrontend UIRazor Pages / Views (MVC)LatestServer-side HTML renderingStylingBootstrap5.3+Modern responsive layouts and typographyIconographyBootstrap IconsLatestEnterprise UI iconographyClient ScriptingVanilla JavaScriptES6+Real-time billing calculations and DOM updatesPrint & ReportingCSS3 @media printW3C StandardA4 letterhead pagination and print-chrome suppression5. Installation & SetupPrerequisites.NET SDK 8.0 or higherMicrosoft SQL Server (LocalDB, Express, or standard instance)Visual Studio 2022 / VS Code / .NET CLIGitStep 1 — Clone the RepositoryBashgit clone [https://github.com/NimshanDilunika/ShanDistribution.git](https://github.com/NimshanDilunika/ShanDistribution.git)
cd ShanDistribution
Step 2 — Configure the Connection StringOpen appsettings.json (or appsettings.Development.json) and configure your SQL Server credentials:JSON{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ShanDistributionDb;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
Step 3 — Apply Database MigrationsRun the Entity Framework migration command to construct the schema, keys, and relational constraints:Bashdotnet ef database update
Step 4 — Launch the ApplicationStart the Kestrel web server:Bashdotnet run
Access the application dashboard at https://localhost:5001 or http://localhost:5000.6. Database Schema & MigrationsThe database models mirror commercial wholesale distribution operations:Entity TablePrimary KeyDescription & RoleCustomersCustomerIdStores customer billing profiles, contact persons, telephone numbers, and addresses.SuppliersSupplierIdStores confectionery manufacturing and regional distribution partner details.ProductsProductIdTracks confectionery inventory levels, unit descriptions, and standard wholesale prices.CustomerInvoicesCIIdRecords sales transactions, gross amounts, trade discount percentages, net payable, and balances.InvoiceProductsInvoiceProductIdBilled line items storing exact point-of-sale snapshots of quantities and negotiated prices.InvoicePaymentsInvoicePaymentIdPayment logs capturing cash collections, dates, and installments applied to an invoice.SupplierBillsSBIdCaptures supplier deliveries, gross bills, vendor trade deductions, and balance obligations.BillProductsBillProductIdLine-item records of confectionery stock received from suppliers.BillPaymentsBillPaymentIdRecords cash disbursements and bank settlements paid out to suppliers.
