//
//  TKAlert.h
//  TKAlert
//
//  Created by TakatoriYasuhiro on 2016/08/17.
//  Copyright © 2016年 TakatoriYasuhiro. All rights reserved.
//
#import <UIKit/UIKit.h>
@interface TKNativeAlert : NSObject
+ (TKNativeAlert *)sharedManager;
-(void)showSingleSelectAlert
:(UIViewController*)viewController
:(NSString*)title
:(NSString*)message
:(NSString*)buttonTitle
:(NSString*)callerGameObjecName
:(NSString*)callbackMethodName;
-(void)showDoubleSelectAlert
:(UIViewController*)viewController
:(NSString*)title
:(NSString*)message
:(NSString*)leftButtonTitle
:(NSString*)rightButtonTitle
:(NSString*)callerGameObjecName
:(NSString*)callbackMethodName;
@end
