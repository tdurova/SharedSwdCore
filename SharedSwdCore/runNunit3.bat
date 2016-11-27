@echo off 
SET "var=%*"
CALL SET var=%%var:-xml=--result%%
\src\packages\NUnit.ConsoleRunner.3.4.1\tools\nunit3-console.exe %var%;format=nunit2