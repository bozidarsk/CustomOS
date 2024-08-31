using System.Runtime;
using static System.Math;

namespace Runtime;

#pragma warning disable CS0675 // Bitwise-or operator used on a sign-extended operand; consider casting to a smaller unsigned type first

internal static unsafe class Math 
{
	[RuntimeExport("scalbn")]
	private static double scalbn(double x, int n) 
	{
		double a, b;

		*((ulong*)(&a)) = 0x7fe0000000000000;
		*((ulong*)(&b)) = 0x10000000000000;

		if (n > 1023) 
		{
			x *= a;
			n -= 1023;

			if (n > 1023) 
			{
				x *= a;
				n -= 1023;

				if (n > 1023)
					return x * a;
			}
		}
		else if (n < -1022) 
		{
			x *= b;
			n += 1022;

			if (n < -1022) 
			{
				x *= b;
				n += 1022;

				if (n < -1022)
					return x * b;
			}
		}

		double scale;

		*((ulong*)(&scale)) = ((ulong)((uint)(0x3ff + n) << 20) << 32);

		return x * scale;
	}

	[RuntimeExport("exp")]
	private static double exp(double x) 
	{
		const double ln2hi = 6.93147180369123816490e-01; /* 0x3fe62e42, 0xfee00000 */
		const double ln2lo = 1.90821492927058770002e-10; /* 0x3dea39ef, 0x35793c76 */
		const double invln2 = 1.44269504088896338700e+00; /* 0x3ff71547, 0x652b82fe */
		const double P1   =  1.66666666666666019037e-01; /* 0x3FC55555, 0x5555553E */
		const double P2   = -2.77777777770155933842e-03; /* 0xBF66C16C, 0x16BEBD93 */
		const double P3   =  6.61375632143793436117e-05; /* 0x3F11566A, 0xAF25DE2C */
		const double P4   = -1.65339022054652515390e-06; /* 0xBEBBBD41, 0xC5D26BF1 */
		const double P5   =  4.13813679705723846039e-08; /* 0x3E663769, 0x72BEA4D0 */
		double* half = stackalloc double[2] { 0.5, -0.5 };

		double a, b;
		*((ulong*)(&a)) = 0x7fe0000000000000;
		*((ulong*)(&b)) = 0x170000000000000;

		double hi, lo, c, xx;
		int k, sign;
		uint hx;

		hx = (uint)(*((ulong*)(&x)) >> 32);

		sign = (int)(hx >> 31);
		hx &= 0x7fffffff; /* high word of |x| */

		/* special cases */
		if (hx >= 0x40862e42) /* if |x| >= 709.78... */
		{
			if (double.IsNaN(x))
				return x;

			if (hx == 0x7ff00000 && sign == 1) /* -inf */
				return 0;

			if (x > 709.782712893383973096) /* overflow if x!=inf */
				return a * x;

			if (x < -745.13321910194110842) /* underflow */
				return b * b;
		}

		/* argument reduction */
		if (hx > 0x3fd62e42) /* if |x| > 0.5 ln2 */
		{
			if (hx >= 0x3ff0a2b2) /* if |x| >= 1.5 ln2 */
				k = (int)(invln2 * x + half[sign]);
			else
				k = 1 - sign - sign;

			hi = x - k * ln2hi; /* k*ln2hi is exact here */
			lo = k * ln2lo;
			x = hi - lo;
		}
		else if (hx > 0x3e300000) /* if |x| > 2**-28 */
		{
			k = 0;
			hi = x;
			lo = 0;
		}
		else
			return 1 + x;

		/* x is now in primary range */
		xx = x *x;
		c = x - xx*(P1+xx*(P2+xx*(P3+xx*(P4+xx*P5))));
		x = 1 + (x*c/(2-c) - lo + hi);

		return (k == 0) ? x : scalbn(x, k);
	}

	[RuntimeExport("log")]
	private static double log(double x) 
	{
		const double ln2_hi = 6.93147180369123816490e-01;  /* 3fe62e42 fee00000 */
		const double ln2_lo = 1.90821492927058770002e-10;  /* 3dea39ef 35793c76 */
		const double two54  = 1.80143985094819840000e+16;  /* 43500000 00000000 */
		const double Lg1 = 6.666666666666735130e-01;  /* 3FE55555 55555593 */
		const double Lg2 = 3.999999999940941908e-01;  /* 3FD99999 9997FA04 */
		const double Lg3 = 2.857142874366239149e-01;  /* 3FD24924 94229359 */
		const double Lg4 = 2.222219843214978396e-01;  /* 3FCC71C5 1D8E78AF */
		const double Lg5 = 1.818357216161805012e-01;  /* 3FC74664 96CB03DE */
		const double Lg6 = 1.531383769920937332e-01;  /* 3FC39A09 D078C69F */
		const double Lg7 = 1.479819860511658591e-01;  /* 3FC2F112 DF3E5244 */

		double hfsq, f, s, z, R, w, t1, t2, dk;
		int k, hx, i, j;
		uint lx;

		hx = (int)(*((ulong*)(&x)) >> 32);
		lx = (uint)(*((ulong*)(&x)));

		k = 0;
		if (hx < 0x00100000) /* x < 2**-1022  */
		{
			if (((hx & 0x7fffffff) | lx) == 0)
				return -two54/0.0; /* log(+-0)=-inf */

			if (hx < 0)
				return (x - x)/0.0; /* log(-#) = NaN */

			/* subnormal number, scale up x */
			k -= 54;
			x *= two54;
			hx = (int)(*((ulong*)(&x)) >> 32);  
		}

		if (hx >= 0x7ff00000)
			return x+x;

		k += (hx >> 20) - 1023;
		hx &= 0x000fffff;
		i = (hx + 0x95f64) & 0x100000;
		*((ulong*)(&x)) &= 0xffffffff;
		*((ulong*)(&x)) |= (ulong)(hx | (i ^ 0x3ff00000)) << 32;

		k += i >> 20;
		f = x - 1.0;
		if ((0x000fffff & (2 + hx)) < 3) /* -2**-20 <= f < 2**-20 */
		{
			if (f == 0.0) 
			{
				if (k == 0)
					return 0.0;

				dk = (double)k;
				return dk * ln2_hi + dk * ln2_lo;
			}

			R = f * f * (0.5 - 0.33333333333333333 * f);
			if (k == 0)
				return f - R;

			dk = (double)k;
			return dk * ln2_hi - ((R - dk * ln2_lo) - f);
		}

		s = f / (2.0 + f);
		dk = (double)k;
		z = s * s;
		i = hx - 0x6147a;
		w = z * z;
		j = 0x6b851 - hx;
		t1 = w * (Lg2 + w * (Lg4 + w * Lg6));
		t2 = z * (Lg1 + w * (Lg3 + w * (Lg5 + w * Lg7)));
		i |= j;
		R = t2 + t1;

		if (i > 0) 
		{
			hfsq = 0.5 * f * f;
			if (k == 0)
				return f - (hfsq - s * (hfsq + R));

			return dk * ln2_hi - ((hfsq - (s * (hfsq + R) + dk * ln2_lo))-f);
		}
		else 
		{
			if (k == 0)
				return f - s * (f - R);

			return dk * ln2_hi - ((s * (f - R) - dk * ln2_lo) - f);
		}
	}

	[RuntimeExport("pow")]
	private static double pow(double x, double y)
	{
		double* bp = stackalloc double[2] {1.0, 1.5,};
		double* dp_h = stackalloc double[2] { 0.0, 5.84962487220764160156e-01,}; /* 0x3FE2B803, 0x40000000 */
		double* dp_l = stackalloc double[2] { 0.0, 1.35003920212974897128e-08,}; /* 0x3E4CFDEB, 0x43CFD006 */
		const double two53  =  9007199254740992.0; /* 0x43400000, 0x00000000 */
		const double huge   =  1.0e300;
		const double tiny   =  1.0e-300;
		/* poly coefs for (3/2)*(log(x)-2s-2/3*s**3) */
		const double L1 =  5.99999999999994648725e-01; /* 0x3FE33333, 0x33333303 */
		const double L2 =  4.28571428578550184252e-01; /* 0x3FDB6DB6, 0xDB6FABFF */
		const double L3 =  3.33333329818377432918e-01; /* 0x3FD55555, 0x518F264D */
		const double L4 =  2.72728123808534006489e-01; /* 0x3FD17460, 0xA91D4101 */
		const double L5 =  2.30660745775561754067e-01; /* 0x3FCD864A, 0x93C9DB65 */
		const double L6 =  2.06975017800338417784e-01; /* 0x3FCA7E28, 0x4A454EEF */
		const double P1 =  1.66666666666666019037e-01; /* 0x3FC55555, 0x5555553E */
		const double P2 = -2.77777777770155933842e-03; /* 0xBF66C16C, 0x16BEBD93 */
		const double P3 =  6.61375632143793436117e-05; /* 0x3F11566A, 0xAF25DE2C */
		const double P4 = -1.65339022054652515390e-06; /* 0xBEBBBD41, 0xC5D26BF1 */
		const double P5 =  4.13813679705723846039e-08; /* 0x3E663769, 0x72BEA4D0 */
		const double lg2     =  6.93147180559945286227e-01; /* 0x3FE62E42, 0xFEFA39EF */
		const double lg2_h   =  6.93147182464599609375e-01; /* 0x3FE62E43, 0x00000000 */
		const double lg2_l   = -1.90465429995776804525e-09; /* 0xBE205C61, 0x0CA86C39 */
		const double ovt     =  8.0085662595372944372e-017; /* -(1024-log2(ovfl+.5ulp)) */
		const double cp      =  9.61796693925975554329e-01; /* 0x3FEEC709, 0xDC3A03FD =2/(3ln2) */
		const double cp_h    =  9.61796700954437255859e-01; /* 0x3FEEC709, 0xE0000000 =(float)cp */
		const double cp_l    = -7.02846165095275826516e-09; /* 0xBE3E2FE0, 0x145B01F5 =tail of cp_h*/
		const double ivln2   =  1.44269504088896338700e+00; /* 0x3FF71547, 0x652B82FE =1/ln2 */
		const double ivln2_h =  1.44269502162933349609e+00; /* 0x3FF71547, 0x60000000 =24b 1/ln2*/
		const double ivln2_l =  1.92596299112661746887e-08; /* 0x3E54AE0B, 0xF85DDF44 =1/ln2 tail*/

		double z,ax,z_h,z_l,p_h,p_l;
		double y1,t1,t2,r,s,t,u,v,w;
		int i,j,k,yisint,n;
		int hx,hy,ix,iy;
		uint lx,ly;

		hx = (int)(*((ulong*)(&x)) >> 32);
		lx = (uint)(*((ulong*)(&x)));

		hy = (int)(*((ulong*)(&y)) >> 32);
		ly = (uint)(*((ulong*)(&y)));

		ix = hx & 0x7fffffff;
		iy = hy & 0x7fffffff;

		/* x**0 = 1, even if x is NaN */
		if ((iy|ly) == 0)
			return 1.0;
		/* 1**y = 1, even if y is NaN */
		if (hx == 0x3ff00000 && lx == 0)
			return 1.0;
		/* NaN if either arg is NaN */
		if (ix > 0x7ff00000 || (ix == 0x7ff00000 && lx != 0) ||
		    iy > 0x7ff00000 || (iy == 0x7ff00000 && ly != 0))
			return x + y;

		/* determine if y is an odd int when x < 0
		 * yisint = 0       ... y is not an integer
		 * yisint = 1       ... y is an odd int
		 * yisint = 2       ... y is an even int
		 */
		yisint = 0;
		if (hx < 0) {
			if (iy >= 0x43400000)
				yisint = 2; /* even integer y */
			else if (iy >= 0x3ff00000) {
				k = (iy>>20) - 0x3ff;  /* exponent */
				if (k > 20) {
					j = (int)(ly>>(52-k));
					if ((j<<(52-k)) == ly)
						yisint = 2 - (j&1);
				} else if (ly == 0) {
					j = iy>>(20-k);
					if ((j<<(20-k)) == iy)
						yisint = 2 - (j&1);
				}
			}
		}

		/* special value of y */
		if (ly == 0) {
			if (iy == 0x7ff00000) {  /* y is +-inf */
				if (((ix-0x3ff00000)|lx) == 0)  /* (-1)**+-inf is 1 */
					return 1.0;
				else if (ix >= 0x3ff00000) /* (|x|>1)**+-inf = inf,0 */
					return hy >= 0 ? y : 0.0;
				else                       /* (|x|<1)**+-inf = 0,inf */
					return hy >= 0 ? 0.0 : -y;
			}
			if (iy == 0x3ff00000)    /* y is +-1 */
				return hy >= 0 ? x : 1.0/x;
			if (hy == 0x40000000)    /* y is 2 */
				return x*x;
			if (hy == 0x3fe00000) {  /* y is 0.5 */
				if (hx >= 0)     /* x >= +0 */
					return Sqrt(x);
			}
		}

		ax = Abs(x);
		/* special value of x */
		if (lx == 0) {
			if (ix == 0x7ff00000 || ix == 0 || ix == 0x3ff00000) { /* x is +-0,+-inf,+-1 */
				z = ax;
				if (hy < 0)   /* z = (1/|x|) */
					z = 1.0/z;
				if (hx < 0) {
					if (((ix-0x3ff00000)|yisint) == 0) {
						z = (z-z)/(z-z); /* (-1)**non-int is NaN */
					} else if (yisint == 1)
						z = -z;          /* (x<0)**odd = -(|x|**odd) */
				}
				return z;
			}
		}

		s = 1.0; /* sign of result */
		if (hx < 0) {
			if (yisint == 0) /* (x<0)**(non-int) is NaN */
				return (x-x)/(x-x);
			if (yisint == 1) /* (x<0)**(odd int) */
				s = -1.0;
		}

		/* |y| is huge */
		if (iy > 0x41e00000) { /* if |y| > 2**31 */
			if (iy > 0x43f00000) {  /* if |y| > 2**64, must o/uflow */
				if (ix <= 0x3fefffff)
					return hy < 0 ? huge*huge : tiny*tiny;
				if (ix >= 0x3ff00000)
					return hy > 0 ? huge*huge : tiny*tiny;
			}
			/* over/underflow if x is not close to one */
			if (ix < 0x3fefffff)
				return hy < 0 ? s*huge*huge : s*tiny*tiny;
			if (ix > 0x3ff00000)
				return hy > 0 ? s*huge*huge : s*tiny*tiny;
			/* now |1-x| is tiny <= 2**-20, suffice to compute
			   log(x) by x-x^2/2+x^3/3-x^4/4 */
			t = ax - 1.0;       /* t has 20 trailing zeros */
			w = (t*t)*(0.5 - t*(0.3333333333333333333333-t*0.25));
			u = ivln2_h*t;      /* ivln2_h has 21 sig. bits */
			v = t*ivln2_l - w*ivln2;
			t1 = u + v;
			*((ulong*)(&t1)) &= 0xffffffff00000000;
			t2 = v - (t1-u);
		} else {
			double ss,s2,s_h,s_l,t_h,t_l;
			n = 0;
			/* take care subnormal number */
			if (ix < 0x00100000) {
				ax *= two53;
				n -= 53;
				ix = (int)(*((ulong*)(&ax)) >> 32);
			}
			n += ((ix)>>20) - 0x3ff;
			j = ix & 0x000fffff;
			/* determine interval */
			ix = j | 0x3ff00000;   /* normalize ix */
			if (j <= 0x3988E)      /* |x|<sqrt(3/2) */
				k = 0;
			else if (j < 0xBB67A)  /* |x|<sqrt(3)   */
				k = 1;
			else {
				k = 0;
				n += 1;
				ix -= 0x00100000;
			}
			*((ulong*)(&ax)) &= 0xffffffff;
			*((ulong*)(&ax)) |= (ulong)ix << 32;

			/* compute ss = s_h+s_l = (x-1)/(x+1) or (x-1.5)/(x+1.5) */
			u = ax - bp[k];        /* bp[0]=1.0, bp[1]=1.5 */
			v = 1.0/(ax+bp[k]);
			ss = u*v;
			s_h = ss;
			*((ulong*)(&s_h)) &= 0xffffffff00000000;
			/* t_h=ax+bp[k] High */
			t_h = 0.0;
			*((ulong*)(&t_h)) &= 0xffffffff;
			*((ulong*)(&t_h)) |= (ulong)(((ix>>1)|0x20000000) + 0x00080000 + (k<<18)) << 32;
			t_l = ax - (t_h-bp[k]);
			s_l = v*((u-s_h*t_h)-s_h*t_l);
			/* compute log(ax) */
			s2 = ss*ss;
			r = s2*s2*(L1+s2*(L2+s2*(L3+s2*(L4+s2*(L5+s2*L6)))));
			r += s_l*(s_h+ss);
			s2 = s_h*s_h;
			t_h = 3.0 + s2 + r;
			*((ulong*)(&t_h)) &= 0xffffffff00000000;
			t_l = r - ((t_h-3.0)-s2);
			/* u+v = ss*(1+...) */
			u = s_h*t_h;
			v = s_l*t_h + t_l*ss;
			/* 2/(3log2)*(ss+...) */
			p_h = u + v;
			*((ulong*)(&p_h)) &= 0xffffffff00000000;
			p_l = v - (p_h-u);
			z_h = cp_h*p_h;        /* cp_h+cp_l = 2/(3*log2) */
			z_l = cp_l*p_h+p_l*cp + dp_l[k];
			/* log2(ax) = (ss+..)*2/(3*log2) = n + dp_h + z_h + z_l */
			t = (double)n;
			t1 = ((z_h + z_l) + dp_h[k]) + t;
			*((ulong*)(&t1)) &= 0xffffffff00000000;
			t2 = z_l - (((t1 - t) - dp_h[k]) - z_h);
		}

		/* split up y into y1+y2 and compute (y1+y2)*(t1+t2) */
		y1 = y;
		*((ulong*)(&y1)) &= 0xffffffff00000000;
		p_l = (y-y1)*t1 + y*t2;
		p_h = y1*t1;
		z = p_l + p_h;
		j = (int)(*((ulong*)(&z)) >> 32);
		i = (int)(*((ulong*)(&z)));
		if (j >= 0x40900000) {                      /* z >= 1024 */
			if (((j-0x40900000)|i) != 0)        /* if z > 1024 */
				return s*huge*huge;         /* overflow */
			if (p_l + ovt > z - p_h)
				return s*huge*huge;         /* overflow */
		} else if ((j&0x7fffffff) >= 0x4090cc00) {  /* z <= -1075 */  // FIXME: instead of abs(j) use unsigned j
			if (((j-0xc090cc00)|i) != 0)        /* z < -1075 */
				return s*tiny*tiny;         /* underflow */
			if (p_l <= z - p_h)
				return s*tiny*tiny;         /* underflow */
		}
		/*
		 * compute 2**(p_h+p_l)
		 */
		i = j & 0x7fffffff;
		k = (i>>20) - 0x3ff;
		n = 0;
		if (i > 0x3fe00000) {  /* if |z| > 0.5, set n = [z+0.5] */
			n = j + (0x00100000>>(k+1));
			k = ((n&0x7fffffff)>>20) - 0x3ff;  /* new k for n */
			t = 0.0;
			*((ulong*)(&t)) &= 0xffffffff;
			*((ulong*)(&t)) |= (ulong)(n & ~(0x000fffff>>k)) << 32;
			n = ((n&0x000fffff)|0x00100000)>>(20-k);
			if (j < 0)
				n = -n;
			p_h -= t;
		}
		t = p_l + p_h;
		*((ulong*)(&t)) &= 0xffffffff00000000;
		u = t*lg2_h;
		v = (p_l-(t-p_h))*lg2 + t*lg2_l;
		z = u + v;
		w = v - (z-u);
		t = z*z;
		t1 = z - t*(P1+t*(P2+t*(P3+t*(P4+t*P5))));
		r = (z*t1)/(t1-2.0) - (w + z*w);
		z = 1.0 - (r-z);
		j = (int)(*((ulong*)(&z)) >> 32);
		j += n<<20;
		if ((j>>20) <= 0)  /* subnormal output */
			z = scalbn(z,n);
		else 
		{
			*((ulong*)(&z)) &= 0xffffffff;
			*((ulong*)(&z)) |= (ulong)j << 32;
		}
		return s*z;
	}
}
