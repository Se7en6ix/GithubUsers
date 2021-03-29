# Github User Search 
#C# Developer Exam for MachShip


## Requirements
1. Visual Studio 2019 installed on machine.
2. Must have .NET Framework Core 3.1 installed on machine.
3. Internet connection
4. Docker

## Pre-Required Setup

https://docs.docker.com/docker-for-windows/install/

#### Windows Subsystem for Linux 

https://docs.microsoft.com/en-us/windows/wsl/install-win10#manual-installation-steps



# Setup Redis
docker run --name myRedis -p 5009:6379 -d redis

## Building and Running the API

1. Make sure that Redis is up and running.
2. Open the project solution "GithubUsers.sln"
3. Restore Nuget Packages
4. Rebuild the entire solution.
5. Run the Application via Kestrel

## API Usage

1. Run the Application via Kestrel.
2. You'll be welcomed by the Swagger UI page
3. Click on the GithubUsers enpoint and click "Try it Out" button
4. key-in the required parameters then click execute.


## Sample API payload and response

*Request body:*
``` 
    ["fabpot", "timcorey", "ahejlsberg", "julielerman","InvalidUser1, InvalidUser2"]
```

*Response body:*
``` 
[
  {
    "id": 4226954,
    "login": "ahejlsberg",
    "name": "Anders Hejlsberg",
    "company": "Microsoft",
    "followers": 9045,
    "publicRepositories": 2,
    "averageFollowers": 4522
  },
  {
    "id": 47313,
    "login": "fabpot",
    "name": "Fabien Potencier",
    "company": "Symfony/Blackfire",
    "followers": 11319,
    "publicRepositories": 55,
    "averageFollowers": 205
  },
  {
    "id": 5007120,
    "login": "julielerman",
    "name": "Julie Lerman",
    "company": "The Data Farm",
    "followers": 1081,
    "publicRepositories": 94,
    "averageFollowers": 11
  },
  {
    "id": 1839873,
    "login": "TimCorey",
    "name": "Tim Corey",
    "company": "IAmTimCorey",
    "followers": 543,
    "publicRepositories": 13,
    "averageFollowers": 41
  }
]
```


