$SelectedFile = dir *.nupkg | Sort-Object name | select -Last 1
nuget push "$SelectedFile" -Source https://api.nuget.org/v3/index.json
