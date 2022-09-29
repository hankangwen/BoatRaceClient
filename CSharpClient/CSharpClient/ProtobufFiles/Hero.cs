using System.Collections.Generic;
using ProtoBuf;

namespace BoatRace
{
    [ProtoContract]
    public class Hero
    {
        [ProtoMember(1)]
        public Info info { get; set; }

        [ProtoMember(2)] 
        public Job job;
        public enum Job
        {
            Soldier,
            Swords
        }

        [ProtoMember(3)]
        public List<string> equip;
    }

    [ProtoContract]
    public class Info
    {
        [ProtoMember(1)]
        public int age { get; set; }
        
        [ProtoMember(2)]
        public string name { get; set; }
        
        [ProtoMember(3)]
        public string sex { get; set; }
    }
}