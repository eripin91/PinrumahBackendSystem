using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PinBackendSystem.Areas.Identity
{
	public class PasswordHasherWithOldMembershipSupport : IPasswordHasher<PinrumahUser>
	{
		//an instance of the default password hasher
		IPasswordHasher<PinrumahUser> _identityPasswordHasher = new PasswordHasher<PinrumahUser>();

		//Hashes the password using old algorithm from the days of ASP.NET Membership
		internal static string HashPasswordInOldFormat(string password)
		{
			var sha1 = SHA1.Create();
			var data = Encoding.UTF8.GetBytes("KZgft0ebFCrZUzeeiix"+password);
			var sha1data = sha1.ComputeHash(data);

			var sb = new StringBuilder();
			foreach (var hashByte in sha1data)
			{
				sb.AppendFormat("{0:x2}", hashByte);
			}

			return sb.ToString();
		}

		//the passwords of the new users will be hashed with new algorithm
		public string HashPassword(PinrumahUser user, string password)
		{
			return _identityPasswordHasher.HashPassword(user, password);
		}

		public PasswordVerificationResult VerifyHashedPassword(PinrumahUser user,
					string hashedPassword, string providedPassword)
		{
			string pwdHash2 = HashPasswordInOldFormat(providedPassword);


			if (hashedPassword == pwdHash2)
			{
				//first we check the hashed password with "old" hash
				return PasswordVerificationResult.Success;
			}
			else
			{
				//if old hash doesn't work - use the default approach 
				return _identityPasswordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
			}
		}
	}
}
