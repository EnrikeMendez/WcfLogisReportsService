using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

// NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
[AspNetCompatibilityRequirements(RequirementsMode
	= AspNetCompatibilityRequirementsMode.Allowed)]
public class Service : IService
{
	
	[WebInvoke(Method = "GET",
	BodyStyle = WebMessageBodyStyle.Wrapped,
	ResponseFormat = WebMessageFormat.Json)]
	public string GetData(int number)
	{
		return string.Format("You entered: {0}", number);
	}

	public CompositeType GetDataUsingDataContract(CompositeType composite)
	{
		if (composite == null)
		{
			throw new ArgumentNullException("composite");
		}
		if (composite.BoolValue)
		{
			composite.StringValue += "Suffix";
		}
		return composite;
	}
}