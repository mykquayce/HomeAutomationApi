# HomeAutomationApi

### Elgato
```bash
curl --verbose http://localhost:38925/elgato/elgato

curl --request PUT --verbose http://localhost:38925/elgato/elgato/power/off
curl --request PUT --verbose http://localhost:38925/elgato/elgato/power/on
curl --request PUT --verbose http://localhost:38925/elgato/elgato/power/toggle

curl --request PUT --verbose http://localhost:38925/elgato/elgato/brightness/dimmest
curl --request PUT --verbose http://localhost:38925/elgato/elgato/brightness/dimmer
curl --request PUT --verbose http://localhost:38925/elgato/elgato/brightness/dim
curl --request PUT --verbose http://localhost:38925/elgato/elgato/brightness/half
curl --request PUT --verbose http://localhost:38925/elgato/elgato/brightness/bright
curl --request PUT --verbose http://localhost:38925/elgato/elgato/brightness/brighter
curl --request PUT --verbose http://localhost:38925/elgato/elgato/brightness/brightest

curl --request PUT --verbose http://localhost:38925/elgato/elgato/temperature/coolest
curl --request PUT --verbose http://localhost:38925/elgato/elgato/temperature/cooler
curl --request PUT --verbose http://localhost:38925/elgato/elgato/temperature/cool
curl --request PUT --verbose http://localhost:38925/elgato/elgato/temperature/half
curl --request PUT --verbose http://localhost:38925/elgato/elgato/temperature/warm
curl --request PUT --verbose http://localhost:38925/elgato/elgato/temperature/warm
curl --request PUT --verbose http://localhost:38925/elgato/elgato/temperature/warmest
```
