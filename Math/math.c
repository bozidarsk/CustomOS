#include <definitions.h>
#include <math.h>

float inverseLerp(const float a, const float b, const float x) { return (x - a) / (b - a); }
float abs(const float x) { return (x >= 0) ? x : -x; }
float min(const float a, const float b) { return (a < b) ? b : a; }
float max(const float a, const float b) { return (a > b) ? b : a; }
float clamp(const float a, const float b, const float x) { return max(a, min(x, b)); }
float floor(const float x) { return (int32)x; }
float ceiling(const float x) { return floor(x) + 1; }

float sqrt(const float x) 
{
    if (x < 0) { return 0; }
    union 
    {
        int i;
        float x;
    } u;

    u.x = x;
    u.i = (1<<29) + (u.i >> 1) - (1<<22); 

    u.x =       u.x + x/u.x;
    u.x = 0.25f*u.x + x/u.x;
    return u.x;
}

float ln(const float x) { float n = x + 1; return n - (n*n / 2) + (n*n*n / 3) - (n*n*n*n / 4) + (n*n*n*n*n / 5) - (n*n*n*n*n*n / 6) + (n*n*n*n*n*n*n / 7) - (n*n*n*n*n*n*n*n / 8) + (n*n*n*n*n*n*n*n*n / 9) - (n*n*n*n*n*n*n*n*n*n / 10) + (n*n*n*n*n*n*n*n*n*n*n / 11) - (n*n*n*n*n*n*n*n*n*n*n*n / 12) + (n*n*n*n*n*n*n*n*n*n*n*n*n / 13) - (n*n*n*n*n*n*n*n*n*n*n*n*n*n / 14) + (n*n*n*n*n*n*n*n*n*n*n*n*n*n*n / 15) - (n*n*n*n*n*n*n*n*n*n*n*n*n*n*n*n / 16); }
float log(const float x) { return ln(x) / ln(10); }
float log(const float x, const float a) { return ln(x) / ln(a); }

float sin(const float x) { return x - (x*x*x / 6) + (x*x*x*x*x / 120) - (x*x*x*x*x*x*x / 5040) + (x*x*x*x*x*x*x*x*x / 362880) - (x*x*x*x*x*x*x*x*x*x*x / 39916800) + (x*x*x*x*x*x*x*x*x*x*x*x*x / 6227020800) - (x*x*x*x*x*x*x*x*x*x*x*x*x*x*x / 1307674368000) + (x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x / 355687428096000) - (x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x / 121645100408832000); }
float cos(const float x) { return 1 - (x*x / 2) + (x*x*x*x / 24) - (x*x*x*x*x*x / 720) + (x*x*x*x*x*x*x*x / 40320) - (x*x*x*x*x*x*x*x*x*x / 3628800) + (x*x*x*x*x*x*x*x*x*x*x*x / 479001600) - (x*x*x*x*x*x*x*x*x*x*x*x*x*x / 87178291200) + (x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x / 20922789888000) - (x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x / 6402373705728000) + (x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x*x / 2432902008176640000); }
float tan(const float x) { return sin(x) / cos(x); }

float asin(const float x) { return 1 / sin(x); }
float acos(const float x) { return 1 / cos(x); }
float atan(const float x) { return 1 / tan(x); }
float atan2(const float y, const float x) { return 2 * atan(y / (sqrt((x*x) + (y*y)) + x)); }

uint32 fact(const uint32 x) 
{
    uint32 n = x;
    uint32 result = 1;
    while (n > 0) { result *= n; n--; }
    return result;
}

float pow10(const int32 x) 
{
    float n = abs(x);
    float result = 1;
    while (n > 0) { result *= 10; n--; }
    return (x < 0) ? 1 / result : result;
}

float exp(const float x) { return pow(E, x); }
float lerp(const float a, const float b, const float x) { return a + (x * (b - a)); }
float2 lerp(const float2 a, const float2 b, const float x) { return f2(lerp(a.x, b.x, x), lerp(a.y, b.y, x)); }
float3 lerp(const float3 a, const float3 b, const float x) { return f3(lerp(a.x, b.x, x), lerp(a.y, b.y, x), lerp(a.z, b.z, x)); }
float smoothMin(const float a, const float b, const float x) { float t = clamp(0, 1, (b - a + x) / (2 * x)); return a * t + b * (1 - t) - x * t * (1 - t); }
float smoothMax(const float a, const float b, const float x) { return smoothMin(a, b, x * -1); }
float smoothstep(const float a, const float b, const float x) { float t = clamp(0, 1, (x - a) / (b - a)); return t * t * (3 - (2 * t)); }

float distance(const float3 a, const float3 b) { return sqrt(((b.x - a.x) * (b.x - a.x)) + ((b.y - a.y) * (b.y - a.y)) + ((b.z - a.z) * (b.z - a.z))); }
float distance(const float2 a, const float2 b) { return sqrt(((b.x - a.x) * (b.x - a.x)) + ((b.y - a.y) * (b.y - a.y))); }
float length(const float3 a) { return sqrt((a.x * a.x) + (a.y * a.y) + (a.z * a.z)); }
float length(const float2 a) { return sqrt((a.x * a.x) + (a.y * a.y)); }
float3 normalize(const float3 a) { float3 v = a; return v / length(v); }
float2 normalize(const float2 a) { float2 v = a; return v / length(v); }
float dot(const float3 a, const float3 b) { return (a.x * b.x) + (a.y * b.y) + (a.z * b.z); }
float dot(const float2 a, const float2 b) { return (a.x * b.x) + (a.y * b.y); }
float3 cross(const float3 a, const float3 b) { return f3((a.y * b.z) - (a.z * b.y), (a.z * b.x) - (a.x * b.z), (a.x * b.y) - (a.y * b.x)); }

float2 RaySphere(const float3 center, const float radius, const float3 rayOrigin, const float3 rayDir) // returns f2(distance to sphere, distance inside sphere)
{
    float a = 1;
    float3 offset = f3(rayOrigin.x - center.x, rayOrigin.y - center.y, rayOrigin.z - center.z);
    float b = 2 * dot(offset, rayDir);
    float c = dot(offset, offset) - radius * radius;

    float disciminant = b * b - 4 * a * c;

    if (disciminant > 0) 
    {
        float s = sqrt(disciminant);
        float dstToSphereNear = max(0, (-b - s) / (2 * a));
        float dstToShpereFar = (-b + s) / (2 * a);

        if (dstToShpereFar >= 0) 
        {
            return f2(dstToSphereNear, dstToShpereFar - dstToSphereNear);
        }
    }

    return f2(-1, 0);
}

float2 Rotate(const float2 origin, const float2 point, const float angle) 
{
    float x = origin.x + ((point.x - origin.x) * cos(angle)) + ((point.y - origin.y) * sin(angle));
    float y = origin.y + ((point.x - origin.x) * sin(angle)) + ((point.y - origin.y) * cos(angle));

    return f2(x, y);
}

float3 MidPoint(const float3 a, const float3 b) { return f3((a.x + b.x) / 2, (a.y + b.y) / 2, (a.z + b.z) / 2); }
float2 MidPoint(const float2 a, const float2 b) { return f2((a.x + b.x) / 2, (a.y + b.y) / 2); }
float3 MovePoint(float3 a, float3 b, float distance) { return a + (normalize(b - a) * distance); }
float2 MovePoint(float2 a, float2 b, float distance) { return a + (normalize(b - a) * distance); }
float3 MovePoint01(const float3 a, const float3 b, const float distance) { return lerp(a, b, distance); }
float2 MovePoint01(const float2 a, const float2 b, const float distance) { return lerp(a, b, distance); }

bool IsPrime(const uint32 num)
{
    if (num == 1) { return true; }

    uint32 sqrtNum = sqrt(num);
    for (uint32 i = 2; i <= sqrtNum; i++) 
    {
        if (num % i == 0) { return false; }
    }
 
    return true;
}

float pow(const float a, const float b) 
{
    if (floor(b) == ceiling(b)) 
    {
        if (b == 0) { return 1; }
        float result = 1;
        uint16 i = abs(b);
        while (i > 0) { result *= a; i--; }
        return (b < 0) ? 1 / result : result;
    }

    if (a < 0) { return 0; }
    int32 p = floor(abs(b));
    float result = pow(a, p);
    if (abs(b) - p > 0.3 && abs(b) - p < 0.7) { result *= sqrt(a); }
    return (b < 0) ? 1 / result : result;
}