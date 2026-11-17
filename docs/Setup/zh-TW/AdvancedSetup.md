# 進階設定

首次架設可參考[初次安裝](./InitialSetup.md)。

## 允許區網連線

1. 在伺服器電腦開啟命令提示字元，執行 `ipconfig`，找出連線中網路介面的 IPv4 位址。以下以 `192.168.1.2` 為例。
2. 在 `MHServerEmu\Config.ini` 同一層編輯或建立 `ConfigOverride.ini`。在既有的 `[Frontend]` 區段加入或修改 `PublicAddress`。

   ```ini
   [Frontend]
   PublicAddress=192.168.1.2
   ```

3. 編輯 `Apache24\htdocs\SiteConfig.xml`，更新驗證伺服器位址。

   ```xml
   <str name="AuthServerAddress" value="192.168.1.2" />
   ```

4. 重新啟動伺服器。其他電腦啟動遊戲時使用 `-siteconfigurl=192.168.1.2/SiteConfig.xml`。若連線被防火牆擋住，需允許客戶端連入 Apache 的 TCP 80／443 及遊戲前端的 TCP 4306。

## 允許外網連線

依上面的步驟設定，將位址換成其他玩家可存取的公網 IP 或網域。依實際架構設定防火牆與路由器連接埠轉送。預設 TCP 80 提供 `SiteConfig.xml`，TCP 443 處理登入，TCP 4306 提供遊戲連線。前端連接埠可由 `Config.ini` 查閱。後端 Dashboard 可留在本機的 TCP 8080。

## 管理帳號

可在伺服器主控台或具備權限的遊戲聊天視窗執行 `!` 指令：

- `!account create [email] [playerName] [password]` 建立帳號。電子郵件和玩家名稱各自必須唯一。
- `!account userlevel [email] [0|1|2]` 設定權限。0 為一般玩家，1 為版主，2 為管理員。高權限可執行更多指令，包括管理帳號與關閉伺服器。
- `!account password [email] [newPassword]` 更改密碼。

輸入 `!commands` 或參考[伺服器指令](../../ServerEmu/ServerCommands.md)。

## 啟用伺服器垃圾回收

上游建議約 50 名以上玩家時考慮啟用 .NET Server GC。在 `MHServerEmu.exe` 同一層的 `MHServerEmu.runtimeconfig.json` 中，於 `configProperties` 加入 `"System.GC.Server": true`。Server GC 可能使用較多記憶體。可在同一區段加入 `"System.GC.HeapHardLimitPercent": 80`，將 GC 受控堆積的上限設為可用記憶體的 80%。修改後重新啟動伺服器。詳見 [.NET 文件](https://learn.microsoft.com/zh-tw/dotnet/standard/garbage-collection/workstation-server-gc)。

## 設定載入畫面提示

1. 將 [LiveLoadingTips.xml](../../../assets/LiveLoadingTips.xml) 複製到 `Apache24\htdocs`。
2. 在 `SiteConfig.xml` 將 `EnableLiveTips` 和 `EnableLiveTipsDownloader` 設為 `true`。
3. 將 `LoadScreenTipsURL` 設為 `http://localhost/LiveLoadingTips.xml`。若有其他玩家連線，使用玩家可存取的伺服器位址。
4. 視需要修改 `LiveTipsQueryInterval`；預設每 15 分鐘查詢一次。
5. 編輯 `LiveLoadingTips.xml` 加入提示。文字需符合客戶端的[語系代碼](../../GameData/Locale.md)。每次發布新內容時，同步更新 XML 根節點的 `Date`，客戶端才會重新載入。
