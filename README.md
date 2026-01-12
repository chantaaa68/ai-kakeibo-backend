# ai-kakeibo-backend

AI駆動開発で作成された家計簿(Kakeibo)アプリケーションのバックエンドAPI

## 技術スタック

- **.NET 8.0** - 最新の.NETフレームワーク
- **ASP.NET Core Web API** - RESTful APIフレームワーク
- **Entity Framework Core 8.0** - O/Rマッピング
- **SQL Server** - データベース
- **Swagger/OpenAPI** - API仕様とドキュメント
- **Docker** - コンテナ化

## プロジェクト構成

```
src/AiKakeiboBackend/
├── Controllers/         # APIコントローラー
├── Models/             # エンティティモデル
│   ├── Users.cs
│   ├── Kakeibo.cs
│   ├── Category.cs
│   ├── CategoryDefault.cs
│   ├── Icon.cs
│   ├── KakeiboItem.cs
│   ├── KakeiboItemFrequency.cs
│   ├── NewsletterTemplate.cs
│   └── Interface/
│       └── KakeiboInterface.cs
├── DTOs/              # Data Transfer Objects
├── Data/              # データベースコンテキスト
│   └── KakeiboDbContext.cs
└── Services/          # ビジネスロジック（今後追加予定）
```

## 主な機能

### 1. ユーザー管理 (Users)
- ユーザー登録
- ユーザー情報の管理

### 2. 家計簿管理 (Kakeibo)
- 家計簿の作成
- 家計簿情報の管理

### 3. カテゴリー管理 (Category)
- カスタムカテゴリーの作成
- 収入/支出の区分
- アイコンの設定

### 4. 取引管理 (KakeiboItem)
- 収入・支出の記録
- カテゴリー別の分類
- 日付範囲でのフィルタリング

### 5. 固定費管理 (KakeiboItemFrequency)
- 固定費の登録
- 固定費の頻度設定

## セットアップ

### Dockerを使用した起動（推奨）

Dockerを使用することで、環境構築の手間を省き、すぐにアプリケーションを起動できます。

#### 必要な環境
- Docker
- Docker Compose

#### 起動手順

1. リポジトリのクローン
```bash
git clone <repository-url>
cd ai-kakeibo-backend
```

2. Docker Composeでアプリケーションを起動
```bash
docker-compose up -d
```

このコマンドにより、以下のサービスが起動します:
- **db**: SQL Server 2022（ポート 1433）
- **api**: ASP.NET Core Web API（ポート 5000）

3. アプリケーションの確認

ブラウザで以下のURLにアクセスしてSwagger UIを確認できます:
- http://localhost:5000/swagger

4. ログの確認
```bash
# すべてのログを表示
docker-compose logs -f

# APIのログのみ表示
docker-compose logs -f api

# DBのログのみ表示
docker-compose logs -f db
```

5. 停止
```bash
docker-compose down
```

6. 停止してデータも削除
```bash
docker-compose down -v
```

#### データベース接続情報
- **サーバー**: localhost,1433
- **ユーザー名**: sa
- **パスワード**: YourStrong@Passw0rd
- **データベース名**: AiKakeiboDb

### ローカル環境での起動

#### 必要な環境
- .NET 8.0 SDK
- SQL Server または SQL Server LocalDB

#### データベース接続文字列の設定

`appsettings.json`ファイルの接続文字列を環境に合わせて変更してください。

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AiKakeiboDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

#### マイグレーションの実行

```bash
# マイグレーションの作成
dotnet ef migrations add InitialCreate --project src/AiKakeiboBackend

# データベースの更新
dotnet ef database update --project src/AiKakeiboBackend
```

#### アプリケーションの実行

```bash
# ソリューションディレクトリから
dotnet run --project src/AiKakeiboBackend

# または、プロジェクトディレクトリ内で
cd src/AiKakeiboBackend
dotnet run
```

アプリケーションが起動したら、ブラウザで以下のURLにアクセスしてSwagger UIを確認できます:
- https://localhost:5001/swagger
- http://localhost:5000/swagger

## CORS設定

フロントエンドからのリクエストを受け入れるため、以下のオリジンに対してCORSが有効になっています:
- http://localhost:3000 (React/Next.js用)
- http://localhost:5173 (Vite用)

必要に応じて`Program.cs`で設定を変更してください。

## トラブルシューティング

### Dockerコンテナが起動しない場合

1. ポートが既に使用されていないか確認
```bash
# Windowsの場合
netstat -ano | findstr :1433
netstat -ano | findstr :5000

# Linux/Macの場合
lsof -i :1433
lsof -i :5000
```

2. Dockerが起動しているか確認
```bash
docker ps
```

3. ログを確認してエラーメッセージを確認
```bash
docker-compose logs
```

### データベースマイグレーションのエラー

アプリケーション起動時に自動的にマイグレーションが実行されますが、エラーが発生した場合は以下を試してください:

1. コンテナを再起動
```bash
docker-compose restart api
```

2. データベースコンテナの状態を確認
```bash
docker-compose exec db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -Q "SELECT 1"
```

## ライセンス

このプロジェクトはMITライセンスの下で公開されています。
