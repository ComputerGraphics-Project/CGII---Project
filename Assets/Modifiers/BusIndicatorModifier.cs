namespace Mapbox.Unity.MeshGeneration.Modifiers
{
	using UnityEngine;
	using Mapbox.Unity.MeshGeneration.Components;
	using Mapbox.Unity.MeshGeneration.Data;
	using Mapbox.Unity.Map;
	using Mapbox.Utils;
	using System;

	[CreateAssetMenu(menuName = "Mapbox/Modifiers/Bus Indicator Modifier")]
	public class BusIndicatorModifier : GameObjectModifier
	{
		public override void Run(VectorEntity ve, UnityTile tile)
		{
			var min = ve.MeshFilter.sharedMesh.subMeshCount;
			var mats = new Material[min];

			BusServiceAvailability busServiceAvailability = GameObject.Find("BusIndicator").GetComponent<BusServiceAvailability>();
			Vector2d latLon = GameObject.Find("CitySimulatorMap").GetComponent<AbstractMap>().WorldToGeoPosition(ve.Transform.position);
			float[] indicators = busServiceAvailability.GetPTAL((float) latLon[0], (float) latLon[1]);
			
			for (int i = 0; i < min; i++)
			{				
				Material mat = new Material(Shader.Find("Specular"));
				mat.color = probabilityToColor(indicators[busServiceAvailability.GetCurrentStep()]);
				mats[i] = mat;
			}
			
			ve.MeshRenderer.materials = mats;
		}
		private Color probabilityToColor(float probability) {
			if (probability < 0.5f) {
				return new Color(0.0f, probability*2f, 1f - probability*2f, 1f);
			} else {
				return new Color((probability-0.5f)*2f, 1f - (probability-0.5f)*2f, 0.0f, 1f);
			}
		}
	}
}
