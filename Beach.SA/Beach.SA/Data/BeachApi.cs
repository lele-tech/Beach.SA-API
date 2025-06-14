namespace Beach.SA.Data
{
    public class BeachApi
    {
        //metodo que inicializa la comunicacion con la api

        public HttpClient Inicial()
        {
            //variable para manejar el protocolo HttpClient
            var client= new HttpClient();

            
            client.BaseAddress = new Uri("http://www.apiwebhotelbeachsa.somee.com/");

            return client;
        }

    }
}
