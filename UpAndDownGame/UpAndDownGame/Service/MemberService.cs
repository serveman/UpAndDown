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
        private HashSet<Member> Members { get; set; }
        private Member CurrentMember { get; set; }

        public MemberService() { }

        public void ReadMembersInformation()
        {
            Members = ReadMemberFile();
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

            CurrentMember = SelectMemberByName(name);

            DisplayMemberInformation(CurrentMember);

            Members.Add(CurrentMember);

            Console.WriteLine();
        }

        private Member SelectMemberByName(string name)
        {
            Console.Clear();

            Member? currentMember = null;
            foreach (Member member in Members)
            {
                string displayString = "";
                if (member.Name == name && currentMember == null)
                {
                    displayString = ">> Selected:";
                    currentMember = new Member
                    {
                        Name = member.Name,
                        PlayCountList = member.PlayCountList
                    };
                }
                Console.WriteLine($"{displayString, -12} {member.Name,-12}");
            }

            return currentMember == null
                ? CreateNewMember(name)
                : currentMember.Value;
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
            foreach (Member member in Members)
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
                Console.WriteLine($"{displayString,-11}{member.Name,-12}");
            }

            if (isNotExistName)
            {
                throw new MemberNotFoundException("존재하지 않는 사용자 이름입니다.");
            }

            Assert.IsNotNull(selectedMember);
            if (Members.Remove(selectedMember.Value))
            {
                UpdateMembersInformation();
                Console.WriteLine($"{name} 사용자가 정상적으로 삭제되었습니다.");
            }
            else
            {
                throw new Exception("유저를 삭제하는 중에 무언가 잘못되었습니다. 뭐지?");
            }
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
                PlayCountList = Enumerable.Range(1, 5)
                    .Select(level => new Count
                    (
                        level: level,
                        success: 0,
                        failure: 0
                    )).ToList()
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
            Members.Remove(CurrentMember);

            CurrentMember = member;

            Members.Add(member);

            UpdateMembersInformation();
        }

        private void UpdateMembersInformation()
        {
            UpdateMemberToFile(Members);
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

                    case MemberMenu.Exit:
                        throw new ExitGameByUserException("게임을 종료합니다 !!!");

                    default:
                        //
                        break;
                }

                Console.WriteLine();

            } while (inputMenu != MemberMenu.Select);

        }

        private void DisplayMemberMenu()
        {
            DisplayMembersAll();
            Console.WriteLine("".PadRight(20, '*'));
            Console.WriteLine("* 동작을 선택해주세요");
            Console.WriteLine("1. 유저 선택 또는 신규 생성");
            Console.WriteLine("2. 유저 삭제");
            Console.WriteLine("0. 종료");
            Console.WriteLine("".PadRight(20, '*'));
            Console.Write("메뉴 입력: ");
        }

        private void DisplayMembersAll()
        {
            foreach (Member member in Members)
            {
                Console.WriteLine($"{member.Name, 15}: Total PlayCount: {member.PlayCountList.Sum(count => count.Total)}");
            }
            Console.WriteLine("".PadLeft(40, '='));
            Console.WriteLine();
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
                case 0: return MemberMenu.Exit;
                default:
                    Console.WriteLine("유효하지 않은 메뉴입니다. 다시 시도해주세요.");
                    return MemberMenu.Invalid;
            }
        }
    }
}
