#include <definitions.h>
#include <math.h>
#define __LO(x) *(int*)&x
#define __HI(x) *(1 + (int*)&x)

// https://netlib.org/fdlibm/

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

float sin(const float x) 
{
    float num = x;
    float output = 0;
    bool greater = false;

    while (num > PI * 2) { num -= PI * 2; }
    if (num > PI) { num -= PI; greater = true; }
    for (uint32 n = 1; n <= 6; n++) { output += ((float)intpow(num, (2 * n) - 1) / (float)fact((2 * n) - 1)) * ((n % 2 != 0) ? 1 : -1); }

    return (greater) ? -output : output;
}

float cos(const float x) 
{
    float num = x;
    float output = 0;
    bool greater = false;

    while (num > PI * 2) { num -= PI * 2; }
    if (num > PI) { num -= PI; greater = true; }
    for (uint32 n = 1; n <= 7; n++) { output += ((float)intpow(num, (2 * n) - 2) / (float)fact((2 * n) - 2)) * ((n % 2 != 0) ? 1 : -1); }

    return (greater) ? -output : output;
}

float asin(const float x) 
{
    double n = x;
    double
    one =  1.00000000000000000000e+00, /* 0x3FF00000, 0x00000000 */
    huge =  1.000e+300,
    pio2_hi =  1.57079632679489655800e+00, /* 0x3FF921FB, 0x54442D18 */
    pio2_lo =  6.12323399573676603587e-17, /* 0x3C91A626, 0x33145C07 */
    pio4_hi =  7.85398163397448278999e-01, /* 0x3FE921FB, 0x54442D18 */
    /* coefficient for R(n^2) */
    pS0 =  1.66666666666666657415e-01, /* 0x3FC55555, 0x55555555 */
    pS1 = -3.25565818622400915405e-01, /* 0xBFD4D612, 0x03EB6F7D */
    pS2 =  2.01212532134862925881e-01, /* 0x3FC9C155, 0x0E884455 */
    pS3 = -4.00555345006794114027e-02, /* 0xBFA48228, 0xB5688F3B */
    pS4 =  7.91534994289814532176e-04, /* 0x3F49EFE0, 0x7501B288 */
    pS5 =  3.47933107596021167570e-05, /* 0x3F023DE1, 0x0DFDF709 */
    qS1 = -2.40339491173441421878e+00, /* 0xC0033A27, 0x1C8A2D4B */
    qS2 =  2.02094576023350569471e+00, /* 0x40002AE5, 0x9C598AC8 */
    qS3 = -6.88283971605453293030e-01, /* 0xBFE6066C, 0x1B8D0159 */
    qS4 =  7.70381505559019352791e-02; /* 0x3FB3B8C5, 0xB12E9282 */

    double t,w,p,q,c,r,s;
    int hx,ix;
    hx = __HI(n);
    ix = hx&0x7fffffff;
    if(ix>= 0x3ff00000) {       /* |n|>= 1 */
        if(((ix-0x3ff00000)|__LO(n))==0)
            /* asin(1)=+-pi/2 with inexact */
        return n*pio2_hi+n*pio2_lo; 
        return (n-n)/(n-n);     /* asin(|n|>1) is NaN */   
    } else if (ix<0x3fe00000) { /* |n|<0.5 */
        if(ix<0x3e400000) {     /* if |n| < 2**-27 */
        if(huge+n>one) return n;/* return n with inexact if n!=0*/
        } else 
        t = n*n;
        p = t*(pS0+t*(pS1+t*(pS2+t*(pS3+t*(pS4+t*pS5)))));
        q = one+t*(qS1+t*(qS2+t*(qS3+t*qS4)));
        w = p/q;
        return n+n*w;
    }
    /* 1> |n|>= 0.5 */
    w = one-abs(n);
    t = w*0.5;
    p = t*(pS0+t*(pS1+t*(pS2+t*(pS3+t*(pS4+t*pS5)))));
    q = one+t*(qS1+t*(qS2+t*(qS3+t*qS4)));
    s = sqrt(t);
    if(ix>=0x3FEF3333) {    /* if |n| > 0.975 */
        w = p/q;
        t = pio2_hi-(2.0*(s+s*w)-pio2_lo);
    } else {
        w  = s;
        __LO(w) = 0;
        c  = (t-w*w)/(s+w);
        r  = p/q;
        p  = 2.0*s*r-(pio2_lo-2.0*c);
        q  = pio4_hi-2.0*w;
        t  = pio4_hi-(p-q);
    }    
    if(hx>0) return t; else return -t;    
}

float tan(const float x) { return sin(x) / cos(x); }
float acos(const float x) { return (PI / 2) - asin(x); }
float atan(const float x) { return asin(x / sqrt((x*x) + 1)); }
float atan2(const float y, const float x) { return 2 * atan(y / (sqrt((x*x) + (y*y)) + x)); }

float log2(const float x) { return ln(x) / LN2; }
float log10(const float x) { return ln(x) / LN10; }
float log(const float x, const float a) { return ln(x) / ln(a); }
float ln(const float x) 
{
    double n = x;
    double hfsq,f,s,z,R,w,t1,t2,dk;
    int k,hx,i,j;
    unsigned lx;

    double
    ln2_hi  =  6.93147180369123816490e-01,  /* 3fe62e42 fee00000 */
    ln2_lo  =  1.90821492927058770002e-10,  /* 3dea39ef 35793c76 */
    two54   =  1.80143985094819840000e+16,  /* 43500000 00000000 */
    Lg1 = 6.666666666666735130e-01,  /* 3FE55555 55555593 */
    Lg2 = 3.999999999940941908e-01,  /* 3FD99999 9997FA04 */
    Lg3 = 2.857142874366239149e-01,  /* 3FD24924 94229359 */
    Lg4 = 2.222219843214978396e-01,  /* 3FCC71C5 1D8E78AF */
    Lg5 = 1.818357216161805012e-01,  /* 3FC74664 96CB03DE */
    Lg6 = 1.531383769920937332e-01,  /* 3FC39A09 D078C69F */
    Lg7 = 1.479819860511658591e-01,  /* 3FC2F112 DF3E5244 */
    zero = 0.0;

    hx = __HI(n);       /* high word of n */
    lx = __LO(n);       /* low  word of n */

    k=0;
    if (hx < 0x00100000) {          /* n < 2**-1022  */
        if (((hx&0x7fffffff)|lx)==0) 
        return -two54/zero;     /* log(+-0)=-inf */
        if (hx<0) return (n-n)/zero;    /* log(-#) = NaN */
        k -= 54; n *= two54; /* subnormal number, scale up n */
        hx = __HI(n);       /* high word of n */
    } 
    if (hx >= 0x7ff00000) return n+n;
    k += (hx>>20)-1023;
    hx &= 0x000fffff;
    i = (hx+0x95f64)&0x100000;
    __HI(n) = hx|(i^0x3ff00000);    /* normalize n or n/2 */
    k += (i>>20);
    f = n-1.0;
    if((0x000fffff&(2+hx))<3) { /* |f| < 2**-20 */
        if(f==zero) {if(k==0) return zero;  else {dk=(double)k;
                 return dk*ln2_hi+dk*ln2_lo;}}
        R = f*f*(0.5-0.33333333333333333*f);
        if(k==0) return f-R; else {dk=(double)k;
                 return dk*ln2_hi-((R-dk*ln2_lo)-f);}
    }
    s = f/(2.0+f); 
    dk = (double)k;
    z = s*s;
    i = hx-0x6147a;
    w = z*z;
    j = 0x6b851-hx;
    t1= w*(Lg2+w*(Lg4+w*Lg6)); 
    t2= z*(Lg1+w*(Lg3+w*(Lg5+w*Lg7))); 
    i |= j;
    R = t2+t1;
    if(i>0) {
        hfsq=0.5*f*f;
        if(k==0) return f-(hfsq-s*(hfsq+R)); else
             return dk*ln2_hi-((hfsq-(s*(hfsq+R)+dk*ln2_lo))-f);
    } else {
        if(k==0) return f-s*(f-R); else
             return dk*ln2_hi-((s*(f-R)-dk*ln2_lo)-f);
    }
}

uint32 fact(const uint32 x) 
{
    uint32 n = x;
    uint32 output = 1;
    while (n > 0) { output *= n; n--; }
    return output;
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

bool IsPrime(const uint32 x)
{
    uint32 sqrtNum = sqrt(x);
    for (uint32 i = 2; i <= sqrtNum; i++) { if (x % i == 0) { return false; } }
    return true;
}

float pow10(const int32 x) { return intpow(10, x); }
float intpow(const float a, const int32 b) 
{
    float output = 1;
    for (int32 i = 0; i < ((b < 0) ? -b : b); i++) { output *= a; }
    return (b < 0) ? 1 / output : output;
}

double copysign(double x, double y) { __HI(x) = (__HI(x)&0x7fffffff)|(__HI(y)&0x80000000); return x; }
double scalbn(double x, int32 n) 
{
    double
    two54   =  1.80143985094819840000e+16, /* 0x43500000, 0x00000000 */
    twom54  =  5.55111512312578270212e-17, /* 0x3C900000, 0x00000000 */
    huge   = 1.0e+300,
    tiny   = 1.0e-300;

    int  k,hx,lx;
    hx = __HI(x);
    lx = __LO(x);
        k = (hx&0x7ff00000)>>20;        /* extract exponent */
        if (k==0) {             /* 0 or subnormal x */
            if ((lx|(hx&0x7fffffff))==0) return x; /* +-0 */
        x *= two54; 
        hx = __HI(x);
        k = ((hx&0x7ff00000)>>20) - 54; 
            if (n< -50000) return tiny*x;   /*underflow*/
        }
        if (k==0x7ff) return x+x;       /* NaN or Inf */
        k = k+n; 
        if (k >  0x7fe) return huge*copysign(huge,x); /* overflow  */
        if (k > 0)              /* normal result */
        {__HI(x) = (hx&0x800fffff)|(k<<20); return x;}
        if (k <= -54) 
        {
            if (n > 50000)  /* in case integer overflow in n+k */
            { return huge*copysign(huge,x); }   /*overflow*/
        } else return tiny*copysign(tiny,x);  /*underflow*/
        k += 54;                /* subnormal result */
        __HI(x) = (hx&0x800fffff)|(k<<20);
        return x*twom54;
}

float pow(const float x, const float y) 
{
    double a = x;
    double b = y;
    double z,ax,z_h,z_l,p_h,p_l;
    double y1,t1,t2,r,s,t,u,v,w;
    int /*i0,i1,*/i,j,k,yisint,n;
    int hx,hy,ix,iy;
    unsigned lx,ly;

    double
    bp[] = {1.0, 1.5,},
    dp_h[] = { 0.0, 5.84962487220764160156e-01,}, /* 0x3FE2B803, 0x40000000 */
    dp_l[] = { 0.0, 1.35003920212974897128e-08,}, /* 0x3E4CFDEB, 0x43CFD006 */
    zero    =  0.0,
    one =  1.0,
    two =  2.0,
    two53   =  9007199254740992.0,  /* 0x43400000, 0x00000000 */
    huge    =  1.0e300,
    tiny    =  1.0e-300,
    /* poly coefs for (3/2)*(log(a)-2s-2/3*s**3 */
    L1  =  5.99999999999994648725e-01, /* 0x3FE33333, 0x33333303 */
    L2  =  4.28571428578550184252e-01, /* 0x3FDB6DB6, 0xDB6FABFF */
    L3  =  3.33333329818377432918e-01, /* 0x3FD55555, 0x518F264D */
    L4  =  2.72728123808534006489e-01, /* 0x3FD17460, 0xA91D4101 */
    L5  =  2.30660745775561754067e-01, /* 0x3FCD864A, 0x93C9DB65 */
    L6  =  2.06975017800338417784e-01, /* 0x3FCA7E28, 0x4A454EEF */
    P1   =  1.66666666666666019037e-01, /* 0x3FC55555, 0x5555553E */
    P2   = -2.77777777770155933842e-03, /* 0xBF66C16C, 0x16BEBD93 */
    P3   =  6.61375632143793436117e-05, /* 0x3F11566A, 0xAF25DE2C */
    P4   = -1.65339022054652515390e-06, /* 0xBEBBBD41, 0xC5D26BF1 */
    P5   =  4.13813679705723846039e-08, /* 0x3E663769, 0x72BEA4D0 */
    lg2  =  6.93147180559945286227e-01, /* 0x3FE62E42, 0xFEFA39EF */
    lg2_h  =  6.93147182464599609375e-01, /* 0x3FE62E43, 0x00000000 */
    lg2_l  = -1.90465429995776804525e-09, /* 0xBE205C61, 0x0CA86C39 */
    ovt =  8.0085662595372944372e-0017, /* -(1024-log2(ovfl+.5ulp)) */
    cp    =  9.61796693925975554329e-01, /* 0x3FEEC709, 0xDC3A03FD =2/(3ln2) */
    cp_h  =  9.61796700954437255859e-01, /* 0x3FEEC709, 0xE0000000 =(float)cp */
    cp_l  = -7.02846165095275826516e-09, /* 0xBE3E2FE0, 0x145B01F5 =tail of cp_h*/
    ivln2    =  1.44269504088896338700e+00, /* 0x3FF71547, 0x652B82FE =1/ln2 */
    ivln2_h  =  1.44269502162933349609e+00, /* 0x3FF71547, 0x60000000 =24b 1/ln2*/
    ivln2_l  =  1.92596299112661746887e-08; /* 0x3E54AE0B, 0xF85DDF44 =1/ln2 tail*/

    // i0 = ((*(int*)&one)>>29)^1; i1=1-i0;
    hx = __HI(a); lx = __LO(a);
    hy = __HI(b); ly = __LO(b);
    ix = hx&0x7fffffff;  iy = hy&0x7fffffff;

    /* b==zero: a**0 = 1 */
    if((iy|ly)==0) return one;  

    /* +-NaN return a+b */
    if(ix > 0x7ff00000 || ((ix==0x7ff00000)&&(lx!=0)) ||
       iy > 0x7ff00000 || ((iy==0x7ff00000)&&(ly!=0))) 
        return a+b; 

    /* determine if b is an odd int when a < 0
     * yisint = 0   ... b is not an integer
     * yisint = 1   ... b is an odd int
     * yisint = 2   ... b is an even int
     */
    yisint  = 0;
    if(hx<0) {  
        if(iy>=0x43400000) yisint = 2; /* even integer b */
        else if(iy>=0x3ff00000) {
        k = (iy>>20)-0x3ff;    /* exponent */
        if(k>20) {
            j = ly>>(52-k);
            if((unsigned)(j<<(52-k))==ly) yisint = 2-(j&1);
        } else if(ly==0) {
            j = iy>>(20-k);
            if((j<<(20-k))==iy) yisint = 2-(j&1);
        }
        }       
    } 

    /* special value of b */
    if(ly==0) {     
        if (iy==0x7ff00000) {   /* b is +-inf */
            if(((ix-0x3ff00000)|lx)==0)
            return  b - b;  /* inf**+-1 is NaN */
            else if (ix >= 0x3ff00000)/* (|a|>1)**+-inf = inf,0 */
            return (hy>=0)? b: zero;
            else            /* (|a|<1)**-,+inf = inf,0 */
            return (hy<0)?-b: zero;
        } 
        if(iy==0x3ff00000) {    /* b is  +-1 */
        if(hy<0) return one/a; else return a;
        }
        if(hy==0x40000000) return a*a; /* b is  2 */
        if(hy==0x3fe00000) {    /* b is  0.5 */
        if(hx>=0)   /* a >= +0 */
        return sqrt(a); 
        }
    }

    ax   = abs(a);
    /* special value of a */
    if(lx==0) {
        if(ix==0x7ff00000||ix==0||ix==0x3ff00000){
        z = ax;         /*a is +-0,+-inf,+-1*/
        if(hy<0) z = one/z; /* z = (1/|a|) */
        if(hx<0) {
            if(((ix-0x3ff00000)|yisint)==0) {
            z = (z-z)/(z-z); /* (-1)**non-int is NaN */
            } else if(yisint==1) 
            z = -z;     /* (a<0)**odd = -(|a|**odd) */
        }
        return z;
        }
    }
    
    n = (hx>>31)+1;

    /* (a<0)**(non-int) is NaN */
    if((n|yisint)==0) return (a-a)/(a-a);

    s = one; /* s (sign of result -ve**odd) = -1 else = 1 */
    if((n|(yisint-1))==0) s = -one;/* (-ve)**(odd int) */

    /* |b| is huge */
    if(iy>0x41e00000) { /* if |b| > 2**31 */
        if(iy>0x43f00000){  /* if |b| > 2**64, must o/uflow */
        if(ix<=0x3fefffff) return (hy<0)? huge*huge:tiny*tiny;
        if(ix>=0x3ff00000) return (hy>0)? huge*huge:tiny*tiny;
        }
    /* over/underflow if a is not close to one */
        if(ix<0x3fefffff) return (hy<0)? s*huge*huge:s*tiny*tiny;
        if(ix>0x3ff00000) return (hy>0)? s*huge*huge:s*tiny*tiny;
    /* now |1-a| is tiny <= 2**-20, suffice to compute 
       log(a) by a-a^2/2+a^3/3-a^4/4 */
        t = ax-one;     /* t has 20 trailing zeros */
        w = (t*t)*(0.5-t*(0.3333333333333333333333-t*0.25));
        u = ivln2_h*t;  /* ivln2_h has 21 sig. bits */
        v = t*ivln2_l-w*ivln2;
        t1 = u+v;
        __LO(t1) = 0;
        t2 = v-(t1-u);
    } else {
        double ss,s2,s_h,s_l,t_h,t_l;
        n = 0;
    /* take care subnormal number */
        if(ix<0x00100000)
        {ax *= two53; n -= 53; ix = __HI(ax); }
        n  += ((ix)>>20)-0x3ff;
        j  = ix&0x000fffff;
    /* determine interval */
        ix = j|0x3ff00000;      /* normalize ix */
        if(j<=0x3988E) k=0;     /* |a|<sqrt(3/2) */
        else if(j<0xBB67A) k=1; /* |a|<sqrt(3)   */
        else {k=0;n+=1;ix -= 0x00100000;}
        __HI(ax) = ix;

    /* compute ss = s_h+s_l = (a-1)/(a+1) or (a-1.5)/(a+1.5) */
        u = ax-bp[k];       /* bp[0]=1.0, bp[1]=1.5 */
        v = one/(ax+bp[k]);
        ss = u*v;
        s_h = ss;
        __LO(s_h) = 0;
    /* t_h=ax+bp[k] High */
        t_h = zero;
        __HI(t_h)=((ix>>1)|0x20000000)+0x00080000+(k<<18); 
        t_l = ax - (t_h-bp[k]);
        s_l = v*((u-s_h*t_h)-s_h*t_l);
    /* compute log(ax) */
        s2 = ss*ss;
        r = s2*s2*(L1+s2*(L2+s2*(L3+s2*(L4+s2*(L5+s2*L6)))));
        r += s_l*(s_h+ss);
        s2  = s_h*s_h;
        t_h = 3.0+s2+r;
        __LO(t_h) = 0;
        t_l = r-((t_h-3.0)-s2);
    /* u+v = ss*(1+...) */
        u = s_h*t_h;
        v = s_l*t_h+t_l*ss;
    /* 2/(3log2)*(ss+...) */
        p_h = u+v;
        __LO(p_h) = 0;
        p_l = v-(p_h-u);
        z_h = cp_h*p_h;     /* cp_h+cp_l = 2/(3*log2) */
        z_l = cp_l*p_h+p_l*cp+dp_l[k];
    /* log2(ax) = (ss+..)*2/(3*log2) = n + dp_h + z_h + z_l */
        t = (double)n;
        t1 = (((z_h+z_l)+dp_h[k])+t);
        __LO(t1) = 0;
        t2 = z_l-(((t1-t)-dp_h[k])-z_h);
    }

    /* split up b into y1+y2 and compute (y1+y2)*(t1+t2) */
    y1  = b;
    __LO(y1) = 0;
    p_l = (b-y1)*t1+b*t2;
    p_h = y1*t1;
    z = p_l+p_h;
    j = __HI(z);
    i = __LO(z);
    if (j>=0x40900000) {                /* z >= 1024 */
        if(((j-0x40900000)|i)!=0)           /* if z > 1024 */
        return s*huge*huge;         /* overflow */
        else {
        if(p_l+ovt>z-p_h) return s*huge*huge;   /* overflow */
        }
    } else if((j&0x7fffffff)>=0x4090cc00 ) {    /* z <= -1075 */
        if(((j-0xc090cc00)|i)!=0)       /* z < -1075 */
        return s*tiny*tiny;     /* underflow */
        else {
        if(p_l<=z-p_h) return s*tiny*tiny;  /* underflow */
        }
    }
    /*
     * compute 2**(p_h+p_l)
     */
    i = j&0x7fffffff;
    k = (i>>20)-0x3ff;
    n = 0;
    if(i>0x3fe00000) {      /* if |z| > 0.5, set n = [z+0.5] */
        n = j+(0x00100000>>(k+1));
        k = ((n&0x7fffffff)>>20)-0x3ff; /* new k for n */
        t = zero;
        __HI(t) = (n&~(0x000fffff>>k));
        n = ((n&0x000fffff)|0x00100000)>>(20-k);
        if(j<0) n = -n;
        p_h -= t;
    } 
    t = p_l+p_h;
    __LO(t) = 0;
    u = t*lg2_h;
    v = (p_l-(t-p_h))*lg2+t*lg2_l;
    z = u+v;
    w = v-(z-u);
    t  = z*z;
    t1  = z - t*(P1+t*(P2+t*(P3+t*(P4+t*P5))));
    r  = (z*t1)/(t1-two)-(w+z*w);
    z  = one-(r-z);
    j  = __HI(z);
    j += (n<<20);
    if((j>>20)<=0) z = scalbn(z,n); /* subnormal output */
    else __HI(z) += (n<<20);
    return s*z;
}