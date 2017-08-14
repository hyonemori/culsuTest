//
//  TKNativeAlert.m
//  TKNativeAlert
//
//  Created by TakatoriYasuhiro on 2016/08/17.
//  Copyright © 2016年 TakatoriYasuhiro. All rights reserved.
//

#import "TKNativeAlert.h"
@interface TKNativeAlert ()

@end

@implementation TKNativeAlert

static TKNativeAlert *sharedData_ = nil;

+ (TKNativeAlert *)sharedManager{
    if (!sharedData_) {
        sharedData_ = [TKNativeAlert new];
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
-(void)showSingleSelectAlert
:(UIViewController*)viewController
:(NSString*)title
:(NSString*)message
:(NSString*)buttonTitle
:(NSString*)callerGameObjecName
:(NSString*)callbackMethodName
{
    //Alert Create
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:title message:message preferredStyle:UIAlertControllerStyleAlert];
    
    // Button Set
    [alertController addAction:[UIAlertAction actionWithTitle:buttonTitle style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        [self selectedActionWith:0 :callerGameObjecName :callbackMethodName];
    }]];
    
    // iPad用の設定
    alertController.popoverPresentationController.sourceView = viewController.view;
    alertController.popoverPresentationController.sourceRect = CGRectMake(100.0, 100.0, 20.0, 20.0);
    
    [viewController presentViewController:alertController animated:YES completion:nil];
}

-(void)showDoubleSelectAlert
:(UIViewController*)viewController
:(NSString*)title
:(NSString*)message
:(NSString*)leftButtonTitle
:(NSString*)rigthButtonTitle
:(NSString*)callerGameObjecName
:(NSString*)callbackMethodName
{
    //Alert Create
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:title message:message preferredStyle:UIAlertControllerStyleAlert];
    
    // Button Set
    [alertController addAction:[UIAlertAction actionWithTitle:leftButtonTitle style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        [self selectedActionWith:1:callerGameObjecName :callbackMethodName];
    }]];
    [alertController addAction:[UIAlertAction actionWithTitle:rigthButtonTitle style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        [self selectedActionWith:2 :callerGameObjecName :callbackMethodName];
    }]];
    
    // iPad用の設定
    alertController.popoverPresentationController.sourceView = viewController.view;
    alertController.popoverPresentationController.sourceRect = CGRectMake(100.0, 100.0, 20.0, 20.0);
    
    [viewController presentViewController:alertController animated:YES completion:nil];
}

-(void)selectedActionWith
:(int)index
:(NSString*)callerGameObjecName
:(NSString*)callbackMethodName
{
    UnitySendMessage(
                     [callerGameObjecName UTF8String],
                     [callbackMethodName UTF8String],
                     (char *) [[NSString stringWithFormat:@"%d", index] UTF8String]);
}

@end