using System;
using System.Collections.Generic;
using System.Linq;
using UpAndDown.Interface;
using UpAndDown.User;
using UpAndDown.User.Model;

namespace UpAndDown.Service
{
    public class MemberService : MemberIO, IMemberService
    {
        HashSet<Member> members;

        private Member CurrentMember { get; set; }

        public void ReadMembersInformation()
        {
            members = ReadMemberFile();
        }

        /// <summary>
        /// members 목록 중에서 선택한 이름이 존재하는 경우에 >> 표시 
        /// </summary>
        public void SelectMemberInformation()
        {
            string name;

            Console.Write("플레이 할 유저 이름을 입력해주세요: ");
            name = Console.ReadLine();

            Console.Clear();
            bool isExistName = false;
            foreach (Member member in members)
            {
                if (member.Name == name && !isExistName)
                {
                    Console.Write(">> ");
                    CurrentMember = new Member
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
                Console.WriteLine($"Read: {member.Name,-12}");
            }

            if (!isExistName)
            {
                CurrentMember = CreateNewMember(name);
            }

            DisplayMemberInformation(CurrentMember);

            members.Add(CurrentMember);

            Console.WriteLine();
        }

        private void DisplayMemberInformation(Member member)
        {
            int n = 1;
            Console.WriteLine();
            Console.WriteLine($"Name: {member.Name,-12}");
            foreach (Count cnt in member.PlayCountList)
            {
                Console.WriteLine($"[난이도{n++}] Success:{cnt.Success,3}, \tFailure:{cnt.Failure,3}, \tTotal:{cnt.Total,3}");
            }
        }

        private Member CreateNewMember(string name)
        {
            Member newMember = new Member
            {
                Name = name,
                PlayCountList = Enumerable.Repeat(
                    new Count
                    {
                        Success = 0,
                        Failure = 0
                    }, 5).ToList()
            };

            Console.WriteLine(">> New Member !!");
            return newMember;
        }

        public Member GetCurrentMember()
        {
            return CurrentMember;
        }

        public void SaveCurrentMember(Member member)
        {
            members.Remove(CurrentMember);

            CurrentMember = member;

            members.Add(member);
        }

        public void UpdateMembersInformation()
        {
            UpdateMemberFile(members);
        }
    }
}
