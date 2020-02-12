#ifndef __MD5_H__
#define  __MD5_H__

typedef unsigned int uint;
#define CHUNK_BYTE 64//一个chunk的字节数
#define CHUNK_SIZE 16//一个chunk的长度, uint[16]
//四个位运算函数
#define  F(a,b,c) (((a) & (b))|((~a) & (c)))
#define  G(a,b,c) (((a) & (c))|((b) & (~c)))
#define  H(a,b,c) ((a) ^ (b) ^ (c))
#define  I(a,b,c) ((b) ^ ((a) | (~c)))
//ABCD初始值
#define _atemp  0x67452301;
#define _btemp  0xefcdab89;
#define _ctemp  0x98badcfe;
#define _dtemp  0x10325476;

//k[64] K[i] = floor(2^(32) * abs(sin(i + 1))) 
//s[64] 循环左移的位数 s[64] = { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7,12, 17, 22, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 6, 10,15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };
//uint s[64];
uint k[64];

//以字节为单位
uint totalByte;//总共的字节数
uint lastByte;//最后一个数据块的字节数
uint A;
uint B;
uint C;
uint D;
//16 * 4 字节
uint chunk[CHUNK_SIZE];

//字符串的MD5


//文件的MD5


//初始化 : k[64] ABCD 文件长度 最后一个数据块的长度
void init();

//循环移位
uint shift(uint tmp, uint shiftNumber);

//获取文件的长度


//获取最后一块数据的长度


//一个chunk的处理
void chunkMD5(uint* chunk);

//填充冗余信息 
void fillChunk(uint* chunk);


//整型转字符串,最后的MD5输出
//获取每一个字节的数据


#endif __MD5_H__