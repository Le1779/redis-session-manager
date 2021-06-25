# SSO Redis

## Auth Server

### 安裝Redis
- Redis官網沒有Windows版本，所以下載微軟的版本[安裝網址](https://github.com/microsoftarchive/redis/releases)。
- 安裝的過程中記得勾選要新增環境變數。

### 安裝套件
- `Microsoft.Web.RedisSessionStateProvid`
- `JWT`

### 首次登入
- https://auth.server?returnUrl=[登入成功之後要跳轉的頁面] 。
- 如果認證成功會產生一組 `Token` ，作為之後驗證的依據。
- `Token` 當作參數帶回跳轉前的頁面。

### 第二次登入
- https://auth.serverapi/auth/validate?token=[上面得到的Token]
- 驗證 `Token` 是否合法以及檢查認證是否過期。
- 如果驗證通過，延長認證的期限。

### 其他站台登入
假如 `ServerA` 已經走過一次登入的流程，這時候瀏覽器已經保存 `Auth Server` 的SessionId在Cookie中，所以當 `ServerB` 重新導向登入的流程，只要檢查對應的Session是否存在就可以檢查認證是否還有效。

### 登出
透過SessionID刪除Redis中的資料。

## Web Server

### 身分認證的接口 (Index)
- 接收 `Auth Server` 回傳的 `Token`。
- 將 `Token` 保存在 `Cookie` 中，在之後的訪問都帶上。
- 每次都會跟`Auth Server` 檢查 `Token` 是否合法。