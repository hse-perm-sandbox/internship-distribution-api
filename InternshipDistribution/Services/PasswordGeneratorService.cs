namespace InternshipDistribution.Services
{
    public class PasswordGeneratorService
    {
        private const string ValidChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
        private readonly Random _random = new();

        public string Generate(int length = 12)
        {
            var chars = new char[length];

            for (int i = 0; i < length; i++)
                chars[i] = ValidChars[_random.Next(ValidChars.Length)];

            return new string(chars);
        }
    }
}
