//
//  TTPUnityBilling.m
//  Unity-iPhone
//
//  Created by Tabtale on 12/12/2018.
//

#import <Foundation/Foundation.h>
#import "TTPUnityServiceManager.h"
#import <TT_Plugins_Core/TTPIbilling.h>

extern "C" {
    
    void ttpValidateReceiptAndReport(const char * price, const char * currency, const char * productId)
    {
        TTPServiceManager *serviceManager = [TTPUnityServiceManager sharedInstance];
        id<TTPIbilling> billing = [serviceManager get:@protocol(TTPIbilling)];
        if (billing != nil) {
            NSString *sPrice =[[NSString alloc] initWithUTF8String: price != NULL ? price : ""];
            NSString *sCurrency =[[NSString alloc] initWithUTF8String:currency != NULL ? currency : ""];
            NSString *sProductId =[[NSString alloc] initWithUTF8String:productId != NULL ? productId : ""];
            [billing validateReceiptAndReport:sPrice currency:sCurrency productID:sProductId completionHandler:^(BOOL verified, ValidationFailureReason validationResult, NSString *errorMessage) {
              NSString *str = [NSString stringWithFormat:@"{ \"price\" : \"%@\", \"currency\" : \"%@\", \"productId\" : \"%@\", \"valid\" : %s, \"failureReason\" : %li, \"error\" : \"%@\"  }", sPrice, sCurrency, sProductId, verified ? "true" : "false", validationResult, errorMessage];
                id<TTPIunityMessenger> unityMessenger = [serviceManager get:@protocol(TTPIunityMessenger)];
                if (unityMessenger != nil) {
                    [unityMessenger unitySendMessage:"OnValidateResponse" message:[str UTF8String]];
                }
            }];
        }
    }
    
    void ttpNoAdsPurchased(bool purchased)
    {
        TTPServiceManager *serviceManager = [TTPUnityServiceManager sharedInstance];
        id<TTPIbilling> billing = [serviceManager get:@protocol(TTPIbilling)];
        if(billing != nil)
        {
            [billing setNoAdsItemPurchased:purchased];
        }
    }

    bool ttpWasUserDetectedInChina()
    {
        TTPServiceManager *serviceManager = [TTPUnityServiceManager sharedInstance];
        id<TTPIbilling> billing = [serviceManager get:@protocol(TTPIbilling)];
        if(billing != nil)
        {
            return [billing wasUserDetectedInChina];
        }
        return false;
    }

    extern void ttpReportPurchaseToConversion(const char * message)
    {
        TTPServiceManager *serviceManager = [TTPUnityServiceManager sharedInstance];
        id<TTPIbilling> billing = [serviceManager get:@protocol(TTPIbilling)];
        if(billing != nil && message != NULL)
        {
            NSString *json = [[NSString alloc] initWithUTF8String:message];
            NSData *data = [json dataUsingEncoding:NSUTF8StringEncoding];
            NSDictionary *dic = [NSJSONSerialization JSONObjectWithData:data options:0 error:nil];
            if(dic != nil){
                NSString *currency = [dic objectForKey:@"currency"];
                NSString *productId = [dic objectForKey:@"productId"];
                NSNumber *price = [dic objectForKey:@"price"];
                NSNumber *consumable = [dic objectForKey:@"consumable"];
                if(productId != nil && price != nil){
                    [billing reportPurchaseToConversion:currency price:[price floatValue] productId:productId consumable:[consumable boolValue]];
                }
            }
        }
    }
    
}
