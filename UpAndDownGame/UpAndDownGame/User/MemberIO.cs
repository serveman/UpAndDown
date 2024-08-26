using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace UpAndDown.User
{
    public class MemberIO
    {
        private const string MEMBER_FILENAME = "members.txt";

        public static HashSet<Member> ReadMemberFile()
        {
            HashSet<Member> members = new HashSet<Member>();

            try
            {
                using (StreamReader sr = new StreamReader($"{MEMBER_FILENAME}"))
                {
                    using (JsonTextReader reader = new JsonTextReader(sr))
                    {
                        JObject json = (JObject)JToken.ReadFrom(reader);

                        JArray membersArray = (JArray)json["MEMBERS"];
                        if(membersArray == null)
                        {
                            throw new Exception("유저 정보가 존재하지 않습니다");
                        }

                        foreach (JObject memberJson in membersArray)
                        {
                            JArray playCountArray = (JArray)memberJson["COUNT"];
                            if (playCountArray == null)
                            {
                                throw new Exception("Count 정보가 존재하지 않습니다");
                            }

                            List<Count> playCountList = new List<Count>();
                            foreach (JObject playCountJson in playCountArray)
                            {
                                playCountList.Add(new Count
                                {
                                    Success = (int)playCountJson["SUCCESS"],
                                    Failure = (int)playCountJson["FAILURE"]
                                });
                            }

                            Member member = new Member
                            {
                                Name = memberJson["NAME"]?.Value<string>(),
                                PlayCountList = playCountList
                            };
#if DEBUG
                            Console.WriteLine($"Read: {member.Name, -12}");
#endif
                            members.Add(member);
                        }
                    }
                }

#if DEBUG
                Console.WriteLine("맴버를 정상적으로 불러왔습니다");
#endif
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"{MEMBER_FILENAME} 파일이 존재하지 않습니다. 신규로 생성합니다.");
                Console.WriteLine(e.Message);

                CreateMemberFile();
            }
            catch (IOException e)
            {
                Console.WriteLine("Member 파일을 불러오는 중에 무언가 문제가 생겼습니다.");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Member 파일내에 정보가 없습니다.");
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine();
            }

            return members;
        }

        public static void UpdateMemberFile(HashSet<Member> members)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter($"{MEMBER_FILENAME}", append: false))
                {
                    JArray membersArray = new JArray();

#if DEBUG
                    foreach (Member member in members)
                    {
                        Console.WriteLine($"Update: {member.Name, -12}");
                    }
#endif

                    foreach (Member member in members)
                    {
                        List<Count> countList = member.PlayCountList;
                        JArray countArray = new JArray();
                        foreach (Count count in countList)
                        {
                            JObject countObject = new JObject
                            {
                                { "SUCCESS", (int)count.Success },
                                { "FAILURE", (int)count.Failure }
                            };
                            countArray.Add(countObject);
                        }

                        membersArray.Add(
                            new JObject
                            {
                                { "NAME", member.Name },
                                { "COUNT", countArray }
                            }
                        );
                    }

                    JObject obj = new JObject
                    {
                        { "MEMBERS", membersArray }
                    };
                    sw.Write(obj);

#if DEBUG
                    Console.WriteLine("맴버를 정상적으로 저장했습니다.");
#endif
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("지정된 폴더를 찾지 못해 파일을 저장하지 못했습니다.");
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine("입출력 오류로 인해 파일을 저장하지 못했습니다.");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("알 수 없는 오류로 인해 파일을 저장하지 못했습니다.");
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine();
            }
        }

        private static void CreateMemberFile()
        {
            try
            {
                FileStream fs = File.Create(MEMBER_FILENAME);

                Console.WriteLine("맴버 관리용 파일을 정상적으로 생성했습니다.");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("지정된 폴더를 찾지 못해 파일을 생성하지 못했습니다.");
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine("입출력 오류로 인해 파일을 생성하지 못했습니다.");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("알 수 없는 오류로 인해 파일을 생성하지 못했습니다.");
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine();
            }
        }
    }
}
