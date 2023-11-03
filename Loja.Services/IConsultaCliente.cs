using Loja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Loja.Services
{
    // OBSERVAÇÃO: Você pode usar o comando "Renomear" no menu "Refatorar" para alterar o nome da interface "IConsultaCliente" no arquivo de código e configuração ao mesmo tempo.
    [ServiceContract]
    public interface IConsultaCliente
    {
        [OperationContract]
        ClienteInfo ConsultarPorEmail(string chave, string email);
    }
}
