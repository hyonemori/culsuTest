#import "TKNativeAlert.h"

extern "C" void showSingleSelectAlert(const char * alertTitle,
                                      const char * alertMessage,
                                      const char * buttonTitle,
                                      const char * callerGameObjectName,
                                      const char * callbackMethodName
                                 )
{
    NSString *alertTitleStr =  [NSString stringWithCString:alertTitle encoding:NSUTF8StringEncoding];
    NSString *alertMessageStr =  [NSString stringWithCString:alertMessage encoding:NSUTF8StringEncoding];
    NSString *buttonTitleStr =  [NSString stringWithCString:buttonTitle encoding:NSUTF8StringEncoding];
    NSString *callerGameObjectNameStr =  [NSString stringWithCString:callerGameObjectName encoding:NSUTF8StringEncoding];
    NSString *callbackMethodNameStr =  [NSString stringWithCString:callbackMethodName encoding:NSUTF8StringEncoding];
    [[TKNativeAlert sharedManager]
     showSingleSelectAlert:UnityGetGLViewController()
     :alertTitleStr
     :alertMessageStr
     :buttonTitleStr
     :callerGameObjectNameStr
     :callbackMethodNameStr
     ];
}

extern "C" void showDoubleSelectAlert(const char * alertTitle,
                                      const char * alertMessage,
                                      const char * leftButtonTitle,
                                      const char * rightButtonTitle,
                                      const char * callerGameObjectName,
                                      const char * callbackMethodName
                                      )
{
    NSString *alertTitleStr =  [NSString stringWithCString:alertTitle encoding:NSUTF8StringEncoding];
    NSString *alertMessageStr =  [NSString stringWithCString:alertMessage encoding:NSUTF8StringEncoding];
    NSString *leftButtonTitleStr =  [NSString stringWithCString:leftButtonTitle encoding:NSUTF8StringEncoding];
    NSString *rightButtonTitleStr =  [NSString stringWithCString:rightButtonTitle encoding:NSUTF8StringEncoding];
    NSString *callerGameObjectNameStr =  [NSString stringWithCString:callerGameObjectName encoding:NSUTF8StringEncoding];
    NSString *callbackMethodNameStr =  [NSString stringWithCString:callbackMethodName encoding:NSUTF8StringEncoding];
    [[TKNativeAlert sharedManager]
     showDoubleSelectAlert:UnityGetGLViewController()
     :alertTitleStr
     :alertMessageStr
     :leftButtonTitleStr
     :rightButtonTitleStr
     :callerGameObjectNameStr
     :callbackMethodNameStr
     ];
}