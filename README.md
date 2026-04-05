# Notepad++ Clone (WPF & .NET 8)

A high-performance, multi-tab text editor built with C# and WPF, inspired by the versatility of Notepad++. This project demonstrates advanced UI management, file system operations, and efficient text processing.

## 🚀 Key Features

- **Multi-Tab Interface:** Manage multiple files simultaneously using a dynamic tab system.
- **Folder Explorer:** Integrated TreeView to browse, open, and manage local directories directly within the app.
- **Advanced Search & Replace:** Powerful engine using **Regular Expressions (Regex)** to find or replace text in the current tab or across all open files.
- **File Management:** Full support for Open, Save, and "Save As" operations, including a "Close All" feature with unsaved changes detection (asterisk indicator).
- **Directory Services:** Ability to copy paths, copy folders, and paste directories through an intuitive context menu.
- **Modern UI:** Built with WPF and .NET 8, featuring a grid-based layout with adjustable splitters for the explorer panel.

## 🛠️ Technical Stack

- **Language:** C#
- **Framework:** .NET 8.0 Windows
- **UI:** WPF (XAML)
- **Logic Patterns:** Service-oriented architecture with dedicated managers for Files, Search, and Directory operations.

## 📂 Project Structure

- `FileManager.cs`: Handles disk I/O and user save prompts.
- `SearchService.cs`: Contains the logic for Regex-based searching and highlighting.
- `DirectoryService.cs`: Manages TreeView population and directory manipulations (Copy/Paste).
- `TabService.cs`: Logic for creating and tracking dynamic TabItems and their content.

## 🔧 How to Run

1. Clone the repository: `git clone https://github.com/cosminpelin21/Notepad-Clone.git`
2. Open `Notepad++ Clone.sln` in Visual Studio 2022.
3. Restore NuGet packages and build the solution.
4. Run the application (Target: net8.0-windows).

## 👤 Author
**Cosmin Pelin** Student at Transilvania University of Brașov  
Email: cosminpelin21@gmail.com
