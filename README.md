# Setup instructions
run `docker compose -f docker-compose.local.yml up db` to start the database, then connect to it using your favorite MySQL client and run the following commands to create the tables:

```
CREATE TABLE `players` (
  `Id` INT NOT NULL PRIMARY KEY,
  `Sport` VARCHAR(32) NOT NULL,
  `FirstName` VARCHAR(64) NOT NULL,
  `LastName` VARCHAR(64) NOT NULL,
  `NameBrief` VARCHAR(64) NOT NULL,
  `Position` VARCHAR(16) NOT NULL,
  `Age` TINYINT,  
);
```

```
CREATE TABLE `average_ages` (
  `Sport` varchar(32) NOT NULL,
  `Position` varchar(16) NOT NULL,
  `Age` decimal(10,4) NOT NULL,
  PRIMARY KEY (`Sport`,`Position`)
);
```

Afterwards `Ctrl+C` to stop the database container.

# Running the services
Now run `docker compose -f docker-compose.local.yml up` to start all the services. 

## `api` service
Runs on `http://localhost:5000/`, there is a swagger UI configured for easy testing.

## `function` service
Simulates a function (think of Azure / AWS functions) which periodically pulls the data needed and stores it in the database. 



