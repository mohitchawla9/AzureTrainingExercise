using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace AzureWebJobDemo
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        [NoAutomaticTrigger]
        public void TriggerHandler(TextWriter log)
        {
            try
            {
                log.WriteLine("Trigger is Fired");

            }
            catch (Exception ex)
            {
                log.WriteLine("Exception in Trigger Handler", ex);
                throw ex;
            }
        }
    }
}
