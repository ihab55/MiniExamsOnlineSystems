# Exams System Web 🚀

A comprehensive web-based examination system for managing and conducting exams online.

## 📦 Installation Guide

Follow these steps to set up the Exams System Web on your local machine:

### Prerequisites
- [.NET Framework](https://dotnet.microsoft.com/download) 
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) 
- [Visual Studio](https://visualstudio.microsoft.com/) (recommended IDE)

### Setup Instructions

1. **Download the Project**
   - Clone the repository:
     ```bash
     git clone https://github.com/yourusername/exams-system-web.git
     ```
   - Or download as ZIP file and extract

2. **Database Setup**
   - Restore the `EamesDB` database to your SQL Server instance

3. **Configure the Project**
   - Open the solution in Visual Studio
   - Check all project references (ensure NuGet packages are restored)
   - Update the connection string in `DataAccessLayer/DataAccess`:
     ```xml
     <connectionStrings>
       <add name="YourConnectionName" 
            connectionString="Server=YOUR_SERVER;Database=EamesDB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;" 
            providerName="System.Data.SqlClient"/>
     </connectionStrings>
     ```

4. **Run the Application**
   - Build the solution (Ctrl+Shift+B)
   - Press F5 to start debugging

## 🛠️ Project Structure

## 🤝 Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes.

---

⭐ Feel free to star the repository if you find this project useful!
