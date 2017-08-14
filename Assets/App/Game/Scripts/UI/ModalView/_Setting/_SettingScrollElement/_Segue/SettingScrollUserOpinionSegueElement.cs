using System.Collections;
using System.Collections.Generic;
using SWorker;
using TKPopup;
using UnityEngine;

namespace Culsu
{
    public class SettingScrollUserOpinionSegueElement : SettingScrollSegueElementBase
    {
        protected override void OnClick()
        {
            base.OnClick();
            string[] to = new string[]
            {
                CSLocalizeManager.Instance.GetString(TKLOCALIZE.SETTING_USER_OPINION_MAIL_ADDRESS)
            };
            string[] cc = new string[] {""};
            string[] bcc = new string[] {""};
            string subject = CSLocalizeManager.Instance.GetString(TKLOCALIZE.SETTING_USER_OPINION_MAIL_TITLE);
            string message = string.Format
            (
                CSLocalizeManager.Instance.GetString(TKLOCALIZE.SETTING_USER_OPINION_MAIL_MESSAGE),
                CSUserDataManager.Instance.Data.Id
            );
            string imagePath = "";
            SocialWorker.PostMail
            (
                to,
                cc,
                bcc,
                subject,
                message,
                imagePath,
                result =>
                {
                    if (result != SocialWorkerResult.Success)
                    {
                        CSPopupManager.Instance
                            .Create<CSSingleSelectPopup>()
                            .SetTitle
                            (
                                CSLocalizeManager.Instance.GetString(TKLOCALIZE.SETTING_MAILER_ACCOUNT_ERROR_TITLE)
                            )
                            .SetDescription
                            (
                                CSLocalizeManager.Instance.GetString(TKLOCALIZE.SETTING_MAILER_ACCOUNT_ERROR_MESSAGE)
                            );
                    }
                }
            );
        }
    }
}