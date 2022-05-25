# OSC  Vrchat Feeder app
This is just heartbeat sender app for Android ( right now ) , that can work with or without "Notify for MiBand", to send data (Sleep status, heartrate and step count) from most of Xiaomi devices. This app can work with some "HR share" capable BLE devices, if they exposing Heartrate ble service.

When working without "Notify for MiBand", this app (now)  only capable to send HR values to OSC. BLE mode doesnt work with not authenticated devices right now without standart device app (MiFit,Zepp etc).

### Additional features:
  - Send device (phone) orientation data to OSC with 3 euler angles parameters
  - Have multiple profiles with parameters to send
  - Working background
  - Normalize and clamp values data before send, if needed.
  - Send heartrate without notify app.

Doesnt work with hacked version of  "Notify for MiBand".
