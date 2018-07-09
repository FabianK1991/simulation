﻿using Newtonsoft.Json.Linq;
using Simulation.Game.Base;
using Simulation.Game.World;
using Simulation.Util;
using System;

namespace Simulation.Game.Serialization
{
    public class InteriorSerializer
    {
        private static readonly Type interiorType = typeof(Interior);
        private static readonly string[] serializeableProperties = new string[] {
            "ID",
            "blockingGrid",
            "Dimensions",
            "worldLinks"
        };

        public static Interior Deserialize(JObject jObject)
        {
            Interior interior = ReflectionUtils.CallPrivateConstructor<Interior>();

            Deserialize(ref jObject, interior);

            return interior;
        }

        public static JObject Serialize(Interior interior)
        {
            var retObject = new JObject();

            SerializationUtils.SerializeType(interiorType, ref retObject);
            Serialize(interior, ref retObject);

            return retObject;
        }

        protected static void Deserialize(ref JObject jObject, Interior interior)
        {
            SerializationUtils.SetFromObject(jObject, interior, interiorType, serializeableProperties);

            // Deserialize Ambient Objects
            JArray ambientObjects = (JArray)jObject.GetValue("ambientObjects");

            foreach (var ambientObject in ambientObjects)
                interior.AddAmbientObject((AmbientObject)WorldObjectSerializer.Deserialize((JObject)ambientObject));

            // Deserialize Hitable Objects
            JArray containedObjects = (JArray)jObject.GetValue("containedObjects");

            foreach (var containedObject in containedObjects)
                interior.AddContainedObject((HitableObject)WorldObjectSerializer.Deserialize((JObject)containedObject));
        }

        protected static void Serialize(Interior interior, ref JObject jObject)
        {
            SerializationUtils.AddToObject(jObject, interior, interiorType, serializeableProperties);

            // Serialize Ambient Objects
            JArray ambientObjects = new JArray();

            foreach (var ambientObject in interior.AmbientObjects)
                ambientObjects.Add(WorldObjectSerializer.Serialize(ambientObject));

            jObject.Add("ambientObjects", ambientObjects);

            // Serialize Hitable Objects
            JArray containedObjects = new JArray();

            foreach (var containedObject in interior.ContainedObjects)
                containedObjects.Add(WorldObjectSerializer.Serialize(containedObject));

            jObject.Add("containedObjects", containedObjects);
        }
    }
}