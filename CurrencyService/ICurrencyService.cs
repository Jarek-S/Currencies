using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CurrencyService
{
    
    [ServiceContract]
    public interface ICurrencyService
    {
        [OperationContract]
        string[] GetRate(string value);

        [OperationContract]
        string[] GetCodes();

        [OperationContract]
        string GetDate();
    }

}
