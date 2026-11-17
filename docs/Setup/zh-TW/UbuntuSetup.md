# Ubuntu 24.04 安裝

此流程使用 .NET 10 SDK 從原始碼建置伺服器，並使用 Apache2 提供客戶端設定及 HTTPS 登入。請準備 Marvel Heroes Win64 客戶端 `1.52.0.1700` 的遊戲資料。

## 安裝相依套件並建置

```bash
sudo apt update
sudo apt install -y git dotnet-sdk-10.0 apache2 openssl

git clone --branch 1.0.3 --depth 1 https://github.com/Crypto137/MHServerEmu.git
cd MHServerEmu
dotnet build MHServerEmu.sln -c Release
```

若套件管理員找不到 .NET 10 SDK，請依 [Microsoft 的 Ubuntu 安裝文件](https://learn.microsoft.com/zh-tw/dotnet/core/install/linux-ubuntu) 確認 Ubuntu 24.04 的套件來源。SDK 會一併安裝執行階段。

## 複製遊戲資料

以下指令從原始碼根目錄執行。將 `game_dir` 換成遊戲檔案的實際路徑。

```bash
game_dir="/path/to/Marvel Heroes"
server_dir="src/MHServerEmu/bin/Release/net10.0"
mkdir -p "$server_dir/Data/Game"
cp "$game_dir/Data/Game/Calligraphy.sip" "$game_dir/Data/Game/mu_cdata.sip" "$server_dir/Data/Game/"
```

## 設定 Apache

```bash
sudo a2enmod ssl proxy proxy_http rewrite
sudo mkdir -p /etc/apache2/ssl /var/www/mhserveremu
sudo openssl req -x509 -newkey rsa:2048 -nodes -days 3650 \
    -keyout /etc/apache2/ssl/server.key -out /etc/apache2/ssl/server.crt \
    -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost"
sudo install -m 644 assets/SiteConfig.xml /var/www/mhserveremu/SiteConfig.xml
```

建立 `/etc/apache2/sites-available/mhserveremu.conf`。HTTP 直接提供遊戲客戶端要讀取的設定檔，HTTPS 則將登入與其他遊戲網站請求轉送至模擬器。

```apache
<VirtualHost *:80>
    ServerName localhost
    DocumentRoot /var/www/mhserveremu
</VirtualHost>

<VirtualHost *:443>
    ServerName localhost
    DocumentRoot /var/www/mhserveremu

    SSLEngine on
    SSLCertificateFile /etc/apache2/ssl/server.crt
    SSLCertificateKeyFile /etc/apache2/ssl/server.key

    RewriteEngine on
    RewriteRule ^/AuthServer(.*) http://localhost:8080/AuthServer$1 [P]
    RewriteRule ^/MTXStore(.*) http://localhost:8080/MTXStore$1 [P]
    RewriteRule ^/AccountManagement(.*) http://localhost:8080/AccountManagement$1 [P]
    RewriteRule ^/ServerStatus(.*) http://localhost:8080/ServerStatus$1 [P]
    RewriteRule ^/RegionReport(.*) http://localhost:8080/RegionReport$1 [P]
    RewriteRule ^/Metrics(.*) http://localhost:8080/Metrics$1 [P]
    RewriteRule ^/Dashboard(.*) http://localhost:8080/Dashboard$1 [P]
</VirtualHost>
```

確認網站目錄內的 `SiteConfig.xml` 包含下列登入設定。

```xml
<str name="AuthServerAddress" value="localhost" />
<str name="AuthServerUrl" value="/AuthServer/Login/IndexPB" />
<str name="AuthServerPort" value="443" />
```

在專用主機上可停用 Apache 預設站台，再啟用新站台並驗證設定。

```bash
sudo a2dissite 000-default
sudo a2ensite mhserveremu
sudo apache2ctl configtest
sudo systemctl reload apache2
```

## 執行伺服器與連接客戶端

```bash
cd src/MHServerEmu/bin/Release/net10.0
dotnet MHServerEmu.dll
```

伺服器啟動後，在伺服器主機開啟 [http://localhost:8080/Dashboard/](http://localhost:8080/Dashboard/) 建立帳號。

用以下方式啟動 Windows 遊戲客戶端。若是在另一台電腦遊玩，將啟動參數與 `SiteConfig.xml` 的 `AuthServerAddress` 換成伺服器 IP 或網域，並依[進階設定](./AdvancedSetup.md)更新 `[Frontend]` 的 `PublicAddress`。Apache 憑證應與所用的位址相符。

```bat
MarvelHeroesOmega.exe -robocopy -nosteam -siteconfigurl=localhost/SiteConfig.xml
```

## 選用 systemd 服務

建立 `/etc/systemd/system/mhserveremu.service`。將 `User` 和 `WorkingDirectory` 改成實際的 Linux 帳號及伺服器目錄。該帳號需要對伺服器資料目錄有寫入權限。

```ini
[Unit]
Description=MHServerEmu
After=network.target

[Service]
Type=simple
User=mhserveremu
WorkingDirectory=/opt/MHServerEmu/src/MHServerEmu/bin/Release/net10.0
ExecStart=/usr/bin/dotnet MHServerEmu.dll
Restart=on-failure

[Install]
WantedBy=multi-user.target
```

先停止手動啟動的執行個體，再啟用服務。

```bash
sudo systemctl daemon-reload
sudo systemctl enable --now mhserveremu
```

## 疑難排解

- **伺服器無法啟動**。查看主控台，或執行 `journalctl -u mhserveremu`。常見原因是缺少遊戲資料，或 TCP 4306／8080 已被占用。
- **SQLite 預載時找不到 `libdl`**。執行 `whereis libdl.so.2` 查詢實際位置。若確認日誌是缺少 `libdl`，可從原始碼根目錄執行下方指令建立符號連結。範例適用於 x64，請依實際路徑調整。
- **客戶端無法連線**。確認 Apache 執行中，並檢查 `SiteConfig.xml`、`[Frontend]` 的 `BindIP` 與 `PublicAddress`，以及所需連接埠。

```bash
ln -s /usr/lib/x86_64-linux-gnu/libdl.so.2 src/MHServerEmu/bin/Release/net10.0/runtimes/linux-x64/native/libdl.so
```
