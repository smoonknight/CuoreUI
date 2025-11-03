CuoreUI uses ***date versioning*** (***CalVer***).

## Format
The format is: `[year]`.`[month]`.`[day]`.`[revisions]`

> [!TIP]  
> The `[revisions]` field is optional. 
> There are 2 use cases for the `[revisions]` field:
> 1) The version had a bug which the revision fixes ∙ *(Example commit: [3c8fb30](https://github.com/owtysm2/CuoreUI/commit/3c8fb3047b0cc7bd88e548f6730cf215383da3ea))*
> 2) The change was too small to call it a new version ∙ *(Example commit: [349f2ca](https://github.com/owtysm2/CuoreUI/commit/349f2ca801945464aa484ff46d7324f263b3a2cc))*

## Explanation
**The Gregorian calendar is assumed.**

`[year]` - The year the release was published<br>
`[month]` - The month the release was published<br>
`[day]` - The day the release was published<br>
`[revisions]` - How many times the version was modified **after** its initial release
### Examples:
- `2025.9.19` ∙ 2025 September 19th, no revisions
- `2025.9.19.1` ∙ 2025 September 19th, 1 revision
- `2024.12.16 ` ∙ 2024 December 16th, no revisions

## Automation
The root directory's [/build.exe](https://github.com/owtysm2/CuoreUI/blob/master/build.exe) file automates the editing of the [/CuoreUI.Winforms.nuspec](https://github.com/owtysm2/CuoreUI/blob/master/CuoreUI.Winforms.nuspec) file aswell as the NuGet packaging process.\
<br>
> [!IMPORTANT]
> If you feel unsafe running the build executable, you can [get your own nuget.exe](https://www.nuget.org/downloads) cmd tool from the official NuGet website,
> and manually package CuoreUI by pasting `nuget.exe` it into your master directory and running `nuget pack` in any terminal opened inside the master directory.

> [!TIP]
> You **can always decompile `build.exe`** by using tools like **JetBrains dotPeek** or **dnSpy**
### What [/build.exe](https://github.com/owtysm2/CuoreUI/blob/master/build.exe) does:
**1. Updates the .nuspec**
   1) Checks if `CuoreUI.Winforms.nuspec` and `nuget.exe` exist in the running directory
   2) Asks for revision number. 0 means no revision, and is the default. 
   3) Reads the `CuoreUI.Winforms.nuspec`'s text content
   4) Scans each line if it starts with "\<version\>"
   5) When the version line is found, it overwrites the line with the current date on the developer's computer
   6) Save modified data as `CuoreUI.Winforms.nuspec`
   
**2. Builds the .nupkg**
   1) Start `nuget.exe` with the `pack` argument without a window
   2) Redirect the console output into a `output.log` file
   3) The `nuget.exe pack` process then takes care of packaging CuoreUI binaries into `.nupkg` files, which can then be uploaded to NuGet
