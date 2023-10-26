using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CSEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                
                Console.Write("PW 입력 : ");
                string pw = Console.ReadLine();
                if(pw != null && pw != string.Empty)
                {
                    //임의 솔트값 적용된 해시값 생성
                    MyPassword pwd = new MyPassword(pw);
                    Console.Write("솔트 테스트하려면 Y 입력 : ");
                    string saltTest = Console.ReadLine();
                    if(saltTest == "Y")
                    {
                        while (true)
                        {
                            //솔트값을 받아서 해시 생성
                            Console.Write("솔트값 입력 : ");
                            string salt = Console.ReadLine();

                            if (salt != null && salt != string.Empty)
                            {
                                var saltedHash = new MyPassword(pw, salt);
                                Console.WriteLine("솔트된 해시 : " + saltedHash);
                            }
                            else
                            {
                                Console.WriteLine("솔트값을 다시 입력해 주세요");
                                continue;
                            }

                            break;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    Console.WriteLine("프로그램을 종료하시겠습니까? (Y/N)");
                    if(Console.ReadLine() == "Y")
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("비밀번호를 다시 입력해주세요");
                    continue;
                }
            }
        }
    }

    public class MyPassword
    {
        //솔트값
        private string _saltValue;
        public string SaltValue
        {
            get { return _saltValue; }
            set { _saltValue = value; }
        }

        //합친 해시값
        private string _combinedHash;
        public string CombinedHash
        {
            get { return _combinedHash; }
            set { _combinedHash = value; }
        }

        //생성자 : 평문만 삽입후 자동으로 임의 해시값 생성
        public MyPassword(string pw)
        {
            string salt = GetSalt();
            _saltValue = salt;

            string saltedHash = GetSHA256Value(salt + pw);
            _combinedHash = saltedHash;

            Console.WriteLine("생성된 솔트값 : " + salt);
            Console.WriteLine("생성된 전체해시값 : " + saltedHash);
        }

        //생성자 : 솔트 테스트용으로 지정된 솔트값을 평문 앞에 붙여 생성된 해시값을 반환함.
        public MyPassword(string pw, string salt)
        {
            _saltValue = salt;

            string saltedHash = GetSHA256Value(salt + pw);
            _combinedHash = saltedHash;

            Console.WriteLine("생성된 전체해시값 : " + saltedHash);
        }

        //해시값 구하기
        public static string GetSHA256Value(string s)
        {
            //해시값 저장을 위한 str 변수
            string hash = string.Empty;

            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] hashValue = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(s));

                foreach(byte b in hashValue)
                {
                    hash += $"${b:X2}";
                }
            }
            return hash;
        }

        //솔트값 구하기
        private string GetSalt()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                //솔트 길이
                int saltLength = 32;

                //솔트 생성
                byte[] salt = new byte[saltLength];
                rng.GetBytes(salt);

                //Base64 인코딩
                string saltString = Convert.ToBase64String(salt);

                //리턴
                return saltString;
            }
        }
    }
}
