#include "PlatformUtils.h"

const char * MRMTInternalMakeStringCopy(NSString *sourceString)
{
    if(sourceString)
    {
        const char *tempResult = [sourceString UTF8String];
        if (tempResult)
        {
            char* result = (char*)malloc(strlen(tempResult) + 1);
            strcpy(result, tempResult);
            return result;
        }
    }

    return NULL;
}

void MRMTDebugLog(const char *message)
{
    if (message)
    {
        NSString *messageString = [NSString stringWithUTF8String:message];
        if (messageString)
        {
            NSLog(@"%@", messageString);
        }
    }
}

const char * MRMTInternalArrayToJsonString(NSArray<NSString *> *source)
{
    if (source)
    {
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:source options:0 error:nil];
        if (jsonData)
        {
            return MRMTInternalMakeStringCopy([[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding]);
        }
    }

    return NULL;
}

NSArray<NSString *> *MRMTInternalJsonStringToArray(const char *source)
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
