# pull images
docker pull mcr.microsoft.com/dotnet/aspnet:7.0
docker pull mcr.microsoft.com/dotnet/sdk:7.0
if (!$?) { return; }

# build
$secret = 'id=ca_crt,src={0}\.aspnet\https\ca.crt' -f ${env:userprofile}
docker build `
	--secret $secret `
	--tag eassbhhtgu/homeautomationapi:latest `
	.
if (!$?) { return; }

# push
docker push eassbhhtgu/homeautomationapi:latest
if (!$?) { return; }
