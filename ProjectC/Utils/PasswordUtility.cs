using System.Text;
using System.Security.Cryptography;

namespace ProjectC.Utils
{
	public class PasswordUtility
	{
		public static string GetPasswordHash(string password)
		{
			using (var sha = new SHA256Managed())
			{
				byte[] textBytes = Encoding.UTF8.GetBytes(password);
				byte[] hashBytes = sha.ComputeHash(textBytes);

				string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

				return hash;
			}
		}
	}
}
