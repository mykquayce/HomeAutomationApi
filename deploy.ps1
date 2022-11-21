docker pull eassbhhtgu/homeautomationapi:latest
if (!$?) { return; }

docker stack deploy --compose-file .\docker-compose.yml homeautomationapi
