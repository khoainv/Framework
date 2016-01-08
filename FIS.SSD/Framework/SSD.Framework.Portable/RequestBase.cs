using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;

namespace SSD.Framework
{
	public class RequestBase {
        private string _resource;
        private string _ugToken;
        private BaseInput _data;

        public RequestBase(string resource, string ugToken)
        {
            _resource = resource;
            _ugToken = ugToken;
            
        }
        public RequestBase(string resource, string ugToken, BaseInput data)
        {
            _resource = resource;
            _ugToken = ugToken;
            _data = data;
        }
        public BaseInput Data
        {
            get { return _data; }
        }
        [JsonIgnore]
        public string UGToken
        {
            get { return _ugToken; }
        }
		[JsonIgnore]
		public string Resource {
            get { return _resource; }
		}

		[JsonIgnore]
		public HttpMethod Method {
            get { return HttpMethod.Post; }
		}
	}
	
}
