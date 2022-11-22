docker pull mcr.microsoft.com/dotnet/aspnet:7.0
docker pull mcr.microsoft.com/dotnet/sdk:7.0
docker pull eassbhhtgu/homeautomationapi:latest

$base1 = docker image inspect --format '{{.Created}}' mcr.microsoft.com/dotnet/aspnet:7.0
$base2 = docker image inspect --format '{{.Created}}' mcr.microsoft.com/dotnet/sdk:7.0
$target = docker image inspect --format '{{.Created}}' eassbhhtgu/homeautomationapi:latest

if ($base1 -gt $target -or $base2 -gt $target) {

	$secret = 'id=ca_crt,src={0}\.aspnet\https\ca.crt' -f ${env:userprofile}
	docker build `
		--secret $secret `
		--tag eassbhhtgu/homeautomationapi:latest `
		.
	if (!$?) { exit 1; }

	docker push eassbhhtgu/homeautomationapi:latest
	if (!$?) { exit 1; }
}
