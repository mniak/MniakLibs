$projects = @(
    "Mniak.Core",
    "Mniak.IO",
    "Mniak.Network"
)

function GenerateNuSpec { param([string] $project)
    $csproj = Join-Path (Join-Path (Join-Path .. src) $project ) ($project + ".csproj")
    NuGet pack $csproj
}

foreach ($project in $projects) {
    GenerateNuSpec $project IncludeReferencedProjects
}