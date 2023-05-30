using System.Data;
using System.ServiceModel;
using System.ServiceModel.Web;

// NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
[ServiceContractAttribute]
public interface IReport_Service
{
	[OperationContract]
	[WebInvoke(Method = "GET",
	 ResponseFormat = WebMessageFormat.Json)]
	string GetData(int number1, int number2);

	[OperationContract]
	CompositeType GetDataUsingDataContract(CompositeType composite);

	// TODO: agregue aquí sus operaciones de servicio
	
	[OperationContract]
    [WebInvoke(Method = "GET",
     ResponseFormat = WebMessageFormat.Json)]
    string GetMonitoreoRep();


    [OperationContract]
    [WebInvoke(Method = "GET",
    ResponseFormat = WebMessageFormat.Json)]
    string GetConsultaErrores();
	
}