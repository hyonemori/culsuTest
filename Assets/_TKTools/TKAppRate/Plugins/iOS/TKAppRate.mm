#import "TKAlert.h"

extern "C" void showAppRatePopup(const char * alertTitle,
                                 const char * alertMessage,
                                 const char * alertRateLaterTitle,
                                 const char * alertCancelTitle,
                                 const char * alertRateTitle,
                                 const char * url,
                                 const char * callerGameObjectName,
                                 const char * callbackMethodName
                                 )
{
    NSString *alertTitleStr =  [NSString stringWithCString:alertTitle encoding:NSUTF8StringEncoding];
    NSString *alertMessageStr =  [NSString stringWithCString:alertMessage encoding:NSUTF8StringEncoding];
    NSString *alertCancelTitleStr =  [NSString stringWithCString:alertCancelTitle encoding:NSUTF8StringEncoding];
    NSString *alertRateStr =  [NSString stringWithCString:alertRateTitle encoding:NSUTF8StringEncoding];
    NSString *alertRateLaterStr =  [NSString stringWithCString:alertRateLaterTitle encoding:NSUTF8StringEncoding];
    NSString *urlStr =  [NSString stringWithCString:url encoding:NSUTF8StringEncoding];
    NSString *callerGameObjectNameStr =  [NSString stringWithCString:callerGameObjectName encoding:NSUTF8StringEncoding];
    NSString *callbackMethodNameStr =  [NSString stringWithCString:callbackMethodName encoding:NSUTF8StringEncoding];
    [[TKAlert sharedManager]
     showAppRateAlert:UnityGetGLViewController()
     :alertTitleStr
     :alertMessageStr
     :alertRateLaterStr
     :alertCancelTitleStr
     :alertRateStr
     :urlStr
     :callerGameObjectNameStr
     :callbackMethodNameStr
     ];
}