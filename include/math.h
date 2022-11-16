#ifndef MATH_H

#define MATH_H "Mental Abuse To Humans"
#include <definitions.h>
#include <vectors.h>
#define TAU 6.283185307179586476925286766559
#define PI 3.1415926535897932384626433832795
#define E 2.7182818284590452353602874713527
#define GOLDEN_RATIO 1.6180339887498948482045868343657
#define DEG2RAD 0.01745329251994329576923690768489
#define RAD2DEG 57.295779513082320876798154814105

float inverseLerp(const float a, const float b, const float x);
float abs(const float x);
float min(const float a, const float b);
float max(const float a, const float b);
float clamp(const float a, const float b, const float x);
float floor(const float x);
float ceiling(const float x);

float sqrt(const float x);
float ln(const float x);
float log(const float x);
float log(const float x, const float a);

float sin(const float x);
float cos(const float x);
float tan(const float x);
float asin(const float x);
float acos(const float x);
float atan(const float x);
float atan2(const float y, const float x);

uint32 fact(const uint32 x);
float pow10(const int32 x);
float exp(const float x);
float lerp(const float a, const float b, const float x);
float2 lerp(const float2 a, const float2 b, const float2 x);
float3 lerp(const float3 a, const float3 b, const float3 x);
float smoothMin(const float a, const float b, const float x);
float smoothMax(const float a, const float b, const float x);
float smoothstep(const float a, const float b, const float x);

float distance(const float3 a, const float3 b);
float distance(const float2 a, const float2 b);
float length(const float3 a);
float length(const float2 a);
float3 normalize(const float3 a);
float2 normalize(const float2 a);
float dot(const float3 a, const float3 b);
float dot(const float2 a, const float2 b);
float3 cross(const float3 a, const float3 b);

float2 RaySphere(const float3 center, const float radius, const float3 rayOrigin, const float3 rayDir);
float2 Rotate(const float2 origin, const float2 point, const float angle);

float3 MidPoint(const float3 a, const float3 b);
float2 MidPoint(const float2 a, const float2 b);
float3 MovePoint(const float3 a, const float3 b, const float _distance);
float2 MovePoint(const float2 a, const float2 b, const float _distance);
float3 MovePoint01(const float3 a, const float3 b, const float _distance);
float2 MovePoint01(const float2 a, const float2 b, const float _distance);

bool IsPrime(const uint32 a);
float pow(const float a, const float b);

#endif