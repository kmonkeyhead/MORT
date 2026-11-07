[CmdletBinding()]
param(
    [string]$RepositoryRoot,
    [switch]$Check
)

$ErrorActionPreference = "Stop"
if ([string]::IsNullOrWhiteSpace($RepositoryRoot)) {
    $RepositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot ".."))
}

$wikiDirectory = Join-Path $RepositoryRoot "docs\wiki"
$contentPath = Join-Path $wikiDirectory "wiki-content.json"
$featureContentPath = Join-Path $wikiDirectory "feature-content.json"
$fileOverridesPath = Join-Path $wikiDirectory "file-overrides.json"
$templatePath = Join-Path $wikiDirectory "template-md.html"
$outputPath = Join-Path $wikiDirectory "index.html"

function Get-RelativePath([string]$BasePath, [string]$TargetPath) {
    $baseUri = New-Object Uri(($BasePath.TrimEnd("\") + "\"))
    $targetUri = New-Object Uri($TargetPath)
    [Uri]::UnescapeDataString($baseUri.MakeRelativeUri($targetUri).ToString())
}

function ConvertTo-SafeJson($Value) {
    ($Value | ConvertTo-Json -Depth 30 -Compress).
        Replace("&", "\u0026").Replace("<", "\u003c").Replace(">", "\u003e")
}

function Get-FileExplanation([string]$Path, [string[]]$TypeNames, [string[]]$MethodNames) {
    $name = [IO.Path]::GetFileNameWithoutExtension($Path)
    $typeText = if ($TypeNames.Count) { $TypeNames -join ", " } else { $name }
    $methodText = if ($MethodNames.Count) { ($MethodNames | Select-Object -First 8) -join ", " } else { "연결된 partial 파일 또는 선언 소비자" }
    $result = [ordered]@{
        category = "기타 구현"
        intent = "$typeText 관련 동작을 현재 프로젝트 구조 안에서 제공한다."
        operation = "주요 실행 지점은 $methodText 이며, 호출자는 파일에 선언된 타입과 상태를 통해 결과를 사용한다."
    }

    switch -Regex ($Path) {
        "\.Designer\.cs$" {
            $result.category = "WinForms UI 선언"
            $result.intent = "$typeText 화면의 컨트롤 생성, 배치, 기본 속성과 이벤트 연결을 보관한다. 사람이 작성한 동작 코드는 같은 이름의 code-behind 파일에 둔다."
            $result.operation = "InitializeComponent가 컨트롤을 만들고 계층에 추가한 뒤 이벤트 핸들러를 연결한다. 런타임 동작은 연결된 partial 클래스가 처리한다."
            break
        }
        "^MORT/Program\.cs$" {
            $result.category = "애플리케이션 시작"
            $result.intent = "프로세스 중복 실행, 업데이트 잔여 파일, DI 컨테이너와 메인 메시지 루프를 초기화하는 실행 진입점이다."
            $result.operation = "Main이 실행 조건을 검사하고 ConfigureServices로 Singleton 서비스를 조립한 뒤 Form1을 실행한다."
            break
        }
        "^MORT/Form1\.cs$" {
            $result.category = "메인 UI 조정"
            $result.intent = "사용자 입력과 설정을 각 Manager·Service·보조 폼으로 연결하는 메인 WinForms 조정자다."
            $result.operation = "폼 초기화 후 설정과 서비스를 연결하고, 버튼·메뉴·단축키 이벤트를 OCR 시작, 번역, 폼 관리 명령으로 전달한다. 주요 지점은 $methodText 이다."
            break
        }
        "^MORT/(SettingManager|AdvencedOptionManager)\.cs$" {
            $result.category = "호환 설정"
            $result.intent = "기존 사용자 설정 파일과 호환되는 키·enum·기본값을 읽고 실행 중 설정 상태로 제공한다."
            $result.operation = "설정 파일을 키별로 파싱해 상태를 채우고 UI와 서비스가 읽도록 노출하며, 저장 시 기존 직렬화 이름과 @KEY 형식을 유지한다."
            break
        }
        "^MORT/Manager/FormManager\.cs$" {
            $result.category = "폼 수명 관리"
            $result.intent = "번역창, OCR 영역, 설정창과 보조 UI의 단일 참조·목록·생성·파괴 순서를 관리한다."
            $result.operation = "Make/Show 메서드가 폼을 만들고 참조를 보관하며, Destroy 계열이 Dispose 상태와 목록을 정리한다. GetITransform은 현재 스킨 폼을 공통 인터페이스로 반환한다."
            break
        }
        "^MORT/Manager/OcrManager\.cs$" {
            $result.category = "OCR 조정"
            $result.intent = "Google OCR 자격 증명과 EasyOCR 준비를 포함해 OCR 엔진별 초기화·사용 가능 상태·호출을 조정한다."
            $result.operation = "선택 엔진에 필요한 런타임과 키를 준비하고 캡처 바이트를 전달한 뒤 엔진 결과 모델을 반환한다."
            break
        }
        "^MORT/Manager/OCRDataManager\.cs$" {
            $result.category = "OCR 결과 모델링"
            $result.intent = "엔진마다 다른 단어·줄·좌표 결과를 영역별 공통 모델로 정규화하고 번역 결과와 결합한다."
            $result.operation = "AddData가 OCR 결과를 ResultData로 만들고 InitLine이 읽기 순서와 병합을 계산하며 ApplyTransResult가 번역 문자열을 위치 데이터에 대응시킨다."
            break
        }
        "^MORT/Manager/TransManager\.cs$" {
            $result.category = "번역 조정"
            $result.intent = "번역 방식 선택, 언어 코드, 사용자 사전, 이전 결과 캐시와 제공자별 API 호출을 하나의 진입점에서 조정한다."
            $result.operation = "StartTrans가 입력을 준비하고 GetTransLinesAsync가 TransType별 구현을 호출한 뒤 성공 결과를 캐시에 반영한다."
            break
        }
        "^MORT/Service/ProcessTranslateService/" {
            $result.category = "OCR·번역 파이프라인"
            $result.intent = "$typeText 타입으로 캡처, OCR, 문자열 비교, 번역, 출력 갱신의 실행 흐름과 세션 상태를 담당한다."
            $result.operation = "처리 스레드와 취소 상태를 준비한 뒤 영역별 OCR 결과를 만들고 변경된 문장을 번역해 현재 스킨 폼에 전달한다. 주요 지점은 $methodText 이다."
            break
        }
        "^MORT/Service/" {
            $result.category = "비즈니스 서비스"
            $result.intent = "$typeText 기능을 UI와 API 구현에서 분리해 DI로 재사용할 수 있게 제공한다."
            $result.operation = "생성자에서 필요한 협력 객체를 받고 $methodText 흐름으로 상태를 준비하거나 기능 결과를 반환한다."
            break
        }
        "^MORT/TransAPI/" {
            $result.category = "번역 제공자 어댑터"
            $result.intent = "$typeText 번역 방식의 인증, 요청 생성, 응답 파싱과 제공자 고유 오류 처리를 캡슐화한다."
            $result.operation = "초기화 메서드가 키와 언어 코드를 저장하고, 번역 메서드가 요청을 보낸 뒤 응답을 문자열과 오류 상태로 변환한다. 주요 지점은 $methodText 이다."
            break
        }
        "^MORT/OcrApi/" {
            $result.category = "OCR 엔진 어댑터"
            $result.intent = "$typeText OCR 엔진의 네이티브·플랫폼 API 초기화, 이미지 변환, 인식 결과 추출을 담당한다."
            $result.operation = "엔진 런타임을 준비하고 픽셀 데이터를 요구 형식으로 변환한 뒤 텍스트와 바운딩 정보를 반환한다. 주요 지점은 $methodText 이다."
            break
        }
        "^MORT/ScreenCapture/" {
            $result.category = "화면 캡처"
            $result.intent = "$typeText 타입으로 Windows Graphics Capture, Direct3D 상호 운용 또는 캡처 UI의 한 단계를 담당한다."
            $result.operation = "대상 창/모니터를 선택하고 프레임을 받아 CPU 바이트와 화면 좌표로 변환한다. 주요 지점은 $methodText 이다."
            break
        }
        "^MORT/Model/" {
            $result.category = "데이터 모델"
            $result.intent = "$typeText 데이터의 필드와 전달 계약을 정의해 서비스·매니저·UI 사이에서 같은 의미를 공유한다."
            $result.operation = "행동보다 데이터 보존이 중심이며 생성자·record 값 또는 속성으로 입력을 받아 소비자가 읽는다."
            break
        }
        "^MORT/LocalizeManager/" {
            $result.category = "로컬라이즈"
            $result.intent = "$typeText 타입으로 앱 언어 선택, 로컬라이즈 데이터 조회 또는 폼의 번역 계약을 제공한다."
            $result.operation = "CSV에서 키별 언어 값을 읽고 현재 AppLanguage에 맞는 문자열을 폼에 반환한다."
            break
        }
        "^MORT/ColorThief/" {
            $result.category = "색상 분석"
            $result.intent = "$typeText 타입으로 이미지 색상을 양자화해 Overlay 글자와 배경에 사용할 대표 색상 후보를 계산한다."
            $result.operation = "픽셀을 색상 공간의 박스로 나누고 빈도 기준으로 병합·정렬해 팔레트를 반환한다."
            break
        }
        "^CloudVision/" {
            $result.category = "Google OCR 래퍼"
            $result.intent = "$typeText 타입으로 Google Cloud Vision 요청과 응답 모델을 메인 앱에서 쓰기 쉬운 OCR 결과로 변환한다."
            $result.operation = "자격 증명으로 Vision 클라이언트를 준비하고 이미지 요청을 보낸 뒤 텍스트와 좌표 결과를 반환한다."
            break
        }
        "^GSTrans/" {
            $result.category = "Google Sheets 번역"
            $result.intent = "$typeText 타입으로 Google Sheets 기반 사용자 번역 데이터의 인증, 조회와 토큰 관리를 제공한다."
            $result.operation = "OAuth 토큰을 준비해 시트 범위를 읽고 원문-번역 매핑을 메인 앱에 반환한다."
            break
        }
        "^Updater/" {
            $result.category = "업데이트"
            $result.intent = "$typeText 타입으로 메인 앱 종료 이후 업데이트 파일 다운로드, 교체, 재실행 과정을 담당한다."
            $result.operation = "인자로 받은 경로와 버전을 확인하고 다운로드 진행을 표시한 뒤 잠금이 풀린 파일을 교체한다."
            break
        }
        "^MORT/.+(Form|UI|Dialog|Page)\.cs$|^MORT/(TransForm|OcrAreaForm|screenForm|Rtt|About|DonatePage)\.cs$" {
            $result.category = "사용자 인터페이스"
            $result.intent = "$typeText 화면에서 사용자 입력을 받고 설정 또는 실행 기능을 관련 Manager·Service에 전달한다."
            $result.operation = "생성 시 현재 설정을 컨트롤에 반영하고 이벤트 핸들러가 $methodText 동작을 호출한다."
            break
        }
    }
    $result
}

if (-not (Test-Path -LiteralPath $contentPath) -or
    -not (Test-Path -LiteralPath $featureContentPath) -or
    -not (Test-Path -LiteralPath $fileOverridesPath) -or
    -not (Test-Path -LiteralPath $templatePath)) {
    throw "Wiki content, feature content, or template is missing."
}

$content = Get-Content -LiteralPath $contentPath -Raw -Encoding UTF8 | ConvertFrom-Json
$features = Get-Content -LiteralPath $featureContentPath -Raw -Encoding UTF8 | ConvertFrom-Json
$fileOverrides = Get-Content -LiteralPath $fileOverridesPath -Raw -Encoding UTF8 | ConvertFrom-Json
$excluded = "[\\/](bin|obj|\.git|packages)[\\/]"
$sourceFiles = @(
    Get-ChildItem -LiteralPath $RepositoryRoot -Recurse -File -Filter "*.cs" |
        Where-Object { $_.FullName -notmatch $excluded -and (Get-RelativePath $RepositoryRoot $_.FullName) -notlike 'PipeClient/*' } |
        Sort-Object FullName
)

$types = New-Object Collections.Generic.List[object]
$markers = New-Object Collections.Generic.List[object]
$fileFacts = New-Object Collections.Generic.List[object]
$directoryCounts = @{}
$latestSourceWrite = [DateTime]::MinValue
$manualExplanationCount = 0

foreach ($file in $sourceFiles) {
    $relativePath = Get-RelativePath $RepositoryRoot $file.FullName
    $lines = [IO.File]::ReadAllLines($file.FullName)
    $text = [string]::Join([Environment]::NewLine, $lines)
    $topDirectory = ($relativePath -split "/")[0]
    if (-not $directoryCounts.ContainsKey($topDirectory)) { $directoryCounts[$topDirectory] = 0 }
    $directoryCounts[$topDirectory]++
    if ($file.LastWriteTimeUtc -gt $latestSourceWrite) { $latestSourceWrite = $file.LastWriteTimeUtc }

    $typeMatches = [regex]::Matches(
        $text,
        "(?m)^\s*(?:(?:public|internal|private|protected|static|sealed|abstract|partial|unsafe)\s+)*(class|interface|record|enum)\s+([A-Za-z_][A-Za-z0-9_]*)"
    )
    $localTypes = @($typeMatches | ForEach-Object { $_.Groups[2].Value } | Select-Object -Unique)
    foreach ($match in $typeMatches) {
        $lineNumber = ($text.Substring(0, $match.Index) -split "`n").Count
        $types.Add([ordered]@{
            kind = $match.Groups[1].Value
            name = $match.Groups[2].Value
            path = $relativePath
            line = $lineNumber
        })
    }

    $methodDetails = New-Object Collections.Generic.List[object]
    $methodMatches = [regex]::Matches(
        $text,
        "(?m)^\s*(public|internal|protected|private)\s+(?:(?:static|async|unsafe|virtual|override|sealed|new|extern|partial)\s+)*(?:[A-Za-z_][A-Za-z0-9_<>\[\],?.() ]+\s+)([A-Za-z_][A-Za-z0-9_]*)\s*\(([^;\r\n]*)\)"
    )
    foreach ($match in $methodMatches) {
        $lineNumber = ($text.Substring(0, $match.Index) -split "`n").Count
        $signature = $match.Value.Trim()
        if ($signature.Length -gt 220) { $signature = $signature.Substring(0, 217) + "..." }
        $signature = [regex]::Replace($signature, '"(?:\\.|[^\"])*"', '"…"')
        $signature = [regex]::Replace($signature, "'(?:\\.|[^'])*'", "'…'")
        $methodDetails.Add([ordered]@{
            access = $match.Groups[1].Value
            name = $match.Groups[2].Value
            line = $lineNumber
            signature = $signature
        })
    }

    $methodNames = @($methodDetails | ForEach-Object { $_.name } | Select-Object -Unique)
    $dependencies = @(
        [regex]::Matches($text, "(?m)^\s*using\s+([A-Za-z_][A-Za-z0-9_.]+)\s*;") |
            ForEach-Object { $_.Groups[1].Value } |
            Select-Object -Unique
    )
    $explanation = Get-FileExplanation $relativePath $localTypes $methodNames
    $overrideProperty = $fileOverrides.PSObject.Properties[$relativePath]
    $documentationSource = "automatic"
    if ($null -ne $overrideProperty) {
        $explanation = $overrideProperty.Value
        $documentationSource = "manual"
        $manualExplanationCount++
    }
    $fileFacts.Add([ordered]@{
        path = $relativePath
        lines = $lines.Count
        updatedUtc = $file.LastWriteTimeUtc.ToString("yyyy-MM-ddTHH:mm:ssZ")
        category = $explanation.category
        intent = $explanation.intent
        operation = $explanation.operation
        generated = $relativePath.EndsWith(".Designer.cs")
        documentationSource = $documentationSource
        types = [object[]]$localTypes
        methods = [object[]]$methodDetails
        dependencies = [object[]]$dependencies
    })

    for ($index = 0; $index -lt $lines.Count; $index++) {
        if ($lines[$index] -match "\b(TODO|FIXME|HACK|QUESTION)\b") {
            $markers.Add([ordered]@{
                tag = $Matches[1]
                path = $relativePath
                line = $index + 1
                text = "$($Matches[1]) 주석 내용은 개인정보 유입 방지를 위해 위키에 포함하지 않는다."
            })
        }
    }
}

$registrations = New-Object Collections.Generic.List[object]
$programPath = Join-Path $RepositoryRoot "MORT\Program.cs"
if (Test-Path -LiteralPath $programPath) {
    $programText = Get-Content -LiteralPath $programPath -Raw -Encoding UTF8
    foreach ($match in [regex]::Matches($programText, "services\.Add(Singleton|Scoped|Transient)<([^>]+)>")) {
        $registrations.Add([ordered]@{
            lifetime = $match.Groups[1].Value
            service = $match.Groups[2].Value.Trim()
        })
    }
}

$directorySummary = @(
    $directoryCounts.GetEnumerator() | Sort-Object Name |
        ForEach-Object { [ordered]@{ name = $_.Name; files = $_.Value } }
)
$reflectedSourceFileCount = $fileFacts.Count
$codeReflectionRate = if ($sourceFiles.Count -eq 0) { 100.0 } else { [Math]::Round(($reflectedSourceFileCount * 100.0) / $sourceFiles.Count, 1) }
$manualExplanationRate = if ($sourceFiles.Count -eq 0) { 0.0 } else { [Math]::Round(($manualExplanationCount * 100.0) / $sourceFiles.Count, 1) }
$scan = [ordered]@{
    latestSourceWriteUtc = if ($latestSourceWrite -eq [DateTime]::MinValue) { $null } else { $latestSourceWrite.ToString("yyyy-MM-ddTHH:mm:ssZ") }
    sourceFileCount = $sourceFiles.Count
    typeCount = $types.Count
    methodCount = ($fileFacts | ForEach-Object { $_.methods.Count } | Measure-Object -Sum).Sum
    markerCount = $markers.Count
    coverage = [ordered]@{
        eligibleSourceFiles = $sourceFiles.Count
        reflectedSourceFiles = $reflectedSourceFileCount
        codeReflectionRate = $codeReflectionRate
        manuallyDocumentedFiles = $manualExplanationCount
        manualExplanationRate = $manualExplanationRate
        automaticallyDocumentedFiles = $reflectedSourceFileCount - $manualExplanationCount
        excludedDirectories = @("bin", "obj", ".git", "packages")
    }
    directories = $directorySummary
    registrations = [object[]]$registrations
    largestFiles = @($fileFacts | Sort-Object lines -Descending | Select-Object -First 15)
    types = [object[]]$types
    markers = [object[]]$markers
    files = [object[]]$fileFacts
}

$payload = ConvertTo-SafeJson ([ordered]@{
    content = $content
    features = [object[]]$features
    scan = $scan
})
$template = Get-Content -LiteralPath $templatePath -Raw -Encoding UTF8
if (-not $template.Contains("__WIKI_PAYLOAD__")) {
    throw "Wiki template does not contain __WIKI_PAYLOAD__."
}
$generated = $template.Replace("__WIKI_PAYLOAD__", $payload).Replace("`r`n", "`n")

if ($Check) {
    if (-not (Test-Path -LiteralPath $outputPath)) {
        throw "Wiki output is missing. Run tools/update-wiki.ps1."
    }
    $existing = [IO.File]::ReadAllText($outputPath).Replace("`r`n", "`n")
    if ($existing -ne $generated) {
        throw "Wiki output is stale. Run tools/update-wiki.ps1 and include docs/wiki/index.html."
    }
    Write-Host "MORT implementation wiki is current."
    return
}

$utf8WithoutBom = New-Object Text.UTF8Encoding($false)
[IO.Directory]::CreateDirectory($wikiDirectory) | Out-Null
[IO.File]::WriteAllText($outputPath, $generated, $utf8WithoutBom)
Write-Host "Updated detailed MORT implementation wiki: $outputPath"
