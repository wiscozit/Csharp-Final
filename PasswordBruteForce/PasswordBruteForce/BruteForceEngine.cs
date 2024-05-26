using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordBruteForce
{
    public class BruteForceEngine
    {
        private readonly string _encryptedPassword;
        private readonly string _Password;
        public string _FoundSalt;
        private readonly int _maxThreads;
        private readonly Action<string> _onPasswordFound;

        public BruteForceEngine(string encryptedPassword, string Password, int maxThreads, Action<string> onPasswordFound)
        {
            _FoundSalt = "N/A";
            _encryptedPassword = encryptedPassword;
            _Password = Password;
            _maxThreads = maxThreads;
            _onPasswordFound = onPasswordFound;
        }

        public void Start()
        {
            var startTime = DateTime.Now;
            Parallel.ForEach(GenerateCombinations(), new ParallelOptions { MaxDegreeOfParallelism = _maxThreads }, (password, state) =>
            {
                if (PasswordManager.VerifyPassword(_Password, password, _encryptedPassword))
                {
                    _onPasswordFound(password);
                    _FoundSalt = password;
                    state.Break();
                }
            });
            var endTime = DateTime.Now;
            Console.WriteLine($"Time elapsed: {endTime - startTime}");
        }

        public string GetSalt()
        {
            return _FoundSalt;
        }

        private IEnumerable<string> GenerateCombinations()
        {
            char[] charset = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            int maxLength = 4; // Set a reasonable max length for demonstration purposes

            for (int length = 1; length <= maxLength; length++)
            {
                var current = new char[length];
                foreach (var combination in GenerateCombinationsRecursive(current, charset, 0))
                {
                    yield return new string(combination);
                }
            }
        }

        private IEnumerable<char[]> GenerateCombinationsRecursive(char[] current, char[] charset, int position)
        {
            if (position == current.Length)
            {
                yield return current.ToArray();
            }
            else
            {
                foreach (var c in charset)
                {
                    current[position] = c;
                    foreach (var combination in GenerateCombinationsRecursive(current, charset, position + 1))
                    {
                        yield return combination;
                    }
                }
            }
        }
    }
}