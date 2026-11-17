# Windows 穩定版初次安裝

## 準備

1. 準備 Marvel Heroes Win64 客戶端 `1.52.0.1700`。若 Steam 收藏庫仍有遊戲，可右鍵點選遊戲，選擇「管理」→「瀏覽本機檔案」。遊戲根目錄應含有 `UnrealEngine3\Binaries\Win64\MarvelHeroesOmega.exe` 及 `Data\Game`。
2. 伺服器需 [.NET 10 Runtime](https://dotnet.microsoft.com/zh-tw/download/dotnet/10.0)（x64），安裝 Windows Desktop Runtime 10 也會一併提供。1.0.3 隨附的 Setup.exe 另需 [.NET 8 Windows Desktop Runtime](https://dotnet.microsoft.com/zh-tw/download/dotnet/8.0)（x64）。可用 `dotnet --list-runtimes` 確認已安裝的版本。
3. 下載並解壓縮 [MHServerEmu-1.0.3.zip](https://github.com/Crypto137/MHServerEmu/releases/download/1.0.3/MHServerEmu-1.0.3.zip)。可用 PowerShell 的 `Get-FileHash .\MHServerEmu-1.0.3.zip -Algorithm SHA256` 核對雜湊值：

   ```text
   dc41a01dcad4a6ced057b6fe1a05baa79b739cf0c574bf4dca2c0d2d4bbadc97
   ```

4. 在解壓縮後的根目錄執行 `Setup.exe`，選取遊戲根目錄。工具會驗證執行檔版本，複製 `Calligraphy.sip` 與 `mu_cdata.sip`，並產生四個批次檔。

## 啟動與登入

1. 執行 `StartServer.bat`。等待模擬器完成初始化。另一個預設最小化的視窗是 Apache。
2. 開啟 [http://localhost:8080/Dashboard/](http://localhost:8080/Dashboard/) 建立帳號，再執行 `StartClient.bat`，用該帳號登入。本機測試也可直接執行 `StartClientAutoLogin.bat`，以預設帳號登入。
3. 遊玩結束後執行 `StopServer.bat` 停止伺服器。

若 Setup 提示版本不符，請檢查所選的是 1.52.0.1700 Win64 客戶端。若提示缺少遊戲資料，檢查客戶端 `Data\Game` 裡的兩個 `.sip` 檔案。若能開啟 Dashboard 但無法登入，確認 Apache 已啟動，而且 [http://localhost/SiteConfig.xml](http://localhost/SiteConfig.xml) 可讀取。

## 更新與備份

1. 停止伺服器。備份 `MHServerEmu\Data\Account.db`，以及 `MHServerEmu\ConfigOverride.ini` 和自行修改過的 Apache 設定、`SiteConfig.xml`。
2. 閱讀[發行說明](https://github.com/Crypto137/MHServerEmu/releases)確認有無移轉步驟。要改用 Nightly 時，可選用 [Windows 1.52 Nightly](https://nightly.link/Crypto137/MHServerEmu/workflows/nightly-release-windows-v52/master?preview)，將其伺服器檔案覆蓋到既有的 `MHServerEmu` 子目錄。
3. 自訂設定寫在與 `Config.ini` 同一層的 `ConfigOverride.ini`。兩者格式相同，更新時可保留覆寫設定。

其他安裝方式請參考[手動安裝](./ManualSetup.md)及[進階設定](./AdvancedSetup.md)。
