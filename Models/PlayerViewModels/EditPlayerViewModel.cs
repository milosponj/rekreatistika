using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Rekreatistika.Models.PlayerViewModels
{
	public class EditPlayerViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Ime i prezime su obavezni podaci!")]
		public string Name { get; set; }

		public string Nickname { get; set; }

		[Required(ErrorMessage = "Email je obavezan.")]
		[EmailAddress]
		public string Email { get; set; }

		public DateTime? DateOfBirth { get; set; }

		public IFormFile Avatar { get; set; }
		public string AvatarPath => Id.ToString() + ".jpg";

		public string FlatpickrDateOfBirthFormat
		{
			get
			{
				if (DateOfBirth.HasValue)
					return String.Format("{0}-{1}-{2}", DateOfBirth.Value.Year, DateOfBirth.Value.Month, DateOfBirth.Value.Day);
				else
					return "";
			}
		}
	}
}
