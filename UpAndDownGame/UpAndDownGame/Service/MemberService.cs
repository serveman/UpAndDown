using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using UpAndDown.Game.Enum;
using UpAndDown.Interface;
using UpAndDown.User;
using UpAndDown.User.Model;

using static UpAndDown.CustomException.CustomExceptions;

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

        private void SelectMember()
        {
            string name;

            do
            {
                Console.WriteLine();
                Console.Write("플레이 할 유저 이름을 입력해주세요: ");
                name = Console.ReadLine();
            } while (string.IsNullOrEmpty(name));

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

            UpdateMembersInformation();
        }

        private void UpdateMembersInformation()
        {
            UpdateMemberToFile(members);
        }

        private void DeleteMember()
        {
            string name;

            do
            {
                Console.WriteLine();
                Console.Write("삭제 할 유저 이름을 입력해주세요: ");
                name = Console.ReadLine();
            } while (string.IsNullOrEmpty(name));

            bool isNotExistName = true;
            Member? selectedMember = null;
            foreach (Member member in members)
            {
                string displayString;
                if (member.Name == name && isNotExistName)
                {
                    displayString = $">> {"Delete:",-8}";
                    selectedMember = member;
                    isNotExistName = false;
                }
                else
                {
                    displayString = "";
                }
                Console.WriteLine($"{displayString, -11}{member.Name, -12}");
            }

            if(isNotExistName)
            {
                throw new MemberNotFoundException("존재하지 않는 사용자 이름입니다.");
            }

            Assert.IsNotNull(selectedMember);
            if (members.Remove(selectedMember.Value))
            {
                UpdateMembersInformation();
                Console.WriteLine($"{name} 사용자가 정상적으로 삭제되었습니다.");
            }
            else
            {
                throw new Exception("유저를 삭제하는 중에 무언가 잘못되었습니다. 뭐지?");
            }
        }

        public void HandleMemberSelection()
        {
            MemberMenu inputMenu;
            do
            {
                DisplayMemberMenu();

                inputMenu = SelectMemberMenu();
                switch (inputMenu)
                {
                    case MemberMenu.Select:
                        SelectMember();
                        break;

                    case MemberMenu.Delete:
                        try
                        {
                            DeleteMember();
                        }
                        catch (MemberNotFoundException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                }

                Console.WriteLine();

            } while (inputMenu != MemberMenu.Select);

        }

        private void DisplayMemberMenu()
        {
            Console.WriteLine("".PadRight(20, '*'));
            Console.WriteLine("* 동작을 선택해주세요");
            Console.WriteLine("1. 유저 선택 또는 신규 생성");
            Console.WriteLine("2. 유저 삭제");
            Console.WriteLine("".PadRight(20, '*'));
            Console.Write("메뉴 입력: ");
        }

        private MemberMenu SelectMemberMenu()
        {
            if (!int.TryParse(Console.ReadLine(), out int input))
            {
                Console.WriteLine("입력값이 잘못되었습니다. 숫자를 입력해주세요.");
                return MemberMenu.Invalid;
            }

            switch(input)
            {
                case 1: return MemberMenu.Select;
                case 2: return MemberMenu.Delete;
                default:
                    Console.WriteLine("유효하지 않은 메뉴입니다. 다시 시도해주세요.");
                    return MemberMenu.Invalid;
            }
        }
    }
}
