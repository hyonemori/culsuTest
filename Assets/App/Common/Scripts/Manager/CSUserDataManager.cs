using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FGFirebaseUser;
using TKF;
using TKEncPlayerPrefs;
using Firebase.Database;
using FGFirebaseFramework;
using TKIndicator;
using TKPopup;

namespace Culsu
{
    public class CSUserDataManager : FGFirebaseUserDataManagerBase<CSUserDataManager, CSUserData>
    {
        /// <summary>
        /// User Registration
        /// </summary>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public IEnumerator UserRegistration_
        (
            string userName,
            Action<bool> onComplete
        )
        {
            //select nation
            GameDefine.NationType selectNation = CSUserRegistrationManager.Instance.GetSelectNation();
            //is has key
            if (TKPlayerPrefs.HasKey(AppDefine.USER_ID_KEY))
            {
                //update data
                yield return UpdateData_
                (
                    GetNewUserData
                    (
                        TKPlayerPrefs.LoadString(AppDefine.USER_ID_KEY),
                        userName,
                        selectNation
                    ),
                    isSucceed =>
                    {
                        onComplete.SafeInvoke(isSucceed);
                    }
                );
                yield break;
            }
            //json data
            UserRegistrationResponseData userRegistrationResponseData = null;
            //uuid 作成
            System.Guid guid = System.Guid.NewGuid();
            string uuid = guid.ToString();
            //form
            WWWForm userRegistrationForm = new WWWForm();
            //set post field
            userRegistrationForm.AddField("user_id", uuid);
            //Log
            Debug.LogFormat("Post UUID:{0}", uuid);
            //id
            string id = "";
            //post
            yield return CSWebRequestManager.Instance.Post_
            (
                UserRegistrationResponseData.URL,
                userRegistrationForm,
                request =>
                {
                    if (request.isError)
                    {
                        Debug.LogErrorFormat("Failed User Registration For Aws error:{0}", request.error);
                    }
                    else
                    {
                        //log
                        Debug.LogFormat
                            ("Succeed User Registration For Aws json:{0}".Green(), request.downloadHandler.text);
                        //json
                        userRegistrationResponseData =
                            JsonUtility.FromJson<UserRegistrationResponseData>(request.downloadHandler.text);
                        //set id
                        id = userRegistrationResponseData.UserId;
                    }
                }
            );
            //id check
            if (userRegistrationResponseData.Status != "0" ||
                id.IsNullOrEmpty())
            {
                yield return CSPopupManager.Instance
                    .Create<CSSingleSelectPopup>()
                    .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                    .SetDescription
                    (
                        string.Format
                        (
                            CSLocalizeManager.Instance.GetString
                                (TKLOCALIZE.USER_REGISTRATION_FAILED_TEXT),
                            userRegistrationResponseData.Status
                        )
                    )
                    .WaitForCompletion();
                onComplete.SafeInvoke(false);
                yield break;
            }
            //new userdata
            CSUserData userData = GetNewUserData(id, userName, selectNation);
            //create
            yield return CSUserDataManager.Instance.Create_
            (
                userData,
                isCreateSucceed =>
                {
                    if (isCreateSucceed)
                    {
                        //save id
                        TKPlayerPrefs.SaveString(AppDefine.USER_ID_KEY, id);
                    }
                    onComplete.SafeInvoke(isCreateSucceed);
                }
            );
        }

        /// <summary>
        /// Get New User Data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <param name="userNation"></param>
        /// <returns></returns>
        private CSUserData GetNewUserData
        (
            string id,
            string userName,
            GameDefine.NationType userNation
        )
        {
            //userdata
            CSUserData userData = new CSUserData()
            {
                Id = id,
                Platform = Application.platform,
                UserName = userName,
                UserNation = userNation,
                Language = Application.systemLanguage,
                LastLoadOrUpdateTimestamp = CSUtcUnixTimeManager.Instance.CurrentUnixTime,
                ResumptionAppData = new CSUserResumptionAppData(),
                KininNum = CSBigIntegerValue.Create(0),
                GameProgressData = new CSUserGameProgressData(),
                UserNationStageData = CSUserNationStageData.Create
                (
                    CSNationStageDataManager.Instance.DataList
                        .Where(s => s.NationType == userNation)
                        .RandomValue()
                ),
                CurrentStageData = CSUserStageData
                    .Create(CSStageDataManager.Instance.GetStageDataFromStageNumber(1)),
                UserPlayerDataList = CSPlayerDataManager.Instance.DataList
                    .Select(s => CSUserPlayerData.Create(s))
                    .ToList(),
                UserHeroList = CSHeroDataManager.Instance.DataList
                    .Select(s => CSUserHeroData.Create(s))
                    .ToList(),
                UserSecretTreasuerList = CSSecretTreasureDataManager.Instance.DataList
                    .Select(s => CSUserSecretTreasureData.Create(s))
                    .Shuffle()
                    .ToList(),
                UserTrophyList = CSTrophyDataManager.Instance.DataList
                    .Select(s => CSUserTrophyData.Create(s))
                    .ToList(),
            };
            //player release
            userData.CurrentNationUserPlayerData.IsReleasedEvenOnce = true;
            //trophy update
            userData.UserTrophyList
                .FirstOrDefault(t => t.Id == CSTrophyDefine.TROPHY_MAX_COLLECT_HERO)
                .SetValue(1);
            //return new user data
            return userData;
        }

        /// <summary>
        /// Load the specified isSucceed.
        /// </summary>
        /// <param name="isSucceed">Is succeed.</param>
        public override IEnumerator Load_(System.Action<bool> isSucceed)
        {
            //user data json
            string userDataJson = "";
            //user id
            string userId = TKPlayerPrefs.LoadString(AppDefine.USER_ID_KEY);
            //if haskey
            if (LocalStorageUtil.LoadText
                (
                    AppDefine.USER_DATA_KEY,
                    out userDataJson,
                    TKFDefine.LocalStoragePathType.PERSISTENT,
                    error =>
                    {
                        Firebase.Analytics.FirebaseAnalytics.LogEvent
                        (
                            "userdata_load_failed_event",
                            new Firebase.Analytics.Parameter[]
                            {
                                new Firebase.Analytics.Parameter
                                (
                                    "user_id",
                                    userId),
                                new Firebase.Analytics.Parameter
                                (
                                    "error_message",
                                    error.Message)
                            }
                        );
                    }
                ) ==
                false
            )
            {
                //erro log
                Debug.LogErrorFormat("user data load failed ! key:{0}", AppDefine.USER_DATA_KEY);
                //bool is renewal user data
                bool isRenewalUserData = false;
                //popup
                yield return CSPopupManager.Instance
                    .Create<CSDoubleSelectPopup>()
                    .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                    .SetDescription
                    (
                        string.Format
                        (
                            CSLocalizeManager.Instance.GetString
                            (
                                TKLOCALIZE.USER_DATA_LOAD_FAILED_TEXT
                            ),
                            userId
                        )
                    )
                    .OnRightButtonClickedDelegate(() => isRenewalUserData = true)
                    .WaitForCompletion();
                //renewal user data
                if (isRenewalUserData)
                {
                    //user registration init
                    CSUserRegistrationManager.Instance.Initialize();
                    //load user registration
                    yield return CSUserRegistrationManager.Instance.Create_();
                    //reload
                    StartCoroutine(Load_(isSucceed));
                    //break
                    yield break;
                }
                else
                {
                    //callback
                    isSucceed.SafeInvoke(false);
                    //break
                    yield break;
                }
            }
            //data set
            _data = JsonUtility.FromJson<CSUserData>(userDataJson);
            //ユーザーデータのチェック
            if (_data == null ||
                _data.Id.IsNullOrEmpty() ||
                _data.Id != userId)
            {
                Debug.LogErrorFormat("user data is invalid Id:{0} UserDataJson:{1}", userId, userDataJson);
                isSucceed.SafeInvoke(false);
                yield break;
            }
            //form
            WWWForm form = new WWWForm();
            //set post field
            form.AddField("user_id", userId);
            //json data
            LoginResponseData loginResponseData = null;
            //post
            yield return CSWebRequestManager.Instance.Post_
            (
                LoginResponseData.URL,
                form,
                request =>
                {
                    if (request.isError)
                    {
                        Debug.LogErrorFormat("Failed User Login For Aws error:{0}", request.error);
                    }
                    else
                    {
                        //log
                        Debug.LogFormat("Succeed User Login For Aws json:{0}".Green(), request.downloadHandler.text);
                        //json
                        loginResponseData = JsonUtility.FromJson<LoginResponseData>
                            (request.downloadHandler.text);
                    }
                }
            );
            //login response data check
            if (loginResponseData == null ||
                loginResponseData.Status != "0")
            {
                yield return CSPopupManager.Instance
                    .Create<CSSingleSelectPopup>()
                    .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                    .SetDescription
                    (
                        string.Format
                        (
                            CSLocalizeManager.Instance.GetString
                                (TKLOCALIZE.USER_LOGIN_FAILED_TEXT),
                            loginResponseData.Status,
                            userId
                        )
                    )
                    .WaitForCompletion();
                //call back
                isSucceed.SafeInvoke(false);
                yield break;
            }
            //set unix time 
            CSUtcUnixTimeManager.Instance.SetUnixTime(loginResponseData.UtcUnixTime);
            //set ticket 
            _data.TicketNum = loginResponseData.AmountTicketNum;
            //set timestamp
            _data.LastLoadOrUpdateTimestamp = loginResponseData.UtcUnixTime;
            //callback
            isSucceed.SafeInvoke(true);
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="data">Data.</param>
        /// <param name="onSucceed">On succeed.</param>
        public override IEnumerator UpdateData_(CSUserData data, System.Action<bool> onSucceed = null)
        {
            //check user data
            if
            (
                data == null ||
                data.Id.IsNullOrEmpty()
            )
            {
                //callback
                onSucceed.SafeInvoke(false);
                //error log
                Debug.LogWarningFormat("UserData is not correct ! id:{0}", data.Id); //break
                yield break;
            }
            //set unix time
            CSUtcUnixTimeManager.Instance.SetUnixTime(DateTime.UtcNow.ToUnixTime());
            //set time stamp
            data.LastLoadOrUpdateTimestamp = CSUtcUnixTimeManager.Instance.CurrentUnixTime;
            //user data serialize
            string userDataJson = JsonUtility.ToJson(data);
            //save
            if (
                LocalStorageUtil.SaveText
                (
                    AppDefine.USER_DATA_KEY,
                    userDataJson,
                    TKFDefine.LocalStoragePathType.PERSISTENT,
                    error =>
                    {
                        Firebase.Analytics.FirebaseAnalytics.LogEvent
                        (
                            "userdata_save_failed_event",
                            new Firebase.Analytics.Parameter[]
                            {
                                new Firebase.Analytics.Parameter
                                (
                                    "user_id",
                                    data.Id),
                                new Firebase.Analytics.Parameter
                                (
                                    "user_data_json",
                                    userDataJson),
                                new Firebase.Analytics.Parameter
                                (
                                    "error_message",
                                    error.Message)
                            }
                        );
                    }
                ) ==
                false
            )
            {
                yield return CSPopupManager.Instance
                    .Create<CSSingleSelectPopup>()
                    .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                    .SetDescription
                    (
                        string.Format
                        (
                            CSLocalizeManager.Instance.GetString
                            (
                                TKLOCALIZE.USER_DATA_SAVE_FAILED_TEXT
                            ),
                            data.Id
                        )
                    )
                    .WaitForCompletion();
                //callback
                onSucceed.SafeInvoke(false);
            }
            //callback
            onSucceed.SafeInvoke(true);
        }
    }
}