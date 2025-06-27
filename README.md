# 📚 School Management System

## 🚀 Project Overview

The **School Management System** is a full-stack web application built with **ASP.NET Core MVC**, designed to simplify and digitize everyday school administrative tasks. It offers essential features like student and teacher management, class and subject assignments, and secure user authentication using **ASP.NET Core Identity**. The UI is fully responsive, crafted with **Bootstrap 5**, ensuring a smooth experience across all devices.


---

## ✨ Features

- **Student Management**: Add, view, edit, and delete student records.
- **Teacher Management**: Full CRUD operations for teacher records.
- **Subject Management**: Manage academic subjects effortlessly.
- **Class Management**: Group students and manage class-wise data.
- **Entity Relationships**:
  - Students belong to classes.
  - Teachers are assigned to teach subjects.
- **Authentication System**:
  - Secure login/register via **ASP.NET Core Identity**.
  - Razor Pages used for authentication views, separate from MVC logic.
- **Responsive UI**:
  - Clean, mobile-friendly interface using **Bootstrap 5**.
- **Database Integration**:
  - Data persistence via **Entity Framework Core** with **SQL Server** backend.

---

## 🛠️ Tech Stack

| Layer        | Technology                       |
|--------------|-----------------------------------|
| Backend      | ASP.NET Core 9.0 (MVC)            |
| Frontend     | HTML5, CSS3, JavaScript           |
| UI Framework | Bootstrap 5                       |
| Database     | SQL Server                        |
| ORM          | Entity Framework Core             |
| Auth         | ASP.NET Core Identity             |
| Tools        | Visual Studio / VS Code, Git, NuGet |

---

## 🧪 Getting Started

### ✅ Prerequisites

Make sure you have the following installed:

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [SSMS (Optional)](https://aka.ms/ssmsfullsetup)
- A code editor: **Visual Studio 2022** (Recommended) or **VS Code**

---

### 📦 Installation Steps

1. **Clone the Repository**

```bash
git clone <your-repository-url>
cd schoolmanagement

Configure the Database Connection

Open appsettings.json and update the DefaultConnection string:

json
Copy
Edit
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=schoolmanagement;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
Replace YOUR_SERVER_NAME with your actual SQL Server instance.

Run EF Core Migrations

bash
Copy
Edit
dotnet tool install --global dotnet-ef        # If not already installed
dotnet restore
dotnet clean
dotnet build

# Add initial migration if not already done
dotnet ef migrations add InitialCreate

# Add Identity-related migration
dotnet ef migrations add AddIdentityTables

# Apply migrations to the database
dotnet ef database update
⚠️ Make sure your connection string is valid before running migrations.

Run the App

bash
Copy
Edit
dotnet run
Visit the output URL (e.g., https://localhost:5001) in your browser.

👤 Admin Setup
Navigate to:

arduino
Copy
Edit
https://localhost:5001/Identity/Account/Register
Register a user — this will be your initial admin account.

(Optional) Assign roles in the database manually or extend functionality later.

🧭 Application Usage
Home Page: Quick overview of features.

NavBar: Navigate between Students, Teachers, Subjects, Classes, Login/Register.

CRUD Pages: Use /Students, /Teachers, etc., for full management capabilities.

Auth Routes:

Register: /Identity/Account/Register

Login: /Identity/Account/Login

Logout: Built into layout navigation.

🤝 Contributing
Got an idea or found a bug? Contributions are welcome!
Feel free to open an issue or submit a pull request.

📜 License
This project is open-source. You’re free to fork, build, and contribute.

🧠 Author
Crafted with care by Harsh  —
Learning one line of code at a time, building systems that matter.

"Great software starts with a single feature — then gets better every commit."
Copy
Edit



