# Project name: aws-net-core-examples

## Description: 
Basic CRUD for Amazon S3 Bucket with ASP.NET CORE

## Instalation:
  1- Clone the repo
  
  2- Build your project to install the neccessary nuget packages

### OPTIONAL 
  If you want to create yourself this project from scratch, you will need to install in `Package Manager Console`:
  
	```Install-package AWSSDK.Extensions.NETCORE.Setup```
	
	```Install-package AWSSDK.S3```
  
##### *NOW YOU CAN USE IT!*

## USAGE:
  1- Run your project
  
  2- Open postman (or equal) and create "post requests" with the specified parameters in the Controller
  
  3- Create the directory: `C:\Users\username\.aws`
  
  [If you want to create a directory in windows with '.' in the beginning of the name you must create it like this: `.aws.`. 
  
  Windows will delete the second '.' and will keep the first one.
  
  4- Inside your directory create a file (with no extension) named: `credentials`
  
  5- Inside the file create a code with the structure that you can find in this repo in a file named: `credentials_Example`
  
You still can set both: `aws_access_key_id` and `aws_secret_access_key` in your settings, but for security, aws recomends to use this alternative for security purposes

## LICENSE:
  GPL LICENSE

#### *Learning Purposes*
 
 
  
  

