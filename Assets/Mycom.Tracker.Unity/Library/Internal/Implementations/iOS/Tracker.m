#import <MyTrackerSDK/MyTrackerSDK.h>
#import <pthread.h>
#import "PlatformUtils.h"

enum
{
    AdClick = 17,
    AdImpression = 18,
    AdRevenue = 19
};

typedef void (*MRMTAttributionListener)(const char *);

@interface MRMTInternalMyTrackerAttributionDelegate : NSObject <MRMyTrackerAttributionDelegate>

@end

MRMTAttributionListener mrmtInternalAttributionListener = nil;
MRMTInternalMyTrackerAttributionDelegate *mrmtInternalMyTrackerAttributionDelegate = nil;
pthread_mutex_t mrmtInternalMutex;

@implementation MRMTInternalMyTrackerAttributionDelegate

- (void)didReceiveAttribution:(MRMyTrackerAttribution *)attribution
{
    pthread_mutex_lock(&mrmtInternalMutex);

    MRMTAttributionListener listener = mrmtInternalAttributionListener;
    if (listener)
    {
        if (attribution)
        {
            const char *deepLink = MRMTInternalMakeStringCopy(attribution.deeplink);
            if (deepLink)
            {
                listener(deepLink);
            }
        }
    }

    pthread_mutex_unlock(&mrmtInternalMutex);
}

@end

NSDictionary<NSString *, NSString *> *MRMTInternalDeserializeString(const char *source)
{
    if (source)
    {
        NSString *sourceString = [NSString stringWithUTF8String:source];
        if (sourceString)
        {
            NSData *jsonData = [sourceString dataUsingEncoding:NSUTF8StringEncoding];
            if (jsonData)
            {
                return [NSJSONSerialization JSONObjectWithData:jsonData options:0 error:nil];
            }
        }
    }

    return nil;
}

void MRMTMyTrackerInitTracker(const char *id)
{
    if (id)
    {
        NSString *idString = [NSString stringWithUTF8String:id];
        if (idString)
        {
            [MRMyTracker setupTracker:idString];
        }
    }    
}

int32_t MRMTMyTrackerIsDebugMode()
{
    return [MRMyTracker isDebugMode];
}

void MRMTMyTrackerSetDebugMode(bool value)
{
    [MRMyTracker setDebugMode:value];
}

void MRMTMyTrackerTrackEvent(const char *name, const char *eventParams)
{
	if (!name)
	{
		return;
	}
	
	NSString *nameString = [NSString stringWithUTF8String:name];
	if (!nameString)
	{
		return;
	}
	
	NSDictionary *eventParamsDictionary = MRMTInternalDeserializeString(eventParams);
	if (eventParamsDictionary)
	{
		[MRMyTracker trackEventWithName:nameString eventParams:eventParamsDictionary];
	}
	else
	{
		[MRMyTracker trackEventWithName:nameString];
	}
}

void MRMTMyTrackerTrackInvite(const char *eventParams)
{
    NSDictionary *eventParamsDictionary = MRMTInternalDeserializeString(eventParams);
    if (eventParamsDictionary)
    {
        [MRMyTracker trackInviteEventWithParams:eventParamsDictionary];
    }
    else
    {
        [MRMyTracker trackInviteEvent];
    }
}

void MRMTMyTrackerTrackLevelLevelParams(int32_t level, const char *eventParams)
{
    NSDictionary *eventParamsDictionary = MRMTInternalDeserializeString(eventParams);

    NSNumber *levelValue = @(level);

    [MRMyTracker trackLevelAchievedWithLevel:levelValue eventParams:eventParamsDictionary];
}

void MRMTMyTrackerTrackLevelParams(const char *eventParams)
{
    NSDictionary *eventParamsDictionary = MRMTInternalDeserializeString(eventParams);

    [MRMyTracker trackLevelAchievedWithLevel:nil eventParams:eventParamsDictionary];
}

void MRMTMyTrackerTrackLogin(const char* userId, const char* vkConnectId, const char *eventParams)
{
    NSDictionary *eventParamsDictionary = MRMTInternalDeserializeString(eventParams);
	NSString *userIdString = userId ? [NSString stringWithUTF8String:userId] : @"";
	NSString *vkConnectIdString = vkConnectId ? [NSString stringWithUTF8String:vkConnectId] : nil;
	[MRMyTracker trackLoginEvent:userIdString withVkConnectId:vkConnectIdString params:eventParamsDictionary];
}

void MRMTMyTrackerTrackRegistration(const char* userId, const char* vkConnectId, const char *eventParams)
{
    NSDictionary *eventParamsDictionary = MRMTInternalDeserializeString(eventParams);
	NSString *userIdString = userId ? [NSString stringWithUTF8String:userId] : @"";
	NSString *vkConnectIdString = vkConnectId ? [NSString stringWithUTF8String:vkConnectId] : nil;
	[MRMyTracker trackRegistrationEvent:userIdString withVkConnectId:vkConnectIdString params:eventParamsDictionary];
}

void MRMTMyTrackerTrackAdEvent(int appEvent,
                               int network,
                               double revenue,
                               const char *currency,
                               const char *source,
                               const char *placementId,
                               const char *adId,
                               const char *format)
{
    MRAdEventBuilder *adEventBuilder = nil;
    switch (appEvent)
    {
        case AdClick:
            adEventBuilder = [MRAdEventBuilder newClickBuilder:(MRAdNetwork) network];
            break;
        case AdImpression:
            adEventBuilder = [MRAdEventBuilder newImpressionBuilder:(MRAdNetwork) network];
            break;
        case AdRevenue:
            if (currency)
            {
                adEventBuilder = [MRAdEventBuilder newRevenueBuilder:(MRAdNetwork) network
                                                             revenue:revenue
                                                            currency:[NSString stringWithUTF8String:currency]];
            }
            break;
        default:
            return;
    }

    if (!adEventBuilder)
    {
        return;
    }

    if (source)
    {
        [adEventBuilder withSource:[NSString stringWithUTF8String:source]];
    }
    if (placementId)
    {
        [adEventBuilder withPlacementId:[NSString stringWithUTF8String:placementId]];
    }
    if (adId)
    {
        [adEventBuilder withAdId:[NSString stringWithUTF8String:adId]];
    }
    if (format)
    {
        [adEventBuilder withAdFormat:[NSString stringWithUTF8String:format]];
    }

    MRAdEvent *adEvent = [adEventBuilder build];
    if (!adEvent)
    {
        return;
    }

    [MRMyTracker trackAdEvent:adEvent];
}

void MRMTMyTrackerFlush()
{
    [MRMyTracker flush];
}

void MRMTMyTrackerSetAttributionListener(MRMTAttributionListener listener)
{
    pthread_mutex_lock(&mrmtInternalMutex);

    mrmtInternalAttributionListener = listener;
    if (mrmtInternalAttributionListener)
    {
        if (nil == mrmtInternalMyTrackerAttributionDelegate)
        {
            mrmtInternalMyTrackerAttributionDelegate = [[MRMTInternalMyTrackerAttributionDelegate alloc] init];
        }
    }
    else
    {
        mrmtInternalMyTrackerAttributionDelegate = nil;
    }

    [MRMyTracker setAttributionDelegate:mrmtInternalMyTrackerAttributionDelegate];

    pthread_mutex_unlock(&mrmtInternalMutex);
}

const char *MRMTMyTrackerGetInstanceId()
{
    return MRMTInternalMakeStringCopy([MRMyTracker instanceId]);
}
