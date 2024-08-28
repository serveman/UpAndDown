using UpAndDown.User.Model;

namespace UpAndDown.Interface
{
    public interface IMemberService
    {
        void ReadMembersInformation();
        void SelectMemberInformation();
        Member GetCurrentMember();
        void SaveCurrentMember(Member member);
        void UpdateMembersInformation();
    }
}
