$localRepository = "C:\NuGet Local Repository"

$projects = @(
    "Mniak.Core",
    "Mniak.IO",
    "Mniak.Network"
)

function PackNuGet { param([string] $project)
    $csproj = Join-Path (Join-Path (Join-Path .. src) $project ) ($project + ".csproj")
    NuGet pack $csproj
}

foreach ($project in $projects) {
    PackNuGet $project IncludeReferencedProjects
}

if (Test-Path $localRepository) {
    Copy *.nupkg $localRepository
}