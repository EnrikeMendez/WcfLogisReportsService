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

    [OperationContract]
    [WebInvoke(Method = "GET",
    ResponseFormat = WebMessageFormat.Json)]
    string GetSql(string sql);
	
	
	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetConsultaCambioPrioridad();


	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetModificaCambioPrioridad(string id_crons, string prioridad);
	
	
	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetModificaCambioPrioridadDinamica();


	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetMail(string Id_Cron, string NumCli, string id_mail, string nombre, string correo, string Tercero,string status);

	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetConsultaReportes(string usuario);
}