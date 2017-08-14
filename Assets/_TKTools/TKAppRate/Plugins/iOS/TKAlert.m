//
//  TKAlert.m
//  TKAlert
//
//  Created by TakatoriYasuhiro on 2016/08/17.
//  Copyright © 2016年 TakatoriYasuhiro. All rights reserved.
//

#import "TKAlert.h"
@interface TKAlert ()
@property NSString *callerGameObjectName;
@property NSString *callbackMethodName;
@end

@implementation TKAlert

static TKAlert *sharedData_ = nil;

+ (TKAlert *)sharedManager{
    if (!sharedData_) {
        sharedData_ = [TKAlert new];
    }
    return sharedData_;
}

- (id)init
{
    self = [super init];
    if (self) {
        //Initialization
    }
    return self;
}

-(void)showAppRateAlert
:(UIViewController*)viewController
:(NSString*)title
:(NSString*)message
:(NSString*)rateLaterTitle
:(NSString*)cancelTitle
:(NSString*)rateTitle
:(NSString*)url
:(NSString*)callerGameObjecName
:(NSString*)callbackMethodName
{
    //Caller Object Name Set
    _callerGameObjectName = [[NSString alloc] initWithString:callerGameObjecName];
    //Callback Method Name
    _callbackMethodName = [[NSString alloc] initWithString:callbackMethodName];
    //Alert Create
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:title message:message preferredStyle:UIAlertControllerStyleAlert];
    
    // Button Set
    [alertController addAction:[UIAlertAction actionWithTitle:rateLaterTitle style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        [self selectedActionWith:0];
    }]];
    [alertController addAction:[UIAlertAction actionWithTitle:cancelTitle style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        [self selectedActionWith:1];
    }]];
    [alertController addAction:[UIAlertAction actionWithTitle:rateTitle style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        //open url
        [[UIApplication sharedApplication]
         openURL:[NSURL URLWithString:url]];
        [self selectedActionWith:2];
    }]];
    
    
    
    // iPad用の設定
    alertController.popoverPresentationController.sourceView = viewController.view;
    alertController.popoverPresentationController.sourceRect = CGRectMake(100.0, 100.0, 20.0, 20.0);
    
    [viewController presentViewController:alertController animated:YES completion:nil];
}

-(void)selectedActionWith:(int)index{
    UnitySendMessage(
                     [_callerGameObjectName UTF8String],
                     [_callbackMethodName UTF8String],
                     (char *) [[NSString stringWithFormat:@"%d", index] UTF8String]);
}

@end