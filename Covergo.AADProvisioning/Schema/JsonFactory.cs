using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Covergo.AADProvisioning.Schema
{
    public abstract class JsonFactory
    {
        private static readonly Lazy<JsonFactory> LargeObjectFactory =
            new Lazy<JsonFactory>(
                () =>
                    new TrustedJsonFactory());

        private static readonly Lazy<JsonFactory> Singleton =
            new Lazy<JsonFactory>(
                () =>
                    JsonFactory.InitializeFactory());

        public static JsonFactory Instance
        {
            get
            {
                return JsonFactory.Singleton.Value;
            }
        }

        public abstract Dictionary<string, object> Create(string json);

        

        public abstract string Create(string[] json);

        public virtual string Create(string[] json, bool acceptLargeObjects)
        {
            string result =
                acceptLargeObjects ?
                    JsonFactory.LargeObjectFactory.Value.Create(json) :
                    this.Create(json);
            return result;
        }

        public abstract string Create(Dictionary<string, object> json);

        
        public abstract string Create(IDictionary<string, object> json);
       
        public abstract string Create(IReadOnlyDictionary<string, object> json);
       

        private static JsonFactory InitializeFactory()
        {
            JsonFactory result;
            if (SystemForCrossDomainIdentityManagementConfigurationSection.Instance.AcceptLargeObjects)
            {
                result = new TrustedJsonFactory();
            }
            else
            {
                result = new Implementation();
            }
            return result;
        }

        private class Implementation : JsonFactory
        {
            public override Dictionary<string, object> Create(string json)
            {
                try
                {
                    Dictionary<string, object> result =
                        JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    return result;
                }
                finally
                {
                }
            }

            public override string Create(string[] json)
            {
                string result = JsonConvert.SerializeObject(json);
                return result;
            }

            public override string Create(Dictionary<string, object> json)
            {
                string result = JsonConvert.SerializeObject(json);
                return result;
            }

            public override string Create(IDictionary<string, object> json)
            {
                string result = JsonConvert.SerializeObject(json);
                return result;
            }

            public override string Create(IReadOnlyDictionary<string, object> json)
            {
                string result = JsonConvert.SerializeObject(json);
                return result;
            }
        }

        //public virtual Dictionary<string, object> Create(string json, bool acceptLargeObjects)
        //{
        //    Dictionary<string, object> result =
        //        acceptLargeObjects ?
        //            JsonFactory.LargeObjectFactory.Value.Create(json) :
        //            this.Create(json);
        //    return result;
        //}

        //public virtual string Create(Dictionary<string, object> json, bool acceptLargeObjects)
        //{
        //    string result =
        //        acceptLargeObjects ?
        //            JsonFactory.LargeObjectFactory.Value.Create(json) :
        //            this.Create(json);
        //    return result;
        //}

        //public virtual string Create(IDictionary<string, object> json, bool acceptLargeObjects)
        //{
        //    string result =
        //        acceptLargeObjects ?
        //            JsonFactory.LargeObjectFactory.Value.Create(json) :
        //            this.Create(json);
        //    return result;
        //}

        //public virtual string Create(IReadOnlyDictionary<string, object> json, bool acceptLargeObjects)
        //{
        //    string result =
        //        acceptLargeObjects ?
        //            JsonFactory.LargeObjectFactory.Value.Create(json) :
        //            this.Create(json);
        //    return result;
        //}
    }
}
