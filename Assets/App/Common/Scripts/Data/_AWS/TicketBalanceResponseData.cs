using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    /// <summary>
    /// チケット残高照会のレスポンスデータ
    /// </summary>
    public class TicketBalanceResponseData
    {
        /// <summary>
        /// リクエスト投げる先のURL
        /// </summary>
        public static readonly string URL =
#if SYMBOL_RELEASE
#if UNITY_IOS
            "https://purchase00001.yakitoriapp.net/sangokushi/ios/v1/item.php";
#elif UNITY_ANDROID
            "https://purchase00001.yakitoriapp.net/sangokushi/android/v1/item.php";
#else
            "";
#endif

#else

#if UNITY_IOS
            "http://52.199.2.138/sangokushi/ios/v1/items.php";
#elif UNITY_ANDROID
            "http://52.199.2.138/sangokushi/android/v1/items.php";
#else
            "";
#endif
    
#endif

        [SerializeField]
        private string status;

        [SerializeField]
        private int paid_ticket;

        [SerializeField]
        private int free_ticket;

        [SerializeField]
        private string message;

        /// <summary>
        /// 有償と無償のチケット総数
        /// </summary>
        public int AmountTicketNum
        {
            get { return paid_ticket + free_ticket; }
        }

        public string Status
        {
            get { return status; }
        }

        public int PaidTicket
        {
            get { return paid_ticket; }
        }

        public int FreeTicket
        {
            get { return free_ticket; }
        }

        public string Message
        {
            get { return message; }
        }
    }
}