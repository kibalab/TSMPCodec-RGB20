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
- `com.kibalab.tsmp.core` 0.0.1 이상
- VRChat Worlds SDK 3.9.0 이상

## 설치

VRChat Creator Companion에서 VPM 저장소를 추가합니다.

```text
https://vpm.kiba.red/
```

그 다음 `TSMP Core`와 `TSMP Codec RGB20`을 설치합니다.

## 사용 방법

1. Core 패키지의 `Packages/com.kibalab.tsmp.core/Samples/TSMPController.prefab`을 씬에 배치합니다.
2. `TSMPSetup`의 Codec 탭에서 `Refresh Codecs`를 누릅니다.
3. `RGB20`을 선택합니다.
4. `Apply Setup`을 실행합니다.

## 배포 상태

현재 beta 단계이며 패키지 버전과 Git 태그는 `v0.0.x-beta.x` 형식을 사용합니다.

## 라이선스

MIT License. Copyright (c) 2026 KIBA_Labs.
