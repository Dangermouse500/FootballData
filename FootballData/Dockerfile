FROM mcr.microsoft.com/dotnet/sdk:5.0 as builder
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

#RUN mkdir -p /TestSites/Dan/Dan1/
#WORKDIR /TestSites/Dan
RUN mkdir -p /var/www/Scott/Dan1
WORKDIR /var/www/Scott

COPY ./FootballData.csproj ./Dan1
#WORKDIR /TestSites/Dan/Dan1
WORKDIR /var/www/Scott/Dan1

RUN dotnet restore ./FootballData.csproj  
RUN dotnet publish -c release -o published -r linux-arm

FROM mcr.microsoft.com/dotnet/runtime:5.0.13-bullseye-slim-arm32v7

WORKDIR /root/  
#COPY --from=builder /TestSites/Dan/Dan1/published .
COPY --from=builder /var/www/Scott/Dan1/published .

CMD ["dotnet", "./FootballData.dll"]