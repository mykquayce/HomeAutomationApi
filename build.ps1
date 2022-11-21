# pull images
docker pull mcr.microsoft.com/dotnet/aspnet:7.0
docker pull mcr.microsoft.com/dotnet/sdk:7.0
if (!$?) { return; }

# build
docker build `
	--secret id=ca_crt,src=${env:userprofile}\.aspnet\https\ca.crt `
	--tag eassbhhtgu/homeautomationapi:latest `
	.
if (!$?) { return; }

# push
docker push eassbhhtgu/homeautomationapi:latest
if (!$?) { return; }
