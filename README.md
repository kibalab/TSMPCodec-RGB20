[한국어](README.ko.md) | [English](README.en.md) | **日本語**

# TSMP Codec RGB20

RGB20 は RGB チャンネルをより高密度に使い、1 フレームにより多くの payload を格納する TSMP codec です。キャプチャと受信経路が色を安定して保持できる場合に使用します。

## 特徴

- RGB ベースの 20-bit TSMP シンボル
- RGB16 より高い payload 密度
- 色再現性が安定した配信/受信経路に適合
- 大きな状態 packet や多数の network component を使う TSMP 構成に有用
- `TSMPSetup` の Codec タブで自動検出

## 要件

- TSMP Core: https://github.com/kibalab/TSMP-Core
- `com.kibalab.tsmp.core` 0.3.0-beta.2 以降
- Unity 2022.3
- VRChat で使用する場合のみ Worlds SDK 3.9.0 以降が必要です。通常の Unity には不要です。

## インストール

VRChat Creator Companion で VPM リポジトリを追加します。

```text
https://vpm.kiba.red/
```

その後、`TSMP Core` と `TSMP Codec RGB20` をインストールします。

通常の Unity では Unity Package Manager で Core 0.3.0-beta.2、基本 codec の Luma4、この codec をインストールします。ローカル checkout は各 package.json を Add package from disk で追加できます。VRCSDK は不要です。UPM は Core 0.3.0-beta.2 を指定し、VPM は Core 0.3.0-beta.2 以降を許可します。

## 使い方

1. Core パッケージの `Packages/com.kibalab.tsmp.core/Samples/TSMPController.prefab` をシーンに配置します。
2. `TSMPSetup` の Codec タブで自動検出された `RGB20` を選択します。
3. 通常の Unity と VRChat の両方で codec と material が自動準備されます。変換メニューは不要です。

## リリース状態

このパッケージは beta 段階で、`v0.0.x-beta.x` 形式のタグを使用します。

## ライセンス

MIT License. Copyright (c) 2026 KIBA_Labs.

## 準備 API の互換性

このソースには、現在リリース候補の Core 0.3.0-beta.2 が必要です。Core 0.2.0 と 0.3.0-beta.1 には `PrepareDecode` がなく、準備マテリアルを未設定にしてもコンパイルできません。このコーデックより先に対応 Core を公開・インストールしてください。準備マテリアル不足時の従来シェーダーへの fallback は、コンパイル後にのみ機能します。

UPM にはバージョン文字列、VPM には範囲を指定します。ローカル/ディスクまたは Git インストールでは、プロジェクトの依存関係に対応 Core も直接指定します。パッケージのメタデータだけでは UPM は GitHub から Core を取得しません。VPM ベータは公開後に試験版表示を有効にし、対応バージョンを選んでください。
