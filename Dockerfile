# ビルドステージ
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# プロジェクトファイルをコピーして依存関係を復元
COPY ["src/AiKakeiboBackend/AiKakeiboBackend.csproj", "src/AiKakeiboBackend/"]
RUN dotnet restore "src/AiKakeiboBackend/AiKakeiboBackend.csproj"

# ソースコードをコピーしてビルド
COPY . .
WORKDIR "/src/src/AiKakeiboBackend"
RUN dotnet build "AiKakeiboBackend.csproj" -c Release -o /app/build

# パブリッシュステージ
FROM build AS publish
RUN dotnet publish "AiKakeiboBackend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 実行ステージ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AiKakeiboBackend.dll"]
