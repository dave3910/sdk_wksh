using EXP_UIAPI.DTO;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXP_UIAPI.APIService
{
    internal static class APIUsuarios
    {
        public static async Task<User> GetUserById(string idUsuario)
        {
            User user = null;

            try
            {
                var result = await "https://jsonplaceholder.typicode.com"
                            .AppendPathSegments("users", idUsuario).GetAsync();

                if (result.StatusCode != 200)
                    throw new Exception ("Error en la llamada del servicio");

                string json = await result.ResponseMessage.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(json);
            }
            catch (Exception)
            {
                throw;
            }


            return user;
        }

        public static async Task<List<User>> GetUserList()
        {
            List<User> listUser = null;

            try
            {
                var result = await "https://jsonplaceholder.typicode.com/users/".GetAsync();

                if (result.StatusCode != 200)
                    throw new Exception("Error en la llamada del servicio");

                string json = await result.ResponseMessage.Content.ReadAsStringAsync();
                listUser = JsonConvert.DeserializeObject<List<User>>(json);
            }
            catch (Exception)
            {
                throw;
            }


            return listUser;
        }
    }
}
