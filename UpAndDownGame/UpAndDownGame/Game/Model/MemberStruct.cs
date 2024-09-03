using System.Collections.Generic;

namespace UpAndDown.Game.Model
{
    public struct Member
    {
        public string Name { get; set; }
        public List<PlayCount> PlayCountList { get; set; }
    }
}
