<!-- [document_link] -->
<!-- [common] -->
[![GitHub downloads](https://img.shields.io/github/downloads/kmonkeyhead/MORT/total.svg?logo=github)](https://github.com/kmonkeyhead/MORT/releases)

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Y8Y6DIUR2)

<img src="https://github.com/kmonkeyhead/MORT/blob/main/MORT_LOGO.png" width="90%"></img>


[![Video Label](https://img.youtube.com/vi/LHTErVnsaws/0.jpg)](https://youtu.be/LHTErVnsaws)

https://youtu.be/LHTErVnsaws

Sample video

# MORT #

<!--[kr]-->
MORT는 OCR을 이용해 화면상에 나온 대사를 추출 , DB나 기계번역을 이용해 번역본을 출력해주는 프로그램입니다.

현재 영어와 일본어 번역/추출을 기본적으로 추출 가능하며, 클립보드에 저장 기능을 이용해 후킹 프로그램과 연동해 번역할 수도 있습니다.

[최신 버전 다운로드 및 릴리즈 노트 - https://blog.naver.com/killkimno/70179867557]

<!--[en]-->
MORT is a program that extracts dialogs from the screen in real time using OCR and outputs a translations using DB or machine translation.

Currently, English and Japanese translation/extraction can be extracted by default, and it can also be translated by linking with a hooking program using the save to clipboard function.

[Latest version download and release notes - https://blog.naver.com/killkimno/70179867557]

<!--[kr]-->
## 지원 기능 ##
<!--[en]-->
## Features ##
<!--[kr]-->
* 실시간 번역
* OCR - TesseractOCR , Windows OCR, Google Cloud Vision OCR, Snipping Tool OCR, Easy OCR
* 번역기 - 네이버 파파고, 구글 웹, 구글 시트, 이지트랜스, 딥플
* DB를 이용한 준한글화
* 다중 OCR 영역
* 이미지 보정
* 실시간 번역

<!--[en]-->
* Realtime translate
* OCR - TesseractOCR , Windows OCR, Google Cloud Vision OCR, Snipping Tool OCR, Easy OCR
* Machine translation - Naver Papago, Google Web, Google Sheet, ezTrans, DeepL
* Language Patch with using DB
* Multiple OCR areas
* Img adjust

<!--[kr]-->
## 요구 사항 ##
<!--[en]-->
## System Requirement ##
<!--[kr]-->
* 윈도우 10 이상
* 64비트 OS
* .NET 9 이상
  - https://dotnet.microsoft.com/ko-kr/download/dotnet/thank-you/runtime-desktop-9.0.4-windows-x64-installer
* 필요 재배포 패키지 Visual Studio 2022 Visual C++ (x64) - vcredist_x64.exe
  - https://aka.ms/vs/17/release/vc_redist.x64.exe
<!--[en]-->
* Windows 10 or higer
* 64bit os
* .NET 9 or higer
  - https://dotnet.microsoft.com/ko-kr/download/dotnet/thank-you/runtime-desktop-9.0.4-windows-x64-installer
* Visual Studio 2022 Visual C++ (x64) - vcredist_x64.exe
  - https://aka.ms/vs/17/release/vc_redist.x64.exe


<!--[kr]-->
## 사용법 ##
<!--[en]-->
## How to use ##
<!--[kr]-->
#### 기본 사용법 ####
<!--[en]-->
#### Basic usage ####

<!--[kr]-->
1. 빠른설정에서 설정을 한 후 리모컨에서 Trnaslate를 눌러 번역 시작
2. 또는 기본설정 탭 -> OCR 언어를 번역할 게임의 언어에 맞춰 설정
3. 리모컨 -> Search 클릭 후 대사가 나오는 영역 선택
4. MORT 메인 폼에서 적용 클릭
5. 리모컨 -> Translate를 눌러 실시간 번역

<!--[en]-->
1. After setting in the quick settings, press Trnaselate on the remote control to start translation
2. Or Preferences tab -> Set the OCR language according to the language of the game to be translated
3. Remote control -> Click Search and select the area where the lines appear
4. Click Apply on MORT main form
5. Remote control -> Real-time translation by pressing Translate

<!--[kr]-->
#### 사용자 메뉴얼 링크 ####
<!--[en]-->
#### User Manual ####
<!--[common]-->
* https://blog.naver.com/killkimno/221904784013

<!--[kr]-->
## 커스텀 사용법 ##
<!--[en]-->
## Custom usage ##
<!--[kr]-->
#### 번역 결과 언어 코드 추가 ####
<!--[en]-->
#### Add translation result language code ####
<!--[kr]-->
UserData/UserTransCode.txt 파일에서 구글 번역기 언어 코드를 추가할 수 있습니다​

```
code, 이름 
(예 : it, 이탈리아어)
```

​언어 코드 리스트는 https://cloud.google.com/translate/docs/languages?hl=en 여기서 확인할 수 있습니다
<!--[en]-->
You can add Google Translator language code in the UserData/UserTransCode.txt file​

```
code, name
(ex : it, Italian)
```

​A list of language codes can be found here: https://cloud.google.com/translate/docs/languages?hl=en

<!--[kr]-->
#### 커스텀 API 사용법 ####
<!--[en]-->
#### Custom API usage ####
<!--[kr]-->
1. HTTP 기반으로 만들어진 커스텀 API를 사용할 수 있습니다
2. 번역 방법 -> 커스텀 API
3. 고급 설정 -> 커스텀 API URL 설정
```
POST 규칙
name - string
text - string - 번역할 문장
target - string - 결과 언어 코드
source - string - 소스 언어 코드

ex
{
"name" : "test",
"text" : "tank divsion" ,
"target" : "ko",
"source" : "en"
...
}


응답 규칙
result - string - 번역 결과
errorCode - string - 에러 코드
errorMessage - string - 에러 메세지

ex
{
"result" : "탱크 사단",
"errorMessage" : "",
"errorCode" : "0"
}
```

4. 예시1 - LibreTranslate 를 사용한 커스텀 API
* 예시로 사용할 번역기 주소
  - https://github.com/LibreTranslate/LibreTranslate 
* 샘플 코드
  - https://github.com/CommitComedian/LT_Mort 
  - 샘플을 제작해 주신 [@CommitComedian](https://github.com/CommitComedian) 님 감사합니다!

  예시2 - NLLB 를 사용한 커스텀 API
* 예시로 사용할 번역기 주소
  - https://github.com/thammegowda/nllb-serve
* 샘플 코드
  - https://github.com/TUVup/NLLB_serveMORTapi
  - 샘플을 제작해 주신 [@TUVup](https://github.com/TUVup) 님 감사합니다!

<!--[en]-->
1. You can use a custom API based on HTTP
2. Translation Type -> Custom API
3. Advanced Settings -> Custom API URL Settings
```
POST Rule
name - string
text - string - ocr string
target - string - translation result language code
source - string - ocr language code

ex
{
"name" : "test",
"text" : "tank divsion" ,
"target" : "ko",
"source" : "en"
...
}


Reponse Rule
result - string - translation result text
errorCode - string - error code
errorMessage - string - error message

ex
{
"result" : "탱크 사단",
"errorMessage" : "",
"errorCode" : "0"
}
```

4. Example1 - Using LibreTranslate
* Translator address to use as an example
  - https://github.com/LibreTranslate/LibreTranslate
* sample code
  - https://github.com/CommitComedian/LT_Mort
  - Thank you [@CommitComedian](https://github.com/CommitComedian) for creating the sample!

  Example2 - Using NLLB
* Translator address to use as an example
  - https://github.com/thammegowda/nllb-serve
* sample code
  - https://github.com/TUVup/NLLB_serveMORTapi
  - Thank you [@TUVup](https://github.com/TUVup) for creating the sample!

<!--[kr]-->
## FAQ ##
<!--[en]-->
## FAQ ##
<!--[kr]-->
<b>전체화면 게임에서 사용할 수 있나요?</b>
- 아뇨 전체화면 게임에서 사용할 수 없습니다. 대신 윈도우 모드, 보더리스 윈도우 모드로 해주세요

<b>32bit 윈도우를 사용하고 있습니다. MORT를 사용할 수 없을까요?</b>
- 32비트 버전 MORT를 이용해 주세요
- https://blog.naver.com/killkimno/222936631523

<b>64bit 윈도우를 사용하는데 0x8007045A 가 발생했습니다. </b>
- CPU가 AVX2를 지원하지 않아 생기는 문제입니다. 32비트 MORT를 이용해 주세요
- https://blog.naver.com/killkimno/222936631523

<!--[en]-->
<b>Can i use FullScreen Mode?</b>
- No you can't use it in fullscreen games, please use windowed mode, borderless windowed mode instead

<b>I'm using 32-bit Windows. Can I use MORT?</b>
- Use 32bit version MORT
- https://blog.naver.com/killkimno/222936631523

<b>I'm using 64-bit Windows. But Can't run with this error 0x8007045A.</b>
- CPU must Support AVX2. If Your CPU not support AVX2, Use 32bit version MORT instead
- https://blog.naver.com/killkimno/222936631523


<!--[kr]-->
## 프로젝트 개발하기 ##
### 개발 환경 ###
<!--[en]-->
## Develop the project
### Development environment ###

<!--[common]-->
* Visaul Studio 2019 or higer
* Tesseract OCR 5.2.0 
* NHocr 0.21

<!--[kr]-->
### 빌드 및 실행 환경 만들기 ###
<!--[en]-->
### Create a build and run environment ###
<!--[kr]-->
1. 프로젝트를 Release 모드로 설정합니다. 타겟 CPU는 X64로 합니다
2. 빌드합니다
3. 실행하면 에러가 뜹니다. 이유는 실행에 필요한 필수 파일이 없기 때문입니다
4. 필수 파일을 받기 위해 최신 빌드 파일을 릴리즈 폴더에 압축을 풉니다 
- https://drive.google.com/drive/folders/0BxO-Nrmd-kR7dVp5TWpMQ09jMFU?resourcekey=0-bx6_8OEv3WAGzz9Au9fxNg
5. DLL 폴더의 MORT_CORE.DLL , nhocr.DLL 을 수정할려면 아래 관련된 프로젝트를 참고하시기 바랍니다

<!--[en]-->
1. Set the project to Release mode. Set the target CPU to X64.
2. Build the project first
3. Can't run after build. The reason is that there are no essential files required for run
4. Unzip the latest build files into the release folder to get the required files 
- https://drive.google.com/drive/folders/0BxO-Nrmd-kR7dVp5TWpMQ09jMFU?resourcekey=0-bx6_8OEv3WAGzz9Au9fxNg
5. MORT_CORE.DLL , nhocr.DLL To modify This dll, please refer to the related projects below

<!--[kr]-->
### 관련 프로젝트 ###
<!--[en]-->
### Related Project ###
<!--[common]-->

* MORT Core - MORT_CORE_DLL
  - https://github.com/kmonkeyhead/MORT_CORE
  
* MORT NHocr - nhocr.DLL
  - https://github.com/kmonkeyhead/MORT_NHOCR

<!--[kr] -->
## 그 외 ##
### 개발 상황 트렐로 ###
<!--[en] -->
## ETC ##
### Development Plans Trello ###
<!--[common]-->

- https://trello.com/b/gPa1EL5x/mort

<!--[kr]-->
### 문의 디스코드 ###
<!--[en]-->
### Discord ###
<!--[common]-->
- [Discord](https://discord.com/invite/ha5yNy9) ![Discord Badge](https://discord.com/api/guilds/742743719958151298/widget.png?style=shield)
