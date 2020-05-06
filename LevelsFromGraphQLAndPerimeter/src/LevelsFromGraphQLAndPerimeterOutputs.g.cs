// This code was generated by Hypar.
// Edits to this code will be overwritten the next time you run 'hypar init'.
// DO NOT EDIT THIS FILE.

using Elements;
using Elements.GeoJSON;
using Elements.Geometry;
using Hypar.Functions;
using Hypar.Functions.Execution;
using Hypar.Functions.Execution.AWS;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace LevelsFromGraphQLAndPerimeter
{
    public class LevelsFromGraphQLAndPerimeterOutputs: ResultsBase
    {
		/// <summary>
		/// Total number of levels.
		/// </summary>
		[JsonProperty("Level Quantity")]
		public double LevelQuantity {get;}


        
        /// <summary>
        /// Construct a LevelsFromGraphQLAndPerimeterOutputs with default inputs.
        /// This should be used for testing only.
        /// </summary>
        public LevelsFromGraphQLAndPerimeterOutputs() : base()
        {

        }


        /// <summary>
        /// Construct a LevelsFromGraphQLAndPerimeterOutputs specifying all inputs.
        /// </summary>
        /// <returns></returns>
        [JsonConstructor]
        public LevelsFromGraphQLAndPerimeterOutputs(double levelquantity): base()
        {
			this.LevelQuantity = levelquantity;

		}

		public override string ToString()
		{
			var json = JsonConvert.SerializeObject(this);
			return json;
		}
	}
}