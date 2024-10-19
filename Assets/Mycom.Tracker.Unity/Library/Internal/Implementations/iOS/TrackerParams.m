#import <MyTrackerSDK/MyTrackerSDK.h>
#import "PlatformUtils.h"

extern const char * MRMTInternalMakeStringCopy(NSString *sourceString);
extern const char * MRMTInternalArrayToJsonString(NSArray<NSString *> *source);
extern NSArray<NSString *> *MRMTInternalJsonStringToArray(const char *source);

int32_t MRMTMyTrackerParamsGetAge()
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        NSNumber *ageNumber = [trackerParams age];
        if (ageNumber)
        {
            return [ageNumber intValue];
        }
    }

    return -1;
}

const char *MRMTMyTrackerParamsGetCustomUserIds()
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        return MRMTInternalArrayToJsonString([trackerParams customUserIds]);
    }

    return NULL;
}

const char *MRMTMyTrackerParamsGetEmails()
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        return MRMTInternalArrayToJsonString([trackerParams emails]);
    }

    return NULL;
}

int32_t MRMTMyTrackerParamsGetGender()
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        switch ([trackerParams gender])
        {
            case MRGenderFemale:
                return 2;
            case MRGenderMale:
                return 1;
            case MRGenderUnknown:
                return 0;
            default:
                return -1;
        }
    }

    return -1;
}

const char *MRMTMyTrackerParamsGetIcqIds()
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        return MRMTInternalArrayToJsonString([trackerParams icqIds]);
    }

    return NULL;
}

const char *MRMTMyTrackerParamsGetLang()
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        return MRMTInternalMakeStringCopy([trackerParams language]);
    }

    return NULL;
}

const char *MRMTMyTrackerParamsGetMrgsAppId()
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        return MRMTInternalMakeStringCopy([trackerParams mrgsAppId]);
    }

    return NULL;
}

const char *MRMTMyTrackerParamsGetMrgsId()
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        return MRMTInternalMakeStringCopy([trackerParams mrgsDeviceId]);
    }

    return NULL;
}

const char *MRMTMyTrackerParamsGetMrgsUserId()
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        return MRMTInternalMakeStringCopy([trackerParams mrgsUserId]);
    }

    return NULL;
}

const char *MRMTMyTrackerParamsGetOkIds()
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        return MRMTInternalArrayToJsonString([trackerParams okIds]);
    }

    return NULL;
}

const char *MRMTMyTrackerParamsGetPhones()
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        return MRMTInternalArrayToJsonString([trackerParams phones]);
    }

    return NULL;
}

const char *MRMTMyTrackerParamsGetVkIds()
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        return MRMTInternalArrayToJsonString([trackerParams vkIds]);
    }

    return NULL;
}

const char *MRMTMyTrackerParamsGetVkConnectIds()
{
	MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
	if (trackerParams)
	{
		return MRMTInternalArrayToJsonString([trackerParams vkConnectIds]);
	}
	
	return NULL;
}

void MRMTMyTrackerParamsSetAge(int32_t value)
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        [trackerParams setAge:@(value)];
    }
}

void MRMTMyTrackerParamsSetCustomUserIds(const char *value)
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        [trackerParams setCustomUserIds:MRMTInternalJsonStringToArray(value)];
    }
}

void MRMTMyTrackerParamsSetEmails(const char *value)
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        [trackerParams setEmails:MRMTInternalJsonStringToArray(value)];
    }
}

void MRMTMyTrackerParamsSetGender(int32_t value)
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        MRGender gender;
        switch (value)
        {
            case 0:
                gender = MRGenderUnknown;
                break;
            case 1:
                gender = MRGenderMale;
                break;
            case 2:
                gender = MRGenderFemale;
                break;
            default:
                gender = MRGenderUnspecified;
                break;
        }

        [trackerParams setGender:gender];
    }
}

void MRMTMyTrackerParamsSetIcqIds(const char *value)
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        [trackerParams setIcqIds:MRMTInternalJsonStringToArray(value)];
    }
}

void MRMTMyTrackerParamsSetLang(const char *value)
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        [trackerParams setLanguage:value ? [NSString stringWithUTF8String:value] : nil];
    }
}

void MRMTMyTrackerParamsSetMrgsAppId(const char *value)
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        [trackerParams setMrgsAppId:value ? [NSString stringWithUTF8String:value] : nil];
    }
}

void MRMTMyTrackerParamsSetMrgsId(const char *value)
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        [trackerParams setMrgsDeviceId:value ? [NSString stringWithUTF8String:value] : nil];
    }
}

void MRMTMyTrackerParamsSetMrgsUserId(const char *value)
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        [trackerParams setMrgsUserId:value ? [NSString stringWithUTF8String:value] : nil];
    }
}

void MRMTMyTrackerParamsSetOkIds(const char *value)
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        [trackerParams setOkIds:MRMTInternalJsonStringToArray(value)];
    }
}

void MRMTMyTrackerParamsSetPhones(const char *value)
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        [trackerParams setPhones:MRMTInternalJsonStringToArray(value)];
    }
}

void MRMTMyTrackerParamsSetVkIds(const char *value)
{
    MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
    if (trackerParams)
    {
        [trackerParams setVkIds:MRMTInternalJsonStringToArray(value)];
    }
}

void MRMTMyTrackerParamsSetVkConnectIds(const char *value)
{
	MRMyTrackerParams *trackerParams = [MRMyTracker trackerParams];
	if (trackerParams)
	{
		[trackerParams setVkConnectIds:MRMTInternalJsonStringToArray(value)];
	}
}
