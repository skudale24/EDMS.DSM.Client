using System;
using System.Net.Mail;

namespace EDM.Email
{
    public class MailMessage
    {
        #region --- Properties ---
        public String Module = String.Empty;
        public String Message = String.Empty;
        public String ConfigKey = String.Empty;
        public long ProgramId;
        public int StatusId = -1;
        #endregion --- Properties ---

        #region --- Constructors ---
        public MailMessage() { ProgramId = EDM.Setting.Session.ProgramId; }
        public MailMessage(String module) : this() { Module = module; }
        public MailMessage(String configKey, long programId) : this() { ProgramId = programId; ConfigKey = configKey; }
        public MailMessage(String module, String configKey, long programId) : this(module) { ProgramId = programId; ConfigKey = configKey; }
        #endregion --- Constructors ---

        #region --- Public Methods ---
        public Boolean SendWLog(String module, Setting.Email.Type objectType, long objectId
            , String fromEmail, String toEmail, String subject, String body, String cc = "", String attachments = "", String bcc = "")
        {
            try
            {
                if (Send(fromEmail, toEmail, subject, body, cc, attachments, bcc))
                {
                    Log lg = new Log(ConfigKey);
                    lg.save(module, objectType, objectId, "To:" + toEmail + ",Cc:" + cc + ",Bcc:" + bcc, subject, body, attachments);

                    Message = "Success";
                    StatusId = 1;
                    return true;
                }
                else
                {
                    StatusId = 0;
                    return false;
                }
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        public Boolean SendWLog(String module, Setting.Email cfg, long objectId, String toEmail, String body
            , String subject = "", String attachments = "", String bccEmail = "", String ccEmail = "")
        {
            try
            {
                EDM.Setting.DB stg = new Setting.DB(ConfigKey, ProgramId);

                if (String.IsNullOrEmpty(subject)) subject = cfg.Subject;
                if (String.IsNullOrEmpty(bccEmail)) bccEmail = cfg.BccEmail;
                if (String.IsNullOrEmpty(ccEmail)) ccEmail = cfg.CcEmail;

                if (Send(stg.GetByKey(Setting.Key.FromEmail), toEmail, subject, body, ccEmail, attachments, bccEmail))
                {
                    Log lg = new Log(ConfigKey);
                    lg.save(module, (Setting.Email.Type)cfg.ObjectId, objectId, "To:" + toEmail + ",Cc:" + ccEmail + ",Bcc:" + bccEmail,
                        subject, body, attachments);

                    Message = "Success";
                    StatusId = 1;
                    return true;
                }
                else
                {
                    StatusId = 0;
                    return false;
                }
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        public Boolean SendWLog(String module, Setting.Email.Type objectType, long objectId, String toEmail, String body, String attachments = "")
        {
            try
            {
                EDM.Setting.DB stg = new Setting.DB(ConfigKey, ProgramId);

                Setting.Email cfg = new Setting.Email(ConfigKey, ProgramId);
                cfg.GetById(objectType);

                ///* START |  March 24, 2021 | DSM-3895 */
                //  String domain = stg.GetByKey(EDM.Setting.Key.SiteUrl);
                //  body = body.Replace("%%DOMAINC%%", domain); //DSM-3895
                ///* START |  March 24, 2021 | DSM-3895 */
               
                body = cfg.ReplaceGeneralSubstitutionTags(body);

                Boolean emailSent = false;
                emailSent = Send(stg.GetByKey(Setting.Key.FromEmail), toEmail, cfg.Subject, body, cfg.CcEmail, attachments, cfg.BccEmail);
                //if error sending email, try again without attachments
                if (!emailSent && attachments.Length > 0)
                    emailSent = Send(stg.GetByKey(Setting.Key.FromEmail), toEmail, cfg.Subject, body, cfg.CcEmail, String.Empty, cfg.BccEmail);

                if (emailSent)
                {
                    Log lg = new Log(ConfigKey);
                    lg.save(module, objectType, objectId, "To:" + toEmail + ",Cc:" + cfg.CcEmail + ",Bcc:" + cfg.BccEmail, cfg.Subject, body, attachments);

                    Message = "Success";
                    StatusId = 1;
                    return true;
                }
                else
                {
                    StatusId = 0;
                    return false;
                }
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        public Boolean Send(String fromEmail, String toEmail, String subject, String body
            , String cc = "", String attachments = "", String bcc = "")
        {
            try
            {
                if (toEmail.Length <= 0 || body.Length <= 0) { Message = "To email or body is empty"; return false; }

                EDM.Setting.DB stg = new Setting.DB(ConfigKey, ProgramId);
                /* Aug 16, 2017 | Nibha Kothari | ES-3710: Implement SMTP Port Setting */
                String smtpHost = stg.GetByKey(Setting.Key.SmtpHost);
                String smtpPort = stg.GetByKey(Setting.Key.SmtpPort);
                if (String.IsNullOrEmpty(smtpPort)) smtpPort = "25";
                String smtpUN = stg.GetByKey(Setting.Key.SmtpUN);
                String smtpPwd = stg.GetByKey(Setting.Key.SmtpPwd);

                SmtpClient smtp = new SmtpClient(smtpHost, int.Parse(smtpPort));
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(smtpUN, smtpPwd);
                /* end Aug 16, 2017 | Nibha Kothari | ES-3710: Implement SMTP Port Setting */

                using (System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage())  // MailMessage needs to be disposed to release locks on Attachment Files
                {
                // from
                mm.From = new MailAddress(fromEmail);
                //to
                foreach (String to in toEmail.Split(';'))
                {
                    /* Sep 06, 2017 | Nibha Kothari | SUP-707: UV:16898 Self Installs: Status - Incomplete and Cancelled E-mails for Customers. */
                    if (!String.IsNullOrEmpty(ValidateEmail(to))) mm.To.Add(to);
                }
                // cc
                if (cc.Length > 0 && !cc.EndsWith(";")) cc += ";";
                foreach (String to in cc.Split(';'))
                {
                    /* Sep 06, 2017 | Nibha Kothari | SUP-707: UV:16898 Self Installs: Status - Incomplete and Cancelled E-mails for Customers. */
                    if (!String.IsNullOrEmpty(ValidateEmail(to))) mm.CC.Add(to);
                }

                // bcc
                if (bcc.Length > 0 && !bcc.EndsWith(";")) bcc += ";";
                bcc += stg.GetByKey(Setting.Key.AdminEmail);
                foreach (String to in bcc.Split(';'))
                {
                    /* Sep 06, 2017 | Nibha Kothari | SUP-707: UV:16898 Self Installs: Status - Incomplete and Cancelled E-mails for Customers. */
                    if (!String.IsNullOrEmpty(ValidateEmail(to))) mm.Bcc.Add(to);
                }

                mm.IsBodyHtml = true;
                mm.Subject = subject;
                mm.Body = body;

                if (attachments.Length > 0)
                {
                    String[] attachs = attachments.Split(';');
                    Attachment at = null;
                    String[] temp;
                    foreach (String attach in attachs)
                    {
                        if (attach.Length > 0)
                        {
                            temp = attach.Split(',');
                            at = new Attachment(temp[1]);
                            at.Name = temp[0];
                            mm.Attachments.Add(at);
                        }
                    }
                }

                smtp.Send(mm);
                }
                
                Message = "Success";
                StatusId = 1;
                return true;
            }
            catch (Exception ex) { Message = ex.ToString(); return false; }
        }

        public Boolean Send(String subject, String body)
        {
            try
            {
                EDM.Setting.DB stg = new Setting.DB(ConfigKey, ProgramId);
                String adminEmail = stg.GetByKey(EDM.Setting.Key.AdminEmail);

                String fromEmail = stg.GetByKey(EDM.Setting.Key.FromEmail);
                if (String.IsNullOrEmpty(fromEmail)) fromEmail = "admin@edms.com";

                Send(fromEmail, adminEmail, subject, body);
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }
        #endregion --- Public Methods ---

        #region --- Private Methods ---
        /* Sep 06, 2017 | Nibha Kothari | SUP-707: UV:16898 Self Installs: Status - Incomplete and Cancelled E-mails for Customers. */
        private String ValidateEmail(String email)
        {
            try
            {
                if (!email.Contains("@")) return String.Empty;
            }
            catch { return String.Empty; }
            return email;
        }
        /* end Sep 06, 2017 | Nibha Kothari | SUP-707: UV:16898 Self Installs: Status - Incomplete and Cancelled E-mails for Customers. */
        #endregion --- Private Methods ---
    }
}
