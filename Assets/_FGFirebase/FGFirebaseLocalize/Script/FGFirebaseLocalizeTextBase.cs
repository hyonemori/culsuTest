using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using TKLocalizer;

namespace FGFirebaseLocalize
{
    public class FGFirebaseLocalizeTextBase<TLocalizeManager> : TKLocalizeTextBase<TLocalizeManager>
        where TLocalizeManager : FGFirebaseLocalizeManagerBase<TLocalizeManager>
    {
    }
}