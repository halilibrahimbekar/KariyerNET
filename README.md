# CareerNet
## Using
* clone the repo
* open a terminal app
* write the command below
> docker compose up


You can see the default ports from :
> "docker-compose-override.yml"

Please visit "http://localhost:8080/swagger/index.html" for all api methods. 

* you have to create an elastic index to use application. Call the api below
> /api/Job/createIndex

* use the method below to add new company ( Take the "Id" value from result to use at "AddJobRequest" )
> /api/Company

* use the method below to search jobs 
> /api/job/search

