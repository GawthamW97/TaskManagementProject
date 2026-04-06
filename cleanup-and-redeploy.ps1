# Clean up build artifacts and prepare for fresh deployment
Write-Host "🧹 Cleaning up project..." -ForegroundColor Cyan

# Remove bin and obj folders
Remove-Item -Path "TaskManagementApp\bin" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "TaskManagementApp\obj" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "TaskManagementWeb\bin" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "TaskManagementWeb\obj" -Recurse -Force -ErrorAction SilentlyContinue

Write-Host "✅ Cleaned bin/obj folders"

# Clear user secrets (if any contain old connection strings)
Write-Host "🔐 Clearing user secrets..." -ForegroundColor Cyan
dotnet user-secrets clear --project TaskManagementApp
Write-Host "✅ User secrets cleared"

# Restore packages
Write-Host "📦 Restoring NuGet packages..." -ForegroundColor Cyan
dotnet restore
Write-Host "✅ Packages restored"

# Build the solution
Write-Host "🔨 Building solution..." -ForegroundColor Cyan
dotnet build --configuration Release
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build successful!" -ForegroundColor Green
} else {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "`n✅ All cleanup complete! Ready for deployment." -ForegroundColor Green
Write-Host "Next: Publish using 'dotnet publish --configuration Release'" -ForegroundColor Yellow
