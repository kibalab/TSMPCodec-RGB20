[한국어](README.md) | [English](README.en.md) | **日本語**

# TSMP Codec RGB20

TSMP の RGB20 コーデック パッケージです。

このパッケージは TSMP Core と一緒に使用します。`TSMPSetup` の Codec タブで自動検出される RGB20 codec handler、decode shader、material、prefab、catalog asset を提供します。

## インストール

VRChat Creator Companion に VPM リポジトリを追加します。

```text
https://vpm.kiba.red/
```

その後、`TSMP Codec RGB20` パッケージをインストールします。

## 要件

- `com.kibalab.tsmp.core` 0.0.1 以降
- VRChat Worlds SDK 3.9.0 以降

## 使い方

1. TSMP Core の `TSMPController.prefab`、または同等の TSMP 構成をシーンに追加します。
2. `TSMPSetup` の Codec タブを開きます。
3. `Refresh Codecs` を押します。
4. `RGB20` が一覧に表示されていることを確認して選択します。
5. `Apply Setup` を実行します。

RGB20 は payload 容量を多く必要とする TSMP ストリーム向けの高密度コーデックです。デコード負荷が高くても、1 フレームにより多くのデータを入れたい場合に使用します。

## リリース

このリポジトリは、バージョンタグを push すると GitHub Actions が release artifact を作成し、VPM backend にパッケージを登録するように設定されています。

タグ名は `package.json` の `version` と一致している必要があります。

例:

```bash
git tag v1.0.0
git push origin v1.0.0
```
