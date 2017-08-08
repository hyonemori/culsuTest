//
//  TKAlert.h
//  TKAlert
//
//  Created by TakatoriYasuhiro on 2016/08/17.
//  Copyright © 2016年 TakatoriYasuhiro. All rights reserved.
//
#import <UIKit/UIKit.h>
@interface TKAlert : NSObject
+ (TKAlert *)sharedManager;
-(void)showAppRateAlert
:(UIViewController*)viewController
:(NSString*)title
:(NSString*)message
:(NSString*)rateLaterTitle
:(NSString*)cancelTitle
:(NSString*)rateTitle
:(NSString*)url
:(NSString*)callerGameObjecName
:(NSString*)callbackMethodName;
@end
