FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /home

EXPOSE 80
EXPOSE 443

COPY *.csproj .

RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final

WORKDIR /home

COPY --from=build /home/out .

ENTRYPOINT ["dotnet", "ExternalTranslator.dll"]