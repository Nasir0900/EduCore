# EduCore ERP - Database Blueprint

## Version
v1.0

## Project Vision

EduCore ERP is a modular University Enterprise Resource Planning (ERP) system designed for a **single university**.

The system aims to digitize all major university operations including:

- Administration
- Academics
- Finance
- Human Resources
- Student Services
- Reporting

The database is designed using normalization principles and follows a modular architecture to support future expansion.

---

# Database Architecture

```
Administration
│
├── Users
├── Roles
├── Permissions
├── Audit Logs
└── Settings

Academics
│
├── Faculty
├── Department
├── Academic Program
├── Academic Session
├── Part
├── Semester
├── Curriculum
├── Course
├── Course Offering
├── Teacher
├── Student
├── Registration
├── Attendance
├── Examination
├── Marks
└── Results

Finance
│
├── Fee Categories
├── Fee Structure
├── Student Accounts
├── Payments
├── Scholarships
├── Discounts
└── Financial Reports

Human Resources
│
├── Employees
├── Payroll
├── Leave
├── Attendance
└── Performance

Student Services
│
├── Library
├── Hostel
├── Transport
├── Student Cards
└── Counseling

Reports
```

---

# Academics Module

---

# Faculty

## Purpose

Represents an academic faculty.

Examples

- Faculty of Science
- Faculty of Engineering
- Faculty of Business Administration

### Columns

- FacultyId (PK)
- FacultyName
- Description
- CreatedDate

### Relationships

Faculty

↓

Departments (One-to-Many)

---

# Department

## Purpose

Represents a department within a faculty.

Examples

- Physics
- Chemistry
- Computer Science

### Columns

- DepartmentId (PK)
- DepartmentName
- Description
- FacultyId (FK)
- CreatedDate

### Relationships

Faculty

↓

Department

↓

Academic Programs

↓

Teachers

↓

Courses

---

# Academic Program

## Purpose

Represents a degree program offered by a department.

Examples

- BS Physics
- BS Computer Science
- MSc Physics

### Columns

- AcademicProgramId (PK)
- ProgramName
- Description
- DepartmentId (FK)
- CreatedDate

### Relationships

Department

↓

Academic Programs

↓

Academic Sessions

---

# Academic Session

## Purpose

Represents an admission batch.

Examples

- 2025-2029
- 2026-2030
- 2027-2031

### Columns

- AcademicSessionId (PK)
- SessionName
- StartYear
- EndYear
- AcademicProgramId (FK)
- CreatedDate

### Relationships

Academic Program

↓

Academic Sessions

↓

Parts

---

# Part

## Purpose

Represents an academic year/part within a program.

Examples

BS Programs

Part I
Part II
Part III
Part IV

MSc Programs

Part III
Part IV

### Columns

- PartId (PK)
- PartName
- PartNumber
- AcademicSessionId (FK)
- CreatedDate

### Relationships

Academic Session

↓

Parts

↓

Semesters

---

# Semester

## Purpose

Represents an academic semester.

Examples

Semester 1

Semester 2

Semester 3

Semester 4

Semester 5

Semester 6

Semester 7

Semester 8

### Columns

- SemesterId (PK)
- Title
- SemesterNumber
- PartId (FK)
- CreatedDate

### Relationships

Part

↓

Semesters

↓

Curriculum

---

# Curriculum

## Purpose

Defines courses offered in a semester.

Categories

Core Courses

Elective Courses

### Relationships

Semester

↓

Curriculum

↓

Courses

---

# Course

## Purpose

Represents a subject taught by the university.

Examples

Physics I

Calculus

Programming Fundamentals

Digital Logic

### Relationships

Department

↓

Courses

↓

Course Offerings

---

# Course Offering

## Purpose

Represents a course offered in a specific semester.

Contains

- Teacher
- Semester
- Course
- Section (future)
- Schedule (future)

---

# Teacher

## Purpose

Represents academic staff.

Future

Teacher will inherit from Employee.

Relationships

Department

↓

Teachers

↓

Course Offerings

---

# Student

## Purpose

Represents an enrolled student.

Relationships

Academic Session

↓

Students

↓

Registrations

---

# Registration

## Purpose

Stores student registrations.

Relationships

Student

↓

Course Offerings

---

# Attendance

Attendance belongs to Course Offering.

Course Offering

↓

Attendance

↓

Student

---

# Examination

Stores examination details.

Midterm

Final

Practical

Quiz

Assignment

---

# Marks

Stores student marks.

Relationships

Student

↓

Marks

↓

Result

---

# Result

Stores final grades and GPA calculations.

Future

CGPA

Transcript

Degree Audit

---

# Finance Module

Student

↓

Fee Account

↓

Payments

↓

Scholarships

↓

Discounts

↓

Fines

---

# Human Resources

Employee

↓

Teacher

↓

Payroll

↓

Leave

↓

Performance

---

# Student Services

Library

Hostel

Transport

Student ID

Counseling

---

# Future Modules

- Alumni Portal
- Parent Portal
- Mobile Application
- LMS Integration
- AI Assistant
- Timetable Generator
- Degree Audit
- Online Admissions
- Hostel Management
- Inventory Management

---

# Development Philosophy

- Modular Architecture
- Clean Code
- Scalable Database Design
- SOLID Principles
- Repository Pattern (Future)
- Service Layer (Future)
- Role-Based Security
- Git Version Control
- Continuous Documentation
