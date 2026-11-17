# Windows 手動安裝

本指南適合自行配置 Apache 與伺服器的使用者。使用 Windows 穩定版可依[初次安裝](./InitialSetup.md)操作。

## 設定伺服器

1. 從 [Apache Lounge](https://www.apachelounge.com/download/) 下載支援 OpenSSL 的 Apache Win64。
2. 將 `Apache24` 解壓縮到 `C:\Apache24`。使用其他路徑時，調整 `Apache24\conf\httpd.conf` 中的 `SRVROOT`。
3. 在 `Apache24\conf\httpd.conf` 啟用下列六行。若行首有 `#`，移除該註解符號。

   ```apache
   LoadModule rewrite_module modules/mod_rewrite.so
   LoadModule proxy_module modules/mod_proxy.so
   LoadModule proxy_http_module modules/mod_proxy_http.so
   LoadModule ssl_module modules/mod_ssl.so
   LoadModule socache_shmcb_module modules/mod_socache_shmcb.so
   Include conf/extra/httpd-ssl.conf
   ```

4. 在 `Apache24\conf\extra\httpd-ssl.conf` 的 `<VirtualHost _default_:443>` 區段加入登入轉送規則。1.0.3 內附的 `SiteConfig.xml` 使用 `/AuthServer/Login/IndexPB`，此處保留 `/AuthServer` 路徑，與 1.0.3 安裝包一致。

   ```apache
   RewriteEngine on
   RewriteRule ^/AuthServer(.*) http://localhost:8080/AuthServer$1 [P]
   ```

5. 將 [server.crt](../../../assets/ssl/server.crt) 與 [server.key](../../../assets/ssl/server.key) 放在 `Apache24\conf`。也可使用自己的 TLS 憑證與金鑰，並將 Apache 設定指向這兩個檔案。
6. 將 [SiteConfig.xml](../../../assets/SiteConfig.xml) 放在 `Apache24\htdocs`。
7. 準備伺服器程式。可使用 [1.0.3 穩定版](https://github.com/Crypto137/MHServerEmu/releases/tag/1.0.3)、[Windows 1.52 Nightly](https://nightly.link/Crypto137/MHServerEmu/workflows/nightly-release-windows-v52/master?preview)，或以 .NET 10 SDK 從原始碼建置。執行 1.0.3 需 .NET 10 Runtime。
8. 將遊戲客戶端 `Data\Game` 裡的 `Calligraphy.sip` 與 `mu_cdata.sip` 複製到伺服器執行檔所在目錄下的 `Data\Game`。

## 啟動與登入

1. 執行 `Apache24\bin\httpd.exe`，再啟動 MHServerEmu 並等待載入。
2. 在 [http://localhost:8080/Dashboard/](http://localhost:8080/Dashboard/) 建立帳號。
3. 啟動遊戲時加上 `-siteconfigurl=localhost/SiteConfig.xml`，用剛建立的電子郵件和密碼登入。若要獨立啟動，執行 `MarvelHeroesOmega.exe -robocopy -nosteam -siteconfigurl=localhost/SiteConfig.xml`。
4. 登入成功時，伺服器主控台會顯示客戶端連線資訊。

自訂伺服器參數可寫入 `ConfigOverride.ini`，設定名稱可查同目錄的 `Config.ini`。區網和公開連線請參考[進階設定](./AdvancedSetup.md)。
