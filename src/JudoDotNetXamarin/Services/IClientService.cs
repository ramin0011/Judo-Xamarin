using System;
using JudoDotNetXamarin.Models;
using Newtonsoft.Json.Linq;

namespace JudoDotNetXamarin
{
	public interface IClientService
	{
		JObject GetClientDetails ();
		string GetSDKVersion ();
	}
}

