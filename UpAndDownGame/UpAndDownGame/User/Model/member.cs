using System.Collections.Generic;

namespace UpAndDown.User.Model
{
    public struct Member
    {
        public string Name { get; set; }
        public List<Count> PlayCountList { get; set; }
    }
}
