using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class UserViewModel
    {


		//[JsonProperty(PropertyName = "Id")]
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string Gender { get; set; }
		public string PermanentAddress { get; set; }
		public string TemporaryAddress { get; set; }
		public string HouseNumber { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string Country { get; set; }
		public int RoleId { get; set; }
	}
}
