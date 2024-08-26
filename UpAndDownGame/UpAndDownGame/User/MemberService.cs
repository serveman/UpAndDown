using System;
using System.Collections.Generic;

namespace UpAndDown.User
{
    public class MemberService
    {
        private HashSet<Member> members = new HashSet<Member>();

        private Member CurrentMember { get; set; }

        public void ReadMemberInfomation()
        {
            members = MemberIO.ReadMemberFile();

#if DEBUG
            string name;

            Console.Write("이름을 입력해주세요: ");
            name = Console.ReadLine();

            Console.Clear();
            bool isExistName = false;
            foreach (Member member in members)
            {
                if(member.Name == name && !isExistName)
                {
                    Console.Write(">> ");
                    this.CurrentMember = new Member
                    {
                        Name = member.Name,
                        PlayCountList = member.PlayCountList
                    };
                    isExistName = true;
                }
                else
                {
                    Console.Write("   ");
                }
                Console.WriteLine($"Read: {member.Name, -12}");
            }
            if(!isExistName)
            {
                List<Count> newCountList = new List<Count>();
                for(int i = 0; i < 5; i++)
                {
                    newCountList.Add(new Count { Success = 0, Failure = 0 });
                }

                this.CurrentMember = new Member
                {
                    Name = name,
                    PlayCountList = newCountList
                };
            }

            members.Add(CurrentMember);

            Console.WriteLine();
#endif
        }

        public Member GetCurrentMember()
        {
            return this.CurrentMember;
        }

        public void SaveCurrentMember(Member member)
        {
            members.Remove(this.CurrentMember);

            this.CurrentMember = member;

            members.Add(member);
        }

        public void UpdateMemberInformation()
        {
            MemberIO.UpdateMemberFile(members);
        }
    }
}
