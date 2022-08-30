# OfficeManager
OfficeManager.API

# Run App in Local environment
	- Open Develeoper Poweshell with root path of 'docker-compose.yml'  
		OR
	- After opening Solution, right click on docker-compose, Click on 'Open in Terminal'
	- In docker-compose.override.yml file, you need to update 'ConnectionStrings:Default' section with your local Machine IP address
	- for more info , https://jack-vanlightly.com/blog/2017/9/24/how-to-connect-to-your-local-sql-server-from-inside-docker

# Docker Commands
	- To Build App :  docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build
	  or
	- To Up Docker Images : docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
	- To down Docker Images :docker-compose -f docker-compose.yml -f docker-compose.override.yml down

	- docker --help
	- List Containers : docker ps
	- List docker images : docker image ls
  