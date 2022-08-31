# Coding Standard

## General :
	1. Property name must be camelCase and do not start with _ (Underscore)
	2. Please try to remove all possible warning and null check references before push code
	3. Try to use async method and use await in that method
	4. Do not use static method for any mapping
	5. Always keep practice remove unnecessary namespace from class file (Right click and click 'Remove and Sort Usings')

## Application Project
	1. DTOs must put in different folder (OfficeManager.Application\Dtos)
	2. Add new module related command and query in 'Feature' folder
	3. Do not use constant messages in class file, try to use it from Constant\Messages file, if missing then add it that file
