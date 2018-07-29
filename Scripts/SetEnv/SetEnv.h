//
//  SetEnv.h
//  Unity-iPhone
//
//  Created by Aaron Sarazan on 7/21/18.
//

#ifndef SetEnv_h
#define SetEnv_h

#include <stdio.h>

#ifdef __cplusplus
extern "C" {
#endif
    
int unitySetEnv(const char * __name, const char * __value);
    
#ifdef __cplusplus
}
#endif

#endif /* SetEnv_h */
