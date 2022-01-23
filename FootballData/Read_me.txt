To create a site on the Raspberry Pi, follow these steps:

1. Create a new folder within /home/pi
2. Copy all files over apart from the bin and obj folders
3. Open Putty and log on to the Pi and navigate to that folder and run the following commands:
4. dotnet run
5. dotnet publish -c Release
6. sudo docker build -t dangermouse500/footballdata:0.3 .
7. sudo docker run -it --rm -p 5000:80 --name football-data-yo dangermouse500/footballdata:0.3