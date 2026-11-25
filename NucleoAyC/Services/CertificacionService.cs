using Google.Protobuf.WellKnownTypes;
using ModelNucleo.Model;
using ModelNucleo.Model.Facturacion;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class CertificacionService
{
    private readonly HttpClient _httpClient;

    public CertificacionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public string CertificarDocumento(CertificacionRequest request)
    {

        string jsonRequest = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore, // Ignora valores nulos
            DefaultValueHandling = DefaultValueHandling.Ignore // Ignora valores por defecto
        });
        string result = "";
        //url desarrollo
        //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://certificador.infile.com.sv/api/v2/certificacion/test/documento/certificar");
        //url produccion
        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://certificador.infile.com.sv/api/v2/certificacion/prod/documento/certificar");
        httpWebRequest.Method = "POST";
        httpWebRequest.Accept = "application/json";
        httpWebRequest.Headers.Add("usuario", "02032402111018");
        //llave produccion
        httpWebRequest.Headers.Add("llave", "06a8e6fa9b918de10cd7db58cce29b2d");
       // llave desarrollo
      //  httpWebRequest.Headers.Add("llave", "b09897717b90441cb0e217c12e44d0fe");

        httpWebRequest.ContentType = "application/json";
        //  var jsonRequest = JsonSerializer.Serialize(request);
        try
        {
            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonRequest); 
            }
            
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                string value2 = streamReader.ReadToEnd();
                result = JsonConvert.DeserializeObject(value2).ToString();
            }

            Console.WriteLine("HTTP STATUS CERTIFICACION: " + httpWebResponse.StatusCode);
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                using HttpWebResponse httpWebResponse2 = (HttpWebResponse)ex.Response;
                using StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream());
                result = streamReader2.ReadToEnd().ToString();
            }
        }
        return result;
    }
}