# Akka.NET Code samples
This repository contains the code samples for my 'Processing IoT data with Akka.NET' workshop.

Next to the starter solution, and the slides, you will also see a few branches of completed workshops.

## Getting ready
Since there are quite a few moving parts to this demo/workshop, it's best to run through the following steps in order to have a working state to start our coding from:

1. **Visual Studio 2017**  
If you don't already have Visual Studio 2017, you can use the 'Visual Studio Community Edition'. This version will be sufficient to run this demo. You can download it for free [here](https://visualstudio.microsoft.com/vs/community/). 
2. **.NET Framework 4.7.2**  
We will be running this demo on the full .NET Framework, version 4.7.2. You can find the SDK installer for .NET Framework 4.7.2 [here](https://dotnet.microsoft.com/download/thank-you/net472-developer-pack).
3. **.NET Core 2.1**  
Our demo uses ASP.NET Core 2.1. In order to have the web server component available for debugging, you should install .NET Core 2.1. You can download it [here](https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.1.503-windows-x64-installer).
4. **Get the code**  
If you are faimiliar with GIT, you can simply clone this repository. If not, you can simply download it as a .zip archive, using the button at the top of this page:  
![download as zip](https://github.com/Belenar/AkkaDotNetDemo/blob/master/img/download_as_zip.png)
5. **Creating the database**  
You will need some local databases to make everything work. The connection strings are configured to use a LocalDB instance, which is installed on your system together with Visual Studio. If something below doesn't work, you should check your Visual Studio installer to see if the feature is installed.  
   * **If you know what you're doing:** run the `resources/AkkaNetResults_db_create.sql` file on your LocalDB instances.
   * **Otherwise, just follow these steps:**
      1. Open Visual Studio
      2. Open the Server Explorer (View > Server Explorer)
      3. Use the `Connect to Database` button ath the top of the Server Explorer
      4. Connect to the `master` database on `(localdb)\MSSQLLocalDB`. Just type or past the server name. It will not appear in the dropdown:  
      ![connect to DB](https://github.com/Belenar/AkkaDotNetDemo/blob/master/img/connect_to_db.png)
      5. Start a new query on that connection:  
      ![new DB query](https://github.com/Belenar/AkkaDotNetDemo/blob/master/img/new_query.png)
      6. Copy-past the contents of the `resources/AkkaNetResults_db_create.sql` file in this repository.
      7. Click the run button at the top, you should get `Command(s) completed successfully.` in the output window:  
      ![run the DB script](https://github.com/Belenar/AkkaDotNetDemo/blob/master/img/run_query.png)
6. **Build and run the code**  
Now all there's left to do is open the solution and see if it runs without exceptions. If you have little expereince using .NET, simply follow these steps:
   1. Open `Axxes.AkkaNetDemo.sln` in Visual Studio
   2. Right-click the solution and restore the NuGet packages:  
   ![restore packages](https://github.com/Belenar/AkkaDotNetDemo/blob/master/img/restore_packages.png)
   3. Run the solution by pressing F5 (or use the 'Start' button at the top):  
   ![start button](https://github.com/Belenar/AkkaDotNetDemo/blob/master/img/start_button.png)
7. **Verify if everything works**  
If all is well, you should have 2 windows that opened up. Let's check if everything is OK:
   1. There should be a webserver running, if should look like this (without errors):  
   ![running web server](https://github.com/Belenar/AkkaDotNetDemo/blob/master/img/webserver_window.png)
   2. There should also be a load generator (WinForms app):
   ![running load generator](https://github.com/Belenar/AkkaDotNetDemo/blob/master/img/load_gen_window.png)
   3. When you press the start button on the load generator, requests should start coming in on the web server:
   ![load on web server](https://github.com/Belenar/AkkaDotNetDemo/blob/master/img/webserver_load.png)
   4. And the UI of the load generator should update as well:
   ![load on load generator](https://github.com/Belenar/AkkaDotNetDemo/blob/master/img/load_gen_load.png)

## Things to investigate if something doesn't go as planned
Look at these things first:
* Are your databases created correctly?
* Is there another process running on port 50004?
* Is your user allowed to open port 50004? (Maybe try running Visual Studio as Administrator)
* Is your firewall interfering?