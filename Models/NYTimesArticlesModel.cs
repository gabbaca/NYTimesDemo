using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace NYTimesDemo.Models
{    
    public partial class NYTimesArticlesModel
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("num_results")]
        public long NumResults { get; set; }

        [JsonProperty("results")]
        public Result[] Results { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("asset_id")]
        public long AssetId { get; set; }

        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("published_date")]
        public DateTimeOffset PublishedDate { get; set; }

        [JsonProperty("updated")]
        public DateTimeOffset Updated { get; set; }

        [JsonProperty("section")]
        public string Section { get; set; }

        [JsonProperty("subsection")]
        public string Subsection { get; set; }

        [JsonProperty("nytdsection")]
        public string Nytdsection { get; set; }

        [JsonProperty("adx_keywords")]
        public string AdxKeywords { get; set; }

        [JsonProperty("column")]
        public object Column { get; set; }

        [JsonProperty("byline")]
        public string Byline { get; set; }

        [JsonProperty("type")]
        public ResultType Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("abstract")]
        public string Abstract { get; set; }

        [JsonProperty("des_facet")]
        public string[] DesFacet { get; set; }

        [JsonProperty("org_facet")]
        public string[] OrgFacet { get; set; }

        [JsonProperty("per_facet")]
        public string[] PerFacet { get; set; }

        [JsonProperty("geo_facet")]
        public string[] GeoFacet { get; set; }

        [JsonProperty("media")]
        public Media[] Media { get; set; }

        [JsonProperty("eta_id")]
        public long EtaId { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("type")]
        public MediaType Type { get; set; }

        [JsonProperty("subtype")]
        public Subtype Subtype { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("approved_for_syndication")]
        public long ApprovedForSyndication { get; set; }

        [JsonProperty("media-metadata")]
        public MediaMetadatum[] MediaMetadata { get; set; }
    }

    public partial class MediaMetadatum
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("format")]
        public Format Format { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }

    public enum Format { MediumThreeByTwo210, MediumThreeByTwo440, StandardThumbnail };

    public enum Subtype { Photo };

    public enum MediaType { Image };

    public enum Source { NewYorkTimes };

    public enum ResultType { Article };

    public partial class NYTimesArticlesModel
    {
        public static NYTimesArticlesModel FromJson(string json) => JsonConvert.DeserializeObject<NYTimesArticlesModel>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this NYTimesArticlesModel self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                FormatConverter.Singleton,
                SubtypeConverter.Singleton,
                MediaTypeConverter.Singleton,
                SourceConverter.Singleton,
                ResultTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class FormatConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Format) || t == typeof(Format?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Standard Thumbnail":
                    return Format.StandardThumbnail;
                case "mediumThreeByTwo210":
                    return Format.MediumThreeByTwo210;
                case "mediumThreeByTwo440":
                    return Format.MediumThreeByTwo440;
            }
            throw new Exception("Cannot unmarshal type Format");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Format)untypedValue;
            switch (value)
            {
                case Format.StandardThumbnail:
                    serializer.Serialize(writer, "Standard Thumbnail");
                    return;
                case Format.MediumThreeByTwo210:
                    serializer.Serialize(writer, "mediumThreeByTwo210");
                    return;
                case Format.MediumThreeByTwo440:
                    serializer.Serialize(writer, "mediumThreeByTwo440");
                    return;
            }
            throw new Exception("Cannot marshal type Format");
        }

        public static readonly FormatConverter Singleton = new FormatConverter();
    }

    internal class SubtypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Subtype) || t == typeof(Subtype?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "photo")
            {
                return Subtype.Photo;
            }
            throw new Exception("Cannot unmarshal type Subtype");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Subtype)untypedValue;
            if (value == Subtype.Photo)
            {
                serializer.Serialize(writer, "photo");
                return;
            }
            throw new Exception("Cannot marshal type Subtype");
        }

        public static readonly SubtypeConverter Singleton = new SubtypeConverter();
    }

    internal class MediaTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(MediaType) || t == typeof(MediaType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "image")
            {
                return MediaType.Image;
            }
            throw new Exception("Cannot unmarshal type MediaType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (MediaType)untypedValue;
            if (value == MediaType.Image)
            {
                serializer.Serialize(writer, "image");
                return;
            }
            throw new Exception("Cannot marshal type MediaType");
        }

        public static readonly MediaTypeConverter Singleton = new MediaTypeConverter();
    }

    internal class SourceConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Source) || t == typeof(Source?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "New York Times")
            {
                return Source.NewYorkTimes;
            }
            throw new Exception("Cannot unmarshal type Source");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Source)untypedValue;
            if (value == Source.NewYorkTimes)
            {
                serializer.Serialize(writer, "New York Times");
                return;
            }
            throw new Exception("Cannot marshal type Source");
        }

        public static readonly SourceConverter Singleton = new SourceConverter();
    }

    internal class ResultTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ResultType) || t == typeof(ResultType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Article")
            {
                return ResultType.Article;
            }
            throw new Exception("Cannot unmarshal type ResultType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ResultType)untypedValue;
            if (value == ResultType.Article)
            {
                serializer.Serialize(writer, "Article");
                return;
            }
            throw new Exception("Cannot marshal type ResultType");
        }

        public static readonly ResultTypeConverter Singleton = new ResultTypeConverter();
    }
}
