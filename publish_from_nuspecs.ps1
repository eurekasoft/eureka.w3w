param (
    [Parameter(Mandatory = $true)]
    [string]$version,

    [Parameter(Mandatory = $false)]
    [string[]]$projectPaths = @("eureka.w3w\eureka.w3w.vbproj"),

    [Parameter(Mandatory = $false)]
    [string]$outputDir = "Builds"
)

$ErrorActionPreference = "Stop"
$tag = "v$version"

# Check if git tag already exists
$tagExists = git tag --list $tag | Where-Object { $_ -eq $tag }
if ($tagExists) {
    Write-Error "Git tag '$tag' already exists."
    exit 1
}

# Clean/Create output directory
if (Test-Path $outputDir) {
    Remove-Item -Recurse -Force $outputDir
}
New-Item -ItemType Directory -Path $outputDir | Out-Null

# Ensure nuget.exe is available
$nugetExe = "nuget.exe"
if (-not (Get-Command $nugetExe -ErrorAction SilentlyContinue)) {
    Write-Error "'nuget.exe' is required to pack .nuspec files. Please ensure it's in your PATH."
    exit 1
}

foreach ($proj in $projectPaths) {
    if (-not (Test-Path $proj)) {
        Write-Warning "Project file not found: $proj"
        continue
    }

    $projDir = Split-Path $proj -Parent
    $nuspecFile = Get-ChildItem -Path $projDir -Filter *.nuspec | Select-Object -First 1

    if (-not $nuspecFile) {
        Write-Warning "No .nuspec file found in $projDir. Skipping."
        continue
    }

    Write-Host "Building project: $proj"
    dotnet clean $proj
    dotnet restore $proj
    dotnet build $proj -c Release

    Write-Host "Packing NuGet package from: $nuspecFile"
    & $nugetExe pack $nuspecFile.FullName -Version $version -OutputDirectory $outputDir
}

# Warn about any .nupkg files ignored by .gitignore
Get-ChildItem -Path $outputDir -Filter *.nupkg | ForEach-Object {
    if (git check-ignore $_.FullName) {
        Write-Warning "$($_.FullName) is ignored by .gitignore. Forcing add."
    }
}


# Ask user for confirmation before proceeding with git operations (y/n)
$confirmation = Read-Host "Do you want to proceed with git operations? (y/n)"
if ($confirmation -ne 'y') {
    Write-Host "Aborting git operations."
    exit 0
}

# Git add, commit, tag
git add -f $outputDir
git commit -m "Build packages for version $version"
git tag $tag

# Push changes and tag
git push
git push origin $tag