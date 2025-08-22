# C Analyzer (WinForms)

A lightweight Windows Forms tool that **quickly inspects C source files**. It counts functions, variables, `for`/`while` loops, and `printf`/`scanf` calls, and shows a cleaned view of the code (comments removed, trimmed lines).

> ⚠️ This is a **heuristic analyzer** intended for quick insights—not a full C parser. It works best for single-line declarations and definitions.

---

## ✨ Features

- **Counts**
  - Function definitions (single-line `retType name(...) {`)
  - Variable declarations (single-line; ignores prototypes)
  - `for` and `while` loops
  - `printf` and `scanf` calls
- **Pre-cleaning**
  - Removes block comments `/* ... */` and line comments `// ...`
  - Removes string literals before matching `printf`/`scanf` to avoid false positives
- **UI**
  - Text box for file path
  - “Analyze” button
  - Summary panel with counts and file metadata
  - Code preview area with normalized lines

---

## 🚀 Getting Started

### Prerequisites
- Windows 10/11  
- **.NET 6+ SDK** (or Visual Studio 2022)  
  _WinForms target framework: `net6.0-windows`_

> If your project targets **.NET Framework 4.8**, just open and build with Visual Studio.

### Build & Run (Visual Studio)
1. Open the solution (`.sln`).
2. Ensure the WinForms project targets `net6.0-windows` (or your chosen TF).
3. Build and run (F5).

### Build & Run (dotnet CLI)
```bash
dotnet restore
dotnet build
dotnet run --project C_Analyzer
🧭 Usage

Enter or browse to a .c file path.

Click Analyze.

Read summary counts and inspect the cleaned code preview.

The default path in the code points to simple_types.c. Replace it with your own file path or add a file picker dialog.

🔍 What’s Detected (Regex Summary)

For loop

\bfor\b\s*\([^;]*;[^;]*;[^;]*\)


While loop

\bwhile\b\s*\(


printf / scanf calls

\bprintf\s*\(
\bscanf\s*\(


Comment stripping

/\*.*?\*/    // block comments  (Singleline)
//.*?$       // line comments   (Multiline)


String literal removal (before printf/scanf match)

"(?:\\.|[^"\\])*"


Variable/function detection uses simple heuristics (keywords + line shape). It may miss multi-line signatures, typedefs, macros, attributes, or complex pointer/array declarations.

📁 Project Structure (suggested)
C_Analyzer/
├─ Form1.cs
├─ Form1.Designer.cs
├─ Program.cs
├─ README.md
└─ (optional) sample-files/
   └─ simple_types.c

🧪 Sample C File

simple_types.c (example used during development):

#include <stdio.h>

int main() {
    char c = 'A';
    int i = 123;
    long l = 1234567890L;
    float f = 3.14159f;
    double d = 2.718281828459;

    printf("Character: %c\n", c);
    printf("Integer: %d\n", i);
    printf("Long: %ld\n", l);
    printf("Float: %.5f\n", f);
    printf("Double: %.12f\n", d);

    return 0;
}

📌 Roadmap

 Count multiple declarators on a single line (e.g., int a, *p, b;)

 Handle multi-line function signatures and declarations

 Recognize signed/unsigned, long long, arrays, attributes

 Add file picker, recent files list, and drag-drop

 Export results to JSON/CSV

🤝 Contributing

PRs and suggestions are welcome! Please open an issue to discuss major changes first.

📝 License

MIT — feel free to use, modify, and distribute.


Want me to add a small **GIF screenshot** section or **badges** (e.g., .NET version, license) at the top?

