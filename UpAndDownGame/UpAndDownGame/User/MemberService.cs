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
                        PlayCount = member.PlayCount
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
                this.CurrentMember = new Member
                {
                    Name = name,
                    PlayCount = new Count
                    {
                        Success = 0,
                        Failure = 0
                    }
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
