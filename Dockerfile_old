FROM mcr.microsoft.com/dotnet/sdk:5.0 as builder
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

#RUN mkdir -p /root/src/app  
#WORKDIR /root/src/app
#RUN mkdir -p /var/www/DanTestLive
WORKDIR /var/www/DanTest

#COPY FootballData     FootballData  
COPY ./FootballData.csproj ./Dan1
#WORKDIR /root/src/app/FootballData

#RUN dotnet restore ./FootballData.csproj  
#RUN dotnet publish -c release -o published -r linux-arm

#FROM mcr.microsoft.com/dotnet/runtime:5.0.13-bullseye-slim-arm32v7

#WORKDIR /root/  
#COPY --from=builder /root/src/app/FootballData/published .

#CMD ["dotnet", "./FootballData.dll"]

#  This command works - sudo docker build -t dangermouse500/footballdata:0.1 .

#docker build -t dangermouse500/footballdata:0.1 .

#cd C:\Users\Dan\source\repos\FootballData\FootballData


## SECOND METHOD BELOW FROM SCOTTISH GUY - https://www.youtube.com/watch?v=f0lMGPB10bM

# GET BASE SDK
# FROM mcr.microsoft.com/dotnet/core/sdk:5.0 as build-env
# WORKDIR /app
# # Copy csproj file and restore any dependencies 
# COPY *.csproj ./
# RUN dotnet restore

# # Copy project files
# COPY . ./
# RUN dotnet publish -c Release -o out

# #Generate runtime image
# FROM mcr.microsoft.com/dotnet/core/aspnet:5.0
# WORKDIR /app
# EXPOSE 80
# COPY --from=build-env /app/out .
# ENTRYPOINT ["dotnet", "FootballData.dll"]