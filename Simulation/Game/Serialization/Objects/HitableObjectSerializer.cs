﻿using Newtonsoft.Json.Linq;
using Simulation.Game.Objects;
using Simulation.Util;
using System;

namespace Simulation.Game.Serialization.Objects
{
    public class HitableObjectSerializer: GameObjectSerializer
    {
        private static readonly Type hitableObjectType = typeof(HitableObject);
        private static readonly string[] serializeableProperties = new string[] {
            "relativeHitBoxBounds",
            "relativeBlockingBounds",
            "UseSameBounds",
            "BlockingType",
            "HitBoxBounds",
            "BlockingBounds",
            "UnionBounds"
        };

        protected static void Deserialize(ref JObject jObject, HitableObject hitableObject)
        {
            GameObjectSerializer.Deserialize(ref jObject, hitableObject);

            SerializationUtils.SetFromObject(jObject, hitableObject, hitableObjectType, serializeableProperties);
        }

        protected static void Serialize(HitableObject hitableObject, ref JObject jObject)
        {
            GameObjectSerializer.Serialize(hitableObject, ref jObject);

            SerializationUtils.AddToObject(jObject, hitableObject, hitableObjectType, serializeableProperties);
        }
    }
}