# ReportesFacturacion

This project was generated with .NET Framework version 4.7.2.

Console service that send daily billing report to the finance area

## Configurations App.config

1- `MinimalAPI_Intramex` Miniminal api url

2- `LogPath` Path of log file for each execution

3- `DaysOfBackup` Variable of days for delete log files

4- `Email` Recipient email to whom the email will be sent

5- `UrlNas` Path where the generated excel file will be saved as an email attachment

6- Credentials of access NAS 4

## Test

Execute console project

## Build

Compile as `Release` to build the project. The build artifacts will be stored in the `bin/` directory

## Install
1- Copy files to server folder

2- Create log folder with reference to the `LogPath` in the App.config

3- Open `Task Scheduler` and create basic new task and configure execute properties  

