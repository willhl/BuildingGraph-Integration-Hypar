# BuildingGraph-Integration-Hypar

Example intergrations with GraphQL queries and Hypar functions.

Currently there's only the one Hypar function which was created to explore the feasabiliy of GraphQL query driving functions in Hypar:
- Levels From GraphQL And Perimeter - Creates levels in Hypar at the elevations defined from a GraphQL query and with manually drawn perimeter in Hypar.

The GraphQL endpoint used in this example is a [Building Graph Server instance](https://github.com/willhl/BuildingGraph-Server) with some levels pre-populated by a Dynamo script.

There're two inputs required, a JSON file defining the API URI and any required headers, and the perimiter polygon which is drawn directly in hyapr.

Example connection data JSON:
```
{
  "URI": "{uri here}",
  "Headers": {
    "Authorization": "{Auth token here}"
  }
}
```
There's also a "ConnectionExample.json" with the URI and credetails used for the public API which can be used if you wanted to run it yourself. The live API is subject to change and rate limited so might not work consitently.


First working function does what it says it does:

![Hypar Example](https://github.com/willhl/BuildingGraph-Integration-Hypar/blob/master/Docs/HyparWorkflowExample.png?raw=true)

The GraphQL query actualy includes parameters for the building name and project name, which are currently just hard coded, as this is just a proof of concept example at this stage.

Here's how the GraphQL query that the function is running looks, and the data that's returned.
![GraphiGL Example](https://github.com/willhl/BuildingGraph-Integration-Hypar/blob/master/Docs/GraphiQLExample.PNG?raw=true)

Here's how the data structure looks in Neo4j.
![Neo4j Example](https://github.com/willhl/BuildingGraph-Integration-Hypar/blob/master/Docs/Neo4jExample.PNG?raw=true)
