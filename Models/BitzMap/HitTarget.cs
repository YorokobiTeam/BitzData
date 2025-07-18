﻿
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace BitzData.Models.BitzMap
{
    public class HitTarget
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; } = -1;
        public HitTargetType Type { get; set; } = HitTargetType.Single;
        public HitTargetDirection[] Directions { get; set; } = [];



    }

    [JsonConverter(typeof(JsonStringEnumConverter<HitTargetType>))]
    public enum HitTargetType
    {
        Single,
        Hold,
        Spam
    }


    [JsonConverter(typeof(JsonStringEnumConverter<HitTargetDirection>))]
    public enum HitTargetDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}
