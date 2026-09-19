**한국어** | [English](README.en.md) | [日本語](README.md)

# TSMP Codec RGB20

RGB20은 RGB 채널을 더 촘촘하게 사용해 TSMP payload 밀도를 높이는 고밀도 코덱입니다. 화면 경로가 색을 안정적으로 보존할 때 더 많은 데이터를 한 프레임에 담기 위한 선택지입니다.

## 특징

- RGB 기반 20-bit TSMP 심볼
- RGB16보다 더 높은 payload 밀도
- 색상 보존 품질이 좋은 송출/수신 경로에 적합
- 더 큰 상태 패킷이나 더 많은 네트워크 컴포넌트를 사용하는 TSMP 구성에 유용
- `TSMPSetup` Codec 탭에서 자동 검색

## 요구 사항

- TSMP Core: https://github.com/kibalab/TSMP-Core
- `com.kibalab.tsmp.core` 1.0.0 이상
- Unity 2022.3
- VRChat에서 사용할 때만 Worlds SDK 3.9.0 이상 필요. 일반 Unity에는 SDK가 필요하지 않습니다.

## 설치

VRChat Creator Companion에서 VPM 저장소를 추가합니다.

```text
https://vpm.kiba.red/
```

그 다음 `TSMP Core`와 `TSMP Codec RGB20`을 설치합니다.

일반 Unity에서는 Unity Package Manager로 Core 1.0.0, 기본 코덱 Luma4, 이 코덱을 설치합니다. 로컬 저장소는 각 package.json을 Add package from disk로 추가할 수 있으며 VRCSDK를 설치할 필요가 없습니다. UPM은 Core 1.0.0을 지정하고, VPM은 Core 1.0.0 이상을 허용합니다.

## 사용 방법

1. Core 패키지의 `Packages/com.kibalab.tsmp.core/Samples/TSMPController.prefab`을 씬에 배치합니다.
2. `TSMPSetup`의 Codec 탭에서 자동 검색된 `RGB20`을 선택합니다.
3. 일반 Unity와 VRChat 모두 같은 방식으로 코덱과 머티리얼을 자동 준비합니다. 별도 변환 메뉴는 필요하지 않습니다.

## 배포 상태

RGB20 2.0.0은 직전 정식 버전 1.0.0 이후의 베타 변경사항을 모두 통합한 정식 버전입니다. Core 1.0.0을 먼저 설치하세요. VCC에서 시험판 표시를 켤 필요가 없습니다.

## 라이선스

MIT License. Copyright (c) 2026 KIBA_Labs.

## 준비 API 호환성

이 버전은 Core 1.0.0과 해당 코덱 준비·출력 API가 필요합니다. 코덱을 설치하기 전에 Core를 업데이트하세요. 준비 머티리얼이 없으면 기존 셰이더 경로를 사용할 수 있지만, 호환되지 않는 Core API를 대신하지는 못합니다.

UPM은 Core 1.0.0, VPM은 Core >=1.0.0을 사용합니다. 로컬/디스크 또는 Git 설치에서는 프로젝트 의존성에 Core도 직접 지정해야 합니다. 패키지 메타데이터만으로 UPM이 GitHub에서 Core를 가져오지는 않습니다. VRChat Worlds SDK는 VPM에서만 의존합니다.
