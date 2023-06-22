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
	string GetMail(string Id_Cron, string NumCli, string id_mail, string nombre, string correo, string Tercero, string status, string hdnURI);

	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetConsultaReportes(string usuario, string status);

	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetStatusReporte(string idreporte, string accion);

	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetListaCorreos(string lista);

	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetArmarContactos(string mail_id);

	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetArmarCorreos(string mail_list, string id_client);

	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetValidaCorreos(string mail_list);

	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetValidaCorreosOriginales(string mail_list);

	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetEliminaOriginales(string mail_list);

	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetInsertaRegistro(string nameobjeto, string contenareglo);

	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetModificaReporte(string usuario, string idCron);

	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetReporte(string usuario, string idreporte);

	[OperationContract]
	[WebInvoke(Method = "GET",
	ResponseFormat = WebMessageFormat.Json)]
	string GetReferencia(string idfrecuencia);
}