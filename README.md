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
```
