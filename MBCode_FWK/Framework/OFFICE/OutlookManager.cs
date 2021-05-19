#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Collections.Generic;
using Outlook = Microsoft.Office.Interop.Outlook;

#endregion

namespace MBCode.Framework.Managers
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha: 24/10/2017.
    /// Descripción: clase que controla l'enviament de missatges Outlook.
    /// </summary>
    public static class OutlookManager
    {
        public static int SendEmail(string EMail_Address, string Subject, string Body_Message, 
                                    List<Tuple<string, string>> FileAttachments, out string ErrMsg)
        {
                try
                {
                    //  Create the Outlook application by using inline initialization.
                        Outlook.Application oApp = new Outlook.Application();
                    //  Create the new message by using the simplest approach.
                        Outlook.MailItem oMsg = oApp.CreateItem(Outlook.OlItemType.olMailItem) as Outlook.MailItem;  
                    //  Add a recipient. EMail address of destinataires.
                        Outlook.Recipient oRecip = (Outlook.Recipient)oMsg.Recipients.Add(EMail_Address);
                        oRecip.Resolve();
                    //  Set the basic properties.
                        oMsg.Subject = Subject ?? string.Empty;
                        oMsg.Body = Body_Message ?? string.Empty;
                    //  Add an attachment.
                        List<Outlook.Attachment> fileAttach = new List<Outlook.Attachment>();
                        if (FileAttachments != null)
                        {
                            int indexAttachment = 1;
                            foreach (Tuple<string, string> fileAttachment in FileAttachments)
                            {
                                string fileSource = fileAttachment.Item1;
                                string attachDisplayName = fileAttachment.Item2;
                                int iPosition = (int)oMsg.Body.Length + indexAttachment;
                                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                                fileAttach.Add(oMsg.Attachments.Add(fileSource, iAttachType, iPosition, attachDisplayName));
                                indexAttachment++;
                            }
                        }
                    //  If you want to, display the message.
                        oMsg.Display(true);  //modal
                    //  Send the message (not for us).
                        //oMsg.Save();
                        //oMsg.Send();
                    //  Explicitly release objects.
                        fileAttach.Clear();
                        fileAttach = null;
                        oMsg = null;
                        oApp = null;
                    //  Default return value.
                        ErrMsg = string.Empty;
                        return 0;
                }
            //  Simple error handler.
                catch (Exception ex)
                {
                    ErrMsg = MsgManager.ExcepMsg(ex);
                    return -1;
                }
        }

    }
}
