﻿//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.5.0.0 (NJsonSchema v10.1.15.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."

namespace IntranetCore.WebClient.Models
{
    using IntranetCore.Api.ValidationAttributes;
    using System = global::System;
    
   
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.15.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class EtiquetaModel 
    {
        /// <summary>Nombre de la etiqueta</summary>
        [Newtonsoft.Json.JsonProperty("nombre", Required = Newtonsoft.Json.Required.Always)]
        public string Nombre { get; set; }

        [MustBeHexColor]
        /// <summary>Color de la etiqueta en hexadecimal. Ej: #FF4499</summary>
        [Newtonsoft.Json.JsonProperty("hexColor", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string HexColor { get; set; }
    
        /// <summary>Fecha de la última modificación efectuada a la etiqueta</summary>
        [Newtonsoft.Json.JsonProperty("fechaUltimaModificacion", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset FechaUltimaModificacion { get; set; }
    
    
    }
    
    /// <summary>Entrada/post de la intranet</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.15.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class EntradaModel 
    {
        /// <summary>Identificador único</summary>
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid Id { get; set; }
    
        /// <summary>Titulo de la entrada</summary>
        [Newtonsoft.Json.JsonProperty("titulo", Required = Newtonsoft.Json.Required.Always)]
        public string Titulo { get; set; }
    
        /// <summary>Contenido de la entrada</summary>
        [Newtonsoft.Json.JsonProperty("contenido", Required = Newtonsoft.Json.Required.Always)]
        public string Contenido { get; set; }
    
        /// <summary>Etiquetas asociadas con esta entrada</summary>
        [Newtonsoft.Json.JsonProperty("etiquetas", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<EtiquetaModel> Etiquetas { get; set; }
    
    
    }
    
    /// <summary>Modelo para crear una nueva entrada o post en la intranet</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.15.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class EntradaForCreationModel 
    {
        /// <summary>Titulo de la entrada</summary>
        [Newtonsoft.Json.JsonProperty("titulo", Required = Newtonsoft.Json.Required.Always)]
        public string Titulo { get; set; }
    
        /// <summary>Contenido de la entrada</summary>
        [Newtonsoft.Json.JsonProperty("contenido", Required = Newtonsoft.Json.Required.Always)]
        public string Contenido { get; set; }
    
        /// <summary>Lista de nombres de etiquetas para asociar con la entrada que se quiere crear</summary>
        [Newtonsoft.Json.JsonProperty("etiquetas", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<string> Etiquetas { get; set; }
    
    
    }
    
    /// <summary>Modelo para crear asociar una etiqueta</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.15.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class EtiquetaForAssociationModel 
    {
        /// <summary>Nombre de la etiqueta</summary>
        [Newtonsoft.Json.JsonProperty("nombre", Required = Newtonsoft.Json.Required.Always)]
        public string Nombre { get; set; }
    
    
    }
    
    /// <summary>Modelo para crear una nueva etiqueta</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.15.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class EtiquetaForCreationModel 
    {
        /// <summary>Nombre de la etiqueta. Debe ser único</summary>
        [Newtonsoft.Json.JsonProperty("nombre", Required = Newtonsoft.Json.Required.Always)]
        public string Nombre { get; set; }

        [MustBeHexColor]
        /// <summary>Color de la etiqueta en hexadecimal. Ej: #FF4499</summary>
        [Newtonsoft.Json.JsonProperty("hexColor", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string HexColor { get; set; }
    
    
    }
    
    /// <summary>Modelo para actualizar una etiqueta</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.15.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class EtiquetaForUpdateModel 
    {
        /// <summary>Color de la etiqueta en hexadecimal. Ej: #FF4499</summary>
        [Newtonsoft.Json.JsonProperty("hexColor", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string HexColor { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.5.0.0 (NJsonSchema v10.1.15.0 (Newtonsoft.Json v12.0.0.0))")]
    public partial class ApiException : System.Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException) 
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + response.Substring(0, response.Length >= 512 ? 512 : response.Length), innerException)
        {
            StatusCode = statusCode;
            Response = response; 
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.5.0.0 (NJsonSchema v10.1.15.0 (Newtonsoft.Json v12.0.0.0))")]
    public partial class ApiException<TResult> : ApiException
    {
        public TResult Result { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException) 
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }
    }

}

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore  472
#pragma warning restore  114
#pragma warning restore  108