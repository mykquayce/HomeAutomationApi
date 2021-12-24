#! /bin/bash

docker build --tag eassbhhtgu/homeautomationapi:latest .
if [ $? -eq 1 ]; then exit 1; fi

docker stack deploy --compose-file ./docker-compose.yml homeautomationapi
if [ $? -eq 1 ]; then exit 1; fi
