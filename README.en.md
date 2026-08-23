🌏[한국어](README.kr.md) | [English](README.en.md)


[![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2Fkmonkeyhead%2FMORT&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=hits&edge_flat=false)](https://hits.seeyoufarm.com)
[![GitHub downloads](https://img.shields.io/github/downloads/kmonkeyhead/MORT/total.svg?logo=github)](https://github.com/kmonkeyhead/MORT/releases)

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Y8Y6DIUR2)

<img src="https://github.com/kmonkeyhead/MORT/blob/main/MORT_LOGO.png" width="90%"></img>


[![Video Label](https://img.youtube.com/vi/LHTErVnsaws/0.jpg)](https://youtu.be/LHTErVnsaws)

https://youtu.be/LHTErVnsaws

Sample video

# MORT #

MORT is a program that extracts dialogs from the screen in real time using OCR and outputs a translations using DB or machine translation.

Currently, English and Japanese translation/extraction can be extracted by default, and it can also be translated by linking with a hooking program using the save to clipboard function.

[Latest version download and release notes - https://blog.naver.com/killkimno/70179867557]

## Features ##
* Realtime translate
* OCR - TesseractOCR , Windows OCR, NHOcr
* Machine translation - Naver Papago, Google Web, Google Sheet, ezTrans, DeepL
* Language Patch with using DB
* Multiple OCR areas
* Img adjust

## System Requirement ##
* Windows 10 or higer
* 64bit os
* Net Framework 4.7.2 or higer
* Visual Studio 2022 Visual C++ (x64) - vcredist_x64.exe
  - https://aka.ms/vs/17/release/vc_redist.x64.exe


## How to use ##
#### Basic usage ####

1. After setting in the quick settings, press Trnaselate on the remote control to start translation
2. Or Preferences tab -> Set the OCR language according to the language of the game to be translated
3. Remote control -> Click Search and select the area where the lines appear
4. Click Apply on MORT main form
5. Remote control -> Real-time translation by pressing Translate

#### User Manual ####

* https://blog.naver.com/killkimno/221904784013


## Develop the project
### Development environment ###

* Visaul Studio 2019 이상
* Tesseract OCR 5.2.0 
* NHocr 0.21

### Create a build and run environment ###

1. Build the project first
2. Can't run after build. The reason is that there are no essential files required for run
3. Unzip the latest build files into the release folder to get the required files 
- https://drive.google.com/drive/folders/0BxO-Nrmd-kR7dVp5TWpMQ09jMFU?resourcekey=0-bx6_8OEv3WAGzz9Au9fxNg
4. MORT_CORE.DLL , MORT_WIN10OCR.DLL, nhocr.DLL To modify This dll, please refer to the related projects below

### Related Project ###

* MORT Core - MORT_CORE_DLL
  - https://github.com/kmonkeyhead/MORT_CORE
  
* MORT Win OCR - MORT_WIN10OCR.DLL
  - https://github.com/kmonkeyhead/MORT_WIN10OCR
  
* MORT NHocr - nhocr.DLL
  - https://github.com/kmonkeyhead/MORT_NHOCR

## ETC ##
### Development Plans Trello ###

- https://trello.com/b/gPa1EL5x/mort

### Discord ###
- [Discord](https://discord.com/invite/ha5yNy9) ![Discord Badge](https://discord.com/api/guilds/742743719958151298/widget.png?style=shield)
