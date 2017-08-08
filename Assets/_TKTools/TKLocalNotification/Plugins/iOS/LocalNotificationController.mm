#import "Prefix.pch"
#import "UnityAppController.h"

@interface LocalNotificationController : UnityAppController
+(void)load;
@end

@implementation LocalNotificationController
+(void)load
{
    extern const char* AppControllerClassName;
    AppControllerClassName = "LocalNotificationController";
}

// アプリが起動時実行
- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    float version = [[[UIDevice currentDevice] systemVersion] floatValue];
    if (version >= 8.0)
    {
        if ([application respondsToSelector:@selector(registerUserNotificationSettings:)]) {
            UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:UIUserNotificationTypeAlert|UIUserNotificationTypeSound categories:nil];
            [application registerUserNotificationSettings:settings];
        }
    }
    return YES;
}

@end
