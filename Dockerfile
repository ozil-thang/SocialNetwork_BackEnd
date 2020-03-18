FROM mcr.microsoft.com/dotnet/core/sdk:2.1
 
WORKDIR /home/app
 
COPY . .
 
RUN dotnet restore
 
RUN dotnet publish ./Api/Api.csproj -o /publish/
 
WORKDIR /publish
 
ENV ASPNETCORE_URLS="http://0.0.0.0:5000"
 
ENTRYPOINT ["dotnet", "Api.dll"]
