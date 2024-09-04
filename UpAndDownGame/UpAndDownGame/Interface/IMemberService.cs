using UpAndDown.Game.Model;

namespace UpAndDown.Interface
{
    public interface IMemberService
    {
        Member GetCurrentMember();

        void ReadMembersInformation();
        void SaveCurrentMember(Member member);

        void HandleMemberSelection();
    }
}
