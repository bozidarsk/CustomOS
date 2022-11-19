#include <vectors.h>

float4 f4(const float x, const float y, const float z, const float w) { float4 v = { x, y, z, w }; return v; }
float3 f3(const float x, const float y, const float z) { float3 v = { x, y, z }; return v; }
float2 f2(const float x, const float y) { float2 v = { x, y }; return v; }

int4 i4(const int32 x, const int32 y, const int32 z, const int32 w) { int4 v = { x, y, z, w }; return v; }
int3 i3(const int32 x, const int32 y, const int32 z) { int3 v = { x, y, z }; return v; }
int2 i2(const int32 x, const int32 y) { int2 v = { x, y }; return v; }