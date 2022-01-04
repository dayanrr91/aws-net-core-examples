# aws-net-core-examples

## Description: 
  ### Basic operations in Amazon S3 Bucket with .NET 6 including:
  	- Creating a bucket
	- Deleting a bucket
	- Upload file to a bucket
	- Deleting file from a bucket
	- Get file from bucket

## Tools used
  - Visual Studio 2022 (https://visualstudio.microsoft.com/vs/)
  - aws-cli (https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html)
  - .NET 6 (https://dotnet.microsoft.com/en-us/download/dotnet/6.0) 

## Instalation and Usage:
  1- Clone the repo
  
  2- Build your project to install the necessary nuget packages

  3- Download and install the "aws-cli" tool (https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html).
  
  4- Check everything was installed correctly with "aws --version" command.
  
  5- Open a cmd console and type: "aws configure". Then put your credentials and region. After that in "C:\Users\YourUserName\.aws" folder two files will be created, config with the region and credentials with your aws keys.
  
  6- Launch the Api project and read the swagger documentation.
  
### OPTIONAL 
  If you want to create yourself you need to follow steps 3 to 6 in order to install "aws-cli". 
  Then create a project from scratch, you will need to open th `Package Manager Console` and run the following commands:
  
	Install-package AWSSDK.Extensions.NETCORE.Setup
	
	Install-package AWSSDK.S3
  
##### *NOW YOU CAN USE IT!*

## LICENSE:
  GPL LICENSE

#### *Learning Purposes*
