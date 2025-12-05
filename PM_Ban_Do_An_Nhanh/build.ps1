$msbuildPath = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
$solutionPath = "PM_Ban_Do_An_Nhanh.sln"

Write-Host "Building solution..." -ForegroundColor Green
& $msbuildPath $solutionPath /t:Build /p:Configuration=Debug /v:minimal /nologo

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nBuild succeeded!" -ForegroundColor Green
} else {
    Write-Host "`nBuild failed with exit code: $LASTEXITCODE" -ForegroundColor Red
}
