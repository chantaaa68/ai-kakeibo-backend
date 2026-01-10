# ai-kakeibo-backend

AI駆動開発で作成された家計簿(Kakeibo)アプリケーションのバックエンドAPI

## 技術スタック

- **.NET 8.0** - 最新の.NETフレームワーク
- **ASP.NET Core Web API** - RESTful APIフレームワーク
- **Entity Framework Core 8.0** - O/Rマッピング
- **SQL Server** - データベース
- **Swagger/OpenAPI** - API仕様とドキュメント

## プロジェクト構成

```
src/AiKakeiboBackend/
├── Controllers/         # APIコントローラー
│   ├── TransactionsController.cs
│   ├── CategoriesController.cs
│   └── BudgetsController.cs
├── Models/             # エンティティモデル
│   ├── User.cs
│   ├── Category.cs
│   ├── Transaction.cs
│   └── Budget.cs
├── DTOs/              # Data Transfer Objects
│   ├── TransactionDto.cs
│   ├── CategoryDto.cs
│   └── BudgetDto.cs
├── Data/              # データベースコンテキスト
│   └── KakeiboDbContext.cs
└── Services/          # ビジネスロジック（今後追加予定）
```

## 主な機能

### 1. 取引管理 (Transactions)
- 収入・支出の記録
- カテゴリー別の分類
- 日付範囲でのフィルタリング
- CRUD操作

### 2. カテゴリー管理 (Categories)
- カスタムカテゴリーの作成
- 収入/支出の区分
- アイコンと色の設定
- CRUD操作

### 3. 予算管理 (Budgets)
- 月次予算の設定
- カテゴリー別の予算
- CRUD操作

## セットアップ

### 必要な環境

- .NET 8.0 SDK
- SQL Server または SQL Server LocalDB

### データベース接続文字列の設定

`appsettings.json`ファイルの接続文字列を環境に合わせて変更してください。

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AiKakeiboDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### マイグレーションの実行

```bash
# マイグレーションの作成
dotnet ef migrations add InitialCreate --project src/AiKakeiboBackend

# データベースの更新
dotnet ef database update --project src/AiKakeiboBackend
```

### アプリケーションの実行

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

## API エンドポイント

### Transactions
- `GET /api/transactions` - 取引一覧の取得
- `GET /api/transactions/{id}` - 特定の取引の取得
- `POST /api/transactions?userId={userId}` - 新規取引の作成
- `PUT /api/transactions/{id}` - 取引の更新
- `DELETE /api/transactions/{id}` - 取引の削除

### Categories
- `GET /api/categories` - カテゴリー一覧の取得
- `GET /api/categories/{id}` - 特定のカテゴリーの取得
- `POST /api/categories?userId={userId}` - 新規カテゴリーの作成
- `PUT /api/categories/{id}` - カテゴリーの更新
- `DELETE /api/categories/{id}` - カテゴリーの削除

### Budgets
- `GET /api/budgets` - 予算一覧の取得
- `GET /api/budgets/{id}` - 特定の予算の取得
- `POST /api/budgets?userId={userId}` - 新規予算の作成
- `PUT /api/budgets/{id}` - 予算の更新
- `DELETE /api/budgets/{id}` - 予算の削除

## CORS設定

フロントエンドからのリクエストを受け入れるため、以下のオリジンに対してCORSが有効になっています:
- http://localhost:3000 (React/Next.js用)
- http://localhost:5173 (Vite用)

必要に応じて`Program.cs`で設定を変更してください。

## ライセンス

このプロジェクトはMITライセンスの下で公開されています。
