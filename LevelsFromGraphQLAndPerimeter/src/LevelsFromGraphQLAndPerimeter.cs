using Elements;
using Elements.Geometry;
using System;
using System.Collections.Generic;
using LevelsFromGraphQLAndPerimeter.GraphQL;

namespace LevelsFromGraphQLAndPerimeter
{
      public static class LevelsFromGraphQLAndPerimeter
    {
        /// <summary>
        /// The LevelsFromGraphQLAndPerimeter function.
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A LevelsFromGraphQLAndPerimeterOutputs instance containing computed results and the model with any new elements.</returns>
        public static LevelsFromGraphQLAndPerimeterOutputs Execute(Dictionary<string, Model> inputModels, LevelsFromGraphQLAndPerimeterInputs input)
        {
            //get the connection and query inputs which should have been provided as JSON data
            var connectionInfoFilePath = input.Connection.LocalFilePath;
            var connJSON = System.IO.File.ReadAllText(connectionInfoFilePath);
            dynamic connectionArgs = Newtonsoft.Json.JsonConvert.DeserializeObject(connJSON);

            if (connectionArgs.URI == null) throw new ArgumentException("URI must be provided.");
            var uri = connectionArgs.URI.Value;
            var headers = new Dictionary<string, string>();

            if (connectionArgs.Headers != null)
            {
                headers = connectionArgs.Headers.ToObject<Dictionary<string, string>>();
            }


            //the GraphQL query to get all the levels and the data we need to create the Hypar levels
            var levelQuery = @"
query ($buidingName:String, $projectName: String) {
  Levels: Level(filter: { Building: { Name:$buidingName, Projects_some: { Name:$projectName} } }) {
    Id
    Abbreviation
    Elevation(unit:m)
    Name
  }
}
";

            //the variables required for the GraphQL levels query
            var queryVariables = new Dictionary<string, object>();
            queryVariables.Add("buidingName", "Dynamo Tower");
            queryVariables.Add("projectName", "Project Graph");

            var client = new ReallySimpleGraphQLClient(uri, headers);
            var response = client.SendRequest(levelQuery, queryVariables);

            if (response == null) throw new ArgumentException("No data was returned: Check connection and query.");

            var newElements = new List<Element>();
            int lvlCount = 0;
            if (response.Levels != null)
            {

                var lamina = new Elements.Geometry.Solids.Lamina(input.Perimeter, false);
                var geomRep = new Representation(new List<Elements.Geometry.Solids.SolidOperation>() { lamina });
                var lvlMatl = new Material("level", Colors.White, 0.0f, 0.0f);

                foreach (dynamic lvl in response.Levels)
                {
                    var elevation = lvl.Elevation.Value;
                    var name = lvl.Name.Value;
                    var Id = lvl.Id.Value;
                    Guid idGuid;
                    if (!Guid.TryParse((string)Id, out idGuid)) idGuid = Guid.NewGuid();

                    newElements.Add(new Level(elevation, idGuid, name));
                    newElements.Add(new LevelPerimeter(elevation, input.Perimeter, Guid.NewGuid(), name));
                    newElements.Add(new Panel(input.Perimeter, lvlMatl, new Transform(0, 0, elevation), geomRep, false, Guid.NewGuid(), name));

                    lvlCount++;
                }

            }

            var output = new LevelsFromGraphQLAndPerimeterOutputs(lvlCount);
            output.Model.AddElements(newElements);

            return output;
        }
      }
}