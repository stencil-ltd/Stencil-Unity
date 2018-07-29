//
//  SetEnv.c
//  Unity-iPhone
//
//  Created by Aaron Sarazan on 7/21/18.
//

#include "SetEnv.h"
#include <stdlib.h>

int unitySetEnv(const char * __name, const char * __value) {
    NSLog(@"Unity Set Env: %s -> %s", __name, __value);
    return setenv(__name, __value, 0);
}
