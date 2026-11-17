# MHServerEmu 繁體中文指南

MHServerEmu 是《Marvel Heroes》的伺服器模擬器，重建了遊戲於 2017 年停止營運時的伺服器功能與內容。

## 下載與安裝

Windows 初次架設請從 [1.0.3 穩定版](https://github.com/Crypto137/MHServerEmu/releases/tag/1.0.3) 下載 [MHServerEmu-1.0.3.zip](https://github.com/Crypto137/MHServerEmu/releases/download/1.0.3/MHServerEmu-1.0.3.zip)，搭配 Win64 遊戲客戶端 `1.52.0.1700`。穩定版已附 Apache 與設定工具。Nightly 提供 Windows 和 Linux 構建，通常包含較新的功能，需自行準備設定。各客戶端版本的 Nightly 下載入口見[英文 README](./README.md#nightly)。

| 遊戲客戶端版本 | 支援程度 |
| --- | --- |
| 1.52.0.1700 | 完整支援 |
| 1.48.0.1712（BUE 之前） | 初步支援 |
| 1.53.0.203（測試伺服器） | 初步支援 |
| 1.4.0.26（PS4） | 初步支援 |

- [Windows 穩定版初次安裝](./docs/Setup/zh-TW/InitialSetup.md)
- [Windows 手動安裝](./docs/Setup/zh-TW/ManualSetup.md)
- [進階設定（區網、遠端、帳號、GC、載入提示）](./docs/Setup/zh-TW/AdvancedSetup.md)
- [Ubuntu 24.04 安裝](./docs/Setup/zh-TW/UbuntuSetup.md)

## 常見問題

**去哪裡取得遊戲客戶端？** 如果以前在 Steam 玩過，可查看自己的遊戲庫。上游專案未提供客戶端下載連結。

**如何更新？** 關閉伺服器，備份帳號資料庫 `MHServerEmu\Data\Account.db` 和自訂設定，再依發行說明覆蓋伺服器檔案。切換至 Nightly 時請選擇與客戶端相符的版本。

**有支援早期版本嗎？** 1.48 與 1.53 目前屬初步支援，可能遇到影響遊玩的問題。2013 年中 1.10 版的早期研究可參考 [MHServerEmu2013](https://github.com/Crypto137/MHServerEmu2013)。

**會新增英雄或升級畫面嗎？** 專案以還原原版遊戲的伺服器端為目標。公開的研究成果可供其他開發者延伸使用。

架設問題可至[上游 Discord](https://discord.gg/hjR8Bj52t3) 的 `#setup-help` 詢問。更多資訊見[開發部落格](https://crypto137.github.io/MHServerEmu/)及[英文技術文件](./docs/Index.md)。
