#ifndef __MD5_H__
#define  __MD5_H__
//四个转换函数
#define  F(a,b,c) (((a) & (b))|((~a) & (c)))
#define  G(a,b,c) (((a) & (c))|((b) & (~c)))
#define  H(a,b,c) ((a) ^ (b) ^ (c))
#define  I(a,b,c) ((b) ^ ((a) | (~c)))
//ABCD初始值
#define _atemp = 0x67452301;
#define _btemp = 0xefcdab89;
#define _ctemp = 0x98badcfe;
#define _dtemp = 0x10325476;













#endif __MD5_H__