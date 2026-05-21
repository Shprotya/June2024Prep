# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

A WPF desktop application for patient and appointment management at a medical clinic, backed by SQL Server via Entity Framework 6. The solution contains two projects:

- **June2024Prep** — WPF UI app (the main application)
- **DataManagement** — Console app for seeding the database with sample data

## Build & Run

Open `June2024Prep.sln` in Visual Studio 2022. Requires .NET Framework 4.7.2.

**Command-line build:**
```
msbuild June2024Prep.sln /p:Configuration=Debug
```

**Run the WPF app:** Set `June2024Prep` as startup project and press F5.

**Run the seeder:** Set `DataManagement` as startup project and run — it inserts sample patients and appointments into the database.

No test projects are configured in this solution.

## Architecture

### Data Layer — `June2024Prep/Patient.cs`

All data models and the EF6 DbContext live in a single file:

- `Patient` — `PatientID`, `FirstName`, `Surname`, `DOB`, `ContactNumber`; has a navigation collection `Appointments`
- `Appointment` — `AppointmentID`, `AppointmentTime`, `AppointmentNotes`, `PatientID`; foreign key back to `Patient`
- `PatientData : DbContext` — connects to SQL Server database `OODExam_YelyzavetaKareieva`; exposes `DbSet<Patient>` and `DbSet<Appointment>`; configures the one-to-many relationship in `OnModelCreating`

### UI Layer — `June2024Prep/MainWindow.xaml(.cs)`

Two-column Material Design layout (Light theme, DeepPurple/Lime palette):

- Left column: `ListBox` bound to the patient list
- Right column: patient detail form fields + appointments sub-section

Event handler stubs in `MainWindow.xaml.cs` are **not yet implemented**: `AddPatient`, `AddAppointment`, `EditAppointment`, and `PatientSelected`.

### Database Seeder — `DataManagement/Program.cs`

Instantiates `PatientData`, creates `Patient` and `Appointment` objects, adds them, and calls `SaveChanges()`. Run once to populate a fresh database.

## Key Dependencies

| Package | Version | Purpose |
|---|---|---|
| EntityFramework | 6.5.2 | ORM / SQL Server access |
| MaterialDesignThemes | 5.3.2 | WPF UI components |
| Microsoft.Xaml.Behaviors.Wpf | 1.1.77 | XAML interaction triggers |

NuGet packages are restored automatically by Visual Studio; `packages/` folder is in the repo root.
